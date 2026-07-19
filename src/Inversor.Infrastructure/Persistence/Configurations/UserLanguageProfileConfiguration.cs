
using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inversor.Infrastructure.Persistence.Configurations;

public class UserLanguageProfileConfiguration : IEntityTypeConfiguration<UserLanguageProfile>
{
    public void Configure(EntityTypeBuilder<UserLanguageProfile> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.LearnLanguageCode).HasMaxLength(10).IsRequired();
        builder.Property(p => p.AssessedLevel).HasMaxLength(5).IsRequired();

        builder.HasOne(p => p.User)
               .WithMany(u => u.LanguageProfiles)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
