using Model.models;
using MovieTinder.Model;
using MovieTinder.Model.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieTinder
{
    public partial class App : Application
    {
        public static uint ScreenHeight { get; set; }
        public static uint ScreenWidth { get; set; }

        public static MovieApi MovieAPI { get; set; }

        public static Dictionary<string, string> BestImageSize;
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "DragAndDrop_Experimental" });
            App.Current.Resources["Theme"] = Theme.Instance;
            MovieAPI = MovieApi.Instance;
            MainPage = new NavigationPage(new GenreSelect());
            InitalizeApiAsync();
        }

        private async void InitalizeApiAsync()
        {
            await MovieApi.LoadImageSizes();
            BestImageSize = MovieApi.ImageSizes.GetAllBestSizes(ScreenWidth);
        }
        public static string GetImage(Movie movie, ImageSizes.ImageType type) => movie.GetImageURL(GetSize(type), type);
        public static string GetSize(ImageSizes.ImageType type) => BestImageSize[type.ToString("g")];

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
