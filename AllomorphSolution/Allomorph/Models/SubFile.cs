﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("SubFile")]
    public class SubFile
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int FolderID { get; set; }
        [Required]
        public int LanguageID { get; set; }
        public int SubDownloadCounter { get; set; }

        [Required]
        public string SubName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastChange { get; set; }

        public virtual Folder Folders { get; set; }
        public virtual ICollection<Language> Languages { get; set; }

        public SubFile()
        {
            this.LastChange = DateTime.Now;
            this.SubDownloadCounter = 0;
            this.Languages = new List<Language>();
        }
    }
}
