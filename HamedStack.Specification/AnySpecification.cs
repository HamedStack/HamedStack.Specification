using System.Linq.Expressions;

namespace HamedStack.Specification;

/// <summary>
/// Represents a specification that checks if any item in a collection satisfies another specification.
/// </summary>
/// <typeparam name="T">The type of the object containing the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public class AnySpecification<T, TItem> : Specification<T>
{
    private readonly Expression<Func<T, IEnumerable<TItem>>> _propertySelector;
    private readonly ISpecification<TItem> _itemSpecification;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnySpecification{T, TItem}"/> class.
    /// </summary>
    /// <param name="propertySelector">The expression to select the collection from the object.</param>
    /// <param name="itemSpecification">The specification that an item should satisfy.</param>
    public AnySpecification(Expression<Func<T, IEnumerable<TItem>>> propertySelector, ISpecification<TItem> itemSpecification)
    {
        _propertySelector = propertySelector;
        _itemSpecification = itemSpecification;
    }
    /// <summary>
    /// Converts the specification into a LINQ expression.
    /// </summary>
    /// <returns>A LINQ expression that represents the specification.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var entityParameter = Expression.Parameter(typeof(T), "entity");
        var anyMethod = typeof(Enumerable).GetMethods().First(m => m.Name == "Any" && m.GetParameters().Length == 2);
        anyMethod = anyMethod.MakeGenericMethod(typeof(TItem));
        var innerLambda = _itemSpecification.ToExpression();
        var anyExpression = Expression.Call(anyMethod, Expression.Invoke(_propertySelector, entityParameter), innerLambda);
        return Expression.Lambda<Func<T, bool>>(anyExpression, entityParameter);
    }
}