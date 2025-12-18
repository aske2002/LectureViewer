using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Infrastructure.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Resource> Resources => Set<Resource>();

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
    public DbSet<LectureProcessingJob> LectureProcessingJobs => Set<LectureProcessingJob>();
    public DbSet<OfficeConversionMediaProcessingJob> OfficeConversionMediaProcessingJobs => Set<OfficeConversionMediaProcessingJob>();
    public DbSet<MediaTranscodingMediaProcessingJob> MediaConversionMediaProcessingJobs => Set<MediaTranscodingMediaProcessingJob>();
    public DbSet<KeywordExtractionMediaProcessingJob> KeywordExtractionMediaProcessingJobs => Set<KeywordExtractionMediaProcessingJob>();
    public DbSet<CategoryClassificationMediaProcessingJob> CategoryClassificationMediaProcessingJobs => Set<CategoryClassificationMediaProcessingJob>();
    public DbSet<MediaProcessingJobAttempt> MediaProcessingJobAttempts => Set<MediaProcessingJobAttempt>();
    public DbSet<MediaProcessingJobLog> MediaProcessingJobLogs => Set<MediaProcessingJobLog>();
    public DbSet<ResumeExtractionMediaProcessingJob> ResumeExtractionMediaProcessingJobs => Set<ResumeExtractionMediaProcessingJob>();
    public DbSet<DocumentInfoExtractionJob> DocumentInfoExtractionJobs => Set<DocumentInfoExtractionJob>();

    // Lecture related DbSets
    public DbSet<Lecture> Lectures => Set<Lecture>();
    public DbSet<LectureContent> LectureContents => Set<LectureContent>();

    // Keyword related DbSets
    public DbSet<Keyword> Keywords => Set<Keyword>();
    public DbSet<DocumentKeyword> DocumentKeywords => Set<DocumentKeyword>();
    public DbSet<TranscriptKeywordOccurrence> TranscriptKeywordOccurrences => Set<TranscriptKeywordOccurrence>();
    public DbSet<CourseKeyword> CourseKeywords => Set<CourseKeyword>();

    // Transcript related DbSets
    public DbSet<Transcript> Transcripts => Set<Transcript>();
    public DbSet<TranscriptItem> TranscriptItems => Set<TranscriptItem>();

    // Document related DbSets
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentPage> DocumentPages => Set<DocumentPage>();

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
