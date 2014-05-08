namespace Allomorph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryExtension",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(),
                        SubFileID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Category", t => t.CategoryID)
                .ForeignKey("dbo.SubFile", t => t.SubFileID)
                .Index(t => t.CategoryID)
                .Index(t => t.SubFileID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Folder",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    SubFileID = c.Int(nullable: false),
                    FolderName = c.String(),
                    Link = c.String(),
                    Poster = c.String(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.SubFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FolderID = c.Int(),
                        UserID = c.Int(),
                        SubName = c.String(),
                        LastChange = c.DateTime(nullable: false),
                        LastChangedByUser = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Folder", t => t.FolderID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.FolderID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.SubFileLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubFileID = c.Int(),
                        StartTime = c.String(),
                        Duration = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubFile", t => t.SubFileID)
                .Index(t => t.SubFileID);
            
            CreateTable(
                "dbo.SubFileLineTranslation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubFileLineID = c.Int(),
                        LanguageID = c.Int(),
                        LineText = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Language", t => t.LanguageID)
                .ForeignKey("dbo.SubFileLine", t => t.SubFileLineID)
                .Index(t => t.SubFileLineID)
                .Index(t => t.LanguageID);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        RequestText = c.String(),
                        Counter = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "UserID", "dbo.User");
            DropForeignKey("dbo.SubFile", "UserID", "dbo.User");
            DropForeignKey("dbo.Request", "UserID", "dbo.User");
            DropForeignKey("dbo.SubFileLine", "SubFileID", "dbo.SubFile");
            DropForeignKey("dbo.SubFileLineTranslation", "SubFileLineID", "dbo.SubFileLine");
            DropForeignKey("dbo.SubFileLineTranslation", "LanguageID", "dbo.Language");
            DropForeignKey("dbo.SubFile", "FolderID", "dbo.Folder");
            DropForeignKey("dbo.CategoryExtension", "SubFileID", "dbo.SubFile");
            DropForeignKey("dbo.CategoryExtension", "CategoryID", "dbo.Category");
            DropIndex("dbo.Comment", new[] { "UserID" });
            DropIndex("dbo.Request", new[] { "UserID" });
            DropIndex("dbo.SubFileLineTranslation", new[] { "LanguageID" });
            DropIndex("dbo.SubFileLineTranslation", new[] { "SubFileLineID" });
            DropIndex("dbo.SubFileLine", new[] { "SubFileID" });
            DropIndex("dbo.SubFile", new[] { "UserID" });
            DropIndex("dbo.SubFile", new[] { "FolderID" });
            DropIndex("dbo.CategoryExtension", new[] { "SubFileID" });
            DropIndex("dbo.CategoryExtension", new[] { "CategoryID" });
            DropTable("dbo.Comment");
            DropTable("dbo.Request");
            DropTable("dbo.User");
            DropTable("dbo.Language");
            DropTable("dbo.SubFileLineTranslation");
            DropTable("dbo.SubFileLine");
            DropTable("dbo.Folder");
            DropTable("dbo.SubFile");
            DropTable("dbo.Category");
            DropTable("dbo.CategoryExtension");
        }
    }
}
