using System;
using System.Collections.Generic;
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
            this.SubFileLines = new HashSet<SubFileLine>();
            this.CategoryExtensions = new HashSet<CategoryExtension>();
            this.LastChange = DateTime.Now;
            //this.LastChangedByUser = this.Users.UserName;
        }
        
        public int ID { get; set; }
        public int? FolderID { get; set; }
        public int? UserID { get; set; }
        public string SubName { get; set; }
        public DateTime LastChange { get; set; }
        public string LastChangedByUser { get; set; }

        public virtual Folder Folders { get; set; }
        public virtual User Users { get; set; }
        public virtual ICollection<SubFileLine> SubFileLines { get; set; }
        public virtual ICollection<CategoryExtension> CategoryExtensions { get; set; }
    }
}
