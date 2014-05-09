namespace Allomorph.Migrations
{
    using Allomorph.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Allomorph.DAL.SubtitleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Allomorph.DAL.SubtitleContext context)
            
        {
            var subtitles = new List<SubFile>
            {
                new SubFile{ID=1,FolderID=1,SubName="Game of Thrones s01e01",LastChangedByUser="Bertel"},
                new SubFile{ID=2,FolderID=2,SubName="Game of Thrones s01e02",LastChangedByUser="Atli"}
            };
            subtitles.ForEach(s => context.SubFiles.Add(s));


            var categories = new List<Category>
            {
                new Category{ID=1,CategoryName="Kvikmyndir"},
                new Category{ID=2,CategoryName="Þættir"},
                new Category{ID=3,CategoryName="Teiknimyndir"},
                new Category{ID=4,CategoryName="Íslenskt efni"}
            };

            categories.ForEach(c => context.Categorys.Add(c));


            var users = new List<User>
            {
                new User{ID=1,UserName="Bertel", Password="password"},
                new User{ID=2,UserName="Atli", Password="password2"}
            };
            users.ForEach(u => context.Users.Add(u));

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
