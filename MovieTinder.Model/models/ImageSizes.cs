using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Model.models
{
    public class ImageSizes
    {
        private static List<string> defaultSize = new List<string> { "original" };
        public string base_url { get; set; }
        public string secure_base_url { get; set; }
        public List<string> backdrop_sizes { get; set; } = defaultSize;
        public List<string> logo_sizes { get; set; } = defaultSize;
        public List<string> poster_sizes { get; set; } = defaultSize;
        public List<string> profile_sizes { get; set; } = defaultSize;
        public List<string> still_sizes { get; set; } = defaultSize;



        public enum ImageType
        {
            Backdrop,
            Logo,
            Poster,
            Profile,
            Still
        }
        public Dictionary<string, string> GetAllBestSizes(uint w)
        {
            Dictionary<string, string> sizes = new Dictionary<string, string>();
            foreach(ImageType type in Enum.GetValues(typeof(ImageType)))
            {
                sizes.Add(type.ToString("g"), GetBestSize(w, type));
            }
            return sizes;
        }
        public string GetBestSize(uint w, ImageType type)
        {
            var possibleSizesList = GetSizeList(type);
            if (possibleSizesList == null) return null;
            possibleSizesList = possibleSizesList
                .Where(s => s != "original")
                .ToList()
                .Where(s => GetIntSize(s) > w)
                .ToList();
            possibleSizesList.Sort((a, b) => GetIntSize(a) < GetIntSize(b) ? -1 : 1);
            return possibleSizesList.DefaultIfEmpty("original").First();
        }

        private List<string> GetSizeList(ImageType type)
        {
            switch (type)
            {
                case ImageType.Backdrop:
                    return backdrop_sizes;
                case ImageType.Logo:
                    return logo_sizes;
                case ImageType.Poster:
                    return poster_sizes;
                case ImageType.Profile:
                    return profile_sizes;
                case ImageType.Still:
                    return still_sizes;
                default:
                    return null;
            }
        }
        private int GetIntSize(string Dimension) => int.Parse(new Regex("[wh]").Replace(Dimension, string.Empty));
    }
}
