using Model.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.dataModels
{
    public class Configuration
    {
        public class Root
        {
            public ImageSizes images { get; set; }
            public List<string> change_keys { get; set; }
        }
    }

}
