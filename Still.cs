using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;

namespace GadgeteerApp1 {
    enum Action {Init, Pump, Collect, Shutdown };
    enum PumpInput { None, Wash, Boiler };
    enum PumpOutput { None, Boiler, Disposal };

    class State {        
        public string name;
        public Action action;
        public PumpInput pump_input;
        public PumpOutput pump_output;
        public float pump_duration;
        public float min_temp;
        public float max_temp;
        public int output_id;

        public State(string name, Action action, PumpInput pump_input=PumpInput.None, PumpOutput pump_output=PumpOutput.None, float min_temp=-1, float max_temp=-1, float pump_duration=-1, int output_id=-1) {
            this.name = name;
            this.action = action;
            this.pump_input = pump_input;
            this.pump_output = pump_output;
            this.pump_duration = pump_duration;
            this.min_temp = min_temp;
            this.max_temp = max_temp;
            this.output_id = output_id;
        }

        public string pump_input_str() {
            return (new string[3] { "None", "Wash", "Boiler" })[(int)pump_input];
        }
        
        public string pump_output_str() {
            return (new string[3] { "None", "Boiler", "Disposal" })[(int)pump_output];
        }

        public string str()
        {

            // Describe a pumping action
            if (action==Action.Pump && (pump_input != PumpInput.None) && (pump_output != PumpOutput.None) && (pump_duration > 0)) {
                string src = pump_input_str();
                string dest = pump_output_str();
                return "Pumping "+src+" to "+dest+" for "+pump_duration+"s";
            }

            // Describe collecting distilate
            if (action == Action.Collect && min_temp > 0 && max_temp > 0 && min_temp < max_temp && output_id > 0) {
                return min_temp+"C to "+max_temp+"C, into "+output_id;
            }

            // Describe shutting down
            if (action == Action.Shutdown) {
                return "Shutting down";
            }

            return "unknown state!";
        }
    }

    class Distillery {
        Thread controllerThread;
        Thread sensorThread;
        State current_state;
        Relays relays;
        Screen screen;

        public Distillery(Relays relays, Display_T35 display)
        {
            this.current_state = new State("init", Action.Init, PumpInput.None, PumpOutput.None, -1, -1, -1, -1);
            this.relays = relays;
            this.screen = new Screen(display, current_state);
            screen.log("initialized");
        }

        public void Start()
        {
            controllerThread = new Thread(ControllerThreadFunc);
            sensorThread = new Thread(SensorThreadFunc);
            controllerThread.Start();
        }

        void ControllerThreadFunc() {
            while (current_state == null || current_state.action != Action.Shutdown)
            {
                getCurrentState();
                screen.updateState(current_state, false);
                screen.log(current_state.str());
                Thread.Sleep(3000);
            }
        }

        // TODO
        void SensorThreadFunc() {
            while (current_state == null || current_state.action != Action.Shutdown) {
                Thread.Sleep(3000);
            }
        }

        void pumpOn()
        {
            relays.Relay1 = true;
        }

        void pumpOff()
        {
            relays.Relay1 = false;
        }

        bool setPumpInput(PumpInput input)
        {
            if (input == PumpInput.Wash)
            {
                relays.Relay2 = true;
                return true;
            }

            if (input == PumpInput.Boiler)
            {
                relays.Relay2 = true;
                return true;
            }

            return false;
        }

        bool setPumpOutput(PumpOutput output)
        {
            if (output == PumpOutput.Boiler)
            {
                relays.Relay3 = true;
                return true;
            }

            if (output == PumpOutput.Disposal)
            {
                relays.Relay3 = true;
                return true;
            }

            return false;
        }

        bool setPump(PumpInput input, PumpOutput output, int ms)
        {
            pumpOff();
            bool input_result = setPumpInput(input);
            bool output_result = setPumpOutput(output);
            bool success = (input_result || output_result);
            Debug.Print("Pumping from " + input.ToString() + " to " + output.ToString());

            if (success && (ms > 0))
            {
                pumpOn();
                Thread.Sleep(ms);
                pumpOff();
            }
            return success;
        }

        void fillBoiler()
        {
            int pump_ms = 1000 * 5;  // 5 seconds
            setPump(PumpInput.Wash, PumpOutput.Disposal, pump_ms);
            Thread.Sleep(pump_ms);
        }

        void emptyBoiler()
        {
            int pump_ms = 1000 * 5;  // 5 seconds
            setPump(PumpInput.Boiler, PumpOutput.Disposal, pump_ms);
            Thread.Sleep(pump_ms);
        }

