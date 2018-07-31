using CalcolatoreXamarin.ViewModels;
using Org.Visiontech.Commons;
using Org.Visiontech.Credential;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Visiontech.Services.Utils;
using VisiontechCommons;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CalcolatoreXamarin.Shared.ViewModels
{
    public class LoginModel : BaseViewModel
    {

        protected readonly ITokenService tokenService = Container.ServiceProvider.GetService(typeof(ITokenService)) as ITokenService;
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;
        protected readonly CredentialSoapClient credentialSoapClient = Container.ServiceProvider.GetService(typeof(CredentialSoapClient)) as CredentialSoapClient;

        private bool canLogin = false;
        public bool CanLogin
        {
            get { return canLogin; }
            set { SetProperty(ref canLogin, value); }
        }

        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set {
                SetProperty(ref username, value);
                CheckCanLogin();
            }
        }

        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set {
                SetProperty(ref password, value);
                CheckCanLogin();
            }
        }

        private void CheckCanLogin()
        {
            CanLogin = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && IsConnected;
        }

        public ICommand LoginCommand { get; }

        public LoginModel()
        {
            LoginCommand = new Command(Login);
            IsConnectedChanged += LoginModel_IsConnectedChanged;
        }

        private void LoginModel_IsConnectedChanged(object sender, bool e)
        {
            CheckCanLogin();
        }
        public void Reload()
        {
            if (!Device.WPF.Equals(Device.RuntimePlatform))
            {
                Username = Preferences.Get("username", string.Empty);
                Password = Preferences.Get("password", string.Empty);
            } else
            {
                Username = string.Empty;
                Password = string.Empty;
            }
        }
        private async void Login()
        {

            IsBusy = true;

            if (IsConnected && CanLogin) {
                string token = await tokenService.GetToken(Username, Password);

                if (!string.IsNullOrWhiteSpace(token))
                {

                    authenticatingMessageInspector.Bearer = token;

                    credentialDTO credentialDTO = await Task.Run(() => credentialSoapClient.my() );

                    if (!(credentialDTO is null) && !string.IsNullOrWhiteSpace(credentialDTO.id))
                    {
                        if (!Device.WPF.Equals(Device.RuntimePlatform))
                        {
                            Preferences.Set("username", Username);
                            Preferences.Set("password", Password);
                        }

                        IsLogged = true;
                    }

                }
            }

            IsBusy = false;

        }

    }
}
