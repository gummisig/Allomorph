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
            IEnumerable<SubFile> subfiles = db.SubFiles.OrderByDescending(s => s.SubDownloadCounter).Take(10);
            IEnumerable<Request> requests = db.Requests.OrderByDescending(r => r.ReqUpvoteCounter).Take(10);
            return View(Tuple.Create(subfiles, requests));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Um Allomorph.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Hafðu samband.";
            return View();
        }
    }
}