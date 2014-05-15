using Allomorph.DAL;
using Allomorph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Allomorph.Controllers
{
    public class HomeController : Controller
    {
        private SubtitleContext db = new SubtitleContext();

        public ActionResult Index()
        {
            IEnumerable<SubFile> subfiles = (from s in db.SubFiles
                                             orderby s.SubDownloadCounter descending
                                             select s).Take(10).ToList();

            IEnumerable<Request> requests = (from r in db.Requests
                                             orderby r.ReqUpvoteCounter descending
                                             select r).Take(10).ToList();

            return View(Tuple.Create(subfiles, requests));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}