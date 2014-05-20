using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Allomorph.Models;
using Allomorph.DAL;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Allomorph.Controllers
{
    public class RequestController : Controller
    {
        public RequestController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        private SubtitleContext db = new SubtitleContext();
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        // GET: /Request/
        public ViewResult Index(RequestViewModel rvm)
        {
            string sortOrder = rvm.sortOrder;
            string currentFilter = rvm.currentFilter;
            string searchString = rvm.searchString;
            int? page = rvm.page;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.VoteSortParm = String.IsNullOrEmpty(sortOrder) ? "vote_asc" : "";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var req = from s in db.Requests
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                req = (from s in db.Requests
                      where (s.RequestName.ToUpper().Contains(searchString.ToUpper()))
                      || (s.RequestText.ToUpper().Contains(searchString.ToUpper()))
                      select s);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    req = req.OrderByDescending(s => s.RequestName);
                    break;
                case "date_desc":
                    req = req.OrderByDescending(s => s.DateCreated);
                    break;
                case "date_asc":
                    req = req.OrderBy(s => s.DateCreated);
                    break;
                case "name_asc":
                    req = req.OrderBy(s => s.RequestName);
                    break;
                case "vote_asc":
                    req = req.OrderBy(s => s.ReqUpvoteCounter);
                    break;
                default:  // Votes descending
                    req = req.OrderByDescending(s => s.ReqUpvoteCounter);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(req.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        [ValidateInput(false)]
        public ActionResult RequestVote(int requestID)
        {
            string usr = System.Web.HttpContext.Current.User.Identity.Name;

            var reqLike = db.Likes.Where(s => s.RequestID == requestID);

            if (reqLike != null)
            {
                foreach (var like in reqLike)
                {
                    if (like.LikeUserName == usr)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            Like newLike = new Like() { RequestID = requestID, LikeUserName = usr };
            db.Likes.Add(newLike);

            var request = db.Requests.Find(requestID);
            if (request != null)
            {
                request.ReqUpvoteCounter += 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult RequestVote(Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        request.ReqUpvoteCounter += 1;
        //        db.SaveChanges();
        //    }
        //    return View("Index");
        //}

        // GET: /Request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: /Request/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,UserName,RequestName,RequestText")] Request request)
        {
            if (ModelState.IsValid)
            {
                string usr = System.Web.HttpContext.Current.User.Identity.Name;
                if (usr != null)
                {
                    request.UserName = usr;
                }
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: /Request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: /Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,RequestName,RequestText,ReqUpvoteCounter,DateCreated")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: /Request/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: /Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
