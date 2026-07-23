using Inversor.Core.Application.Abstractions;
using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inversor.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserLanguageProfile> UserLanguageProfiles => Set<UserLanguageProfile>();
    public DbSet<MacroTag> MacroTags => Set<MacroTag>();
    public DbSet<GrammarTopic> GrammarTopics => Set<GrammarTopic>();
    public DbSet<TranslationSubmission> TranslationSubmissions => Set<TranslationSubmission>();
    public DbSet<SubmitTag> SubmitTags => Set<SubmitTag>();
    public DbSet<TopicMastery> TopicMasteries => Set<TopicMastery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
