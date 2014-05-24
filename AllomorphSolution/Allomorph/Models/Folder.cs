using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Folder")]
    public class Folder
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nafn á möppu má ekki vera meira en 50 stafir.")]
        public string FolderName { get; set; }

        [Required]
        public string Link { get; set; }
        [Required]
        public string Poster { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Lýsing má ekki vera meira en 500 stafir.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public virtual Category Categories { get; set; }

        public Folder()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}