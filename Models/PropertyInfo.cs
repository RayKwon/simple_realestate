using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleRealestate.Models
{
    public class PropertyInfo
    {
        public int Id { get; set; }
        public PropertyType PropertyType { get; set; }
        public int Price { get; set; }
        public int Bond { get; set; }
        public DateTime AuctionDate { get; set; }
        public DateTime AvailableDate { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<Photo> Photos { get; set; }
        public Agent Agent { get; set; }
        public Enquiry EnquiryInput { get; set; }
        
        
    }
}