using System;
using System.Collections;

namespace DistilleryNamespace
{
    public enum Action { Init, Pump, Collect, Shutdown };

    public class State
    {
        public Action action { get; set; }
        
        // Pump
        public string dest { get; set; }
        public string src { get; set; }
        public int time { get; set; }

        // Collect
        public long min_temp { get; set; }
        public long max_temp { get; set; }
        public int output_id { get; set; }

        public State() {
            action = Action.Init;
        }

        public State(Hashtable data) {
            string action_str = (string)data["action"];
            if (action_str == "shutdown")
            {
                action = Action.Shutdown;
                return;
            }
            if (action_str == "pump")
            {
                action = Action.Pump;
                src = (string)data["src"];
                dest = (string)data["dest"];
                time = (int)((long)data["time"]);
                return;
            }
            if (action_str == "collect")
            {
                action = Action.Collect;
                min_temp = (long)data["min_temp"];
                max_temp = (long)data["max_temp"];
                output_id = (int)((long)data["output_id"]);
                return;
            }
            action = Action.Shutdown;
        }

        public override string ToString()
        {
            if(action == Action.Pump)
                return "Pump "+src+"-"+dest+" for "+time;
            if (action == Action.Collect)
                return "Coll. "+min_temp+"-"+max_temp+" in "+output_id;
            return "??action??";
        }
    }
}
