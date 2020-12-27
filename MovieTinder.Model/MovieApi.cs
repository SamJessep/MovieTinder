using MovieTinder.Model.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTinder.Model
{
    public class MovieApi
    {
        private const string API_KEY = "579872d8976e8f07d27624584808fee2";
        private const string BASE_URL = "https://api.themoviedb.org/3";

        private static string Language = "en-US";
        private static List<Movie> MoviesSeen = new List<Movie>();

        private static List<Genre> SelectedGenres = new List<Genre>();

        public static Movie Start(List<Genre> genres)
        {
            SelectedGenres = genres;
            return SelectOne(GenreSearch<Movie>(genres).results);
        }

        public static Result<T> GenreSearch<T>(List<Genre> genres, int Page, bool IncludeAdult)
        {
            var include_genres = string.Join(",", genres.Select(genre=>genre.id));
            var ApiClient = new RequestHelper(BASE_URL);
            var result = ApiClient.Get($"/discover/{GetMediaString<T>()}?api_key={API_KEY}&language={Language}&include_adult={IncludeAdult}&page={Page}&with_genres={include_genres}");
            return Parse<Result<T>>(result.Content);
        }

        public static Result<T> GenreSearch<T>(List<Genre> genres) => GenreSearch<T>(genres, 1, false);

        public static Result<T> GetSimilar<T>(int movieID)
        {
            var ApiClient = new RequestHelper(BASE_URL);
            var result = ApiClient.Get($"/{GetMediaString<T>()}/{movieID}/similar?api_key={API_KEY}");
            return Parse<Result<T>>(result.Content);
        }

        public static GenreResult GetGenres()
        {
            var ApiClient = new RequestHelper(BASE_URL);
            var result = ApiClient.Get($"/genre/movie/list?api_key={API_KEY}");
            
            return Parse<GenreResult>(result.Content);
        }

        private static T Parse<T>(string json)
        {
            T results = JsonConvert.DeserializeObject<T>(json);
            return results;
        }

        public static bool HasSeen(Movie movie)
        {
            return MoviesSeen.IndexOf(movie) != -1;
        }

        public static void AddMovie(Movie movie)
        {
            MoviesSeen.Add(movie);
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

        private static Movie Like(int page)
        {
            var similar = GetSimilar<Movie>(MoviesSeen[MoviesSeen.Count - 1].id);
            var foundMovie = SelectOne(similar.results);
            if (foundMovie != null) return foundMovie;
            if(similar.page < similar.total_pages)
            {
                Like(similar.page + 1);
            }
            return null;
        }

        public static Movie Like()
        {
            return Like(1);
        }

        public static Movie Dislike()
        {
            var result = MoviesSeen.Count - 1 > 0 ?
                GetSimilar<Movie>(MoviesSeen[MoviesSeen.Count - 1].id) :
                GenreSearch<Movie>(SelectedGenres);
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
