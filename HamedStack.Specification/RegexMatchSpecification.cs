using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace HamedStack.Specification;

/// <summary>
/// Represents a specification that checks if a property matches a regex pattern.
/// </summary>
/// <typeparam name="T">The type of objects to be queried using the specification.</typeparam>
/// <typeparam name="TValue">The type of the property being checked.</typeparam>
public class RegexMatchSpecification<T, TValue> : Specification<T>
{
    private readonly Func<T, TValue?> _propertySelector;
    private readonly string _pattern;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegexMatchSpecification{T, TValue}"/> class.
    /// </summary>
    /// <param name="propertySelector">A function to select the property from an entity.</param>
    /// <param name="pattern">The regex pattern to match against the property.</param>
    public RegexMatchSpecification(Func<T, TValue?> propertySelector, string? pattern)
    {
        _propertySelector = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));
        _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
    }

    /// <summary>
    /// Returns an expression that checks if the selected property matches the regex pattern.
    /// </summary>
    /// <returns>The expression that checks for a regex match.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        return entity => Regex.IsMatch(_propertySelector(entity) == null ? string.Empty : _propertySelector(entity)!.ToString()!, _pattern);
    }
}