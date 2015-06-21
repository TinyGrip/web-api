namespace OutdoorSolution.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialClimbing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaComments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AreaId = c.Guid(nullable: false),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.AreaId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Created = c.DateTime(nullable: false),
                        Rating = c.Double(nullable: false),
                        RatingsCount = c.Int(nullable: false),
                        Location = c.Geography(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AreaImages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        AreaId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.Walls",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Location = c.Geography(),
                        AreaId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.RouteComments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RouteId = c.Guid(nullable: false),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.RouteId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Path = c.Geometry(),
                        Complexity = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                        WallId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Walls", t => t.WallId, cascadeDelete: true)
                .Index(t => t.WallId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteComments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RouteComments", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.Routes", "WallId", "dbo.Walls");
            DropForeignKey("dbo.AreaComments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AreaComments", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Walls", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Areas", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AreaImages", "AreaId", "dbo.Areas");
            DropIndex("dbo.Routes", new[] { "WallId" });
            DropIndex("dbo.RouteComments", new[] { "UserId" });
            DropIndex("dbo.RouteComments", new[] { "RouteId" });
            DropIndex("dbo.Walls", new[] { "AreaId" });
            DropIndex("dbo.AreaImages", new[] { "AreaId" });
            DropIndex("dbo.Areas", new[] { "UserId" });
            DropIndex("dbo.AreaComments", new[] { "UserId" });
            DropIndex("dbo.AreaComments", new[] { "AreaId" });
            DropTable("dbo.Routes");
            DropTable("dbo.RouteComments");
            DropTable("dbo.Walls");
            DropTable("dbo.AreaImages");
            DropTable("dbo.Areas");
            DropTable("dbo.AreaComments");
        }
    }
}
