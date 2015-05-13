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
    class BlankWidget : BaseWidget {
        BaseElement background = new BaseElement();

        public BlankWidget()
            : base() {
            background.set_position(0, 28);
            background.set_size(UserInterface.Instance.screen_width, UserInterface.Instance.screen_height - 28);
            background.set_colours(BackgroundColor, BorderColor);
            elements.Add(background);
        }
    }
}