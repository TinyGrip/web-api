namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UriToString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AreaImages", "Url", c => c.String(maxLength: 512));
            AddColumn("dbo.Walls", "Image", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Walls", "Image");
            DropColumn("dbo.AreaImages", "Url");
        }
    }
}
