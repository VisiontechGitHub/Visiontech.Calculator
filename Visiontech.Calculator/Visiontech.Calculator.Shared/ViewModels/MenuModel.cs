using CalcolatoreXamarin.Shared.Services;
using CalcolatoreXamarin.Shared.ViewModels;
using CalcolatoreXamarin.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using VisiontechCommons;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Shared.ViewModels
{
    public class MenuModel : BaseViewModel
    {

        protected readonly ITranslateService translateService = Container.ServiceProvider.GetService(typeof(ITranslateService)) as ITranslateService;
        public enum MenuAction
        {
            LensPreview,
            Logout
        };
        public ObservableCollection<Tuple<string, MenuAction>> MenuValues { get; set; }

        private Tuple<string, MenuAction> selectedAction;
        public Tuple<string, MenuAction> SelectedAction
        {
            get { return selectedAction; }
            set
            {
                if (selectedAction != value)
                {
                    SetProperty(ref selectedAction, value);
                    if (value != null)
                    {
                        MessagingCenter.Send(this, "MenuAction", value.Item2);
                    }
                }
            }
        }
        public MenuModel()
        {
            MenuValues = new ObservableCollection<Tuple<string, MenuAction>>(Enum.GetValues(typeof(MenuAction)).Cast<MenuAction>().Select(action => new Tuple<string, MenuAction>(translateService.Translate(action.ToString()), action)));
            IsLoggedChanged += MenuModel_IsLoggedChanged;
        }

        private void MenuModel_IsLoggedChanged(object sender, bool IsLogged)
        {
            if (IsLogged)
            {
                SelectedAction = MenuValues.First();
            }
        }

    }
}
