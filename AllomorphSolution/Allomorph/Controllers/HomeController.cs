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
            return View();
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

        public ActionResult Request()
        {
            return View();
        }

        // POST: /Home/Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Request([Bind(Include = "RequestText")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Request");
            }

            return View(request);
        }
    }
}