        // for testing purposes  
        // TODO: should be re-written to fetch a state from the REST API
        int run_count = 0;
        State getCurrentState()
        {
            if (current_state.name == "init" || (run_count <= 1 && current_state.name == "drain"))
            {
                run_count++;
                current_state = new State("fill", Action.Pump, PumpInput.Wash, PumpOutput.Boiler, -1, -1, 2000, -1);
                return current_state;
            }

            // First fraction - collect between 69-72 °C at 91% - approx. 5% total distillate, unpleasant aldehydes, organic acids and esters. Discard.
            if (current_state.name == "fill")
            {
                current_state = new State("1", Action.Collect, PumpInput.None, PumpOutput.None, 50, 72, -1, 1);
                return current_state;
            }

            // Second fraction, 72-77 °C at 93-94% - 10% total distillate, contains ethanol with appreciable amounts of aldehydes and esters.
            if (current_state.name == "1")
            {
                current_state = new State("2", Action.Collect, PumpInput.None, PumpOutput.None, 72, 77, -1, 2);
                return current_state;
            }

            // Third fraction, at 78 °C, 95.5%, largest in volume at 55-60% ,mostly ethanol with very small amounts of congeners
            if (current_state.name == "2")
            {
                current_state = new State("3", Action.Collect, PumpInput.None, PumpOutput.None, 77, (float)78.5, -1, 3);
                return current_state;
            }

            // Fourth fraction at 78.5-85 °C , 90%, most of the higher alcohol's
            if (current_state.name == "3")
            {
                current_state = new State("4", Action.Collect, PumpInput.None, PumpOutput.None, (float)78.5, 85, -1, 4);
                return current_state;
            }

            // Fifth and final fraction at of 85-90 °C at 25-30% - highest boiling point esters and aldehydes
            if (current_state.name == "4")
            {
                current_state = new State("5", Action.Collect, PumpInput.None, PumpOutput.None, 85, 90, -1, 5);
                return current_state;
            }

            if (current_state.name == "5")
            {
                current_state = new State("drain", Action.Pump, PumpInput.Boiler, PumpOutput.Disposal, -1, -1, 2000, -1);
                return current_state;
            }

            current_state = new State("shutdown", Action.Shutdown, PumpInput.None, PumpOutput.None, -1, -1, -1, -1);
            return current_state;
        }

    }

    class Screen {
        Display_T35 display;

        string[] logs;
        const int max_log_lines = 13;
        int lineheight = 16;
        Font log_font;

        GT.Color white = GT.Color.White;
        GT.Color blue = GT.Color.Blue;

        State state;
        float temp_1 = 0;
        float temp_2 = 0;
        float temp_3 = 0;

        public Screen(Display_T35 display_T35, State state) {
            // 320 x 340 
            // ~41.5 characters wide (each char about 7.7px wide) 

            // 15 chars for state (115px)
            // 26 chars for logs (200px)


            this.display = display_T35;
            this.state = state;
            logs = new string[max_log_lines] {"", "", "", "", "", "", "", "", "", "", "", "", ""};
            log_font = Resources.GetFont(Resources.FontResources.NinaB);
            redraw();
        }

        public void log(string s) { 
            int i = max_log_lines-1;
            while (i > 0) {
                logs[i] = logs[i - 1]; 
                i--;
            }
            logs[0] = s;
            redraw();
            Debug.Print(s);
        }

        public void updateState(State state, bool redraw = true)
        {
            this.state = state;
            if (redraw)
            {
                this.redraw();
            }
        }

        public void updateTemperatures(float temp_1, float temp_2, float temp_3) {
            this.temp_1 = temp_1;
            this.temp_2 = temp_2;
            this.temp_3 = temp_3;
            redraw();
        }

        void drawCenteredState(string s, uint y, GT.Color col) {
            uint x = (uint)(115 / 2 - s.Length*7.7 / 2) - 3;
            display.SimpleGraphics.DisplayText(s, log_font, col, x, y);
        }

        public void redraw(){
            display.SimpleGraphics.Clear();

            // draw current state
            drawCenteredState("~ TEMP ~", 20, blue);
            drawCenteredState("T1: " + temp_1.ToString("f2")+" C", 40, white);
            drawCenteredState("T2: " + temp_2.ToString("f2") + " C", 60, white);
            drawCenteredState("T3: " + temp_3.ToString("f2") + " C", 80, white);

            drawCenteredState("~ STATE ~", 120, blue);
            if (state.action == Action.Init)
            {
                drawCenteredState("initializing", 140, white);
            }
            if (state.action == Action.Shutdown)
            {
                drawCenteredState("shutting down", 140, white);
            }
            if (state.action == Action.Pump) { // Pumping
                drawCenteredState("pumping", 140, white);
                drawCenteredState("from: " + state.pump_input_str(), 160, white);
                drawCenteredState("to: " + state.pump_output_str(), 180, white);
            }
            if (state.action == Action.Collect) { // collecting
                drawCenteredState("collecting", 140, white);
                drawCenteredState("from: " + state.min_temp.ToString("f2") + " C", 160, white);
                drawCenteredState("to: " + state.max_temp.ToString("f2") + " C", 180, white);
            }

            // draw divider
            display.SimpleGraphics.DisplayLine(blue, 2, 115, 0, 115, 340);

            // draw logs
            display.SimpleGraphics.DisplayText("~ LOGS ~", log_font, blue, 190, 2);
            display.SimpleGraphics.DisplayLine(blue, 1, 115, 19, 340, 19);
            int y = 1;
            foreach (string line in logs){
                display.SimpleGraphics.DisplayText(line, log_font, white, 128, (uint)(lineheight * y + 10));
                y++;
            }
        }
    }

    public partial class Program {
        void ProgramStarted()
        {
            Distillery still = new Distillery(relays, display_T35);
            still.Start();
        }
    }
}
