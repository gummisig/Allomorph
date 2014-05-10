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
        public int ID { get; set; }
        public string LanguageName { get; set; }
    }
}