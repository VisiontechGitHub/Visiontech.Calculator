using System;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Views.Abstraction
{
    public abstract class EntityTemplate<D> : DataTemplate where D : class
    {

        public EntityTemplate(Func<object> loadTemplate) : base(loadTemplate)
        {
        }

    }
}
