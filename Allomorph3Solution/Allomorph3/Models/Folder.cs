using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    [Table("Folder")]
    public class Folder
    {
        public Folder()
        {
            //this.Languages = new List<Language>();
            //this.Subtitles = new List<Subtitle>();
        }
        [Key]
        public int FolderID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int LanguageID { get; set; }
        [Required]
        public int SubtitleID { get; set; }
        public string FolderName { get; set; }
        public string Link { get; set; }
        public string Poster { get; set; }
        public string Description { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Categorys { get; set; }
        [ForeignKey("LanguageID")]
        public virtual Language Languages { get; set; }
        [ForeignKey("SubtitleID")]
        public virtual Subtitle Subtitles { get; set; }
    }
}