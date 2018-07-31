using CalcolatoreXamarin.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CalcolatoreXamarin.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public readonly ITranslateService Translator = VisiontechCommons.Container.ServiceProvider.GetService(typeof(ITranslateService)) as ITranslateService;

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set {
                if (isBusy != value)
                {
                    SetProperty(ref isBusy, value);
                    if (IsBusyChanged != null)
                    {
                        SetProperty(ref isBusy, value);
                        IsBusyChanged.Invoke(this, value);
                    }
                }
            }
        }

        private static bool isLogged = false;
        public bool IsLogged
        {
            get { return isLogged; }
            set
            {
                if (isLogged != value)
                {
                    SetProperty(ref isLogged, value);
                    if (IsLoggedChanged != null)
                    {
                        IsLoggedChanged.Invoke(this, value);
                    }
                }
            }
        }

        private static bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (isConnected != value)
                {
                    SetProperty(ref isConnected, value);
                    if (IsConnectedChanged != null)
                    {
                        IsConnectedChanged.Invoke(this, value);
                    }
                }
            }
        }

        public event EventHandler<bool> IsBusyChanged;
        public static event EventHandler<bool> IsLoggedChanged;
        public static event EventHandler<bool> IsConnectedChanged;
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public BaseViewModel()
        {
            if (!Device.WPF.Equals(Device.RuntimePlatform))
            {
                Connectivity.ConnectivityChanged += ConnectivityChanged;
                IsConnected = NetworkAccess.Internet.Equals(Connectivity.NetworkAccess);
            } else
            {
                IsConnected = true;
            }

        }

        private void ConnectivityChanged(ConnectivityChangedEventArgs e)
        {
            if (NetworkAccess.Internet.Equals(Connectivity.NetworkAccess))
            {
                IsConnected = true;
            } else
            {
                IsConnected = false;
                IsLogged = false;
            }
        }

    }
}
