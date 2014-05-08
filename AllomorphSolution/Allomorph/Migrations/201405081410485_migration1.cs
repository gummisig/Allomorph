namespace Allomorph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Request", "UserID", "dbo.User");
            DropIndex("dbo.Request", new[] { "UserID" });
            RenameColumn(table: "dbo.Request", name: "UserID", newName: "User_ID");
            AlterColumn("dbo.Request", "User_ID", c => c.Int());
            CreateIndex("dbo.Request", "User_ID");
            AddForeignKey("dbo.Request", "User_ID", "dbo.User", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "User_ID", "dbo.User");
            DropIndex("dbo.Request", new[] { "User_ID" });
            AlterColumn("dbo.Request", "User_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Request", name: "User_ID", newName: "UserID");
            CreateIndex("dbo.Request", "UserID");
            AddForeignKey("dbo.Request", "UserID", "dbo.User", "ID", cascadeDelete: true);
        }
    }
}
