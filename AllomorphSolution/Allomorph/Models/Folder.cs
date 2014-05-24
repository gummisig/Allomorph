using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    [Table("Folder")]
    public class Folder
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Er ekki hægt að setja þetta í einhvern flokk?")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Skráin hlýtur að hafa nafn.")]
        [StringLength(100, ErrorMessage = "Nafn á möppu má ekki vera meira en 100 stafir.")]
        public string FolderName { get; set; }

        [Required(ErrorMessage = "T.d. slóð inn á IMDB.com eða Kvikmyndir.is")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Nennirðu að reyna að finna góða mynd?")]
        public string Poster { get; set; }

        [Required(ErrorMessage = "Bara einhver stutt lýsing.")]
        [StringLength(500, ErrorMessage = "Lýsing má ekki vera meira en 500 stafir.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public virtual Category Categories { get; set; }

        public Folder()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}