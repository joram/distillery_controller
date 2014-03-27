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
using Json.NETMF;

namespace DistilleryNamespace
{


    class Network
    {
        Ethernet_J11D ethernet_J11D;
        Screen screen;
        string base_server_url = "http://192.168.1.4:8000";
        bool GET_response_reveived = false;
        string get_response = "";


        public Network(Ethernet_J11D ethernet_J11D, Screen screen)
        {
            this.screen = screen;
            this.ethernet_J11D = ethernet_J11D;

            this.ethernet_J11D.Interface.Open();
            this.ethernet_J11D.Interface.NetworkInterface.EnableDhcp();
            this.ethernet_J11D.UseDHCP();

            while (!ethernet_J11D.IsNetworkUp || IPAddress() == "0.0.0.0")
            {
                Thread.Sleep(200);
            }
            this.ethernet_J11D.UseThisNetworkInterface(); 
            screen.updateNetwork(IPAddress());
            screen.log("internet connected");
        }

        string IPAddress() {
            return ethernet_J11D.Interface.NetworkInterface.IPAddress;
        }

        public void record_sensor_info(int still_id, int sensor_id, double value)
        {
            string url = base_server_url + "/api/still/" + still_id + "/sensor/" + sensor_id;
            string data = "" + value;
            screen.log(url + " " + data);
            post_data(url, data);
        }

        void post_data(string url, string data)
        {
            var post_content = Gadgeteer.Networking.POSTContent.CreateTextBasedContent(data);
            var req = HttpHelper.CreateHttpPostRequest(url, post_content, null);
            req.ResponseReceived += new HttpRequest.ResponseHandler(post_req_ResponseReceived);
            req.SendRequest();
        }

        void post_req_ResponseReceived(HttpRequest sender, HttpResponse response)
        {
            if (response.StatusCode != "200")
                screen.log("rsp code: " + response.StatusCode);
        }

        public void get_req_ResponseReceived(HttpRequest sender, HttpResponse response)
        {
            if (response.StatusCode == "200")
            {
                this.get_response = response.Text;
            }
            else
            {
                screen.log(response.StatusCode);
            }
            GET_response_reveived = true;
        }
        
        public string get_data(string url)
        {
            this.get_response = "";
            var request = HttpHelper.CreateHttpGetRequest(url);
            request.ResponseReceived += new HttpRequest.ResponseHandler(get_req_ResponseReceived);

            GET_response_reveived = false;
            request.SendRequest();
            while (!GET_response_reveived)
            {
                Thread.Sleep(200);
            }
            return this.get_response;
        }        

        public Hashtable get_json(string url)
        {
            return JsonSerializer.DeserializeString(get_data(url)) as Hashtable;
        }

        public State get_state(string url) {
            return new State(get_json(url));
        }

    }
}