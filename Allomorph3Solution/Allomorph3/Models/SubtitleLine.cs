using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    [Table("SubtitleLine")]
    public class SubtitleLine
    {
        public SubtitleLine()
        {
            this.SubtitleLineTranslations = new List<SubtitleLineTranslation>();
        }
        [Key]
        public int SubtitleLineID { get; set; }
        [Required]
        public int SubtitleID { get; set; }
        public int? SubtitleLineTranslationID { get; set; }
        public int? LineNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [ForeignKey("SubtitleID")]
        public virtual Subtitle Subtitles { get; set; }
        [ForeignKey("SubtitleLineTranslationID")]
        public virtual ICollection<SubtitleLineTranslation> SubtitleLineTranslations { get; set; }
    }
}