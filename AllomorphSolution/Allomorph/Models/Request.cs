using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Request")]
    public class Request
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string RequestText { get; set; }
        public int Counter { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User Users { get; set; }

        public Request()
        {
            this.DateCreated = DateTime.Now;
            this.Counter = 0;
        }

        //public IEnumerable<Request> GetEnumerator()
        //{
        //    return Requests.GetEnumerator();
        //}
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return Requests.GetEnumerator();
        //}
    }
}