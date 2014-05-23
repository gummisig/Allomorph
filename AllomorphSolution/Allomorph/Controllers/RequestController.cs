using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Allomorph.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Allomorph.Repositories;

namespace Allomorph.Controllers
{
    public class RequestController : Controller
    {
        //public RequestController()
        //{
        //    this.ApplicationDbContext = new ApplicationDbContext();
        //    this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        //}

        // Database access layer
        private FolderRepository repo = new FolderRepository();

        ///// <summary>
        ///// Application DB context
        ///// </summary>
        //protected ApplicationDbContext ApplicationDbContext { get; set; }
        ///// <summary>
        ///// User manager - attached to application DB context
        ///// </summary>
        //protected UserManager<ApplicationUser> UserManager { get; set; }

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

            var req = repo.GetAllRequests();

            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(r => (r.RequestName.ToUpper().Contains(searchString.ToUpper()) || r.RequestText.ToUpper().Contains(searchString.ToUpper())));
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
            // Fetch current users UserName
            string usr = System.Web.HttpContext.Current.User.Identity.Name;

            // reqLike is a list of all the likes associated with this request
            var reqLike = repo.GetAllLikes(requestID);

            if (reqLike != null)
            {
                // Loop to check if the current user has already liked this request
                foreach (var like in reqLike)
                {
                    if (like.LikeUserName == usr)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            // Create the like and add it to the database
            Like newLike = new Like() { RequestID = requestID, LikeUserName = usr };
            repo.AddLike(newLike);

            // Find the request
            Request request = repo.GetRequestById(requestID);
            if (request == null)
            {
                return View("NotFound");
            }
            // Add 1 to it's vote counter and save the changes
            request.ReqUpvoteCounter += 1;
            repo.Save();
            return RedirectToAction("Index");
        }

        // GET: /Request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Request request = repo.GetRequestById(id);
            if (request == null)
            {
                return View("NotFound");
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
                repo.AddRequest(request);
                repo.Save();
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
            Request request = repo.GetRequestById(id);
            if (request == null)
            {
                return View("NotFound");
            }
            return View(request);
        }

        // POST: /Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestName,RequestText,ReqUpvoteCounter,DateCreated")] Request request)
        {
            if (ModelState.IsValid)
            {
                repo.Entry(request);
                repo.Save();
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
            Request request = repo.GetRequestById(id);
            if (request == null)
            {
                return View("NotFound");
            }
            return View(request);
        }

        // POST: /Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = repo.GetRequestById(id);
            repo.RemoveRequest(request);
            repo.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
