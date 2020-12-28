using MovieTinder.Model;
using MovieTinder.Model.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace MovieTinderDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenreSelect : ContentPage
    {
        public List<Genre> Genres { get; set; }
        public List<Genre> selectedGenres = new List<Genre>();

        public GenreSelect()
        {
            PopulateGenreList();
            BindingContext = this;
        }
        public async Task PopulateGenreList()
        {
            var result = await MovieApi.GetGenresAsync();
            Genres = result.genres;
            InitializeComponent();
        }

        private void GenreSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as Genre;
            if(selectedGenres.FirstOrDefault(genre => genre.id == selected.id) != null)
            {
                selectedGenres.Remove(selected);
            }
            else
            {
                selectedGenres.Add(selected);
            }
            Debug.WriteLine(selected.name);
        }

        private void SubmitClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(selectedGenres),true);
        }
    }
}