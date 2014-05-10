using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            //this.CategoryExtensions = new List<CategoryExtension>();
            this.Folders = new List<Folder>();
        }

        public int ID { get; set; }
        //public int CategoryExtensionID { get; set; }
        public string CategoryName { get; set; }
        public int FolderID { get; set; }

       // public virtual ICollection<CategoryExtension> CategoryExtensions { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
    }
}