namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class FlashcardChoiceConfiguration : IEntityTypeConfiguration<FlashcardChoice>
{
    public void Configure(EntityTypeBuilder<FlashcardChoice> builder)
    {
        builder.HasOne(c => c.CorrectAnswerForSingleChoicecard).WithOne(c => c.CorrectAnswer).HasForeignKey<FlashcardChoice>(c => c.CorrectAnswerForSingleChoicecardId);
        builder.HasOne(c => c.SingleChoiceCard).WithMany(c => c.Choices).HasForeignKey(c => c.SingleChoicecardId);
        builder.HasMany(c => c.AnswerForSinglecards).WithOne(c => c.SelectedChoice).HasForeignKey(c => c.SelectedChoiceId);

        builder.HasOne(c => c.CorrectAnswerForMultipleChoicecard).WithMany(c => c.CorrectAnswers).HasForeignKey(c => c.CorrectAnswerForMultipleChoicecardId);
        builder.HasOne(c => c.MultipleChoicecard).WithMany(c => c.Choices).HasForeignKey(c => c.MultipleChoicecardId);
        builder.HasMany(c => c.AnswersForMultipleChoicecards).WithMany(c => c.SelectedChoices);

        builder.HasOne(c => c.KeyForFlashcardPair).WithOne(c => c.KeyChoice).HasForeignKey<FlashcardPair>(c => c.KeyChoiceId);
        builder.HasOne(c => c.ValueForFlashcardPair).WithOne(c => c.ValueChoice).HasForeignKey<FlashcardPair>(c => c.ValueChoiceId);
        builder.HasMany(c => c.AnswerKeyForFlashcardPairs).WithOne(p => p.SelectedKey).HasForeignKey(p => p.SelectedKeyId);
        builder.HasMany(c => c.AnswerValueForFlashcardPairs).WithOne(p => p.SelectedValue).HasForeignKey(p => p.SelectedValueId);
    }
}
