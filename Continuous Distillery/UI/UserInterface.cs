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

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GHI.Premium.Net;

namespace Continuous_Distillery
{

    class UserInterface : Canvas
    {
        public int screen_width = 320;
        public int screen_height = 240;
        Display_T35 display;
        Window window;
        TaskbarWidget taskbar;
        Hashtable views = new Hashtable();
        BaseWidget current_view = null;
        LogsWidget logs_widget;

        private static UserInterface instance;
        private UserInterface() {
   
        }

        public static UserInterface Instance {
            get {
                if (instance == null) {
                    instance = new UserInterface();
                }
                return instance;
            }
        }

        public void view_clicked(object sender, EventArgs e) {
            string view_name = ((ImageTextWidget)sender).text;
            set_current_view(view_name);
        }

        public bool set_current_view(string view_name) {
            if (this.current_view != null) {
                this.current_view.unregister_elements();
                this.current_view = null;
            }

            if (views.Contains(view_name)) {
                current_view = (BaseWidget)views[view_name];
                current_view.register_elements();
                taskbar.update_title("Distillery - " + view_name);
                return true;
            }
            return false;
        }

        public void add_view(Image button_image, string button_text, BaseWidget view_widget, bool default_view=false) {
            taskbar.add_dropdown_option(button_image, button_text, view_clicked);
            views.Add(button_text, view_widget);
            if (default_view) {
                set_current_view(button_text);
            }
            if (view_widget.GetType() == typeof(LogsWidget)) {
                logs_widget = (LogsWidget)view_widget;
            }
        }

        public void init(Display_T35 display_T35) {
            this.display = display_T35;
            this.window = display.WPFWindow;
            this.window.Child = this;
            this.taskbar = new TaskbarWidget();
        }

        public void log(string s) {
            if (logs_widget != null) {
                logs_widget.add_log(s);
            }
        }
    }
}