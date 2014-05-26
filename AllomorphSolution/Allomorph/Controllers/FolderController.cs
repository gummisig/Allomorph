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
using System.IO;
using Allomorph.Repositories;

namespace Allomorph.Controllers
{
    public class FolderController : Controller
    {
        // Database access layer
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

            SubFile subtitle = repo.GetSubFileById(folder.ID);
            IEnumerable<Comment> comment = repo.GetCommentsById(folder.ID);

            return View(Tuple.Create(folder, subtitle, comment));
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
        public ActionResult Create([Bind(Include = "ID,CategoryID,FolderName,Link,Poster,Description")] Folder folder, HttpPostedFileBase file, SubFile subfile, int? requestID)
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

                // .Peek() skoðar næsta tákn án þess að taka það úr strauminum
                // -1 Þegar öll skráin hefur verið lesin
                while (streamReader.Peek() != -1)
                {
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
                    SubFileLine tempLine = new SubFileLine()
                    {
                        SubFileID = subfile.ID,
                        LineNumber = Convert.ToInt32(lineNumber), 
                        StartTime = firstTime, 
                        EndTime = secondTime
                    }; 

                    // Tengja línurnar við textana (þýðingarnar)
                    SubFileLineTranslation tempTranslation = new SubFileLineTranslation()
                    {
                        SubFileLineID = tempLine.ID,
                        LineText = text,
                        LanguageID = subfile.LanguageID
                    };
                    SubFileLineTranslation trans = new SubFileLineTranslation()
                    {
                        SubFileLineID = tempLine.ID,
                        LineText = "",
                        LanguageID = 2
                    };
                    if (subfile.LanguageID == 2) {
                        trans.LanguageID = 1;
                    };
                    
                    // Setja textann í gagnagrunninn og vista breytingarnar
                    repo.AddLines(tempLine, tempTranslation, trans);
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
            //Folder folder = repo.GetFolderById(id);
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
        public ActionResult Edit([Bind(Include = "ID,CategoryID,FolderName,Link,Poster,Description,DateCreated")] Folder folder)
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
                var tempEng = repo.GetLineByLang(item.SubFileLineID, 1);
                var tempIce = repo.GetLineByLang(item.SubFileLineID, 2);
                item.EngText = tempEng.LineText;
                item.IceText = tempIce.LineText;
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
            if (ModelState.IsValid)
            {
                foreach (var s in model)
                {
                    var tempEng = repo.GetLineByLang(s.SubFileLineID, 1);
                    var tempIce = repo.GetLineByLang(s.SubFileLineID, 2);
                    var time = repo.GetSubFileLineById(s.SubFileLineID);

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
            string name = file.SubName;
            FileInfo info = new FileInfo(name);

            if (name != null)
            {
                if (langid == 1)
                {
                    name = name.Substring(0, name.Length - 4) + ".en.srt";
                }
                else
                {
                    name = name.Substring(0, name.Length - 4) + ".is.srt";
                }
            }

            var textFile = repo.GetSubFile(langid, file.ID);
            using (StreamWriter writer = info.CreateText())
            {
                foreach (var line in textFile)
                {
                    writer.WriteLine(line.LineNumber);
                    writer.Write(line.SubFileLineStartTime);
                    writer.Write(" --> ");
                    writer.WriteLine(line.SubFileLineEndTime);
                    writer.WriteLine(line.EngText);
                    //End of Textblock
                    writer.WriteLine("");
                }
            }
            file.SubDownloadCounter += 1;
            repo.Save();

            return File(info.OpenRead(), "text/plain", name);
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
