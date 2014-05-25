using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    public class TimeViewModel
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Milliseconds { get; set; }
        public bool Negative { get; set; }

        public TimeViewModel()
        {
            this.Hours = 0;
            this.Minutes = 0;
            this.Seconds = 0;
            this.Milliseconds = 0;
            this.Negative = false;
        }
    }
}