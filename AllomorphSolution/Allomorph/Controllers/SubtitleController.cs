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

namespace Allomorph.Controllers
{
    public class SubtitleController : Controller
    {
        private SubtitleContext db = new SubtitleContext();

        // GET: /Subtitle/
        public ActionResult Index()
        {
            var subfiles = db.SubFiles.Include(s => s.Folders).Include(s => s.Users);
            return View(subfiles.ToList());
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,FolderID,UserID,SubName,LastChange,LastChangedByUser")] SubFile subfile)
        {
            if (ModelState.IsValid)
            {
                db.SubFiles.Add(subfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", subfile.FolderID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", subfile.UserID);
            return View(subfile);
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
        public ActionResult Edit([Bind(Include="ID,FolderID,UserID,SubName,LastChange,LastChangedByUser")] SubFile subfile)
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
