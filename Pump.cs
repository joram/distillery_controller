using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;
using System.Threading;
using Microsoft.SPOT;

namespace Distillery {
    enum PumpInput { None, Wash, Boiler };
    enum PumpOutput { None, Boiler, Disposal };
    class Pump
    {
        Relays relays;

        public Pump(Relays relays)
        {
            this.relays = relays;
        }

        public bool pump(PumpInput source, PumpOutput dest, int duration_ms) {
            pumpOff();
            bool input_result = setPumpInput(source);
            bool output_result = setPumpOutput(dest);
            bool success = (input_result || output_result);
            Debug.Print("Pumping from " + source.ToString() + " to " + dest.ToString());

            if (success && (duration_ms > 0))
            {
                pumpOn();
                Thread.Sleep(duration_ms);
                pumpOff();
            }
            return success;        
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

        //// for testing purposes  
        //// TODO: should be re-written to fetch a state from the REST API
        //int run_count = 0;
        //State getCurrentState()
        //{
        //    if (current_state.name == "init" || (run_count <= 1 && current_state.name == "drain"))
        //    {
        //        run_count++;
        //        current_state = new State("fill", Action.Pump, PumpInput.Wash, PumpOutput.Boiler, -1, -1, 2000, -1);
        //        return current_state;
        //    }

        //    // First fraction - collect between 69-72 °C at 91% - approx. 5% total distillate, unpleasant aldehydes, organic acids and esters. Discard.
        //    if (current_state.name == "fill")
        //    {
        //        current_state = new State("1", Action.Collect, PumpInput.None, PumpOutput.None, 50, 72, -1, 1);
        //        return current_state;
        //    }

        //    // Second fraction, 72-77 °C at 93-94% - 10% total distillate, contains ethanol with appreciable amounts of aldehydes and esters.
        //    if (current_state.name == "1")
        //    {
        //        current_state = new State("2", Action.Collect, PumpInput.None, PumpOutput.None, 72, 77, -1, 2);
        //        return current_state;
        //    }

        //    // Third fraction, at 78 °C, 95.5%, largest in volume at 55-60% ,mostly ethanol with very small amounts of congeners
        //    if (current_state.name == "2")
        //    {
        //        current_state = new State("3", Action.Collect, PumpInput.None, PumpOutput.None, 77, (float)78.5, -1, 3);
        //        return current_state;
        //    }

        //    // Fourth fraction at 78.5-85 °C , 90%, most of the higher alcohol's
        //    if (current_state.name == "3")
        //    {
        //        current_state = new State("4", Action.Collect, PumpInput.None, PumpOutput.None, (float)78.5, 85, -1, 4);
        //        return current_state;
        //    }

        //    // Fifth and final fraction at of 85-90 °C at 25-30% - highest boiling point esters and aldehydes
        //    if (current_state.name == "4")
        //    {
        //        current_state = new State("5", Action.Collect, PumpInput.None, PumpOutput.None, 85, 90, -1, 5);
        //        return current_state;
        //    }

        //    if (current_state.name == "5")
        //    {
        //        current_state = new State("drain", Action.Pump, PumpInput.Boiler, PumpOutput.Disposal, -1, -1, 2000, -1);
        //        return current_state;
        //    }

        //    current_state = new State("shutdown", Action.Shutdown, PumpInput.None, PumpOutput.None, -1, -1, -1, -1);
        //    return current_state;
        //}
    }
}