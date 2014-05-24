using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
	[Table("SubFileLine")]
    public class SubFileLine
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int SubFileID { get; set; }
        [Required]
        public int LineNumber { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:dd:dd:dd,ddd}")]
        public string StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:dd:dd:dd,ddd}")]
        public string EndTime { get; set; }

        public virtual SubFile SubFiles { get; set; }
    }
}