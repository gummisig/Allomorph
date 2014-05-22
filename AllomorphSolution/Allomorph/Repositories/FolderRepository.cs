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

        public Request GetRequestById(int? id)
        {
            var req = db.Requests.Find(id);
            return req;
        }

        public void RemoveRequest(Request request)
        {
            db.Requests.Remove(request);
        }

        public IEnumerable<SubFile> GetSubFilesById(int? id)
        {
            var subfiles = db.SubFiles.Where(s => s.FolderID == id).ToList();
            return subfiles;
        }

        public IEnumerable<SubFileLine> GetSubLinesById(int? id)
        {
            var sublines = db.SubFileLines.Where(l => l.SubFileID == id).ToList();
            return sublines;
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

        public void Save()
        {
            db.SaveChanges();
        }

        public void Entry(object obj) 
        {
            db.Entry(obj).State = EntityState.Modified;
        }
    }
}