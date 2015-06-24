namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WallName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Walls", "Name", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Walls", "Name");
        }
    }
}
