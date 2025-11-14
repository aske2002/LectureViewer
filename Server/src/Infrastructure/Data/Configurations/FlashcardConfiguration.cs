namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class FlashcardConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.HasOne(f => f.Lecture).WithMany(l => l.Flashcards).HasForeignKey(f => f.LectureId);
        builder.HasOne(f => f.ContentCategory).WithMany(c => c.Flashcards).HasForeignKey(f => f.ContentCategoryId);
        builder.HasOne(f => f.ArtifactFromJob).WithMany().HasForeignKey(f => f.ArtifactFromJobId);

        builder.HasDiscriminator(f => f.FlashcardType)
            .HasValue<SingleChoiceFlashCard>(FlashcardType.SingleChoice)
            .HasValue<MultipleChoiceFlashCard>(FlashcardType.MultipleChoice)
            .HasValue<MatchingFlashcard>(FlashcardType.Matching)
            .HasValue<FreeTextFlashCard>(FlashcardType.FreeText)
            .HasValue<TrueFalseFlashCard>(FlashcardType.TrueFalse);
    }
}


internal class SingleChoiceFlashCardConfiguration : IEntityTypeConfiguration<SingleChoiceFlashCard>
{
    public void Configure(EntityTypeBuilder<SingleChoiceFlashCard> builder)
    {
        builder.HasMany(f => f.Choices).WithOne(c => c.SingleChoiceCard).HasForeignKey(c => c.SingleChoicecardId);
        builder.HasOne(f => f.CorrectAnswer).WithOne(c => c.CorrectAnswerForSingleChoicecard).HasForeignKey<FlashcardChoice>(c => c.CorrectAnswerForSingleChoicecardId);
    }
}

internal class MultipleChoiceFlashCardConfiguration : IEntityTypeConfiguration<MultipleChoiceFlashCard>
{
    public void Configure(EntityTypeBuilder<MultipleChoiceFlashCard> builder)
    {
        builder.HasMany(f => f.Choices).WithOne(c => c.MultipleChoicecard).HasForeignKey(c => c.MultipleChoicecardId);
        builder.HasMany(f => f.CorrectAnswers).WithOne(c => c.CorrectAnswerForMultipleChoicecard).HasForeignKey(c => c.CorrectAnswerForMultipleChoicecardId);
    }
}

internal class MatchingFlashCardConfiguration : IEntityTypeConfiguration<MatchingFlashcard>
{
    public void Configure(EntityTypeBuilder<MatchingFlashcard> builder)
    {
        builder.HasMany(f => f.Pairs).WithOne(p => p.Flashcard).HasForeignKey(p => p.FlashcardId);
    }
}

internal class FreeTextFlashCardConfiguration : IEntityTypeConfiguration<FreeTextFlashCard>
{
    public void Configure(EntityTypeBuilder<FreeTextFlashCard> builder)
    {

    }
}

internal class TrueFalseFlashCardConfiguration : IEntityTypeConfiguration<TrueFalseFlashCard>
{
    public void Configure(EntityTypeBuilder<TrueFalseFlashCard> builder)
    {

    }
}

