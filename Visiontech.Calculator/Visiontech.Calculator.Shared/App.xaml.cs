using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CalcolatoreXamarin.Views;
using Microsoft.Extensions.DependencyInjection;
using CalcolatoreXamarin.Shared.ViewModels;
using System;
using CalcolatoreXamarin.Shared.Services;
using CalcolatoreXamarin.Views.Abstraction;
using VisiontechCommons;
using Org.Visiontech.Commons.Services;
using Org.Visiontech.Commons.Models;
using System.Net.Http;
using Org.Visiontech.Commons;
using System.ServiceModel.Description;
using Visiontech.Services.Utils;
using System.ServiceModel.Dispatcher;
using Org.Visiontech.Credential;
using System.ServiceModel;
using System.Reflection;
using System.Collections.Generic;
using Org.Visiontech.Product;
using Org.Visiontech.Person;
using Org.Visiontech.Group;
using Org.Visiontech.Compute;
using System.Collections.ObjectModel;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CalcolatoreXamarin
{
	public partial class App : Application
	{
        public App ()
		{
			InitializeComponent();

            Container.Services.AddSingleton<IProvider<HttpClientHandler>, HttpClientHandlerProvider>();
            Container.Services.AddSingleton<IProvider<HttpClient>, HttpClientProvider>();

            Container.Services.AddSingleton<IAuthenticatingMessageInspector, AuthenticatingMessageInspector>();

            Container.Services.AddSingleton<ITokenService, TokenService>(serviceProvider => new TokenService("https://cas.dev.optoplus.cloud:8543/cas/v1/tickets", "services.dev.optoplus.cloud/optoplus-services-web"));

            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<CredentialSoap, CredentialSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/CredentialSoap")));
            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<GroupSoap, GroupSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/GroupSoap")));
            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<ProductSoap, ProductSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/ProductSoap")));
            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<PersonSoap, PersonSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/PersonSoap")));
            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<ComputeSoap, ComputeSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/ComputeSoap")));

            Container.Services.AddSingleton<MainModel>();
            Container.Services.AddSingleton<MenuModel>();
            Container.Services.AddSingleton<LoginModel>();
            Container.Services.AddSingleton<LensPreviewModel>();

            Container.Services.AddSingleton<ITranslateService, TranslateService>();

            Container.Services.AddSingleton<EntityTemplate<object>, IdentifiableTemplate>();

            MainPage = new MainPage();
        }

    protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
