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
        public int ID { get; set; }
        public int SubFileID { get; set; }
        public int LineNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public virtual SubFile SubFiles { get; set; }

    }
}