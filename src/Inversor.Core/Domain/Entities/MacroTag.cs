namespace Inversor.Core.Domain.Entities;

public class MacroTag
{
    public Guid Id { get; private set; }
    public string Tag { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private readonly List<GrammarTopic> _GrammarTopics = [];
    public IReadOnlyCollection<GrammarTopic> GrammarTopics => _GrammarTopics.AsReadOnly();

    private MacroTag() { }

    public static MacroTag Create(string tag, string description)
    {
        if (string.IsNullOrWhiteSpace(tag)) throw new ArgumentException("Tag is required", nameof(tag));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));
        var macroTag = new MacroTag
        {
            Id = Guid.NewGuid(),
            Tag = tag.ToUpper().Trim(),
            Description = description.Trim()
        };
        return macroTag;
    }
   
    public static MacroTag CreateWithId(Guid id, string tag, string description)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id is required", nameof(id));
        if (string.IsNullOrWhiteSpace(tag)) throw new ArgumentException("Tag is required", nameof(tag));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));
        var macroTag = new MacroTag
        {
            Id = id,
            Tag = tag.ToUpper().Trim(),
            Description = description.Trim()
        };
        return macroTag;
    }
}
