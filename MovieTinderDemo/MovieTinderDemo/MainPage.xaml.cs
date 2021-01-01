using MovieTinder.Model;
using MovieTinder.Model.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieTinder
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;
        private List<Genre> selectedGenres;
        private Dictionary<string, string> searchParams = new Dictionary<string, string>();
        public MainPage(List<Genre> genres)
        {
            selectedGenres = genres;
            InitializeComponent();
            Start();
        }
        public MainPage(List<Genre> genres, DateTime releasedAfter, DateTime releasedBefore)
        {
            AddParam("release_date.lte", releasedBefore.ToString("yyyy-mm-dd"));
            AddParam("release_date.gte", releasedAfter.ToString("yyyy-mm-dd"));
            selectedGenres = genres;
            InitializeComponent();
            Start();
        }

        private void AddParam(string key, string value)
        {
            if (searchParams.ContainsKey(key)) 
            {
                searchParams[key] = value;
            }
            else
            {
                searchParams.Add(key,value);
            }
        }

        private async Task Start()
        {
            var firstMovie = await MovieApi.StartAsync(selectedGenres, searchParams);
            ShowMovie(firstMovie);
        }

        void ShowMovie(Movie movie)
        {
            card.Source = movie.PosterURL;
            title.Text = movie.title;
        }

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            count++;
            countLbl.Text = $"Swipe: {count}";
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Handle the swipe
                    Debug.WriteLine("SWIPED Left");
                    SwipeLeft();
                    break;
                case SwipeDirection.Right:
                    // Handle the swipe
                    Debug.WriteLine("SWIPED Right");
                    SwipeRight();
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    card.BackgroundColor = Color.Yellow;
                    Debug.WriteLine("SWIPED up");
                    SwipeUpAnimateAsync();
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    card.BackgroundColor = Color.Purple;
                    Debug.WriteLine("SWIPED Down");
                    SwipeDownAnimateAsync();
                    break;
            }
        }

        async Task SwipeLeft()
        {
            var movie = await MovieApi.DislikeAsync();
            card.BackgroundColor = Color.Red;
            SwipeSideAnimateAsync(-1);
            ShowMovie(movie);
        }

        async Task SwipeRight()
        {
            var movie = await MovieApi.LikeAsync();
            card.BackgroundColor = Color.Green;
            SwipeSideAnimateAsync(1);
            ShowMovie(movie);
        }

        async Task SwipeSideAnimateAsync(int dir)
        {
            card.TranslateTo(500*dir, Y);
            await card.RotateTo(100*dir*2);
            card.Rotation = 0;
            await card.ScaleTo(0,100);
            await card.TranslateTo(0, 0, 100);
            card.BackgroundColor = Color.Teal;
            await card.ScaleTo(1);
        }

        async Task SwipeUpAnimateAsync()
        {
            await card.ScaleTo(App.ScreenHeight / card.Height);

        }

        async Task SwipeDownAnimateAsync()
        {
            await card.ScaleTo(1);

        }

    }
}
