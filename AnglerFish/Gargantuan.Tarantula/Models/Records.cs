using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gargantuan.Tarantula.Models
{
    public class Record
    {
        public Record(string name,string country)
        {
            Name = name;
            Country = country;
        }

        public string Name { get; set; }
        public string Country { get; set; }
    }
}