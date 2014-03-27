using System;
using System.Collections;

namespace Distillery {
    //enum Action { Init, Pump, Collect, Shutdown };

//    [Serializable] 
    public class State
    {
        public string action { get; set; }
        public string dest { get; set; }
        public string src { get; set; }
        public int time { get; set; }
        public int min_temp { get; set; }
        public int max_temp { get; set; }

        public override string ToString()
        {
            return "src:"+src+", dest:"+dest+" for:"+time;
        }
    }

    //class DebugState
    //{

    //    string src="";
    //    string dest="";
    //    int time=0;

    //    string getSrc() { return src; }
    //    string getDest() { return dest; }        
    //    int getTime() { return time; }

    //    public DebugState() { }

    //    public string ToString() {
    //        return src + " to " + dest + " for " + time + "ms";
    //    }

    //    public bool fromString(string s) {
    //        string[] lines = s.Split(new Char[] { '\n' });
    //        foreach (string line in lines)
    //        {
    //            string[] parts = line.Split(new Char[] { '=' });
    //            if (parts.Length != 2)
    //            {
    //                return false;
    //            }
    //            string key = parts[0];
    //            string value = parts[1];
    //            switch (key) { 
    //                case "src":
    //                    src = value;
    //                    break;
                    
    //                case "dest":
    //                    dest = value;
    //                    break;

    //                case "time":
    //                    time = Convert.ToInt32(value);
    //                    break;
    //            }
    //        }
    //        return true;
    //    }
    //}

    //class State
    //{
    //    public string name;
    //    public Action action;
    //    public PumpInput pump_input;
    //    public PumpOutput pump_output;
    //    public float pump_duration;
    //    public float min_temp;
    //    public float max_temp;
    //    public int output_id;

    //    public State(string name, Action action, PumpInput pump_input = PumpInput.None, PumpOutput pump_output = PumpOutput.None, float min_temp = -1, float max_temp = -1, float pump_duration = -1, int output_id = -1)
    //    {
    //        this.name = name;
    //        this.action = action;
    //        this.pump_input = pump_input;
    //        this.pump_output = pump_output;
    //        this.pump_duration = pump_duration;
    //        this.min_temp = min_temp;
    //        this.max_temp = max_temp;
    //        this.output_id = output_id;
    //    }

    //    public string pump_input_str()
    //    {
    //        return (new string[3] { "None", "Wash", "Boiler" })[(int)pump_input];
    //    }

    //    public string pump_output_str()
    //    {
    //        return (new string[3] { "None", "Boiler", "Disposal" })[(int)pump_output];
    //    }

    //    public string str()
    //    {

    //        // Describe a pumping action
    //        if (action == Action.Pump && (pump_input != PumpInput.None) && (pump_output != PumpOutput.None) && (pump_duration > 0))
    //        {
    //            string src = pump_input_str();
    //            string dest = pump_output_str();
    //            return "Pumping " + src + " to " + dest + " for " + pump_duration + "s";
    //        }

    //        // Describe collecting distilate
    //        if (action == Action.Collect && min_temp > 0 && max_temp > 0 && min_temp < max_temp && output_id > 0)
    //        {
    //            return min_temp + "C to " + max_temp + "C, into " + output_id;
    //        }

    //        // Describe shutting down
    //        if (action == Action.Shutdown)
    //        {
    //            return "Shutting down";
    //        }

    //        return "unknown state!";
    //    }
    //}
}
