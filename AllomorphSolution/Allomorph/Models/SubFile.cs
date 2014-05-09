using System;
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
        public SubFile()
        {
            this.LastChange = DateTime.Now;
            this.SubFileCounter = 0;
        }
        public int ID { get; set; }
        public int UserID { get; set; }
        public int FolderID { get; set; }
        public int SubFileCounter { get; set; }
        public string SubName { get; set; }
        public DateTime LastChange { get; set; }
        public string LastChangedByUser { get; set; }

        public virtual User Users { get; set; }
        public virtual Folder Folders { get; set; }
    }
}
