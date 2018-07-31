using CalcolatoreXamarin.Shared.Services;
using System;
using VisiontechCommons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalcolatoreXamarin.Shared.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension

    {

        protected readonly ITranslateService translateService = Container.ServiceProvider.GetService(typeof(ITranslateService)) as ITranslateService;

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return translateService.Translate(Text);
        }

    }
}
