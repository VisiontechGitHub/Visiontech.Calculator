using CalcolatoreXamarin.ViewModels;
using CalcolatoreXamarin.Views;
using System;
using System.Net;
using Visiontech.Services.Utils;
using VisiontechCommons;
using Xamarin.Essentials;
using Xamarin.Forms;
using static CalcolatoreXamarin.Shared.ViewModels.MenuModel;

namespace CalcolatoreXamarin.Shared.ViewModels
{
    public class MainModel : BaseViewModel
    {
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;

        public event EventHandler<Page> DetailChanging;
        public MainModel()
        {

            MessagingCenter.Subscribe<MenuModel, MenuAction>(this, "MenuAction", (sender, action) =>
            {
                switch (action)
                {

                    case MenuAction.LensPreview:

                        DetailChanging.Invoke(this, new LensPreviewPage());
                        break;

                    case MenuAction.Logout:

                        if (!Device.WPF.Equals(Device.RuntimePlatform))
                        {
                            Preferences.Clear();
                        }

                        authenticatingMessageInspector.Bearer = string.Empty;

                        foreach (Cookie cookie in authenticatingMessageInspector.Cookies)
                        {
                            cookie.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                        }

                        IsLogged = false;

                        break;

                }
            });

        }

    }
}
