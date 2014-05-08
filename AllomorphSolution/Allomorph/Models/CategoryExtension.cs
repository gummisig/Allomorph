using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
	[Table("CategoryExtension")]
    public class CategoryExtension
    {
        public int ID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubFileID { get; set; }

        public virtual Category Categories { get; set; }
        public virtual SubFile SubFiles { get; set; }
    }
}