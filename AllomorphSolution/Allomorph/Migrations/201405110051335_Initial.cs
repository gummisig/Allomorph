namespace Allomorph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.Int(nullable: false),
                        FolderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Folder",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
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
                        SubFile_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubFile", t => t.SubFile_ID)
                .Index(t => t.SubFile_ID);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        RequestText = c.String(),
                        ReqUpvoteCounter = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
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
                        LanguageID = c.Int(nullable: false),
                        SubDownloadCounter = c.Int(nullable: false),
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
                "dbo.FolderCategory",
                c => new
                    {
                        Folder_ID = c.Int(nullable: false),
                        Category_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Folder_ID, t.Category_ID })
                .ForeignKey("dbo.Folder", t => t.Folder_ID, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Folder_ID)
                .Index(t => t.Category_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubFileLine", "SubFileID", "dbo.SubFile");
            DropForeignKey("dbo.SubFile", "UserID", "dbo.User");
            DropForeignKey("dbo.Language", "SubFile_ID", "dbo.SubFile");
            DropForeignKey("dbo.SubFile", "FolderID", "dbo.Folder");
            DropForeignKey("dbo.SubFileLineTranslation", "SubFileLineID", "dbo.SubFileLine");
            DropForeignKey("dbo.SubFileLineTranslation", "LanguageID", "dbo.Language");
            DropForeignKey("dbo.Request", "UserID", "dbo.User");
            DropForeignKey("dbo.Comment", "UserID", "dbo.User");
            DropForeignKey("dbo.FolderCategory", "Category_ID", "dbo.Category");
            DropForeignKey("dbo.FolderCategory", "Folder_ID", "dbo.Folder");
            DropIndex("dbo.FolderCategory", new[] { "Category_ID" });
            DropIndex("dbo.FolderCategory", new[] { "Folder_ID" });
            DropIndex("dbo.SubFile", new[] { "FolderID" });
            DropIndex("dbo.SubFile", new[] { "UserID" });
            DropIndex("dbo.SubFileLineTranslation", new[] { "LanguageID" });
            DropIndex("dbo.SubFileLineTranslation", new[] { "SubFileLineID" });
            DropIndex("dbo.SubFileLine", new[] { "SubFileID" });
            DropIndex("dbo.Request", new[] { "UserID" });
            DropIndex("dbo.Language", new[] { "SubFile_ID" });
            DropIndex("dbo.Comment", new[] { "UserID" });
            DropTable("dbo.FolderCategory");
            DropTable("dbo.SubFile");
            DropTable("dbo.SubFileLineTranslation");
            DropTable("dbo.SubFileLine");
            DropTable("dbo.Request");
            DropTable("dbo.Language");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.Folder");
            DropTable("dbo.Category");
        }
    }
}
