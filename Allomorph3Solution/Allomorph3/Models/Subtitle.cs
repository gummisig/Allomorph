using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    [Table("Subtitle")]
    public class Subtitle
    {
        public Subtitle()
        {
            this.SubtitleLines = new List<SubtitleLine>();
            this.DownloadCounter = 0;
            this.LastChanged = DateTime.Now;
        }
        [Key]
        public int SubtitleID { get; set; }
        [Required]
        public int UserID { get; set; }
        public int? SubtitleLineID { get; set; }
        public int DownloadCounter { get; set; }
        public string SubtitleName { get; set; }
        public DateTime LastChanged { get; set; }

        [ForeignKey("UserID")]
        public virtual User Users { get; set; }
        [ForeignKey("SubtitleLineID")]
        public virtual ICollection<SubtitleLine> SubtitleLines { get; set; }
     }
}