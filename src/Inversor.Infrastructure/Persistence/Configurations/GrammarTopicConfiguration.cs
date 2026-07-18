
using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inversor.Infrastructure.Persistence.Configurations;

public class GrammarTopicConfiguration : IEntityTypeConfiguration<GrammarTopic>
{
    public void Configure(EntityTypeBuilder<GrammarTopic> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Tag).HasMaxLength(100).IsRequired();
        builder.HasIndex(g => g.Tag).IsUnique();

        builder.Property(g => g.Title).HasMaxLength(150);

        builder.HasOne(g => g.MacroTag)
               .WithMany(m => m.GrammarTopics)
               .HasForeignKey(g => g.MacroTagId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
