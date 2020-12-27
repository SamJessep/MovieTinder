using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTinder.Model.models
{
    public class Movie
    {
        public bool adult;
        public string backdrop_path;
        public List<int> genre_ids;
        public int id;
        public string original_language;
        public string overview;
        public string popularity;
        public string poster_path;
        public string release_date;
        public string title;
        public bool video;
        public float vote_average;
        public float vote_count;
    }
}
