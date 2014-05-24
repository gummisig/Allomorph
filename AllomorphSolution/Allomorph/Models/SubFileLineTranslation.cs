using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
	[Table("SubFileLineTranslation")]
    public class SubFileLineTranslation
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int SubFileLineID { get; set; }
        [Required]
        public int LanguageID { get; set; }
        public string LineText { get; set; }

        public virtual SubFileLine SubFileLines { get; set; }
        public virtual Language Languages { get; set; }

        public SubFileLineTranslation()
        {
            this.LineText = " ";
        }
    }
}