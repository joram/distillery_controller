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

namespace Continuous_Distillery {

    class BaseElement : Border {
        public event EventHandler ClickDown;
        public event EventHandler ClickUp;

        protected override void OnTouchDown(Microsoft.SPOT.Input.TouchEventArgs e) {
            if (ClickDown != null) {
                ClickDown(this, e);
            }
        }

        protected override void OnTouchUp(Microsoft.SPOT.Input.TouchEventArgs e) {
            if (ClickUp != null) {
                ClickUp(this, e);
            }
        }

        public void set_position(int x, int y) {
            Canvas.SetTop(this, y);
            Canvas.SetLeft(this, x);
        }

        public void set_size(int width, int height) {
            this.Width = width;
            this.Height = height;
        }

        public void set_colours(Color background, Color boarder) {
            this.Background = new SolidColorBrush(background);
            this.BorderBrush = new SolidColorBrush(boarder);
            if(this.Child != null && this.Child is Text){
                ((Text)this.Child).ForeColor = boarder;
            }
            
        }

        public void set_text(string text, Font font, Color font_colour, int margin=5) {
            Text text_element = new Text(font, text);
            text_element.Width = Width;
            text_element.ForeColor = font_colour;
            text_element.SetMargin(margin);
            this.Child = text_element;
        }

        public void set_image(Image img) {
            this.Child = img;
        }
    }
}