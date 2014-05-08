using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("User")]
    public class User
    {
        public User()
        {
            this.Requests = new HashSet<Request>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}