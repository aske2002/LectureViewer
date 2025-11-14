namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class FlashcardPairConfiguration : IEntityTypeConfiguration<FlashcardPair>
{
    public void Configure(EntityTypeBuilder<FlashcardPair> builder)
    {
        builder.HasOne(c => c.KeyChoice).WithOne(c => c.KeyForFlashcardPair).HasForeignKey<FlashcardPair>(fp => fp.KeyChoiceId);
        builder.HasOne(c => c.ValueChoice).WithOne(c => c.ValueForFlashcardPair).HasForeignKey<FlashcardPair>(fp => fp.ValueChoiceId);
    }
}

internal class FlashcardPairAnswerConfiguration : IEntityTypeConfiguration<FlashcardPairAnswer>
{
    public void Configure(EntityTypeBuilder<FlashcardPairAnswer> builder)
    {
        builder.HasOne(c => c.SelectedKey).WithMany(c => c.AnswerKeyForFlashcardPairs).HasForeignKey(fp => fp.SelectedKeyId);
        builder.HasOne(c => c.SelectedValue).WithMany(c => c.AnswerValueForFlashcardPairs).HasForeignKey(fp => fp.SelectedValueId);
    }
}