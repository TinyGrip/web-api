namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvatarImage", c => c.String());
            AddColumn("dbo.AspNetUsers", "CoverImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CoverImage");
            DropColumn("dbo.AspNetUsers", "AvatarImage");
        }
    }
}
