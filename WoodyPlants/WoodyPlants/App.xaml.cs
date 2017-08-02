using Xamarin.Forms;

namespace WoodyPlants
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            this.MainPage = new NavigationPage(new MainPage());
        }
    }
}
