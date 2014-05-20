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
            this.SubDownloadCounter = 0;
            this.Languages = new List<Language>();
            //this.LastChangedByUser = System.Web.HttpContext.Current.User.Identity.Name;
        }
        public int ID { get; set; }
        public int FolderID { get; set; }
        public int LanguageID { get; set; }
        public int SubDownloadCounter { get; set; }
        public string SubName { get; set; }
        public DateTime LastChange { get; set; }
        //public string LastChangedByUser { get; set; }

        public virtual Folder Folders { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
    }
}
