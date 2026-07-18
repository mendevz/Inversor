namespace Inversor.Core.Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string SubscriptionTier { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime LastUpdatedAt { get; private set; }

    private readonly List<UserLanguageProfile> _languageProfiles = [];
    public IReadOnlyCollection<UserLanguageProfile> LanguageProfiles => _languageProfiles.AsReadOnly();

    private User()
    {
        
    }

    public static User Create(
            string name,
            string lastName,
            string username,
            string email,
            string? phone,
            string subscriptionTier 
        )
    {

        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required", nameof(lastName));
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required", nameof(username));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(subscriptionTier)) throw new ArgumentException("Subscription tier is required", nameof(subscriptionTier));
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            LastName = lastName,
            Username = username,
            Email = email,
            Phone = phone,
            SubscriptionTier = subscriptionTier,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        return user;
    }
}
