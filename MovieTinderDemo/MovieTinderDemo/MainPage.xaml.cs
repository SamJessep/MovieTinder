using MovieTinder.helpers;
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
using static Model.models.ImageSizes;

namespace MovieTinder
{
    public partial class MainPage : ContentPage
    {
        private const int TILT_AMOUNT = 25;
        private const int TILT_SPEED = 150;
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
            var firstMovie = await App.MovieAPI.StartAsync(selectedGenres, searchParams);
            ShowMovie(firstMovie);
        }

        async Task ShowMovie(Movie movie)
        {
            Poster.Source = App.GetImage(movie, ImageType.Poster);
            Title.Text = movie.title;
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
                    Debug.WriteLine("SWIPED up");
                    SwipeUpAnimateAsync();
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    Debug.WriteLine("SWIPED Down");
                    SwipeDownAnimateAsync();
                    break;
            }
        }

        async Task SwipeLeft()
        {
            var movie = await App.MovieAPI.DislikeAsync();
            //card.BackgroundColor = Color.Red;
            await SwipeAnimateOutAsync(-1);
            await ShowMovie(movie);
            await SwipeAnimateInAsync();
        }

        async Task SwipeRight()
        {
            var movie = await App.MovieAPI.LikeAsync();
            //card.BackgroundColor = Color.Green;
            await SwipeAnimateOutAsync(1);
            await ShowMovie(movie);
            await SwipeAnimateInAsync();
        }

        async Task TiltRight()
        {
            await Poster.RotateTo(TILT_AMOUNT, TILT_SPEED);
            LikeDropOn.FadeTo(100);
            LikeDropOff.FadeTo(0);
        }
        async Task TiltLeft()
        {
            await Poster.RotateTo(-TILT_AMOUNT, TILT_SPEED);
            DislikeDropOn.FadeTo(100);
            DislikeDropOff.FadeTo(0);
        }
        async Task TiltReset()
        {
            await Poster.RotateTo(0, TILT_SPEED);
            LikeDropOn.FadeTo(0);
            LikeDropOff.FadeTo(100);
            DislikeDropOn.FadeTo(0);
            DislikeDropOff.FadeTo(100);
        }

        async Task SwipeAnimateOutAsync(int dir)
        {
            Poster.TranslateTo(500 * dir, Y);
            await Poster.RotateTo(100 * dir * 2);
            Poster.Rotation = 0;
        }
        async Task SwipeAnimateInAsync()
        {
            TiltReset();
            await Poster.ScaleTo(0, 100);
            await Poster.TranslateTo(0, 0, 100);
            //card.BackgroundColor = Color.Transparent;
            Poster.Opacity = 0;
            Poster.FadeTo(1);
            await Poster.ScaleTo(1);
        }

        async Task SwipeUpAnimateAsync()
        {
            await Poster.ScaleTo(App.ScreenHeight / Poster.Height);

        }

        async Task SwipeDownAnimateAsync()
        {
            await Poster.ScaleTo(1);

        }

        private void Drop_Like(object sender, DropEventArgs e) => SwipeRight();

        private void Drop_Dislike(object sender, DropEventArgs e) => SwipeLeft();

        private void Dislike_DragOver(object sender, DragEventArgs e) => TiltLeft();

        private void Like_DragOver(object sender, DragEventArgs e) => TiltRight();
        private void Netural_DragOver(object sender, DragEventArgs e) => TiltReset();
        
    }
}
