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

    abstract class BaseWidget {
        public int x = 0;
        public int y = 0;
        public Font font = Resources.GetFont(Resources.FontResources.NinaB);
        public GT.Color BorderColor = GT.Color.Green;
        public GT.Color BackgroundColor = GT.Color.Black;
        public GT.Color PressedBorderColor = GT.Color.Blue;
        public GT.Color PressedBackgroundColor = GT.Color.DarkGray;
        public ArrayList elements = new ArrayList();
        public ArrayList registered_elements = new ArrayList();
        public event EventHandler ClickDown;
        public event EventHandler ClickUp;
        public BaseElement background = new BaseElement();

        public void pressed_colours() {
            foreach (BaseElement element in elements) {
                element.set_colours(PressedBackgroundColor, PressedBorderColor);
            }        
        }

        public void unpressed_colours() {
            foreach (BaseElement element in elements) {
                element.set_colours(BackgroundColor, BorderColor);
            }
        }

        protected virtual void OnClickDown(object sender, EventArgs e) {
            pressed_colours();
            if (ClickDown != null) {
                ClickDown(this, e);
            }
            
        }
        protected virtual void OnClickUp(object sender, EventArgs e) {
            if (ClickUp != null) {
                ClickUp(this, e);
            }
        }

        private void EndAsyncEvent(IAsyncResult iar) {
        }

        public void register_elements() {
            unregister_elements();
            foreach (BaseElement element in elements) {
                UserInterface.Instance.Children.Add(element);
                registered_elements.Add(element);
            }
        }

        public void unregister_elements() {
            foreach (BaseElement element in registered_elements) {
                UserInterface.Instance.Children.Remove(element);
            }
            registered_elements.Clear();
        }

    }
}