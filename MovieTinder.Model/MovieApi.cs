using MovieTinder.Model.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Model.models;
using Model.dataModels;

namespace MovieTinder.Model
{
    public class MovieApi
    {
        public static MovieApi Instance = new MovieApi();
        public static ImageSizes ImageSizes;
        private const string API_KEY = "579872d8976e8f07d27624584808fee2";
        private const string BASE_URL = "https://api.themoviedb.org/3";

        private string Language = "en-US";
        private List<Movie> MoviesSeen = new List<Movie>();

        private List<Movie> MoviesLiked = new List<Movie>();
        private List<Movie> MoviesDisliked = new List<Movie>();

        private List<Genre> SelectedGenres = new List<Genre>();

        private Dictionary<int, int> SeenGenres = new Dictionary<int, int>();
        private Dictionary<string, string> _extraRequestParams = new Dictionary<string, string>();

        private MovieApi(){}

        public static async Task LoadImageSizes()
        {
            var imageSizesJSON = await new RequestHelper(BASE_URL).Get($"/configuration?api_key={API_KEY}");
            ImageSizes = Parse<Configuration.Root>(imageSizesJSON.Content).images;
        }

        public async Task<Movie> StartAsync(List<Genre> genres, Dictionary<string,string> extraParams)
        {
            _extraRequestParams = extraParams;
            SelectedGenres = genres;
            var genreSearchResults = await GenreSearchAsync<Movie>(genres);
            return SelectOne(genreSearchResults.results);
        }

        public async Task<Result<T>> GenreSearchAsync<T>(List<Genre> genres, int Page, bool IncludeAdult=false) => await GenreSearchAsync<T>(genres, "page=" + Page, IncludeAdult);

        public async Task<Result<T>> GenreSearchAsync<T>(List<Genre> genres) => await GenreSearchAsync<T>(genres, 1, false);

        public async Task<Result<T>> GenreSearchAsync<T>(List<Genre> genres, Dictionary<string, string> options)
        {
            string optionsString = "";
            foreach (KeyValuePair<string,string> option in options)
            {
                optionsString += $"{option.Key}={option.Value}";
            }
            return await GenreSearchAsync<T>(genres, optionsString);
        }
        public async Task<Result<T>> GenreSearchAsync<T>(List<Genre> genres, string extraParams, bool includeAdult = false)
        {
            var include_genres = string.Join(",", genres.Select(genre => genre.id));
            var ApiClient = new RequestHelper(BASE_URL);
            var endpoint = $"/discover/{GetMediaString<T>()}?api_key={API_KEY}&language={Language}&IncludeAdult={includeAdult}&with_genres={include_genres}";
            if (extraParams != null)
            {
                endpoint += "&" + extraParams;
            }
            var result = await ApiClient.Get(endpoint);
            return Parse<Result<T>>(result.Content);
        }

        public async Task<Result<T>> GetSimilarAsync<T>(int movieID)
        {
            var ApiClient = new RequestHelper(BASE_URL);
            var result = await ApiClient.Get($"/{GetMediaString<T>()}/{movieID}/similar?api_key={API_KEY}");
            return Parse<Result<T>>(result.Content);
        }

        public async Task<Result<T>> GetRecommendationAsync<T>(int movieID)
        {
            var ApiClient = new RequestHelper(BASE_URL);
            var result = await ApiClient.Get($"/{GetMediaString<T>()}/{movieID}/recommendations?api_key={API_KEY}");
            return Parse<Result<T>>(result.Content);
        }

        public async Task<GenreResult> GetGenresAsync()
        {
            var ApiClient = new RequestHelper(BASE_URL);
            var result = await ApiClient.Get($"/genre/movie/list?api_key={API_KEY}");
            
            return Parse<GenreResult>(result.Content);
        }

        private static T Parse<T>(string json)
        {
            T results = JsonConvert.DeserializeObject<T>(json);
            return results;
        }

        public bool HasSeen(Movie movie)
        {
            return MoviesSeen.FirstOrDefault(seen => seen.title == movie.title) != null;
        }

        public void AddMovie(Movie movie)
        {
            MoviesSeen.Add(movie);
            foreach(var genre in movie.genre_ids)
            {
                SeenGenres[genre] = SeenGenres.ContainsKey(genre) ? SeenGenres[genre] + 1 : 1;
            }
        }

        public int GetMostCommonGenreID()
        {
            int highestID = 0;
            var highest = 0;
            foreach(KeyValuePair<int,int> genre in SeenGenres)
            {
                if (genre.Value > highest) highestID = genre.Key;
            }
            return highestID;
        }

        public void ClearSeen()
        {
            MoviesSeen.Clear();
        }

        private Movie SelectOne(List<Movie> movies)
        {
            foreach(var movie in movies)
            {
                if (!HasSeen(movie)) {
                    AddMovie(movie);
                    return movie;
                }
            }
            return null;
        }

        private async Task<Movie> LikeAsync(int page)
        {
            MoviesLiked.Add(MoviesSeen.Last());
            var similar = await GetRecommendationAsync<Movie>(MoviesLiked.Last().id);
            var foundMovie = SelectOne(similar.results);
            if (foundMovie != null) return foundMovie;
            if(similar.page < similar.total_pages)
            {
                await LikeAsync(similar.page + 1);
            }
            return null;
        }

        public async Task<Movie> LikeAsync() =>  await LikeAsync(1);

        public async Task<Movie> DislikeAsync()
        {
            MoviesDisliked.Add(MoviesSeen.Last());
            var result = MoviesSeen.Count - 1 > 0 ?
                await GetRecommendationAsync<Movie>(MoviesLiked.Last().id) :
                await GenreSearchAsync<Movie>(SelectedGenres, _extraRequestParams);
            var foundMovie = SelectOne(result.results);
            return foundMovie;
        }

        private string GetMediaString<T>()
        {
            if (typeof(T).IsAssignableFrom(typeof(Movie)))
            {
                return "movie";
            }
            else if (typeof(T).IsAssignableFrom(typeof(TV)))
            {
                return "tv";
            }
            else if (typeof(T).IsAssignableFrom(typeof(Generic)))
            {
                return "generic";
            }
            return null;
        }
    }
}
