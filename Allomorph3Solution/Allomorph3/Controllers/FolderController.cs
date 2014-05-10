using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Allomorph3.Models;
using Allomorph3.DAL;

namespace Allomorph3.Controllers
{
    public class FolderController : Controller
    {
        private SubtitleContext db = new SubtitleContext();

        // GET: /Folder/
        public ActionResult Index()
        {
            var folders = db.Folders.Include(f => f.Categorys).Include(f => f.Languages).Include(f => f.Subtitles);
            return View(folders.ToList());
        }

        // GET: /Folder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // GET: /Folder/Create
        public ActionResult Create()
        {
            ViewBag.FolderID = new SelectList(db.Categorys, "CategoryID", "CategoryID");
            ViewBag.FolderID = new SelectList(db.Languages, "LanguageID", "LanguageName");
            ViewBag.FolderID = new SelectList(db.Subtitles, "SubtitleID", "SubtitleID");
            return View();
        }

        // POST: /Folder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FolderID,CategoryID,LanguageID,SubtitleID,FolderName,Link,Poster,Description")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Folders.Add(folder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FolderID = new SelectList(db.Categorys, "CategoryID", "CategoryID", folder.FolderID);
            ViewBag.FolderID = new SelectList(db.Languages, "LanguageID", "LanguageName", folder.FolderID);
            ViewBag.FolderID = new SelectList(db.Subtitles, "SubtitleID", "SubtitleID", folder.FolderID);
            return View(folder);
        }

        // GET: /Folder/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            ViewBag.FolderID = new SelectList(db.Categorys, "CategoryID", "CategoryID", folder.FolderID);
            ViewBag.FolderID = new SelectList(db.Languages, "LanguageID", "LanguageName", folder.FolderID);
            ViewBag.FolderID = new SelectList(db.Subtitles, "SubtitleID", "SubtitleID", folder.FolderID);
            return View(folder);
        }

        // POST: /Folder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="FolderID,CategoryID,LanguageID,SubtitleID,FolderName,Link,Poster,Description")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(folder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FolderID = new SelectList(db.Categorys, "CategoryID", "CategoryID", folder.FolderID);
            ViewBag.FolderID = new SelectList(db.Languages, "LanguageID", "LanguageName", folder.FolderID);
            ViewBag.FolderID = new SelectList(db.Subtitles, "SubtitleID", "SubtitleID", folder.FolderID);
            return View(folder);
        }

        // GET: /Folder/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // POST: /Folder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Folder folder = db.Folders.Find(id);
            db.Folders.Remove(folder);
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
