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
using GHI.Premium.Net;

namespace DistilleryNamespace
{


    class Screen
    {
        Display_T35 display;
        GT.Color white = GT.Color.White;
        GT.Color blue = GT.Color.Blue;
        GT.Color red = GT.Color.Red;

        string[] logs;
        const int max_log_lines = 13;
        int lineheight = 16;
        Font log_font;

        string ip_address = "disconnected";

        State state;
        float temp_1 = 0;
        float temp_2 = 0;
        float temp_3 = 0;

        public Screen(Display_T35 display_T35, State state)
        {
            // 320 x 340 
            // ~41.5 characters wide (each char about 7.7px wide) 

            // 15 chars for state (115px)
            // 26 chars for logs (200px)


            this.display = display_T35;
            this.state = state;
            logs = new string[max_log_lines] { "", "", "", "", "", "", "", "", "", "", "", "", "" };
            log_font = Resources.GetFont(Resources.FontResources.NinaB);
            redraw();
        }

        public void log(string s)
        {
            int i = max_log_lines - 1;
            while (i > 0)
            {
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

        public void updateNetwork(string ip_address)
        {
            this.ip_address = ip_address;
            this.redraw();
        }

        public void updateTemperatures(float temp_1, float temp_2, float temp_3)
        {
            this.temp_1 = temp_1;
            this.temp_2 = temp_2;
            this.temp_3 = temp_3;
            redraw();
        }

        void drawCenteredState(string s, uint y, GT.Color col)
        {
            uint x = (uint)(115 / 2 - s.Length * 7.7 / 2) - 3;
            display.SimpleGraphics.DisplayText(s, log_font, col, x, y);
        }

        public void redraw()
        {
            display.SimpleGraphics.Clear();

            // draw current state
            drawCenteredState("~ TEMP ~", 20, blue);
            drawCenteredState("T1: " + temp_1.ToString("f2") + " C", 40, white);
            drawCenteredState("T2: " + temp_2.ToString("f2") + " C", 60, white);
            drawCenteredState("T3: " + temp_3.ToString("f2") + " C", 80, white);

            drawCenteredState("~ STATE ~", 100, blue);
            if (state.action == Action.Init)
            {
                drawCenteredState("initializing", 120, white);
            }
            if (state.action == Action.Shutdown)
            {
                drawCenteredState("shutting down", 120, white);
            }
            if (state.action == Action.Pump)
            { // Pumping
                drawCenteredState("pumping", 120, white);
                drawCenteredState("from: " + state.src, 140, white);
                drawCenteredState("to: " + state.dest, 160, white);
            }
            if (state.action == Action.Collect)
            { // collecting
                drawCenteredState("collecting", 120, white);
                drawCenteredState("from: " + state.min_temp.ToString("f2") + " C", 140, white);
                drawCenteredState("to: " + state.max_temp.ToString("f2") + " C", 160, white);
            }

            // draw network
            drawCenteredState("~ NETWORK ~", 180, blue);
            if (ip_address == "disconnected")
            {
                drawCenteredState(ip_address, 200, red);
            }
            else
            {
                drawCenteredState(ip_address, 200, white);
            }


            // draw divider
            display.SimpleGraphics.DisplayLine(blue, 2, 115, 0, 115, 340);

            // draw logs
            display.SimpleGraphics.DisplayText("~ LOGS ~", log_font, blue, 190, 2);
            display.SimpleGraphics.DisplayLine(blue, 1, 115, 19, 340, 19);
            int y = 1;
            foreach (string line in logs)
            {
                display.SimpleGraphics.DisplayText(line, log_font, white, 128, (uint)(lineheight * y + 10));
                y++;
            }
        }
    }
}