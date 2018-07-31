using CalcolatoreXamarin.ViewModels.Abstraction;
using CalcolatoreXamarin.Views.Abstraction;
using CalcolatoreXamarin.Views.Credential;
using Org.Visiontech.Credential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalcolatoreXamarin.Views.Credential
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CredentialListPage : ContentPage
    {

        private readonly ScrollableListViewModel<credentialDTO, CredentialSoapClient, CredentialSoap> model;

        public CredentialListPage()
        {
            InitializeComponent();
            Title = "Credentials";

            model = new ScrollableListViewModel<credentialDTO, CredentialSoapClient, CredentialSoap>((service, quantity, offset) => {

                findCriteriaDTO findCriteriaDTO = new simpleCriteriaDTO()
                {
                    criteria = new credentialDTO()
                };

                findResultDTO findResultDTO = service.find(new[] { findCriteriaDTO }, quantity, offset);

                return findResultDTO.results.Where(x => x is credentialDTO).Select(x => x as credentialDTO).ToList();

            });

            Content = new ScrollableListView<credentialDTO, CredentialSoapClient, CredentialSoap>(model);
        }
    }
}