using System.Collections.Generic;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<ApplicationUser> Users { get; }

    virtual DatabaseFacade Database => this.Database;
    DbSet<MediaProcessingJob> MediaProcessingJobs { get; }
    DbSet<TranscriptionMediaProcessingJob> TranscriptionMediaProcessingJobs { get; }
    DbSet<FlashcardGenerationMediaProcessingJob> FlashcardGenerationMediaProcessingJobs { get; }
    DbSet<ThumbnailExtractionMediaProcessingJob> ThumbnailExtractionMediaProcessingJobs { get; }
    DbSet<LectureProcessingJob> LectureProcessingJobs { get; }
    DbSet<OfficeConversionMediaProcessingJob> OfficeConversionMediaProcessingJobs { get; }
    DbSet<MediaTranscodingMediaProcessingJob> MediaConversionMediaProcessingJobs { get; }
    DbSet<KeywordExtractionMediaProcessingJob> KeywordExtractionMediaProcessingJobs { get; }
    DbSet<CategoryClassificationMediaProcessingJob> CategoryClassificationMediaProcessingJobs { get; }
    DbSet<MediaProcessingJobAttempt> MediaProcessingJobAttempts { get; }
    DbSet<MediaProcessingJobLog> MediaProcessingJobLogs { get; }

    DbSet<Country> Countries { get; }
    DbSet<Resource> Resources { get; }
    DbSet<Lecture> Lectures { get; }
    DbSet<LectureContent> LectureContents { get; }
    DbSet<Course> Courses { get; }
    DbSet<CourseInstructor> CourseInstructors { get; }
    DbSet<CourseEnrollment> CourseEnrollments { get; }
    DbSet<CourseInviteLink> CourseInviteLinks { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
