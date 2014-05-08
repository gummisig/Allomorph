using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Allomorph.Models;

namespace Allomorph.DAL
{
    public class SubtitleInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SubtitleContext>
    {
        protected override void Seed(SubtitleContext context)
        {
            var subtitles = new List<SubFile>
            {
                new SubFile{ID=1,FolderID=1,SubName="Game of Thrones s01e01",LastChangedByUser="Bertel"},
                new SubFile{ID=2,FolderID=1,SubName="Game of Thrones s01e02",LastChangedByUser="Atli"}
            };
            subtitles.ForEach(s => context.SubFiles.Add(s));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category{ID=1,CategoryName="Kvikmyndir"},
                new Category{ID=2,CategoryName="Þættir"},
                new Category{ID=3,CategoryName="Teiknimyndir"},
                new Category{ID=4,CategoryName="Íslenskt efni"}
            };

            categories.ForEach(c => context.Categorys.Add(c));
            context.SaveChanges();

            var users = new List<User>
            {
                new User{ID=1,UserName="Bertel", Password="password"},
                new User{ID=2,UserName="Atli", Password="password2"}
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}