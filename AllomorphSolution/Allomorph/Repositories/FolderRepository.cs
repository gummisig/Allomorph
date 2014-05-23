using Allomorph.DAL;
using Allomorph.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Allomorph.Repositories
{
    public class FolderRepository
    {
        private SubtitleContext db = new SubtitleContext();

        public IEnumerable<Comment> GetCommentsById(int? id)
        {
            var comments = db.Comments.Where(c => c.FolderID == id).ToList();
            return comments;
        }

        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
        }

        public IEnumerable<Folder> GetAllFolders()
        {
            var folder = from f in db.Folders
                         select f;
            return folder;
        }

        public Folder GetFolderById(int? id)
        {
            Folder folder = db.Folders.Find(id);
            return folder;
        }

        public void AddFolder(Folder folder)
        {
            db.Folders.Add(folder);
        }

        public void RemoveFolder(Folder folder)
        {
            db.Folders.Remove(folder);
        }

        public IEnumerable<Like> GetAllLikes(int id)
        {
            var likes = db.Likes.Where(s => s.RequestID == id);
            return likes;
        }

        public void AddLike(Like like)
        {
            db.Likes.Add(like);
        }

        public IEnumerable<Request> GetAllRequests()
        {
            var req = from r in db.Requests
                      select r;
            return req;
        }

        public Request GetRequestById(int? id)
        {
            var req = db.Requests.Find(id);
            return req;
        }

        public void AddRequest(Request request)
        {
            db.Requests.Add(request);
        }

        public void RemoveRequest(Request request)
        {
            db.Requests.Remove(request);
        }

        public IEnumerable<SubFile> GetAllSubFiles()
        {
            var subfiles = from s in db.SubFiles
                           select s;
            return subfiles;
        }

        public IEnumerable<SubFile> GetSubFilesById(int? id)
        {
            var subfiles = db.SubFiles.Where(s => s.FolderID == id).ToList();
            return subfiles;
        }

        public SubFile GetSubFileById(int id)
        {
            var subfile = db.SubFiles.Where(s => s.FolderID == id).First();
            return subfile;
        }

        public IQueryable GetSubFileLine(int langid, int fileid)
        {
            var combi = (from z in db.SubFileLines
                         join j in db.SubFileLineTranslations on z.ID equals j.SubFileLineID
                         where j.LanguageID == langid
                         select new
                         {
                             z.LineNumber,
                             z.SubFileID,
                             z.StartTime,
                             z.EndTime,
                             j.LineText
                         }).Where(t => t.SubFileID == fileid);
            return combi;
        }

        public IEnumerable<SubFileLine> GetSubLinesById(int? id)
        {
            var sublines = db.SubFileLines.Where(l => l.SubFileID == id).ToList();
            return sublines;
        }

        public SubFileLineTranslation GetLineByLang(int sublineid, int lang)
        {
            var line = db.SubFileLineTranslations.Where(l => l.SubFileLineID == sublineid && l.LanguageID == lang).FirstOrDefault();
            return line;
        }

        public SubFileLine GetTime(int sublineid)
        {
            var time = db.SubFileLines.Where(l => l.ID == sublineid);
            return time.FirstOrDefault();
        }

        public void AddSubFile(SubFile subfile)
        {
            db.SubFiles.Add(subfile);
        }

        public void AddSubLine(SubFileLine line)
        {
            db.SubFileLines.Add(line);
        }

        public void AddSubLineTranslation(SubFileLineTranslation trans)
        {
            db.SubFileLineTranslations.Add(trans);
        }

        public IList<LinesAndTranslations> GetText(int? id)
        {
            var TextList = (from z in db.SubFileLines
                            where z.SubFileID == id
                            select new LinesAndTranslations
                            {
                                FolderID = z.SubFiles.FolderID,
                                LineNr = z.LineNumber,
                                SubFileId = z.SubFileID,
                                SubLineId = z.ID,
                                SubFileLineStartTime = z.StartTime,
                                SubFileLineEndTime = z.EndTime
                            }).ToList();
            return TextList;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Entry(object obj) 
        {
            db.Entry(obj).State = EntityState.Modified;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}