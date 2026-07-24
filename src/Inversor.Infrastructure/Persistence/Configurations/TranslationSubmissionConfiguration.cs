
using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inversor.Infrastructure.Persistence.Configurations;

public class TranslationSubmissionConfiguration : IEntityTypeConfiguration<TranslationSubmission>
{
    public void Configure(EntityTypeBuilder<TranslationSubmission> builder)
    {
        builder.ToTable("TranslationSubmissions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Mode)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.OriginalInput)
            .IsRequired();

        // Mapeo del Enum a string en PostgreSQL
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Columnas opcionales al nacer en Pending
        builder.Property(x => x.CorrectedOutput)
            .IsRequired(false);
        builder.Property(x => x.GeneralFeedback)
            .IsRequired(false);
        builder.Property(x => x.FailureReason)
            .HasMaxLength(1000)
            .IsRequired(false);
        builder.Property(x => x.ProcessedAt)
            .IsRequired(false);
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.UserLanguageProfile)
            .WithMany(p => p.Submissions)
            .HasForeignKey(x => x.UserLanguageProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.SubmitTags)
            .WithOne(t => t.TranslationSubmission)
            .HasForeignKey(t => t.TranslationSubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
