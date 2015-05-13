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
    class TaskbarWidget : BaseWidget {
        DropDownWidget dropdown;
        TextWidget title_text;

        public TaskbarWidget() : base() {
            Image kisi = new Image(Resources.GetBitmap(Resources.BitmapResources.gear));
            title_text = new TextWidget(28, 0, UserInterface.Instance.screen_width - 28, 30, "Distillery");
            dropdown = new DropDownWidget(0, 0, 100, 30, kisi);
        }

        public void update_title(string title) {
            title_text.update_text(title);
        }

        public void add_dropdown_option(Image img, string text, OnClickFunction func) {
            dropdown.add_option(img, text, func);
            dropdown.close_dropdown(this, EventArgs.Empty);
        }

        public void set_start_menu(BaseWidget start_menu) {
        }
    }
}