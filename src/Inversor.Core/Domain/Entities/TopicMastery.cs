namespace Inversor.Core.Domain.Entities;

public class TopicMastery
{
    public Guid Id { get; private set; }
    public Guid UserLanguageId { get; private set; }
    public Guid GrammarTopicId { get; private set; }
    public decimal MasteryScore { get; private set; }
    public int TotalAttempts { get; private set; }
    public int ConsecutiveSuccesses { get; private set; }
    public int CurrentIntervalDays { get; private set; }
    public decimal EasinessFactor { get; private set; } 
    public DateTime NextReviewDate { get; private set; }

    public UserLanguageProfile UserLanguage { get; private set; } = null!;
    public GrammarTopic GrammarTopic { get; private set; } = null!;

    private TopicMastery() { }

    public static TopicMastery Create(
            Guid userLanguageId,
            Guid grammarTopicId
        )
    {
        if (userLanguageId == Guid.Empty) throw new ArgumentException("UserLanguage ID cannot be empty", nameof(userLanguageId));
        if (grammarTopicId == Guid.Empty) throw new ArgumentException("GrammarTopic ID cannot be empty", nameof(grammarTopicId));
        var topicMastery = new TopicMastery
        {
            Id = Guid.NewGuid(),
            UserLanguageId = userLanguageId,
            GrammarTopicId = grammarTopicId,
            MasteryScore = 0.0m,
            TotalAttempts = 0,
            ConsecutiveSuccesses = 0,
            CurrentIntervalDays = 1,
            EasinessFactor = 2.5m, 
            NextReviewDate = DateTime.UtcNow.AddDays(1)
        };
        return topicMastery;
    }

    /// <summary>
    /// Update the topic mastery based on whether the attempt was an error or a success.
    /// </summary>
    public void RecordAttempt(bool isError)
    {
        TotalAttempts++;

        if (isError)
        {
            ConsecutiveSuccesses = 0;
            CurrentIntervalDays = 1;

            EasinessFactor = Math.Max(1.3m, EasinessFactor - 0.20m);

            decimal penalty = MasteryScore > 0.5m ? 0.20m : 0.10m;
            MasteryScore = Math.Max(0m, MasteryScore - penalty);
        }
        else
        {
            ConsecutiveSuccesses++;
            MasteryScore = Math.Min(1m, MasteryScore + 0.10m);

            EasinessFactor = Math.Min(3.0m, EasinessFactor + 0.10m);

            if (ConsecutiveSuccesses == 1)
            {
                CurrentIntervalDays = 2;
            }
            else
            {
                CurrentIntervalDays = (int)Math.Round(
                    CurrentIntervalDays * EasinessFactor, 
                    MidpointRounding.AwayFromZero
                );
            }
        }

        NextReviewDate = DateTime.UtcNow.AddDays(CurrentIntervalDays);
    }
}
