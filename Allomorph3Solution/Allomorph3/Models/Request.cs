using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    [Table("Request")]
    public class Request
    {
        public Request()
        {
            this.DateCreated = DateTime.Now;
            this.UpvoteCounter = 0;
        }

        [Key]
        public int RequestID { get; set; }
        public int? UserID { get; set; }
        public string RequestText { get; set; }
        public int UpvoteCounter { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User Users { get; set; }
    }
}