using CalcolatoreXamarin.Views.Abstraction;
using Org.Visiontech.Credential;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Views.Abstraction
{
    public class IdentifiableTemplate : EntityTemplate<object>
    {

        public IdentifiableTemplate() 
            : base(() =>
                {
                    Label label = new Label()
                    {
                    };
                    label.SetBinding(Label.TextProperty, "Type");

                    return new ViewCell()
                    {
                        View = new Frame()
                        {
                            Content = label,
                            BackgroundColor = Color.Transparent,
                            BorderColor = Color.Transparent,
                            HasShadow = false
                        }
                    };
                }
            )
        {

        }
        
    }
}
