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
    class LogsWidget : BaseWidget {
        int line_height = 18;
        int max_log_lines;
        ArrayList logs = new ArrayList();
        public Font log_font = Resources.GetFont(Resources.FontResources.small);

        public LogsWidget() : base() {
            max_log_lines = (int)((UserInterface.Instance.screen_height - 30)/line_height);
            this.x = x;
            this.y = y;
            background.set_position(0, 28);
            background.set_size(UserInterface.Instance.screen_width, UserInterface.Instance.screen_height - 28);
            background.set_colours(BackgroundColor, BorderColor);
            elements.Clear();
            elements.Add(background);
        }

        public void add_log(string s) {
            Debug.Print(s);
            BaseElement element = create_line(s);
            logs.Insert(0, element);
            if (logs.Count > max_log_lines) {
                logs.RemoveAt(logs.Count-1);
            }
            update_line_positions();
        }

        BaseElement create_line(string text) {
            BaseElement element = new BaseElement();
            element.set_size(UserInterface.Instance.screen_width-8, line_height);
            element.set_colours(BackgroundColor, BackgroundColor);
            element.set_text(text, log_font, BorderColor, 1);
            return element;
        }

        void update_line_positions() {
            int line_y = 30;
            foreach (BaseElement log in logs) {
                log.set_position(5, line_y);
                line_y += line_height;
            }

            elements.Clear();
            elements.Add(background);
            foreach (BaseElement log in logs) { elements.Add(log); }
            this.register_elements();
        }
    }
}