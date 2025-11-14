namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class MediaProcessingJobConfiguration : IEntityTypeConfiguration<MediaProcessingJob>
{
    public void Configure(EntityTypeBuilder<MediaProcessingJob> builder)
    {
        builder.Property(j => j.Status).HasConversion<string>();
        builder.Property(j => j.JobType).HasConversion<string>();
        builder.Property(j => j.ErrorMessage).HasMaxLength(2000);
        builder.HasMany(j => j.Attempts).WithOne(a => a.Job).HasForeignKey(a => a.JobId);
        builder.HasMany(j => j.DependentJobs).WithOne(j => j.ParentJob).HasForeignKey(j => j.ParentJobId);
        builder.Ignore(j => j.Status);

        builder.HasDiscriminator(f => f.JobType)
            .HasValue<TranscriptionMediaProcessingJob>(MediaJobType.Transcription)
            .HasValue<FlashcardGenerationMediaProcessingJob>(MediaJobType.FlashcardGeneration)
            .HasValue<ThumbnailExtractionMediaProcessingJob>(MediaJobType.ThumbnailExtraction)
            .HasValue<OfficeConversionMediaProcessingJob>(MediaJobType.OfficeConversion)
            .HasValue<MediaConversionMediaProcessingJob>(MediaJobType.MediaConversion)
            .HasValue<KeywordExtractionMediaProcessingJob>(MediaJobType.KeywordExtraction)
            .HasValue<CategoryClassificationMediaProcessingJob>(MediaJobType.CategoryClassification);
    }
}



internal class LectureRelatedMediaProcessingJobConfiguration : IEntityTypeConfiguration<LectureRelatedMediaProcessingJob>
{
    public void Configure(EntityTypeBuilder<LectureRelatedMediaProcessingJob> builder)
    {
        builder.HasOne(j => j.LectureContent).WithMany(l => l.ProcessingJobs).HasForeignKey(j => j.LectureContentId);
    }
}

internal class OfficeConversionMediaProcessingJobConfiguration : IEntityTypeConfiguration<OfficeConversionMediaProcessingJob>
{
    public void Configure(EntityTypeBuilder<OfficeConversionMediaProcessingJob> builder)
    {
        builder.HasOne(j => j.InputResource).WithMany().HasForeignKey(j => j.InputResourceId);
        builder.HasOne(j => j.OutputResource).WithMany().HasForeignKey(j => j.OutputResourceId);
    }
}

internal class MediaConversionMediaProcessingJobConfiguration : IEntityTypeConfiguration<MediaConversionMediaProcessingJob>
{
    public void Configure(EntityTypeBuilder<MediaConversionMediaProcessingJob> builder)
    {
        builder.HasOne(j => j.InputResource).WithMany().HasForeignKey(j => j.InputResourceId);
        builder.HasOne(j => j.OutputResource).WithMany().HasForeignKey(j => j.OutputResourceId);
    }
}