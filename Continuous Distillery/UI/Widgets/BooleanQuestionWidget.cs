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

//namespace Continuous_Distillery {

//    public class ResponseEventArgs : EventArgs {
//        public bool answer { get; private set; }
//        public ResponseEventArgs(bool answer) {
//            this.answer = answer;
//        }
//    }

//    class BooleanQuestionWidget : BaseWidget {
//        TextButtonWidget yes_button;
//        TextButtonWidget no_button;
//        TextWidget question;
//        public event EventHandler Response;

//        public BooleanQuestionWidget(int width=175, string text="who are you?") : base(0, 0, 175, 100) {
//            int margin = 5;
//            int button_width = (width-3*margin)/2;
//            this.Width = button_width*2 + margin*3;
//            this.Height = 60 + margin;
//            this.x = (int)(UserInterface.Instance.screen_width / 2 - this.Width / 2);
//            this.y = (int)(UserInterface.Instance.screen_height / 2 - this.Height / 2);
            
//            this.question = new TextWidget(x, y, Width, 30, text);
//            this.question.BorderBrush.Opacity = 0;
            
//            this.yes_button = new TextButtonWidget(x + margin, y + 30, button_width, 30);
//            this.yes_button.Click += new EventHandler(yes_click);

//            this.no_button = new TextButtonWidget(x + margin * 2 + button_width, y + 30, button_width, 30);
//            this.no_button.Click += new EventHandler(no_click);
//        }
//        private void yes_click(object sender, EventArgs e) {
//            if (Response != null)
//                Response(this, new ResponseEventArgs(true));
//        }
//        private void no_click(object sender, EventArgs e) {
//            if (Response != null)
//                Response(this, new ResponseEventArgs(false));
//        }
//    }
//}
