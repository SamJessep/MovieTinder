using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTinder.Model.models
{
    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
