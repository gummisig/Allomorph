using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    [Table("Comment")]
    public class Comment
    {
        public Comment()
        {
            this.DateCreated = DateTime.Now;
        }

        [Key]
        public int CommentID { get; set; }
        [Required]
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("UserID")]
        public virtual User Users { get; set; }
    }
}