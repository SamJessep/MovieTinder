using Model.models;
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

        public string PosterURL => $"https://image.tmdb.org/t/p/original/{poster_path}";
        public string GetImageURL(string size, ImageSizes.ImageType type)
        {
            var path = "";
            switch (type)
            {
                case ImageSizes.ImageType.Backdrop:
                    path = backdrop_path;
                    break;
                case ImageSizes.ImageType.Poster:
                    path = poster_path;
                    break;
            }
            return $"https://image.tmdb.org/t/p/{size}/{path}";
        }
    }

}
