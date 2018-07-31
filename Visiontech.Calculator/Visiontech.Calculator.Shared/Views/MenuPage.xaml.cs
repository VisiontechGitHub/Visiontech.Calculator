using CalcolatoreXamarin.Shared.Services;
using CalcolatoreXamarin.Shared.ViewModels;
using CalcolatoreXamarin.Shared.Views;
using CalcolatoreXamarin.Shared.Views.Abstraction;
using CalcolatoreXamarin.Views;
using CalcolatoreXamarin.Views.Credential;
using CalcolatoreXamarin.Views.Product;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CalcolatoreXamarin.Shared.ViewModels.MenuModel;

namespace CalcolatoreXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : LoadingContentPage<MenuModel>
    {
        public MenuPage ()
		{
			InitializeComponent ();
        }

    }
}