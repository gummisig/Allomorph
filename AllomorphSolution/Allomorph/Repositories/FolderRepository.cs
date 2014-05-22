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

        public void RemoveFolder(Folder folder)
        {
            db.Folders.Remove(folder);
        }

        

        public IEnumerable<SubFile> GetSubFilesById(int? id)
        {
            var subfiles = (from s in db.SubFiles
                            where s.FolderID == id
                            select s).ToList();
            return subfiles;
        }

        public IEnumerable<Comment> GetCommentsById(int? id)
        {
            var comments = (from c in db.Comments
                            where c.FolderID == id
                            select c).ToList();
            return comments;
        }

        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
        }

        public IEnumerable<SubFileLine> GetSubLinesById(int? id)
        {
            var sublines = (from l in db.SubFileLines
                            where l.SubFileID == id
                            select l).ToList();
            return sublines;
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