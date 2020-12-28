using MovieTinder.Model.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace MovieTinder.Model
{
    public class MovieApi
    {
        private const string API_KEY = "579872d8976e8f07d27624584808fee2";
        private const string BASE_URL = "https://api.themoviedb.org/3";

        private static string Language = "en-US";
        private static List<Movie> MoviesSeen = new List<Movie>();

        private static List<Genre> SelectedGenres = new List<Genre>();

        private static Dictionary<int, int> SeenGenres = new Dictionary<int, int>();

        public static async Task<Movie> StartAsync(List<Genre> genres)
        {
            SelectedGenres = genres;
            var genreSearchResults = await GenreSearchAsync<Movie>(genres);
            return SelectOne(genreSearchResults.results);
        }

        public static async Task<Result<T>> GenreSearchAsync<T>(List<Genre> genres, int Page, bool IncludeAdult)
        {
            var include_genres = string.Join(",", genres.Select(genre=>genre.id));
            var ApiClient = new RequestHelper(BASE_URL);
            var result = await ApiClient.Get($"/discover/{GetMediaString<T>()}?api_key={API_KEY}&language={Language}&include_adult={IncludeAdult}&page={Page}&with_genres={include_genres}");
            return Parse<Result<T>>(result.Content);
        }

        public static async Task<Result<T>> GenreSearchAsync<T>(List<Genre> genres) => await GenreSearchAsync<T>(genres, 1, false);

        public static async Task<Result<T>> GetSimilarAsync<T>(int movieID)
        {
            var ApiClient = new RequestHelper(BASE_URL);
            var result = await ApiClient.Get($"/{GetMediaString<T>()}/{movieID}/similar?api_key={API_KEY}");
            return Parse<Result<T>>(result.Content);
        }

        public static async Task<GenreResult> GetGenresAsync()
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

        public static bool HasSeen(Movie movie)
        {
            return MoviesSeen.FirstOrDefault(seen => seen.title == movie.title) != null;
        }

        public static void AddMovie(Movie movie)
        {
            MoviesSeen.Add(movie);
            foreach(var genre in movie.genre_ids)
            {
                SeenGenres[genre] = SeenGenres.ContainsKey(genre) ? SeenGenres[genre] + 1 : 1;
            }
        }

        public static int GetMostCommonGenreID()
        {
            int highestID = 0;
            var highest = 0;
            foreach(KeyValuePair<int,int> genre in SeenGenres)
            {
                if (genre.Value > highest) highestID = genre.Key;
            }
            return highestID;
        }

        public static void ClearSeen()
        {
            MoviesSeen.Clear();
        }

        private static Movie SelectOne(List<Movie> movies)
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

        private static async Task<Movie> LikeAsync(int page)
        {
            var similar = await GetSimilarAsync<Movie>(MoviesSeen[MoviesSeen.Count - 1].id);
            var foundMovie = SelectOne(similar.results);
            if (foundMovie != null) return foundMovie;
            if(similar.page < similar.total_pages)
            {
                LikeAsync(similar.page + 1);
            }
            return null;
        }

        public static async Task<Movie> LikeAsync()
        {
            return await LikeAsync(1);
        }

        public static async Task<Movie> DislikeAsync()
        {
            var result = MoviesSeen.Count - 1 > 0 ?
                await GetSimilarAsync<Movie>(MoviesSeen[MoviesSeen.Count - 1].id) :
                await GenreSearchAsync<Movie>(SelectedGenres);
            var foundMovie = SelectOne(result.results);
            return foundMovie;
        }

        private static string GetMediaString<T>()
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
