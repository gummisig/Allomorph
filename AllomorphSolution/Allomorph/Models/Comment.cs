using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Comment")]
    public class Comment
    {
        public Comment()
        {
            DateCreated = DateTime.Now;
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User Users { get; set; }
    }
}