using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
	[Table("SubFileLine")]
    public class SubFileLine
    {
        public SubFileLine()
        {
            this.SubFileLineTranslations = new HashSet<SubFileLineTranslation>();
        }
        public int ID { get; set; }
        public int SubFileID { get; set; }
        public int SubFileLineTranslationID { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }

        public virtual SubFile SubFiles { get; set; }
        public virtual ICollection<SubFileLineTranslation> SubFileLineTranslations { get; set; }
    }
}