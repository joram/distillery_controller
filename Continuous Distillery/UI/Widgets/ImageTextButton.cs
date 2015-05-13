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
    class TextEventArgs : EventArgs {
    
    }

    class ImageTextWidget : BaseWidget {
        BaseElement image_element = new BaseElement();
        BaseElement text_element = new BaseElement();
        public string text;

        public ImageTextWidget(int x, int y, int width, int height, string text)
            : base() {

            // left hand image
            Image kisi = new Image(Resources.GetBitmap(Resources.BitmapResources.kisi));
            image_element.set_position(x, y);
            image_element.set_colours(BackgroundColor, BorderColor);
            image_element.set_image(kisi);
            elements.Add(image_element);

            // float left text
            text_element.set_position(x+28, y);
            text_element.set_size(width-28, height);
            text_element.set_colours(BackgroundColor, BorderColor);
            text_element.set_text(text, font, BorderColor);
            elements.Add(text_element);

            EventHandler event_handler = new EventHandler(OnClickDown);
            image_element.ClickDown += event_handler;
            text_element.ClickDown += event_handler;

            EventHandler up_event_handler = new EventHandler(OnClickUp);
            image_element.ClickUp += up_event_handler;
            text_element.ClickUp += up_event_handler;

            this.text = text;
            this.register_elements();
        }

        //public void add_on_click_down_handler(OnClickFunction func) {
        //    EventHandler event_handler = new EventHandler(func);
        //    image_element.ClickDown += event_handler;
        //    text_element.ClickDown += event_handler;
        //}

        //public void add_on_click_up_handler(OnClickFunction func) {
        //    EventHandler event_handler = new EventHandler(func);
        //    image_element.ClickUp += event_handler;
        //    text_element.ClickUp += event_handler;
        //}    
    }
}