using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleRealestate.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PropertyId { get; set; }
    }
}