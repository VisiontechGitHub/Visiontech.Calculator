using CalcolatoreXamarin.Shared.ViewModels;
using CalcolatoreXamarin.ViewModels;
using System;
using System.Diagnostics;
using VisiontechCommons;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Views
{
    public partial class MainPage : MasterDetailPage
    {

        public readonly MainModel model = Container.ServiceProvider.GetService(typeof(MainModel)) as MainModel;
        public MainPage()
        {

            InitializeComponent();
            BindingContext = model;

            model.DetailChanging += DetailChanging;
            BaseViewModel.IsLoggedChanged += IsLoggedChanged;
            BaseViewModel.IsConnectedChanged += IsConnectedChanged;

            Master = new MenuPage();
            Detail = new NavigationPage(new LoginPage());

        }

        private void IsLoggedChanged(object sender, bool IsLogged)
        {
            IsGestureEnabled = IsLogged;
            if (!IsLogged)
            {
                Detail = new NavigationPage(new LoginPage());
                IsPresented = false;
            }
        }

        private void DetailChanging(object sender, Page page)
        {
            if (model.IsLogged)
            {
                Detail = new NavigationPage(page);
                IsPresented = false;
            }
        }

        private void IsConnectedChanged(object sender, bool IsConnected)
        {
            if (!IsConnected)
            {
                DisplayAlert(model.Translator.Translate("Alert"), model.Translator.Translate("NoConnectivity"), model.Translator.Translate("OK"));
            }
        }

        private void MasterDetailPage_IsPresentedChanged(object sender, EventArgs e)
        {
            if (!model.IsLogged && IsPresented)
            {
                IsPresented = false;
            }
        }
    }

}