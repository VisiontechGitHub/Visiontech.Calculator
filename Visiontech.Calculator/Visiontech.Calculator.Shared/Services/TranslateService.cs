using CalcolatoreXamarin.Shared.Extensions;
using CalcolatoreXamarin.Shared.Services;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Shared.Services
{
    public class TranslateService : ITranslateService
    {

        const string ResourceId = "Visiontech.Calculator.Shared.Properties.Resources";

        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));
        public string Translate(string Label)
        {
            if (Label == null)
            {
                return "";
            }

            var translation = resmgr.Value.GetString(Label, CrossMultilingual.Current.DeviceCultureInfo);

            if (translation == null)
            {
                translation = resmgr.Value.GetString(Label, CrossMultilingual.Current.NeutralCultureInfoList.First());
            }

            if (translation == null)
            {
                translation = Label;
            }

            return translation;
        }
    }

}
