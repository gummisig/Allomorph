using System;
using Allomorph.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Allomorph.DAL
{
    public class SubtitleContext : DbContext
    {
        public SubtitleContext()
            : base("SubtitleContext")
        {
        }
        
        public DbSet<Category> Categorys { get; set; }
        public DbSet<CategoryExtension> CategoryExtensions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<SubFile> SubFiles { get; set; }
        public DbSet<SubFileLine> SubFileLines { get; set; }
        public DbSet<SubFileLineTranslation> SubFileLineTranslations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}