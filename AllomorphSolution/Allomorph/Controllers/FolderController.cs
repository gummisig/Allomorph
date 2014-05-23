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
using Allomorph.Repositories;

namespace Allomorph.Controllers
{
    public class FolderController : Controller
    {
        // Database
        private SubtitleContext db = new SubtitleContext();
        private FolderRepository repo = new FolderRepository();

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

            var folders = repo.GetAllFolders();

            // Ef það er leitað eftir flokki
            switch (category)
            {
                case 1:
                    folders = folders.Where(f => f.CategoryID == 1);
                    break;
                case 2:
                    folders = folders.Where(f => f.CategoryID == 2);
                    break;
                case 3:
                    folders = folders.Where(f => f.CategoryID == 3);
                    break;
                case 4:
                    folders = folders.Where(f => f.CategoryID == 4);
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
            Folder folder = repo.GetFolderById(id);
            if (folder == null)
            {
                return View("NotFound");
            }

            IEnumerable<SubFile> subtitles = repo.GetSubFilesById(folder.ID);
            IEnumerable<Comment> comment = repo.GetCommentsById(folder.ID);
            IEnumerable<Folder> folders = repo.GetAllFolders().Where(f => f.ID == folder.ID).ToList();

            return View(Tuple.Create(folder, subtitles, comment, folders));
        }

        // GET: /Folder/Create
        [Authorize]
        public ActionResult Create(int? requestID)
        {
            ViewBag.request = repo.GetRequestById(requestID);
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
                // Nær í skráarendinguna á textaskránni
                string extension = Path.GetExtension(file.FileName);
                // Athugar hvort skráin sé á löglegu formi (.srt ending)
                if (extension != ".srt")
                {
                    return View("WrongFile");
                }
                // Geymir gögnin úr textaskránni sem er send inn
                StreamReader streamReader = new StreamReader(file.InputStream);

                // Tengja textaskrána við möppu og gefa henni nafn
                subfile.FolderID = folder.ID;
                subfile.SubName = file.FileName;
                // Setja möppuna og textaskrána í gagnagrunninn
                repo.AddFolder(folder);
                repo.AddSubFile(subfile);

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
                    repo.AddSubLine(tempLine);

                    // Tengja línurnar við textana (þýðingarnar)
                    tempTranslation.SubFileLineID = tempLine.ID;
                    tempTranslation.LineText = text;
                    tempTranslation.LanguageID = subfile.LanguageID;
                    
                    // Setja textann í gagnagrunninn og vista breytingarnar
                    repo.AddSubLineTranslation(tempTranslation);
                    repo.Save();
                }
                // requestID != null ef verið er að uppfylla beiðni
                if (requestID != null)
                {
                    // Ná í beiðni úr gagnagrunni
                    var req = repo.GetRequestById(requestID);
                    // Villumeðhöndlun
                    if (req != null)
                    {
                        // Eyða beiðni úr gagnagrunni og vista breytingar
                        repo.RemoveRequest(req);
                        repo.Save();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                return RedirectToAction("Details", new { id = folder.ID });
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
            Folder folder = repo.GetFolderById(id);
            if (folder == null)
            {
                return View("NotFound");
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
                repo.Entry(folder);
                repo.Save();
                return RedirectToAction("Details", new { id = folder.ID });
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
            Folder folder = repo.GetFolderById(id);
            if (folder == null)
            {
                return View("NotFound");
            }
            return View(folder);
        }

        // POST: /Folder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Folder folder = repo.GetFolderById(id);
            repo.RemoveFolder(folder);
            repo.Save();
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
                // Sækja notendanafn þess sem er innskráður
                string usr = System.Web.HttpContext.Current.User.Identity.Name;

                // Öryggisráðstafanir
                if (usr == null)
                {
                    return RedirectToAction("Details", new { ID = id });
                }
                // Sækja möppuna þar sem athugasemdin var skráð
                Folder folder = repo.GetFolderById(id);
                if (folder == null)
                {
                    return View("NotFound");
                }
                
                // Tengja athugasemdina við möppu og notanda
                comment.FolderID = folder.ID;
                comment.UserName = usr;

                // Setja athugasemdina í gagnagrunninn og vista breytingar
                repo.AddComment(comment);
                repo.Save();

                // Fara til baka á síðuna þar sem athugasemdin var skráð
                return RedirectToAction("Details", new { ID = id });
            }
            return View(comment);
        }

        [HttpGet]
        [Authorize]
        public ActionResult TextEdit(int? id, int? page)
        {
            if (id == null)
            {
                return View("Error");
            }
            IList<LinesAndTranslations> TextList = repo.GetText(id);
            if (TextList.Count == 0)
            {
                return View("NotFound");
            }

            ViewBag.folderid = TextList.FirstOrDefault().FolderID;

            foreach(var item in TextList)
            {

                var tempEng = repo.GetLineByLang(item.SubLineId, 1);

                if (tempEng == null)
                {
                    SubFileLineTranslation temp = new SubFileLineTranslation { SubFileLineID = item.SubLineId, LineText = "", LanguageID = 1 };
                    repo.AddSubLineTranslation(temp);
                    item.EngText = "";
                }
                else
                {
                    item.EngText = tempEng.LineText;
                }

                var tempIce = repo.GetLineByLang(item.SubLineId, 2);

                if (tempIce == null)
                {
                    SubFileLineTranslation temp = new SubFileLineTranslation { SubFileLineID = item.SubLineId, LineText = "", LanguageID = 2 };
                    repo.AddSubLineTranslation(temp);
                    item.IceText = "";
                }
                else
                {
                    item.IceText = tempIce.LineText;
                }
            }
            repo.Save();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(TextList.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult TextEdit(IList<LinesAndTranslations> model, int folderId) //int? lineId, string text, int? languageId)
        {
            if (ModelState.IsValid)
            {
                foreach (var s in model)
                {
                    var tempEng = repo.GetLineByLang(s.SubLineId, 1);
                    //var tempEng = db.SubFileLineTranslations.Where(t => t.SubFileLineID == s.SubLineId).Where(e => e.LanguageID == 1);
                    var tempIce = repo.GetLineByLang(s.SubLineId, 2);
                    //var tempIce = db.SubFileLineTranslations.Where(t => t.SubFileLineID == s.SubLineId).Where(e => e.LanguageID == 2);
                    var time = repo.GetTime(s.SubLineId);

                    tempEng.LineText = s.EngText;
                    tempIce.LineText = s.IceText;
                    time.StartTime = s.SubFileLineStartTime;
                    time.EndTime = s.SubFileLineEndTime;
                }
                repo.Save();
                return RedirectToAction("Details", new { ID = folderId });
            }
            return View(model);
        }

        public FileStreamResult GetFile(int id, int langid)
        {
            var file = repo.GetSubFileById(id);

            file.SubDownloadCounter += 1;
            string name = file.SubName;
            FileInfo info = new FileInfo(name);

            var combi = (from z in db.SubFileLines
                         join j in db.SubFileLineTranslations on z.ID equals j.SubFileLineID
                         where j.LanguageID == langid
                         select new { z.LineNumber,
                                      z.SubFileID,
                                      z.StartTime,
                                      z.EndTime,
                                      j.LineText });

            var combiright = combi.Where(t => t.SubFileID == file.ID);

            if (name != null)
            {
                int temp = name.Length;
                if (langid == 1)
                {
                    name = name.Substring(0, temp - 4) + ".en.srt";
                }
                else
                {
                    name = name.Substring(0, temp - 4) + ".is.srt";
                }
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
            repo.Save();

            return File(info.OpenRead(), "text/plain", name);
        }
    }
}
