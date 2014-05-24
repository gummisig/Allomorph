using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int FolderID { get; set; }

        [Required(ErrorMessage = "Nothing is just nothing.")]
        [StringLength(160, ErrorMessage = "Athugasemd má ekki vera meira en 160 stafir.")]
        public string CommentText { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public Comment()
        {
            DateCreated = DateTime.Now;
        }
    }
}