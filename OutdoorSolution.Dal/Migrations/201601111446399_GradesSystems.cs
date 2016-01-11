namespace OutdoorSolution.Dal.Migrations
{
    using OutdoorSolution.Common;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GradesSystems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FreeClimbingGradesSystem", c => c.Int(nullable: false, defaultValue: (int)FreeClimbingGradesSystems.French));
            AddColumn("dbo.AspNetUsers", "BoulderingGradesSystem", c => c.Int(nullable: false, defaultValue: (int)BoulderingGradesSystems.French));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BoulderingGradesSystem");
            DropColumn("dbo.AspNetUsers", "FreeClimbingGradesSystem");
        }
    }
}
