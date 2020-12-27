using System.Collections.Generic;

namespace MovieTinder.Model.models
{
    public class ResultDetail<T>
    {
        public bool adult;
        public string backdrop_path;
        public bool belongs_to_collection;
        public int budget;
        public List<Genre> genres;
        public string homepage;
        public int id;
        public string imdb_id;
        public string original_language;
        public string original_title;
        public string overview;
        public float popularity;
        public string poster_path;
        public List<Company> production_companies;
        public List<Country> production_countries;
        public string release_date;
        public int revenue;
        public int runtime;
        public List<Language> spoken_languages;
        public string status;
        public string tagline;
        public string title;
        public bool video;
        public float vote_average;
        public int vote_count;
    }
}