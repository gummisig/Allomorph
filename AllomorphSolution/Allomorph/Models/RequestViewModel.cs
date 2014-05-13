using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allomorph.Models
{
    public class RequestViewModel
    {
        public Request Requests { get; set; }
        public IEnumerable<Request> RequestList { get; set; }
    }
}