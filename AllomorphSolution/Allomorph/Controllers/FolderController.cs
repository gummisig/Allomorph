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
        // Database
        private SubtitleContext db = new SubtitleContext();

        // ~/Folder/Index == Yfirlit texta
        public ViewResult Index(SearchViewModel svm)
        {
            string sortOrder = svm.sortOrder;
            string currentFilter = svm.currentFilter;
            string searchString = svm.searchString;
            int? page = svm.page;
            int category = svm.ID;
            
            // Til að flokka eftir nafni á texta eða dagsetningunni sem hann var sendur inn
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            // Alltaf sýna síðu 1 ef það er notað leitina
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

            ViewBag.folderCount = folders.Count();

            // Ef það er leitað eftir flokki
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

            // Leitað eftir nafni á möppu
            if (!String.IsNullOrEmpty(searchString))
            {
                folders = folders.Where(s => s.FolderName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Flokka eftir nafni eða dagsetningu
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

            // Birtir 25 texta á hverri síðu
            int pageSize = 10;
            // pageNumber er sjálfgefið 1 ef engin síða er valin
            int pageNumber = (page ?? 1);
            return View(folders.ToPagedList(pageNumber, pageSize));
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

            IEnumerable<Folder> folders = (from f in db.Folders
                                           where f.ID == folder.ID
                                           select f).ToList();

            return View(Tuple.Create(folder, subtitles, comment, folders));
        }

        // GET: /Folder/Create
        [Authorize]
        public ActionResult Create(int? requestID)
        {
            ViewBag.request = db.Requests.Find(requestID);
            ViewBag.requestId = requestID;
            return View(new Folder());
        }

        // POST: /Folder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryID,FolderName,Link,Poster,Description,RequestID")] Folder folder, HttpPostedFileBase file, SubFile subfile, int? requestID)
        {
            if (ModelState.IsValid)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension != ".srt")
                {
                    return View("WrongFile");
                }
                // Geymir gögnin úr textaskránni sem er send inn
                StreamReader streamReader = new StreamReader(file.InputStream);

                // Tengja textaskrána við möppu
                subfile.FolderID = folder.ID;
                subfile.SubName = file.FileName;
                db.Folders.Add(folder);
                db.SubFiles.Add(subfile);

                // .Peek() skoðar næsta tákn án þess að taka það úr
                // -1 Þegar öll skráin hefur verið lesin
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

                    // Til að ná öllum textalínum (þ.e. línur sem innihalda lesin texta)
                    while (nextline != "")
                    {
                        if (nextline == null)
                        {
                            break;
                        }
                        text += "\n" + nextline;
                        nextline = streamReader.ReadLine();
                    }

                    // Tekur inn allar upplýsingar fyrir línumódelið
                    tempLine.LineNumber = Convert.ToInt32(lineNumber);
                    tempLine.StartTime = firstTime;
                    tempLine.EndTime = secondTime;
                    tempLine.SubFileID = subfile.ID;

                    // Setja línuna í gagnagrunninn
                    db.SubFileLines.Add(tempLine);

                    // Tengja línurnar við textana (þýðingarnar)
                    tempTranslation.SubFileLineID = tempLine.ID;
                    tempTranslation.LineText = text;
                    tempTranslation.LanguageID = subfile.LanguageID;
                    
                    // Setja textann í gagnagrunninn og vista breytingarnar
                    db.SubFileLineTranslations.Add(tempTranslation);
                    db.SaveChanges();
                }
                // requestID != null ef verið er að uppfylla beiðni
                if (requestID != null)
                {
                    // Ná í beiðni úr gagnagrunni
                    var req = db.Requests.Find(requestID);
                    // Villumeðhöndlun
                    if (req != null)
                    {
                        // Eyða beiðni úr gagnagrunni og vista breytingar
                        db.Requests.Remove(req);
                        db.SaveChanges();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(folder);
        }

        // GET: /Folder/Edit/5
        [Authorize]
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
        [Authorize]
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
            // Geyma 'id' svo hægt sé að nálgast það í 'View-inu'
            ViewBag.folderid = id;
            return View(new Comment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "ID,UserName,FolderID,CommentText")] Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                // Sækja notendanafn þess sem skráði athugasemd
                string usr = System.Web.HttpContext.Current.User.Identity.Name;

                // Öryggisráðstafanir
                if (usr == null)
                {
                    return RedirectToAction("Details", new { ID = id });
                }
                // Sækja möppuna þar sem athugasemdin var skráð
                Folder folder = db.Folders.Find(id);
                if (folder == null)
                {
                    return HttpNotFound();
                }
                
                // Tengja athugasemdina við möppuna
                comment.FolderID = folder.ID;
                comment.UserName = usr;

                // Setja athugasemdina í gagnagrunninn og vista breytingar
                db.Comments.Add(comment);
                db.SaveChanges();

                // Fara til baka á síðuna þar sem athugasemdin var skráð
                return RedirectToAction("Details", new { ID = id });
            }
            return View(comment);
        }

        [HttpGet]
        [Authorize]
        public ActionResult TextEdit(int? id, int? page)
        {
            IList<LinesAndTranslations> TextList = (from z in db.SubFileLines
                                                    where z.SubFileID == id                           
                                                    select new LinesAndTranslations { FolderID = z.SubFiles.FolderID, LineNr = z.LineNumber, SubFileId = z.SubFileID, SubLineId = z.ID }).ToList();

            ViewBag.folderid = TextList.FirstOrDefault().FolderID;

            foreach(var item in TextList)
            {

                var tempEng = (from z in db.SubFileLineTranslations
                               where z.SubFileLineID == item.SubLineId && z.LanguageID == 1
                               select z).FirstOrDefault();

                if (tempEng == null)
                {
                    SubFileLineTranslation temp = new SubFileLineTranslation { SubFileLineID = item.SubLineId, LineText = "", LanguageID = 1 };
                    db.SubFileLineTranslations.Add(temp);
                    db.SaveChanges();
                    item.EngText = "";
                }
                else
                {
                    item.EngText = tempEng.LineText;
                }

                var tempIce = (from z in db.SubFileLineTranslations
                               where z.SubFileLineID == item.SubLineId && z.LanguageID == 2
                               select z).FirstOrDefault();

                if (tempIce == null)
                {
                    SubFileLineTranslation temp = new SubFileLineTranslation { SubFileLineID = item.SubLineId, LineText = "", LanguageID = 2 };
                    db.SubFileLineTranslations.Add(temp);
                    db.SaveChanges();
                    item.IceText = "";
                }
                else
                {
                    item.IceText = tempIce.LineText;
                }
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(TextList.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult TextEdit(IList<LinesAndTranslations> model, int folderId) //int? lineId, string text, int? languageId)
        {
            foreach (var s in model)
            {
                var temp = from trans in db.SubFileLineTranslations
                           where trans.SubFileLineID == s.SubLineId
                           select trans;

                var tempEng = from engtrans in temp
                              where engtrans.LanguageID == 1
                              select engtrans;

                var tempIce = from icetrans in temp
                              where icetrans.LanguageID == 2
                              select icetrans;

                tempEng.FirstOrDefault().LineText = s.EngText;
                tempIce.FirstOrDefault().LineText = s.IceText;
                db.SaveChanges();
                
            }
            return RedirectToAction("Details", new { ID = folderId });
        }

        public FileStreamResult GetFile(int id, int langid)
        {
            var file = (from s in db.SubFiles
                       where id == s.FolderID
                       select s).First();

            file.SubDownloadCounter += 1;
            string name = file.SubName;
            FileInfo info = new FileInfo(name);

            var combi = (from z in db.SubFileLines
                        join j in db.SubFileLineTranslations on z.ID equals j.SubFileLineID
                        where j.LanguageID == langid
                        select new { z.LineNumber,z.SubFileID, z.StartTime, z.EndTime, j.LineText });

            var combiright = from t in combi
                             where t.SubFileID == file.ID
                             select t;

            if (info.Exists)
            {
                int temp = name.Length;
                name = name.Substring(0,temp - 4) + "-I.srt"; 
            }
            using (StreamWriter writer = info.CreateText())
            {
                foreach (var line in combiright)
                {
                    writer.WriteLine(line.LineNumber);
                    writer.Write(line.StartTime);
                    writer.Write(" --> ");
                    writer.WriteLine(line.EndTime);
                    writer.WriteLine(line.LineText);
                    //End of Textblock
                    writer.WriteLine("");
                }
            }
            db.SaveChanges();

            return File(info.OpenRead(), "text/plain", name);
        }
    }
}
