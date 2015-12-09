using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OutdoorSolution.Domain.Models;
using System.Data.Entity;

namespace OutdoorSolution.Dal
{
    /// <summary>
    /// Unit of work implmentation
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Wall> Walls { get; set; }

        public DbSet<AreaComment> AreaComments { get; set; }

        public DbSet<RouteComment> RouteComments { get; set; }

        public DbSet<AreaImage> AreaImages { get; set; }

        public DbSet<Route> Routes { get; set; }
    }
}