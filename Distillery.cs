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
using Gadgeteer.Modules.Seeed;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GHI.Premium.Net;

namespace DistilleryNamespace
{

    class Distillery
    {
        Thread controllerThread;
        Thread sensorThread;
        State current_state;
//        Relays relays;
        Screen screen;
        Network network;
        Pump pump;

        public Distillery(Display_T35 display, Ethernet_J11D ethernet_J11D, Relays relays)
        {
            Debug.Print("initializing now...");
            //this.relays = relays;
            this.current_state = new State();
            this.screen = new Screen(display, current_state);
            this.network = new Network(ethernet_J11D, screen);
            this.pump = new Pump(relays);
            screen.log("initialized components");
            //this.network.record_sensor_info(1, 2, 666);
        }

        public void Start()
        {
            controllerThread = new Thread(ControllerThreadFunc);
            sensorThread = new Thread(SensorThreadFunc);
            controllerThread.Start();
        }

        void updateState() {
            State state = network.get_state("http://summer:8000/api/debug/fill/");
            screen.log(""+state);

        }

        void ControllerThreadFunc()
        {
            updateState();
            while (current_state == null || current_state.action != Action.Shutdown)
            {
                //getCurrentState();
                //screen.updateState(current_state, false);
                //screen.log(current_state.str());
                Thread.Sleep(5000);
            }
        }

        // TODO
        void SensorThreadFunc()
        {
            while (current_state == null || current_state.action != Action.Shutdown)
            {
                Thread.Sleep(3000);
            }
        }
    }
}