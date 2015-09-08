namespace OutdoorSolution.Dal.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OutdoorSolution.Dal.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "OutdoorSolution.Models.ApplicationDbContext";
        }

        protected override void Seed(OutdoorSolution.Dal.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole(RoleNames.User), new IdentityRole(RoleNames.Admin));
        }
    }
}
