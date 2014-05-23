using Allomorph.Models;
using Allomorph.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Allomorph.Controllers
{
    public class HomeController : Controller
    {
        // Database access layer
        private FolderRepository repo = new FolderRepository();

        public ActionResult Index()
        {
            IEnumerable<SubFile> subfiles = repo.GetAllSubFiles().OrderByDescending(s => s.SubDownloadCounter).Take(10);
            IEnumerable<Request> requests = repo.GetAllRequests().OrderByDescending(r => r.ReqUpvoteCounter).Take(10);
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