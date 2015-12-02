namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AreaImageUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AreaImages", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AreaImages", "UserId");
            AddForeignKey("dbo.AreaImages", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AreaImages", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AreaImages", new[] { "UserId" });
            DropColumn("dbo.AreaImages", "UserId");
        }
    }
}
