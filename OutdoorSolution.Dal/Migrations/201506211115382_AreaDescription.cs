namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AreaDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Areas", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Areas", "Description");
        }
    }
}
