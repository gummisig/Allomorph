using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph3.Models
{
    public enum CategoryName
    {
        Movie, Episode, Cartoon, Islenskt
    }

    [Table("Category")]
    public class Category
    {
        public Category()
        {
            this.Folders = new List<Folder>();
        }

        [Key]
        public int CategoryID { get; set; }
        public CategoryName CategoryName { get; set; }

        public virtual ICollection<Folder> Folders { get; set; }
    }
}