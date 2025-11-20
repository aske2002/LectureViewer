using System.Collections.Generic;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<ApplicationUser> Users { get; }

    virtual DatabaseFacade Database => this.Database;

    DbSet<Country> Countries { get; }
    DbSet<Resource> Resources { get; }
    DbSet<Lecture> Lectures { get; }
    DbSet<LectureContent> LectureContents { get; }
    DbSet<Course> Courses { get; }
    DbSet<CourseInstructor> CourseInstructors { get; }
    DbSet<CourseEnrollment> CourseEnrollments { get; }
    DbSet<CourseInviteLink> CourseInviteLinks { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
