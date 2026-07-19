
namespace Inversor.Core.Domain.Entities;

public class UserLanguageProfile
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string NativeLanguageCode { get; private set; } = string.Empty;
    public string LearnLanguageCode { get; private set; } = string.Empty;
    public string AssessedLevel { get; private set; } = string.Empty;
    public int DailyRequestCount { get; private set; }
    public DateTime LastRequestDate { get; private set; }

    public User User { get; private set; } = null!;

    private readonly List<TranslationSubmission> _submissions = [];
    public IReadOnlyCollection<TranslationSubmission> Submissions => _submissions.AsReadOnly();

    private readonly List<TopicMastery> _topicMasteries = [];
    public IReadOnlyCollection<TopicMastery> TopicMasteries => _topicMasteries.AsReadOnly();

    private UserLanguageProfile()
    {
        
    }

    public static UserLanguageProfile Create(
            Guid userId,
            string nativeLanguageCode,
            string learnLanguageCode,
            string assessedLevel
        )
    {

        if (userId == Guid.Empty) throw new ArgumentException("User ID cannot be empty", nameof(userId));
        if (string.IsNullOrWhiteSpace(learnLanguageCode)) throw new ArgumentException("Language code is required", nameof(learnLanguageCode));
        if (string.IsNullOrWhiteSpace(nativeLanguageCode)) throw new ArgumentException("Native language code is required", nameof(nativeLanguageCode));
        if (string.IsNullOrWhiteSpace(assessedLevel)) throw new ArgumentException("Assessed level is required", nameof(assessedLevel));

        var userLanguageProfile = new UserLanguageProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NativeLanguageCode = nativeLanguageCode.ToLower().Trim(),
            LearnLanguageCode = learnLanguageCode.ToLower().Trim(),
            AssessedLevel = assessedLevel.ToUpper().Trim(),
            DailyRequestCount = 0, 
            LastRequestDate = DateTime.UtcNow
        };
        return userLanguageProfile;
    }

    /// <summary>
    /// Ejecuta el intento de registrar una petición. 
    /// Restablece el contador si la última petición fue en un día anterior.
    /// </summary>
    public void TrackNewRequest()
    {
        var now = DateTime.UtcNow;

        if (LastRequestDate.Date < now.Date)
        {
            DailyRequestCount = 0;
        }

        DailyRequestCount++;
        LastRequestDate = now;
    }

    /// <summary>
    /// Permite actualizar el nivel evaluado cuando el MasteryScore cambie drásticamente.
    /// </summary>
    public void UpdateAssessedLevel(string newLevel)
    {
        if (string.IsNullOrWhiteSpace(newLevel)) throw new ArgumentException("New level cannot be empty");
        AssessedLevel = newLevel.ToUpper().Trim();
    }
}
