using CalcolatoreXamarin.Shared.Services;
using CalcolatoreXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VisiontechCommons;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Shared.Views.Abstraction
{
    public abstract class LoadingContentPage<T> : ContentPage where T : BaseViewModel
    {

        public readonly T model = Container.ServiceProvider.GetService(typeof(T)) as T;

        private BoxView boxView;
        private StackLayout activityLayout;
        private AbsoluteLayout absoluteLayout;
        private ContentView contentView;

        public View LoadingContent {
            get {
                return contentView.Content;
            }
            set {
                contentView.Content = value;
            }
        }

        public LoadingContentPage()
        {

            BindingContext = model;

            model.IsBusyChanged += IsBusyChanged;

            absoluteLayout = new AbsoluteLayout();

            boxView = new BoxView
            {
                BackgroundColor = Color.Black,
                Opacity = 0.25,
                IsVisible = false
            };

            absoluteLayout.Children.Add(boxView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            activityLayout = new StackLayout()
            {
                IsVisible = false
            };

            activityLayout.Children.Add(new BoxView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            });
            activityLayout.Children.Add(new ActivityIndicator()
            {
                IsRunning = true
            });
            activityLayout.Children.Add(new BoxView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            });

            absoluteLayout.Children.Add(activityLayout, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            contentView = new ContentView();

            absoluteLayout.Children.Add(contentView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            Content = absoluteLayout;

        }
        protected void StartLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                activityLayout.IsVisible = true;
                boxView.IsVisible = true;
                absoluteLayout.LowerChild(contentView);
            });
        }

        protected void StopLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                activityLayout.IsVisible = false;
                boxView.IsVisible = false;
                absoluteLayout.RaiseChild(contentView);
            });
        }
        protected void IsBusyChanged(object sender, bool IsBusy)
        {
            if (IsBusy)
            {
                StartLoading();
            }
            else
            {
                StopLoading();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsBusy)
            {
                StartLoading();
            }
        }

    }
}
