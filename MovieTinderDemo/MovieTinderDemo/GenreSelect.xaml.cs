using MovieTinder.Model;
using MovieTinder.Model.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace MovieTinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenreSelect : ContentPage, INotifyPropertyChanged
    {
        public List<Genre> Genres { get; set; }
        public bool BottomButtonsEnabled { get; set; } = false;
        public List<Genre> selectedGenres = new List<Genre>();
        public event PropertyChangedEventHandler PropertyChanged;
        private string _selectedText;
        public string SelectedText { get => _selectedText; set 
            {
                if (value != _selectedText)
                {
                    _selectedText = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string NoneSelected = "None Selected";
        private string SelectedOne = "Selected {0} Genre";
        private string SelectedMultiple = "Selected {0} Genres";
        public GenreSelect()
        {
            PopulateGenreList();
            UpdateSelectedText();
            BindingContext = this;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public async Task PopulateGenreList()
        {
            var result = await MovieApi.GetGenresAsync();
            Genres = result.genres;
            InitializeComponent();
        }

        void UpdateSelectedText()
        {
            if (selectedGenres.Count == 0) {
                SelectedText = NoneSelected;
            } else if (selectedGenres.Count == 1) {
                SelectedText = string.Format(SelectedOne, selectedGenres.Count);
            }
            else
            {
                SelectedText = string.Format(SelectedMultiple, selectedGenres.Count);
            }
        }

        private void SubmitClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(selectedGenres),true);
        }

        private void ClearClicked(object sender, EventArgs e)
        {
            GenreList.SelectedItems = null;
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGenres = e.CurrentSelection.Cast<Genre>().ToList();
            UpdateSelectedText();
            BottomButtonsEnabled = selectedGenres.Count >= 1;
            NotifyPropertyChanged("BottomButtonsEnabled");
        }
    }
}