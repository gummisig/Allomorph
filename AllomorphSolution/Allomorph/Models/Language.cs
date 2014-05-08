using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Language")]
    public class Language
    {
        public Language()
        {
            this.SubFileLineTranslations = new HashSet<SubFileLineTranslation>();
        }

        public int ID { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<SubFileLineTranslation> SubFileLineTranslations { get; set; }
    }
}