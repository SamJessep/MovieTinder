using Xamarin.Forms;

namespace MovieTinder
{
    public partial class App : Application
    {
        public static uint ScreenHeight { get; set; }
        public static uint ScreenWidth { get; set; }
        public App()
        {

            Device.SetFlags(new string[] { "DragAndDrop_Experimental" });
            InitializeComponent();

            App.Current.Resources["Theme"] = Theme.Instance;
            MainPage = new NavigationPage(new GenreSelect());
            //MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
