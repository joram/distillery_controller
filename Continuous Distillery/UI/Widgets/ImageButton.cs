using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Hardware;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GHI.Premium.Net;

namespace Continuous_Distillery {
    class ImageWidget : BaseWidget {
        public ImageWidget(int x, int y, int width, int height)
            : base() {
            Image kisi = new Image(Resources.GetBitmap(Resources.BitmapResources.kisi));

            BaseElement element = new BaseElement();
            element.set_position(x, y);
            element.set_colours(BackgroundColor, BorderColor);
            element.set_image(kisi);
            element.ClickDown += new EventHandler(OnClickDown);
            element.ClickUp += new EventHandler(OnClickUp);
            elements.Add(element);
            this.register_elements();
        }
    }
}