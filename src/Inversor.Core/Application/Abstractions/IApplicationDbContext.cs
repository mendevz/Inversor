using Inversor.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inversor.Core.Application.Abstractions;
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserLanguageProfile> UserLanguageProfiles { get; }
    DbSet<MacroTag> MacroTags { get; }
    DbSet<GrammarTopic> GrammarTopics { get; }
    DbSet<TranslationSubmission> TranslationSubmissions { get; }
    DbSet<SubmitTag> SubmitTags { get; }
    DbSet<TopicMastery> TopicMasteries { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
