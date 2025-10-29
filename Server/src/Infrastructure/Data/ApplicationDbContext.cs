using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using backend.Domain.Entities;
using backend.Domain.Helpers;
using backend.Infrastructure.Data.Configurations;
using backend.Infrastructure.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backend.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<TripDescription> TripDescriptions => Set<TripDescription>();
    public DbSet<ClassYear> ClassYears => Set<ClassYear>();
    public DbSet<Lecture> Lectures => Set<Lecture>();
    public DbSet<Course> Courses => Set<Course>();  
    public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();
    public DbSet<CourseInviteLink> CourseInviteLinks => Set<CourseInviteLink>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureBaseEntity();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        AddStronglyTypedIdConversions(builder);
    }

    private static void AddStronglyTypedIdConversions(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (StronglyTypedIdHelper.IsStronglyTypedId(property.ClrType))
                {
                    var converter = StronglyTypedIdConverters.GetOrAdd(
                        property.ClrType,
                        _ => CreateStronglyTypedIdConverter(property.ClrType));
                    property.SetValueConverter(converter);
                }
            }
        }
    }

    private static readonly ConcurrentDictionary<Type, ValueConverter> StronglyTypedIdConverters = new();

    private static ValueConverter CreateStronglyTypedIdConverter(
        Type stronglyTypedIdType)
    {
        // id => id.Value
        var toProviderFuncType = typeof(Func<,>)
            .MakeGenericType(stronglyTypedIdType, typeof(Guid));
        var stronglyTypedIdParam = Expression.Parameter(stronglyTypedIdType, "Id");
        var toProviderExpression = Expression.Lambda(
            toProviderFuncType,
            Expression.Property(stronglyTypedIdParam, "Value"),
            stronglyTypedIdParam);

        // value => new ProductId(value)
        var fromProviderFuncType = typeof(Func<,>)
            .MakeGenericType(typeof(Guid), stronglyTypedIdType);
        var valueParam = Expression.Parameter(typeof(Guid), "value");
        var ctor = stronglyTypedIdType.GetConstructor(new[] { typeof(Guid) }) ??
            throw new InvalidOperationException(
                $"The type {stronglyTypedIdType} does not have a constructor that takes a Guid.");
        var fromProviderExpression = Expression.Lambda(
            fromProviderFuncType,
            Expression.New(ctor, valueParam),
            valueParam);

        var converterType = typeof(ValueConverter<,>)
            .MakeGenericType(stronglyTypedIdType, typeof(Guid));

        if (Activator.CreateInstance(
            converterType,
            toProviderExpression,
            fromProviderExpression,
            null) is ValueConverter converter)
        {
            return converter;
        }

        throw new InvalidOperationException(
            $"The type {converterType} could not be created.");
    }
}
