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
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string FolderName { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Category Categories { get; set; }

        public Folder()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}