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

    public delegate void OnClickFunction(object sender, EventArgs e);

    //class DropDownOptionWidget : BaseWidget {
    //    public DropDownOptionWidget(string text, OnClickFunction on_click_up, OnClickFunction on_click_down): base() {
    //        this.Ck += new EventHandler(on_click);
    //    }
    //}

    class DropDownWidget : BaseWidget {
        bool showMenu = false;
        int options_width;
        int options_height;
        BaseElement start_image;
        ArrayList options = new ArrayList();

        public DropDownWidget(int x, int y, int width, int height, Image img) : base() {
            this.x = x;
            this.y = y;
            this.options_width = width;
            this.options_height = height;

            start_image = new BaseElement();
            start_image.set_position(x, y);
            start_image.set_colours(BackgroundColor, BorderColor);
            start_image.set_image(img);
            start_image.ClickUp += new EventHandler(toggle_dropdown);
            elements.Add(start_image);
            this.register_elements();
        }

        public void add_option(Image img, string text, OnClickFunction func) {
            int option_y = y + 30 + options_height * options.Count;
            ImageTextWidget option = new ImageTextWidget(x, option_y, options_width, options_height, text);
            option.ClickUp += new EventHandler(close_dropdown);
            option.ClickUp += new EventHandler(func);
            options.Add(option);
        }

        private void open_dropdown(object sender, EventArgs e) {
            this.showMenu = true;
            foreach (ImageTextWidget option_widget in options) {
                option_widget.register_elements();
            }
        }
        public void close_dropdown(object sender, EventArgs e) {
            this.showMenu = false;
            foreach (ImageTextWidget option_widget in options) {
                option_widget.unregister_elements();
                option_widget.unpressed_colours();
            }
        }

        private void toggle_dropdown(object sender, EventArgs e) {
            if (this.showMenu) {
                close_dropdown(sender, e);
            } else {
                open_dropdown(sender, e);
            }
        }

    }
}