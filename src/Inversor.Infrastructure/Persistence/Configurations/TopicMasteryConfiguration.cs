
using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inversor.Infrastructure.Persistence.Configurations;

public class TopicMasteryConfiguration : IEntityTypeConfiguration<TopicMastery>
{
    public void Configure(EntityTypeBuilder<TopicMastery> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.MasteryScore).HasPrecision(5, 4);
        builder.Property(t => t.EasinessFactor).HasPrecision(5, 4);

        builder.HasOne(t => t.UserLanguageProfile)
               .WithMany(p => p.TopicMasteries)
               .HasForeignKey(t => t.UserLanguageProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.GrammarTopic)
               .WithMany()
               .HasForeignKey(t => t.GrammarTopicId)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}
