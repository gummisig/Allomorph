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

        public ViewResult Index(SearchViewModel svm)
        {
            string sortOrder = svm.sortOrder;
            string currentFilter = svm.currentFilter;
            string searchString = svm.searchString;
            int? page = svm.page;
            int category = svm.ID;
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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
                case "date_desc":
                    folders = folders.OrderByDescending(s => s.DateCreated);
                    break;
                case "date_asc":
                    folders = folders.OrderBy(s => s.DateCreated);
                    break;
                default:  // Name ascending 
                    folders = folders.OrderBy(s => s.FolderName);
                    break;
            }
            int pageSize = 10;
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
            IEnumerable<SubFile> subtitles = (from s in db.SubFiles
                                              where s.FolderID == folder.ID
                                              select s).ToList();

            IEnumerable<Comment> comment = (from c in db.Comments
                                           where c.FolderID == folder.ID
                                           select c).ToList();

            return View(Tuple.Create(folder, subtitles, comment));
        }

        // GET: /Folder/Create
        [Authorize]
        public ActionResult Create(string reqName = "")
        {
            ViewBag.newName = reqName;
            return View(Tuple.Create(new Folder(), new SubFile()));
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
                //try
                //{
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
                        tempLine.SubFileID = subfile.ID;

                        db.SubFileLines.Add(tempLine);
                        db.SaveChanges();

                        tempTranslation.SubFileLineID = tempLine.ID;
                        tempTranslation.LineText = text;
                        
                        if(db.Languages.Find(1) == null)
                        {
                            Language Enska = new Language() { LanguageName = "English" };
                            db.Languages.Add(Enska);
                            db.SaveChanges();
                        }
                        tempTranslation.LanguageID = 1;

                        db.SubFileLineTranslations.Add(tempTranslation);
                        db.SaveChanges();

                        i++;
                    }



                    /*List<Text> texts = context.Texts.ToList();
                    if (texts.Count == 0)
                    {
                        return Redirect("http://localhost:40272/Home/");
                    }
                    return View(texts);
                    
                }
                catch (Exception e)
                {
                    ViewBag.Message = "ERROR:" + e.Message.ToString();
                } */

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

        [Authorize]
        public ActionResult CreateComment(int? id)
        {
            ViewBag.folderid = id;
            return View(new Comment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "ID,UserName,FolderID,CommentText,DateCreated")] Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                string usr = System.Web.HttpContext.Current.User.Identity.Name;
                if (usr == null)
                {
                    return RedirectToAction("Details", new { ID = id });
                }
                Folder folder = db.Folders.Find(id);
                if (folder == null)
                {
                    return HttpNotFound();
                }
                comment.FolderID = folder.ID;
                comment.UserName = usr;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", new { ID = id });
            }
            return View(comment);

            //return View(request);
            //String strUser = null;

            //if(user != null){
            //    strUser = user.UserName;
            //}
            //else {
            //    strUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //}

            //if (!String.IsNullOrEmpty(strUser))
            //{
            //    int slashPos = strUser.IndexOf("\\");
            //    if (slashPos != -1)
            //    {
            //        strUser = strUser.Substring(slashPos + 1);
            //    }
            //    comm.UserName = user.ID;

            //    db.Comments.Add(comm);
            //}

            //var comments = db.Comments;

            //var newResult = from c in comments
            //               // where c.FolderID
            //                select new
            //                {
            //                    CommentDate = c.DateCreated.ToString(),
            //                    ID = c.ID,
            //                    CommentText = c.CommentText,
            //                    Username = user.UserName
            //                };

            //return Json(newResult, JsonRequestBehavior.AllowGet);
        }

        public FileStreamResult GetFile(int id)
        {
            IEnumerable<SubFile> file = from s in db.SubFiles
                       where id == s.ID
                       select s;

            FileInfo info = new FileInfo(file.First().SubName);

            IEnumerable<SubFileLine> lines = from t in db.SubFileLines
                                             where t.SubFileID == id
                                             select t;




            if (!info.Exists)
            {
                using (StreamWriter writer = info.CreateText())
                {
                    foreach(var line in lines)
                    {
                        writer.WriteLine(line.LineNumber);
                        writer.Write(line.StartTime);
                        writer.Write(" --> ");
                        writer.WriteLine(line.EndTime);
                        //TODO: get text

                        //End of Textblock
                        writer.WriteLine("");
                    }
                        

                }
            }

            return File(info.OpenRead(), "text/plain", file.First().SubName);
        }
    }
}
