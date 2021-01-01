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
        public static Theme Instance { get; } = new Theme();

        public event PropertyChangedEventHandler PropertyChanged;

        private Theme() { }
    }
}
