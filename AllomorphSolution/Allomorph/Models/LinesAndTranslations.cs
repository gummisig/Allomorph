using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{

    //Used for making a IEnumerable for the TextEdit view
    public class LinesAndTranslations
    {

        public int SubFileId { get; set; }
        public int SubLineId { get; set; }
        public int LineNr { get; set; }
        public int FolderID { get; set; }
        public string EngText { get; set; }
        public string IceText { get; set; }
    }
}