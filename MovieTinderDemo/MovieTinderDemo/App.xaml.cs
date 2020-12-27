using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieTinderDemo
{
    public partial class App : Application
    {
        public static uint ScreenHeight { get; set; }
        public static uint ScreenWidth { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
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

        void OnSwipe()
        {

        }
    }
}
