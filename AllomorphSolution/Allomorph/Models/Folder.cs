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
        public Folder()
        {
            this.Subtitles = new HashSet<SubFile>();
        }
        public int ID { get; set; }
        public int SubFileID { get; set; }
        public string FolderName { get; set; }
        public string Link { get; set; }
        public string Poster { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SubFile> Subtitles { get; set; }
    }
}