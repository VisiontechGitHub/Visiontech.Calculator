using CalcolatoreXamarin.ViewModels.Abstraction;
using CalcolatoreXamarin.Views.Abstraction;
using Org.Visiontech.Product;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalcolatoreXamarin.Views.Product
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductListPage : ContentPage
	{

        private readonly ScrollableListViewModel<productDTO, ProductSoapClient, ProductSoap> model;

        public ProductListPage ()
		{
			InitializeComponent ();
            Title = "Products";

            model = new ScrollableListViewModel<productDTO, ProductSoapClient, ProductSoap>((service, quantity, offset) => {

                findCriteriaDTO findCriteriaDTO = new simpleCriteriaDTO()
                {
                    criteria = new productDTO()
                };

                findResultDTO findResultDTO = service.find(new[] { findCriteriaDTO }, quantity, offset);

                return findResultDTO.results.Where(x => x is productDTO).Select(x => x as productDTO).ToList();

            });

            Content = new ScrollableListView<productDTO, ProductSoapClient, ProductSoap>(model);
        }
	}
}