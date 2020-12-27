using System;
using System.Collections.Generic;
using System.Linq;
using MovieTinder.Model;
using MovieTinder.Model.models;

namespace MovieTinder.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var genres = MovieApi.GetGenres().genres;
            foreach(var genre in genres)
            {
                Console.WriteLine($"{genre.name} : {genre.id}");
            }
            Console.Write("enter a genre id: ");
            var selectedGenreIndex = int.Parse(Console.ReadLine());
            var first = MovieApi.Start(new List<Genre> { genres[selectedGenreIndex] });
            var second = MovieApi.Like();
            var third = MovieApi.Dislike();
        }
    }
}
