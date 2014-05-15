using System;
using System.Collections.Generic;
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
        public string Link { get; set; }
        public string Poster { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Category Categories { get; set; }

        public Folder()
        {
            this.DateCreated = DateTime.Now;
            this.Link = " ";
            this.Poster = " ";
            this.Description = " ";
        }

        public int? RequestID { get; set; }
    }
}