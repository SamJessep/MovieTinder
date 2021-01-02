using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieTinder.helpers
{
    public static class Loader
    {
        public static async Task<ImageSource> LoadImageAsync(string imageUrl)
        {
            Uri uri;
            Uri.TryCreate(imageUrl, UriKind.Absolute, out uri);
            Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(uri));
            return await result;
        }
    }
}
