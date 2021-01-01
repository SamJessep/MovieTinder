using MovieTinder.Model.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieTinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateSelect : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DateTime MinDate { get; set; } = new DateTime(1895, 11, 1);
        public DateTime MaxDate { get; set; } = DateTime.Now;
        public DateTime SelectedMinDate
        {
            get => _selectedMinDate; set
            {
                if (value != _selectedMinDate)
                {
                    _selectedMinDate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime SelectedMaxDate { get => _selectedMaxDate; set
            {
                if (value != _selectedMaxDate)
                {
                    _selectedMaxDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private DateTime _selectedMinDate;
        private DateTime _selectedMaxDate;
        private List<Genre> _selectedGenres;
        public DateSelect(List<Genre> selectedGenres)
        {
            _selectedGenres = selectedGenres;
            InitializeComponent();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void highDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            SelectedMaxDate = e.NewDate;
        }

        private void lowDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            SelectedMinDate = e.NewDate;   
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(_selectedGenres, SelectedMinDate, SelectedMaxDate), true);
        }

        private void SkipButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(_selectedGenres), true);
        }
    }
}