using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Request")]
    public class Request
    {
        public int ID { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Þú verður að velja nafn.")]
        [StringLength(100, ErrorMessage = "Nafn á beiðni má ekki vera meira en 100 stafir.")]
        public string RequestName { get; set; }

        [Required(ErrorMessage = "T.d. slóð inn á IMDB.com eða inn á síðu með enskum texta.")]
        [StringLength(500, ErrorMessage = "Lýsing á beiðni má ekki vera meira en 500 stafir.")]
        public string RequestText { get; set; }
        public int ReqUpvoteCounter { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public Request()
        {
            this.DateCreated = DateTime.Now;
            this.ReqUpvoteCounter = 0;
            this.UserName = "allomorph gestur";
        }
    }
}