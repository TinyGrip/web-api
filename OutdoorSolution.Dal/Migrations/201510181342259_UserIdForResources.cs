namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdForResources : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Walls", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Routes", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Walls", "UserId");
            CreateIndex("dbo.Routes", "UserId");
            AddForeignKey("dbo.Routes", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Walls", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Walls", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Routes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Routes", new[] { "UserId" });
            DropIndex("dbo.Walls", new[] { "UserId" });
            DropColumn("dbo.Routes", "UserId");
            DropColumn("dbo.Walls", "UserId");
        }
    }
}
