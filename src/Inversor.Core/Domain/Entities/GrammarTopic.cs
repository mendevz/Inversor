
namespace Inversor.Core.Domain.Entities;

public class GrammarTopic
{
    public Guid Id { get; private set; }
    public Guid MacroTagId { get; private set; }
    public string Tag { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string TheoryDescription { get; private set; } = string.Empty;
    public MacroTag MacroTag { get; private set; } = null!;

    private GrammarTopic() { }

    public static GrammarTopic Create(
            Guid macroTagId,
            string tag,
            string title,
            string theoryDescription
        )
    {
        if (macroTagId == Guid.Empty) throw new ArgumentException("MacroTag ID cannot be empty", nameof(macroTagId));
        if (string.IsNullOrWhiteSpace(tag)) throw new ArgumentException("Tag is required", nameof(tag));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required", nameof(title));
        if (string.IsNullOrWhiteSpace(theoryDescription)) throw new ArgumentException("Theory description is required", nameof(theoryDescription));
        
        var grammarTopic = new GrammarTopic
        {
            Id = Guid.NewGuid(),
            MacroTagId = macroTagId,
            Tag = tag.ToLower().Trim(),
            Title = title.Trim(),
            TheoryDescription = theoryDescription.Trim()
        };
        return grammarTopic;
    }
}
