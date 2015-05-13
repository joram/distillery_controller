using System;
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
    public partial class Program : GT.Program {
        void ProgramStarted() {
            Distillery.Instance.init(display_T35, ethernet_J11D, wifi_RS21);
            Distillery.Instance.Start();
        }



    }
}
