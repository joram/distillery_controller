using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Gadgeteer.Modules.Seeed;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace Continuous_Distillery {
    
    class Distillery {
        Image option_image = new Image(Resources.GetBitmap(Resources.BitmapResources.kisi));

        Display_T35 display_T35;
        Ethernet_J11D ethernet_J11D;
        WiFi_RS21 wifi_RS21;
        //BooleanQuestionWidget question;
        LogsWidget logs_widget;
        WifiWidget wifi_widget;

        private static Distillery instance;
        private Distillery() { }
        public static Distillery Instance {
            get {
                if (instance == null) {
                    instance = new Distillery();
                }
                return instance;
            }
        }


        public void init(Display_T35 display, Ethernet_J11D ethernet_J11D, WiFi_RS21 wifi_RS21) {
            this.display_T35 = display;
            this.ethernet_J11D = ethernet_J11D;
            this.wifi_RS21 = wifi_RS21;
        }

        public void Start() {
            UserInterface.Instance.init(display_T35);
            this.logs_widget = new LogsWidget();
            this.wifi_widget = new WifiWidget(wifi_RS21);

            UserInterface.Instance.add_view(option_image, "logs", logs_widget, true);
            UserInterface.Instance.add_view(option_image, "wifi", wifi_widget);
        }

        //public void option_1(object sender, EventArgs e) {
        //    Debug.Print("option selected");
        //    question = new BooleanQuestionWidget(text: "ethernet plugged in?");
        //    question.Response += new EventHandler(StartAnswer);
        //}

        //void StartAnswer(object sender, EventArgs e) {
        //    ResponseEventArgs bool_e = (ResponseEventArgs)e;
        //    Debug.Print("answered "+ bool_e.answer);
        //    if (bool_e.answer) {

        //        //Network.Instance.init(ethernet_J11D);
        //        question = new BooleanQuestionWidget(text: "fill the boiler?");
        //        question.Response += new EventHandler(FillBoilerAnswer);

        //    } else {
        //        Debug.Print("not starting");
        //    }
        //}

        //void FillBoilerAnswer(object sender, EventArgs e) {
        //    ResponseEventArgs bool_e = (ResponseEventArgs)e;
        //    if (bool_e.answer) {
        //        Debug.Print("filling boiler 5s");
        //        Thread.Sleep(5000);
        //    }
        //    question = new BooleanQuestionWidget(text: "start boiling?");
        //    question.Response += new EventHandler(StartBoilingAnswer);

        //}

        //void StartBoilingAnswer(object sender, EventArgs e) {
        //    ResponseEventArgs bool_e = (ResponseEventArgs)e;
        //    if (bool_e.answer) {
        //        Debug.Print("boiling 5s");
        //        Thread.Sleep(5000);
        //    }
        //}
    }
}
