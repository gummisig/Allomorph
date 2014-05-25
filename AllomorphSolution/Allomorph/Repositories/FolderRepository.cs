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
        // Database
        private SubtitleContext db = new SubtitleContext();
        
        /// <summary>
        ///     Comment functions
        /// </summary>
        /// <returns>
        ///     Comment.ToList();
        /// </returns>
        
        // Get all comments associated with 'folderId'
        public IEnumerable<Comment> GetCommentsById(int? folderId)
        {
            var comments = db.Comments.Where(c => c.FolderID == folderId).ToList();
            return comments;
        }
        // Add comment 'c' to the database
        public void AddComment(Comment c)
        {
            db.Comments.Add(c);
        }

        /// <summary>
        ///     Folder functions
        /// </summary>
        /// <returns>
        ///     Folder; || Folder.ToList();
        /// </returns>
        
        // Get all Folders
        public IEnumerable<Folder> GetAllFolders()
        {
            var folder = db.Folders.ToList();
            return folder;
        }
        // Get the folder where ID == 'id'
        public Folder GetFolderById(int? id)
        {
            Folder folder = db.Folders.Find(id);
            return folder;
        }
        // Add Folder 'f' to the database
        public void AddFolder(Folder f)
        {
            db.Folders.Add(f);
        }
        // Remove Folder 'f'f from the database
        public void RemoveFolder(Folder f)
        {
            db.Folders.Remove(f);
        }

        /// <summary>
        ///     Like functions
        /// </summary>
        /// <returns>
        ///     Like.ToList();
        /// </returns>
        
        // Get all Likes where associated with 'requestId'
        public IEnumerable<Like> GetAllLikes(int requestId)
        {
            var likes = db.Likes.Where(s => s.RequestID == requestId);
            return likes;
        }
        // Add Like 'l' to the database
        public void AddLike(Like l)
        {
            db.Likes.Add(l);
        }

        /// <summary>
        ///     Request functions
        /// </summary>
        /// <returns>
        ///     Request.ToList();
        /// </returns>
        
        // Get all Requests
        public IEnumerable<Request> GetAllRequests()
        {
            var req = db.Requests.ToList();
            return req;
        }
        // Get Request where ID == 'id'
        public Request GetRequestById(int? id)
        {
            var req = db.Requests.Find(id);
            return req;
        }
        // Add Request 'r' to the database
        public void AddRequest(Request r)
        {
            db.Requests.Add(r);
        }
        // Remove Request 'r' from the database
        public void RemoveRequest(Request r)
        {
            db.Requests.Remove(r);
        }

        /// <summary>
        ///     SubFile functions
        /// </summary>
        /// <returns>
        ///     SubFile || SubFile.ToList();
        /// </returns>
        
        // Get all SubFiles
        public IEnumerable<SubFile> GetAllSubFiles()
        {
            var subfiles = db.SubFiles.ToList();
            return subfiles;
        }
        // Get the SubFile associated with 'folderId'
        public SubFile GetSubFileById(int? folderId)
        {
            var subfile = db.SubFiles.Where(s => s.FolderID == folderId).First();
            return subfile;
        }
        // Add SubFile 's' to the database
        public void AddSubFile(SubFile s)
        {
            db.SubFiles.Add(s);
        }

        /// <summary>
        ///     SubFileLine functions
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        
        // Get all SubFileLines associated with 'subFileId'
        public IEnumerable<SubFileLine> GetSubFileLinesById(int? subFileId)
        {
            var sublines = db.SubFileLines.Where(l => l.SubFileID == subFileId).ToList();
            return sublines;
        }
        // Get the SubFileLine where ID == 'subLineId'
        public SubFileLine GetSubFileLineById(int subLineId)
        {
            var line = db.SubFileLines.Where(l => l.ID == subLineId).FirstOrDefault();
            return line;
        }
        // Add SubFileLine 'l' to the database
        public void AddSubLine(SubFileLine l)
        {
            db.SubFileLines.Add(l);
        }

        /// <summary>
        ///     SubFileLineTranslation functions
        /// </summary>
        /// <returns>
        ///     SubFileLineTranslation; || SubFileLineTranslation.ToList();
        /// </returns>
        
        // Get all SubFileLineTranslations associated with 'subFileLineId'
        public IEnumerable<SubFileLineTranslation> GetTransById(int subFileLineId)
        {
            var trans = db.SubFileLineTranslations.Where(t => t.SubFileLineID == subFileLineId).ToList();
            return trans;
        }
        // Get specific SubFileLineTranslation associated with 'subLineId' and 'langId'
        public SubFileLineTranslation GetLineByLang(int subLineId, int langId)
        {
            var line = db.SubFileLineTranslations.Where(l => l.SubFileLineID == subLineId && l.LanguageID == langId).FirstOrDefault();
            return line;
        }
        // Add SubFileLineTranslation 't' to the database
        public void AddSubLineTranslation(SubFileLineTranslation t)
        {
            db.SubFileLineTranslations.Add(t);
        }
        // Add SubFileLine 'l' and SubFileLineTranslations 'trans1 & trans2' to the database
        public void AddLines(SubFileLine line, SubFileLineTranslation trans1, SubFileLineTranslation trans2)
        {
            db.SubFileLines.Add(line);
            db.SubFileLineTranslations.Add(trans1);
            db.SubFileLineTranslations.Add(trans2);
        }

        /// <summary>
        ///     Other functions
        /// </summary>
        /// <returns>
        ///     LinesAndTranslations.ToList();
        /// </returns>

        // Get all the lines (translations too) associated with 'subFileId'
        public IList<LinesAndTranslations> GetText(int? subFileId)
        {
            var TextList = (from z in db.SubFileLines
                            where z.SubFileID == subFileId
                            select new LinesAndTranslations
                            {
                                FolderID = z.SubFiles.FolderID,
                                LineNumber = z.LineNumber,
                                SubFileID = z.SubFileID,
                                SubFileLineID = z.ID,
                                SubFileLineStartTime = z.StartTime,
                                SubFileLineEndTime = z.EndTime
                            }).ToList();
            return TextList;
        }
        // Get the SubFile associated with 'fileId' and 'langId'
        public IEnumerable<LinesAndTranslations> GetSubFile(int langId, int fileId)
        {
            var combi = (from z in db.SubFileLines
                         join j in db.SubFileLineTranslations on z.ID equals j.SubFileLineID
                         where j.LanguageID == langId
                         select new LinesAndTranslations
                         {
                             LineNumber = z.LineNumber,
                             SubFileID = z.SubFileID,
                             SubFileLineStartTime = z.StartTime,
                             SubFileLineEndTime = z.EndTime,
                             EngText = j.LineText
                         }).Where(t => t.SubFileID == fileId);
            return combi;
        }
        // Save changes to the database
        public void Save()
        {
            db.SaveChanges();
        }
        // Modify properties of specific Model 'obj'
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