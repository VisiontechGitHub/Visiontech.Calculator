using CalcolatoreXamarin.Shared.ViewModels;
using CalcolatoreXamarin.Shared.Views.Abstraction;
using Xamarin.Forms.Xaml;

namespace CalcolatoreXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : LoadingContentPage<LoginModel>
    {
        public LoginPage ()
		{
			InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.Reload();
            if (model.CanLogin)
            {
                model.LoginCommand.Execute(this);
            }
        }

    }
}