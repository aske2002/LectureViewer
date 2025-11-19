using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Infrastructure.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

    // Course related DbSets
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseCategory> CourseCategories => Set<CourseCategory>();
    public DbSet<CourseInstructor> CourseInstructors => Set<CourseInstructor>();
    public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();
    public DbSet<CourseInviteLink> CourseInviteLinks => Set<CourseInviteLink>();

    // Job related DbSets
    public DbSet<MediaProcessingJob> MediaProcessingJobs => Set<MediaProcessingJob>();
    public DbSet<TranscriptionMediaProcessingJob> TranscriptionMediaProcessingJobs => Set<TranscriptionMediaProcessingJob>();
    public DbSet<FlashcardGenerationMediaProcessingJob> FlashcardGenerationMediaProcessingJobs => Set<FlashcardGenerationMediaProcessingJob>();
    public DbSet<ThumbnailExtractionMediaProcessingJob> ThumbnailExtractionMediaProcessingJobs => Set<ThumbnailExtractionMediaProcessingJob>();
    public DbSet<OfficeConversionMediaProcessingJob> OfficeConversionMediaProcessingJobs => Set<OfficeConversionMediaProcessingJob>();
    public DbSet<MediaTranscodingMediaProcessingJob> MediaConversionMediaProcessingJobs => Set<MediaTranscodingMediaProcessingJob>();
    public DbSet<KeywordExtractionMediaProcessingJob> KeywordExtractionMediaProcessingJobs => Set<KeywordExtractionMediaProcessingJob>();
    public DbSet<CategoryClassificationMediaProcessingJob> CategoryClassificationMediaProcessingJobs => Set<CategoryClassificationMediaProcessingJob>();
    public DbSet<MediaProcessingJobAttempt> MediaProcessingJobAttempts => Set<MediaProcessingJobAttempt>();
    public DbSet<MediaProcessingJobLog> MediaProcessingJobLogs => Set<MediaProcessingJobLog>();

    // Lecture related DbSets
    public DbSet<Lecture> Lectures => Set<Lecture>();
    public DbSet<LectureContent> LectureContents => Set<LectureContent>();
    public DbSet<LectureTranscript> LectureTranscripts => Set<LectureTranscript>();
    public DbSet<LectureTranscriptTimestamp> LectureTranscriptTimestamps => Set<LectureTranscriptTimestamp>();
    public DbSet<LectureTranscriptKeyword> LectureTranscriptKeywords => Set<LectureTranscriptKeyword>();
    public DbSet<LectureTranscriptKeywordOccurrence> LectureTranscriptKeywordOccurrences => Set<LectureTranscriptKeywordOccurrence>();  
    
    // Flashcard related DbSets
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();
    public DbSet<FlashcardChoice> FlashCardChoiceAnswers => Set<FlashcardChoice>();
    public DbSet<FlashcardPair> FlashCardPairAnswers => Set<FlashcardPair>();
    public DbSet<FlashcardAnswer> FlashCardChoiceAnswerOptions => Set<FlashcardAnswer>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureEntities();
    }
}
