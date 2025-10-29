using System.Collections.Generic;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<Models.ApplicationUser> Users { get; }

    virtual DatabaseFacade Database => this.Database;

    DbSet<Country> Countries { get; }
    DbSet<Destination> Destinations { get; }
    DbSet<Resource> Resources { get; }
    DbSet<Trip> Trips { get; }
    DbSet<TripDescription> TripDescriptions { get; }
    DbSet<ClassYear> ClassYears { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
