using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    //Used for making a IEnumerable for the TextEdit view
    public class LinesAndTranslations
    {
        public int SubFileID { get; set; }
        public int SubFileLineID { get; set; }
        public int LineNumber { get; set; }
        public int FolderID { get; set; }
        public string EngText { get; set; }
        public string IceText { get; set; }
        public string SubFileLineStartTime { get; set; }
        public string SubFileLineEndTime { get; set; }
    }
}