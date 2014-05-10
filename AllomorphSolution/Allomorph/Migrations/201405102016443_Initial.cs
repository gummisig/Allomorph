namespace Allomorph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryExtension",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        FolderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryExtension_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoryExtension", t => t.CategoryExtension_ID)
                .Index(t => t.CategoryExtension_ID);
            
            CreateTable(
                "dbo.Folder",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryExtensionID = c.Int(nullable: false),
                        FolderName = c.String(),
                        Link = c.String(),
                        Poster = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CommentText = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        RequestText = c.String(),
                        Counter = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.SubFileLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubFileID = c.Int(nullable: false),
                        SubFileLineTranslationID = c.Int(nullable: false),
                        StartTime = c.String(),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubFile", t => t.SubFileID, cascadeDelete: true)
                .Index(t => t.SubFileID);
            
            CreateTable(
                "dbo.SubFileLineTranslation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubFileLineID = c.Int(nullable: false),
                        LanguageID = c.Int(nullable: false),
                        LineText = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Language", t => t.LanguageID, cascadeDelete: true)
                .ForeignKey("dbo.SubFileLine", t => t.SubFileLineID, cascadeDelete: true)
                .Index(t => t.SubFileLineID)
                .Index(t => t.LanguageID);
            
            CreateTable(
                "dbo.SubFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        FolderID = c.Int(nullable: false),
                        SubFileCounter = c.Int(nullable: false),
                        SubName = c.String(),
                        LastChange = c.DateTime(nullable: false),
                        LastChangedByUser = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Folder", t => t.FolderID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.FolderID);
            
            CreateTable(
                "dbo.FolderCategoryExtension",
                c => new
                    {
                        Folder_ID = c.Int(nullable: false),
                        CategoryExtension_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Folder_ID, t.CategoryExtension_ID })
                .ForeignKey("dbo.Folder", t => t.Folder_ID, cascadeDelete: true)
                .ForeignKey("dbo.CategoryExtension", t => t.CategoryExtension_ID, cascadeDelete: true)
                .Index(t => t.Folder_ID)
                .Index(t => t.CategoryExtension_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubFileLine", "SubFileID", "dbo.SubFile");
            DropForeignKey("dbo.SubFile", "UserID", "dbo.User");
            DropForeignKey("dbo.SubFile", "FolderID", "dbo.Folder");
            DropForeignKey("dbo.SubFileLineTranslation", "SubFileLineID", "dbo.SubFileLine");
            DropForeignKey("dbo.SubFileLineTranslation", "LanguageID", "dbo.Language");
            DropForeignKey("dbo.Request", "UserID", "dbo.User");
            DropForeignKey("dbo.Comment", "UserID", "dbo.User");
            DropForeignKey("dbo.FolderCategoryExtension", "CategoryExtension_ID", "dbo.CategoryExtension");
            DropForeignKey("dbo.FolderCategoryExtension", "Folder_ID", "dbo.Folder");
            DropForeignKey("dbo.Category", "CategoryExtension_ID", "dbo.CategoryExtension");
            DropIndex("dbo.FolderCategoryExtension", new[] { "CategoryExtension_ID" });
            DropIndex("dbo.FolderCategoryExtension", new[] { "Folder_ID" });
            DropIndex("dbo.SubFile", new[] { "FolderID" });
            DropIndex("dbo.SubFile", new[] { "UserID" });
            DropIndex("dbo.SubFileLineTranslation", new[] { "LanguageID" });
            DropIndex("dbo.SubFileLineTranslation", new[] { "SubFileLineID" });
            DropIndex("dbo.SubFileLine", new[] { "SubFileID" });
            DropIndex("dbo.Request", new[] { "UserID" });
            DropIndex("dbo.Comment", new[] { "UserID" });
            DropIndex("dbo.Category", new[] { "CategoryExtension_ID" });
            DropTable("dbo.FolderCategoryExtension");
            DropTable("dbo.SubFile");
            DropTable("dbo.SubFileLineTranslation");
            DropTable("dbo.SubFileLine");
            DropTable("dbo.Request");
            DropTable("dbo.Language");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.Folder");
            DropTable("dbo.Category");
            DropTable("dbo.CategoryExtension");
        }
    }
}
