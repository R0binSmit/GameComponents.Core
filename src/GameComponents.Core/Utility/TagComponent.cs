
using EntityComponentSystem.Interfaces;

namespace GameComponents.Core.Utility;

/// <summary>
/// Represents a component that manages a set of unique string tags for an entity.
/// </summary>
/// <remarks>TagComponent provides efficient methods to add, remove, and query tags associated with an entity.
/// Tags are compared using ordinal string comparison and are guaranteed to be unique within the component. This class
/// is typically used in entity-component systems to enable flexible categorization or filtering of entities based on
/// tags.</remarks>
public sealed class TagComponent : IComponent
{
    private readonly HashSet<string> _tags;
    public IReadOnlyCollection<string> Tags => _tags;
    public int Count => _tags.Count;

    public TagComponent()
    {
        _tags = new HashSet<string>(StringComparer.Ordinal);
    }

    public TagComponent(params string[] tags) : this((IEnumerable<string>)tags) { }

    public TagComponent(IEnumerable<string> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);
        _tags = new HashSet<string>(tags, StringComparer.Ordinal);
        ValidateTags(_tags, nameof(tags));
    }

    private static void ValidateTags(IEnumerable<string> tags, string paramName)
    {
        for(int i = 0; i < tags.Count(); i++)
        {
            if (string.IsNullOrWhiteSpace(tags.ElementAt(i)))
            {
                throw new ArgumentException(
                    $"Tag at index {i} is null, empty, or whitespace.",
                    paramName);
            }
        }
    }

    public bool Add(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        return _tags.Add(tag);
    }

    public bool Remove(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        return _tags.Remove(tag);
    }

    public bool Has(string tag)
    {
        return _tags.Contains(tag);
    }

    public bool HasAny(ReadOnlySpan<string> tags)
    {
        foreach (var tag in tags) 
        {
            if (Has(tag)) return true;
        }
        return false;
    }

    public bool HasAll(ReadOnlySpan<string> tags)
    {
        foreach (var tag in tags) 
        {
            if (!Has(tag)) return false;
        }
        return true;
    }

    public void Clear() => _tags.Clear();
}
