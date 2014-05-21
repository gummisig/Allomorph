using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    public class Like
    {
        public int ID { get; set; }
        [Required]
        public int RequestID { get; set; }
        [Required]
        public string LikeUserName { get; set; }
    }
}