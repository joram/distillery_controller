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
    class TextWidget : BaseWidget {
        BaseElement base_element;
        public TextWidget(int x, int y, int width, int height, string text) : base() {
            base_element = new BaseElement();
            base_element.set_position(x, y);
            base_element.set_size(width, height);
            base_element.set_colours(BackgroundColor, BorderColor);
            base_element.set_text(text, font, BorderColor);
            base_element.ClickDown += new EventHandler(OnClickDown);
            base_element.ClickUp += new EventHandler(OnClickUp);
            elements.Add(base_element);
            this.register_elements(); 
        }

        public void update_text(string text) {
            base_element.set_text(text, font, BorderColor);
            register_elements();
        }

        protected override void OnClickDown(object sender, EventArgs e) {}
    }
}