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

namespace Continuous_Distillery {
    class WifiWidget : BaseWidget {
        WiFi_RS21 wifi_RS21;
        ArrayList hotspots = new ArrayList();

        public WifiWidget(WiFi_RS21 wifi_RS21) : base() {
            this.font = Resources.GetFont(Resources.FontResources.small);
            this.x = x;
            this.y = y;
            background.set_position(0, 28);
            background.set_size(UserInterface.Instance.screen_width, UserInterface.Instance.screen_height - 28);
            background.set_colours(BackgroundColor, BorderColor);
            elements.Clear();
            elements.Add(background);

            
           // NetworkInterfaceExtension.AssignNetworkingStackTo((NetworkInterfaceExtension)wifi_RS21);
            this.wifi_RS21 = wifi_RS21;
            this.wifi_RS21.UseThisNetworkInterface();
            this.wifi_RS21.Interface.NetworkInterface.EnableDhcp();
            this.wifi_RS21.UseDHCP();
            this.wifi_RS21.NetworkDown += new GT.Modules.Module.NetworkModule.NetworkEventHandler(wifi_NetworkDown);
            this.wifi_RS21.NetworkUp += new GT.Modules.Module.NetworkModule.NetworkEventHandler(wifi_NetworkUp);
            
            // connect to known wifi
            GT.Timer t = new GT.Timer(1000);
            t.Tick += new GT.Timer.TickEventHandler(t_Tick);
            t.Start();
            
        }

        public void t_Tick(GT.Timer timer) {
            timer.Stop();

            update_hotspots();
            connect_to_hotspot("oram_2.4Ghz", "theAardvarkDidIt");
            get("http://oram.ca", req_ResponseReceived);
        }

        public delegate void ResponseHander(HttpRequest sender, HttpResponse response);
        public void get(string url, ResponseHander func) {
            var request = HttpHelper.CreateHttpGetRequest(url);
            request.ResponseReceived += new HttpRequest.ResponseHandler(func);
            request.SendRequest();
        }

        void req_ResponseReceived(HttpRequest sender, HttpResponse response) {
            UserInterface.Instance.log(response.Text);
        }

        BaseElement create_line(string text) {
            BaseElement element = new BaseElement();
            element.set_size(UserInterface.Instance.screen_width - 8, 18);
            element.set_colours(BackgroundColor, BackgroundColor);
            element.set_text(text, font, BorderColor, 1);
            return element;
        }
        
        public void update_hotspots() {
            hotspots.Clear();
            GHI.Premium.Net.WiFiNetworkInfo[] scanResults = wifi_RS21.Interface.Scan();
            foreach (GHI.Premium.Net.WiFiNetworkInfo result in scanResults) {
                BaseElement element = create_line("SSID: " + result.SSID);
                hotspots.Add(element);
            }

            int line_y = 30;
            elements.Clear();
            elements.Add(background);
            foreach (BaseElement hotspot in hotspots) {
                hotspot.set_position(5, line_y);
                elements.Add(hotspot);
                line_y += 18;
            } 
            register_elements();
            
        }

        // handle the network changed events
        void wifi_NetworkDown(GT.Modules.Module.NetworkModule sender, GT.Modules.Module.NetworkModule.NetworkState state) {
            if (state == GT.Modules.Module.NetworkModule.NetworkState.Down)
                UserInterface.Instance.log("Network Down event; state = Down");
            else
                UserInterface.Instance.log("Network Down event; state = Up");
        }

        void wifi_NetworkUp(GT.Modules.Module.NetworkModule sender, GT.Modules.Module.NetworkModule.NetworkState state) {
            if (state == GT.Modules.Module.NetworkModule.NetworkState.Up) {
                UserInterface.Instance.log("Network Up event; state = Up");
                Debug.Print("IP Address: " + wifi_RS21.NetworkSettings.IPAddress);
            } else
                UserInterface.Instance.log("Network Up event; state = Down");
        }

        public void connect_to_hotspot(string ssid, string password) {
            // locate a specific network
            GHI.Premium.Net.WiFiNetworkInfo info = wifi_RS21.Interface.Scan(ssid)[0];
            if (info != null) {
                wifi_RS21.Interface.Join(info, password);
                UserInterface.Instance.log("connected to: " + ssid);
            } else {
                UserInterface.Instance.log("unable to connect to: " + ssid);
            }
        }
    }
}