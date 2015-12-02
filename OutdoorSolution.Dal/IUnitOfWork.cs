using OutdoorSolution.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OutdoorSolution.Dal
{
    public interface IUnitOfWork : IDisposable
    {
        DbSet<Area> Areas { get; set; }

        DbSet<Wall> Walls { get; set; }

        DbSet<AreaComment> AreaComments { get; set; }

        DbSet<RouteComment> RouteComments { get; set; }

        DbSet<AreaImage> AreaImages { get; set; }

        DbSet<Route> Routes { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);
    }
}