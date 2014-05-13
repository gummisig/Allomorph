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
using System.IO;

namespace Allomorph.Controllers
{
    public class FolderController : Controller
    {
        private SubtitleContext db = new SubtitleContext();

        //public ViewResult SearchView()
        //{
        //    return View(new SearchViewModel());
        //}

        //[HttpPost]
        //public ViewResult SearchView(SearchViewModel model)
        //{
        //    string sortOrder = model.sortOrder;
        //    string currentFilter = model.currentFilter;
        //    string searchString = model.searchString;
        //    int? page = model.page;
        //    int? category = model.category;

        //    if (model.checkbox1 == true)
        //    {
        //        category = 1;
        //    }
        //    else if (model.checkbox2 == true)
        //    {
        //        category = 2;
        //    }
        //    else if (model.checkbox3 == true)
        //    {
        //        category = 3;
        //    }
        //    else if (model.checkbox4 == true)
        //    {
        //        category = 4;
        //    }

        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;
        //    ViewBag.CategoryFilter = category;

        //    var folders = from s in db.Folders
        //                  select s;

        //    switch (category ?? 0)
        //    {
        //        case 1:
        //            folders = from s in folders
        //                      where s.CategoryID == 1
        //                      select s;
        //            break;
        //        case 2:
        //            folders = from s in folders
        //                      where s.CategoryID == 2
        //                      select s;
        //            break;
        //        case 3:
        //            folders = from s in folders
        //                      where s.CategoryID == 3
        //                      select s;
        //            break;
        //        case 4:
        //            folders = from s in folders
        //                      where s.CategoryID == 4
        //                      select s;
        //            break;
        //    }

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        folders = folders.Where(s => s.FolderName.ToUpper().Contains(searchString.ToUpper()));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            folders = folders.OrderByDescending(s => s.FolderName);
        //            break;
        //        default:  // Name ascending 
        //            folders = folders.OrderBy(s => s.FolderName);
        //            break;
        //    }
        //    int pageSize = 5;
        //    int pageNumber = (page ?? 1);
        //    return View(folders.ToPagedList(pageNumber, pageSize));
        //}

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, bool checkbox1 = false, bool checkbox2 = false, bool checkbox3 = false, bool checkbox4 = false)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (checkbox1 == true)
            {
                ViewBag.CategoryFilter = 1;
            }
            else if (checkbox2 == true)
            {
                ViewBag.CategoryFilter = 2;
            }
            else if (checkbox3 == true)
            {
                ViewBag.CategoryFilter = 3;
            }
            else if (checkbox4 == true)
            {
                ViewBag.CategoryFilter = 4;
            }
            ViewBag.CurrentFilter = searchString;
            int category = ViewBag.CategoryFilter ?? 0;

            var folders = from s in db.Folders
                          select s;

            switch (category)
            {
                case 1:
                    folders = from s in folders
                              where s.CategoryID == 1
                              select s;
                    break;
                case 2:
                    folders = from s in folders
                              where s.CategoryID == 2
                              select s;
                    break;
                case 3:
                    folders = from s in folders
                              where s.CategoryID == 3
                              select s;
                    break;
                case 4:
                    folders = from s in folders
                              where s.CategoryID == 4
                              select s;
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                folders = folders.Where(s => s.FolderName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    folders = folders.OrderByDescending(s => s.FolderName);
                    break;
                default:  // Name ascending 
                    folders = folders.OrderBy(s => s.FolderName);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(folders.ToPagedList(pageNumber, pageSize));
            //return View(folders.ToList());
        }

        // GET: /Folder/Details/5
        public ActionResult Details(int? id)
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

        // GET: /Folder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Folder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryID,FolderName,Link,Poster,Description")] Folder folder, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                db.Folders.Add(folder);
                db.SaveChanges();
                try
                {
                    // read from file or write to file
                    StreamReader streamReader = new StreamReader(file.InputStream);
                    int i = 1;
                    // Needs more stuff to add like for example language, user ID
                    SubFile subfile = new SubFile { FolderID = folder.ID, LastChange = DateTime.Now, SubName = file.FileName};
                    db.SubFiles.Add(subfile);
                    db.SaveChanges();
                    while (streamReader.Peek() != -1)
                    {
                        SubFileLine tempLine = new SubFileLine();
                        SubFileLineTranslation tempTranslation = new SubFileLineTranslation();
                        

                        string lineNumber = streamReader.ReadLine();
                        string timeLine = streamReader.ReadLine();

                        string firstTime = timeLine.Substring(0, 12);
                        string secondTime = timeLine.Substring(17);

                        string text = streamReader.ReadLine();
                        string nextline = streamReader.ReadLine();

                        if (nextline != "")
                        {
                            text += "\n" + nextline;
                            streamReader.ReadLine();
                        }

                        tempLine.LineNumber = Convert.ToInt32(lineNumber);
                        tempLine.StartTime = firstTime;
                        tempLine.EndTime = secondTime;

                        db.SubFileLines.Add(tempLine);
                        db.SaveChanges();

                        tempTranslation.SubFileLineID = tempLine.ID;
                        tempTranslation.LineText = text;
                        i++;

                        db.SubFileLineTranslations.Add(tempTranslation);
                        db.SaveChanges();
                    }



                    /*List<Text> texts = context.Texts.ToList();
                    if (texts.Count == 0)
                    {
                        return Redirect("http://localhost:40272/Home/");
                    }
                    return View(texts);
                    */
                }
                catch (Exception e)
                {
                    ViewBag.Message = "ERROR:" + e.Message.ToString();
                } 
                return RedirectToAction("Index");
            }

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
            return View(folder);
        }

        // POST: /Folder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryID,FolderName,Link,Poster,Description")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(folder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
