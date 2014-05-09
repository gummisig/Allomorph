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

namespace Allomorph.Controllers
{
    public class SubtitleController : Controller
    {
        private SubtitleContext db = new SubtitleContext();

        public ViewResult Search(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var folders = from s in db.Folders
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                folders = folders.Where(s => s.FolderName.ToUpper().Contains(searchString.ToUpper()));
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);

            return View(folders.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Subtitle/
        public ActionResult Index()
        {
            var folders = db.Folders;
            return View(folders.ToList());
        }

        // GET: /Subtitle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubFile subfile = db.SubFiles.Find(id);
            if (subfile == null)
            {
                return HttpNotFound();
            }
            return View(subfile);
        }

        // GET: /Subtitle/Create
        public ActionResult Create()
        {
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName");
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName");
            return View();
        }

        // POST: /Subtitle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="ID,UserID,FolderID,SubFileLineID,SubName,LastChange,LastChangedByUser")] SubFile subfile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SubFiles.Add(subfile);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", subfile.FolderID);
        //    ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", subfile.UserID);
        //    return View(subfile);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FolderName,Link,Poster,Description")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Folders.Add(folder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(folder);
        }

        // GET: /Subtitle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubFile subfile = db.SubFiles.Find(id);
            if (subfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", subfile.FolderID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", subfile.UserID);
            return View(subfile);
        }

        // POST: /Subtitle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,UserID,FolderID,SubFileLineID,SubName,LastChange,LastChangedByUser")] SubFile subfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", subfile.FolderID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", subfile.UserID);
            return View(subfile);
        }

        // GET: /Subtitle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubFile subfile = db.SubFiles.Find(id);
            if (subfile == null)
            {
                return HttpNotFound();
            }
            return View(subfile);
        }

        // POST: /Subtitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubFile subfile = db.SubFiles.Find(id);
            db.SubFiles.Remove(subfile);
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
