namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentLength : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaComments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RouteComments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AreaComments", new[] { "UserId" });
            DropIndex("dbo.RouteComments", new[] { "UserId" });
            RenameColumn(table: "dbo.AreaComments", name: "AreaId", newName: "SubjectId");
            RenameColumn(table: "dbo.RouteComments", name: "RouteId", newName: "SubjectId");
            RenameIndex(table: "dbo.AreaComments", name: "IX_AreaId", newName: "IX_SubjectId");
            RenameIndex(table: "dbo.RouteComments", name: "IX_RouteId", newName: "IX_SubjectId");
            AlterColumn("dbo.AreaComments", "Text", c => c.String(nullable: false, maxLength: 2048));
            AlterColumn("dbo.AreaComments", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.RouteComments", "Text", c => c.String(nullable: false, maxLength: 2048));
            AlterColumn("dbo.RouteComments", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AreaComments", "UserId");
            CreateIndex("dbo.RouteComments", "UserId");
            AddForeignKey("dbo.AreaComments", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RouteComments", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteComments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AreaComments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.RouteComments", new[] { "UserId" });
            DropIndex("dbo.AreaComments", new[] { "UserId" });
            AlterColumn("dbo.RouteComments", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.RouteComments", "Text", c => c.String());
            AlterColumn("dbo.AreaComments", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AreaComments", "Text", c => c.String());
            RenameIndex(table: "dbo.RouteComments", name: "IX_SubjectId", newName: "IX_RouteId");
            RenameIndex(table: "dbo.AreaComments", name: "IX_SubjectId", newName: "IX_AreaId");
            RenameColumn(table: "dbo.RouteComments", name: "SubjectId", newName: "RouteId");
            RenameColumn(table: "dbo.AreaComments", name: "SubjectId", newName: "AreaId");
            CreateIndex("dbo.RouteComments", "UserId");
            CreateIndex("dbo.AreaComments", "UserId");
            AddForeignKey("dbo.RouteComments", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AreaComments", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
