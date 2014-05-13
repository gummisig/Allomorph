using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    public class RequestViewModel
    {
        public int ID { get; set; }
        public string sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchString { get; set; }
        public int? page { get; set; }

        public RequestViewModel()
        {
            this.page = 1;
        }
    }
}