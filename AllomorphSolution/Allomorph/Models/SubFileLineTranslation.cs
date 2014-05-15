using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
	[Table("SubFileLineTranslation")]
    public class SubFileLineTranslation
    {
        public int ID { get; set; }
        public int SubFileLineID { get; set; }
        public int LanguageID { get; set; }
        public string LineText { get; set; }

        public virtual SubFileLine SubFileLines { get; set; }
        public virtual Language Languages { get; set; }

        public SubFileLineTranslation()
        {
            this.LineText = "";
        }
    }
}