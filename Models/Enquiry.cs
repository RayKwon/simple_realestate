using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleRealestate.Models
{
    public class Enquiry
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Email field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage="Comment field is required")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public int PropertyId { get; set; }
    }
}