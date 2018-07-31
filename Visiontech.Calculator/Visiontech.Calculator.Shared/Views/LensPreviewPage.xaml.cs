using CalcolatoreXamarin.Shared.ViewModels;
using CalcolatoreXamarin.Shared.Views;
using CalcolatoreXamarin.Shared.Views.Abstraction;
using Org.Visiontech.Compute;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalcolatoreXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LensPreviewPage : LoadingContentPage<LensPreviewModel>
    {
        public LensPreviewPage ()
		{
			InitializeComponent ();
            model.ResponseComputed += ResponseComputed;
        }

        private void ResponseComputed(object sender, Tuple<computeLensRequestDTO, computeLensResponseDTO> tuple)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new AnalizedLensPage(tuple.Item1, tuple.Item2));
            });
        }

    }
}