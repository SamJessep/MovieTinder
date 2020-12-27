using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTinder.Model.models
{
    public class Result<T>
    {
        public int page;
        public List<T> results;
        public int total_pages;
        public int total_results;
    }
}
