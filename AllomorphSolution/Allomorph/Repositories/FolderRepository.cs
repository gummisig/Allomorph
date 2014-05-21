using Allomorph.DAL;
using Allomorph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allomorph.Repositories
{
    public class FolderRepository
    {
        private SubtitleContext db = new SubtitleContext();

        public IEnumerable<Folder> GetAllFolders(int? id)
        {
            var folder = from f in db.Folders
                         select f;

            if (id != null)
            {
                folder = folder.Where(f => f.ID == id);
            }
            return folder;
        }

        public Folder GetFolderById(int? id)
        {
            Folder folder = db.Folders.Find(id);
            return folder;
        }

        public IEnumerable<SubFile> GetSubFilesById(int? id)
        {
            IEnumerable<SubFile> subfiles = (from s in db.SubFiles
                                            where s.FolderID == id
                                            select s).ToList();
            return subfiles;
        }

        public IEnumerable<Comment> GetCommentsById(int? id)
        {
            IEnumerable<Comment> comments = (from c in db.Comments
                                            where c.FolderID == id
                                            select c).ToList();
            return comments;
        }


    }
}