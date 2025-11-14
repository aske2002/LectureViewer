namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class FlashcardAnswerConfiguration : IEntityTypeConfiguration<FlashcardAnswer>
{
    public void Configure(EntityTypeBuilder<FlashcardAnswer> builder)
    {
        builder.HasOne(f => f.Flashcard).WithMany(f => f.Answers).HasForeignKey(f => f.FlashcardId);
        builder.HasOne(f => f.CourseEnrollment).WithMany(c => c.FlashcardAnswers).HasForeignKey(f => f.CourseEnrollmentId);

        builder.HasDiscriminator(f => f.FlashcardType)
            .HasValue<SingleChoiceFlashcardAnswer>(FlashcardType.SingleChoice)
            .HasValue<MultipleChoiceFlashcardAnswer>(FlashcardType.MultipleChoice)
            .HasValue<MatchingFlashcardAnswer>(FlashcardType.Matching)
            .HasValue<FreeTextFlashcardAnswer>(FlashcardType.FreeText)
            .HasValue<TrueFalseFlashcardAnswer>(FlashcardType.TrueFalse);
    }
}

internal class SingleChoiceFlashcardAnswerConfiguration : IEntityTypeConfiguration<SingleChoiceFlashcardAnswer>
{
    public void Configure(EntityTypeBuilder<SingleChoiceFlashcardAnswer> builder)
    {
        builder.HasOne(f => f.SelectedChoice).WithMany(c => c.AnswerForSinglecards).HasForeignKey(c => c.SelectedChoiceId);
    }
}

internal class MultipleChoiceFlashcardAnswerConfiguration : IEntityTypeConfiguration<MultipleChoiceFlashcardAnswer>
{
    public void Configure(EntityTypeBuilder<MultipleChoiceFlashcardAnswer> builder)
    {
        builder.HasMany(f => f.SelectedChoices).WithMany(c => c.AnswersForMultipleChoicecards);
    }
}

internal class MatchingFlashcardAnswerConfiguration : IEntityTypeConfiguration<MatchingFlashcardAnswer>
{
    public void Configure(EntityTypeBuilder<MatchingFlashcardAnswer> builder)
    {
        builder.HasMany(f => f.PairAnswers).WithOne(p => p.FlashcardAnswer).HasForeignKey(p => p.FlashcardAnswerId);
    }
}

internal class FreeTextFlashcardAnswerConfiguration : IEntityTypeConfiguration<FreeTextFlashcardAnswer>
{
    public void Configure(EntityTypeBuilder<FreeTextFlashcardAnswer> builder)
    {

    }
}

internal class TrueFalseFlashcardAnswerConfiguration : IEntityTypeConfiguration<TrueFalseFlashcardAnswer>
{
    public void Configure(EntityTypeBuilder<TrueFalseFlashcardAnswer> builder)
    {

    }
}
