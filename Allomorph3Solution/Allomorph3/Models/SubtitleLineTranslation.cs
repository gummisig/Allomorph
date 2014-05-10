using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    [Table("SubtitleLineTranslation")]
    public class SubtitleLineTranslation
    {
        [Key]
        public int SubtitleLineTranslationID { get; set; }
        [Required]
        public int SubtitleLineID { get; set; }
        [Required]
        public int LanguageID { get; set; }
        public string LineText { get; set; }

        [ForeignKey("SubtitleLineID")]
        public virtual SubtitleLine SubtitleLines { get; set; }
    }
}