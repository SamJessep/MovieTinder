using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MovieTinder
{
    public class Theme : INotifyPropertyChanged
    {
        public static Color FocusColor = Color.DeepSkyBlue;
        public static Color ButtonDefault { get; } = Color.LightGray;
        public static Color ButtonPressed { get; }= FocusColor;
        public static Color BackgroundColor { get; } = Color.FromRgb(244, 244, 244);
        public static Color LikeColor { get; } = Color.FromRgb(20, 255, 86);
        public static Color DislikeColor { get; } = Color.FromRgb(255, 20, 20);
        public static Theme Instance { get; } = new Theme();

        public event PropertyChangedEventHandler PropertyChanged;

        private Theme() { }
    }
}
