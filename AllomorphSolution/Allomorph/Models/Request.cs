using System;
using System.Collections.Generic;
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
        }

        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        public string RequestName { get; set; }
        public string RequestText { get; set; }
        public int ReqUpvoteCounter { get; set; }
        public DateTime DateCreated { get; set; }
    }
}