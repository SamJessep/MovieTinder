using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTinder.Model;
using MovieTinder.Model.models;

namespace MovieTinder.ConsoleTest
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Hello World!");
            uint ScreenWidth = 500;
            var api = MovieApi.Instance;

            //var genreResult = await MovieApi.GetGenresAsync();
            //foreach (var genre in genreResult.genres)
            //{
            //    Console.WriteLine($"{genre.name} : {genre.id}");
            //}
            ////Console.Write("enter a genre id: ");
            ////var selectedGenreIndex = int.Parse(Console.ReadLine());
            //var first = await MovieApi.StartAsync(new List<Genre>() { new Genre() { id = 16, name = "Animation" } });
            //for (var i = 0; i < 5; i++)
            //{
            //    Console.WriteLine("like: " + MovieApi.LikeAsync().GetAwaiter().GetResult().title);
            //}
        }
    }
}
