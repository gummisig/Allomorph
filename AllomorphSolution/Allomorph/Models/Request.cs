using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Request")]
    public class Request
    {
        public Request()
        {
            this.DateCreated = DateTime.Now;
            this.ReqUpvoteCounter = 0;
            this.UserName = "allomorph gestur";
        }

        public int ID { get; set; }
        public string UserName { get; set; }
        [Required]
        public string RequestName { get; set; }
        [Required]
        public string RequestText { get; set; }
        public int ReqUpvoteCounter { get; set; }
        public DateTime DateCreated { get; set; }
    }
}