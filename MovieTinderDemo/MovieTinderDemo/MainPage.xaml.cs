using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieTinderDemo
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;
        public MainPage()
        {
            InitializeComponent();
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
                    card.Color = Color.Red;
                    SwipeSideAnimateAsync(-1);
                    break;
                case SwipeDirection.Right:
                    // Handle the swipe
                    Debug.WriteLine("SWIPED Right");
                    card.Color = Color.Green;
                    SwipeSideAnimateAsync(1);
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    card.Color = Color.Yellow;
                    Debug.WriteLine("SWIPED up");
                    SwipeUpAnimateAsync();
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    card.Color = Color.Purple;
                    Debug.WriteLine("SWIPED Down");
                    SwipeDownAnimateAsync();
                    break;
            }
        }

        async Task SwipeSideAnimateAsync(int dir)
        {
            card.TranslateTo(500*dir, Y);
            await card.RotateTo(100*dir*2);
            card.Rotation = 0;
            await card.ScaleTo(0,100);
            await card.TranslateTo(0, 0, 100);
            card.Color = Color.Teal;
            await card.ScaleTo(1);
            //await card.TranslateTo(500, -500);
            //await card.TranslateTo(0, -500);
            //await card.TranslateTo(0, 0);

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
