using System;
using System.Collections;

using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Networking;
using NetworkModule = Gadgeteer.Modules.Module.NetworkModule;
using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT.Net.NetworkInformation;


namespace GadgeteerApp3 {

    public partial class Program {
        Window mainWindow;
        Canvas canvas = new Canvas();
        Text txtMsg;
        Font baseFont;
        int count = 0;

        private GT.Timer _pollingTimer;
        private  GT.Interfaces.AnalogInput thermometer_1;
        private static double VOLTAGE_ANALOG_RESOLUTION = 0.1857283716035809d;
        private static double VOLTAGE_ANALOG_ZERO_MEASURE = 0.06d;

        private double estimated_voltage = 0d;


        bool current_network_connected = false;

        void SetupDisplay() {
            baseFont = Resources.GetFont(Resources.FontResources.NinaB);
            mainWindow = display_T35.WPFWindow; 
            mainWindow.Child = canvas;
            txtMsg = new Text(baseFont, "Starting Distillery");
            canvas.SetMargin(5);
            canvas.Children.Add(txtMsg);
        }

        void post_data(string url, string data) {
            var post_content = Gadgeteer.Networking.POSTContent.CreateTextBasedContent(data);
            var req = HttpHelper.CreateHttpPostRequest(url, post_content, null);
            req.ResponseReceived += new HttpRequest.ResponseHandler(req_ResponseReceived);
            req.SendRequest();

        }

        void record_sensor_info(int still_id, int sensor_id, double value) {
            Mainboard.SetDebugLED(true);
            string url = "http://oram.servebeer.com:5000/still/" + still_id + "/sensor/" + sensor_id;
            string data = "" + value;
            Debug.Print("recording sensor data: " + url + " " + data);
            post_data(url, data);
        }

        void req_ResponseReceived(HttpRequest sender, HttpResponse response) {
            if (response.StatusCode != "200")
                Debug.Print(response.StatusCode);

            Mainboard.SetDebugLED(false);
        }

        void ethernet_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state) {
            Debug.Print("network up");
        }

        void ethernet_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state) {
            Debug.Print("network down");
        }

        void SetupEthernet() {
            ethernet_J11D.UseDHCP();
            ethernet_J11D.NetworkUp += ethernet_NetworkUp;
            ethernet_J11D.NetworkDown += ethernet_NetworkDown;
        }

        void log(string s) {
            Debug.Print(s);
        }


        void SetupSensors(){
            GT.Socket socket = Gadgeteer.Socket.GetSocket(9, true, null, null);
            thermometer_1 = new GT.Interfaces.AnalogInput(socket, GT.Socket.Pin.Four, null);

            _pollingTimer = new GT.Timer(1000);
            _pollingTimer.Tick += new GT.Timer.TickEventHandler(_pollingTimer_Tick);

            _pollingTimer.Start();
        }


        void _pollingTimer_Tick(GT.Timer timer) {
            double readVoltage = thermometer_1.ReadVoltage();
            double actualVoltage = ((readVoltage - VOLTAGE_ANALOG_ZERO_MEASURE) / VOLTAGE_ANALOG_RESOLUTION);

            double weighted_voltage = estimated_voltage * 0.8d + actualVoltage * 0.2d;
            Debug.Print(weighted_voltage.ToString() +", ("+ readVoltage+"-"+VOLTAGE_ANALOG_ZERO_MEASURE+")/"+VOLTAGE_ANALOG_RESOLUTION);
            estimated_voltage = weighted_voltage;
        }


        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted() {
            log("Distillery Starting");
            //SetupDisplay();
            //SetupEthernet();
            SetupSensors();

            log("Distillery Started");
            startLooping(3000);
        }

        void startLooping(int ms) {
            GT.Timer timer = new GT.Timer(ms);
            timer.Tick += new GT.Timer.TickEventHandler(tick);
            timer.Start();
        }
        
        void tick(GT.Timer timer) {
            Debug.Print("tick ");
        }

        void old_tick(GT.Timer timer) {
            string ip_address = "" + ethernet_J11D.NetworkSettings.IPAddress;

            Debug.Print(ip_address);
            if (count == 0 && ip_address != "0.0.0.0") {
                ListNetworkInterfaces();
            }

            if (ip_address != "0.0.0.0") {
                record_sensor_info(1, 1, 3.1415987 + count);
                count += 1;
            }

        }

        void ListNetworkInterfaces() {
            var settings = ethernet_J11D.NetworkSettings;
            Debug.Print("------------------------------------------------");
            //Debug.Print("MAC: " + ByteExtensions.ToHexString(settings.PhysicalAddress, "-"));
            Debug.Print("IP Address:   " + settings.IPAddress);
            Debug.Print("DHCP Enabled: " + settings.IsDhcpEnabled);
            Debug.Print("Subnet Mask:  " + settings.SubnetMask);
            Debug.Print("Gateway:      " + settings.GatewayAddress);
            Debug.Print("------------------------------------------------");
        }
    }
}
