using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace CalcolatoreXamarin.WPF
{
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            Forms.Init();
            LoadApplication(new CalcolatoreXamarin.App());
        }
    }
}
