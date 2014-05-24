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
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nafn á beiðni má ekki vera meira en 50 stafir.")]
        public string RequestName { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "Lýsing á beiðni má ekki vera meira en 120 stafir.")]
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