﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Language")]
    public class Language
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string LanguageName { get; set; }
    }
}