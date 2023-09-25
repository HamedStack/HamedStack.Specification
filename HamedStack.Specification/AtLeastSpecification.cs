using System.Linq.Expressions;

namespace HamedStack.Specification;

/// <summary>
/// Represents a specification that checks if at least 'n' items in a collection satisfy another specification.
/// </summary>
/// <typeparam name="T">The type of the object containing the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public class AtLeastSpecification<T, TItem> : Specification<T>
{
    private readonly Expression<Func<T, IEnumerable<TItem>>> _propertySelector;
    private readonly ISpecification<TItem> _itemSpecification;
    private readonly int _n;
    /// <summary>
    /// Initializes a new instance of the <see cref="AtLeastSpecification{T, TItem}"/> class.
    /// </summary>
    /// <param name="propertySelector">The expression to select the collection from the object.</param>
    /// <param name="itemSpecification">The specification that an item should satisfy.</param>
    /// <param name="n">The minimum number of items that should satisfy the specification.</param>
    public AtLeastSpecification(Expression<Func<T, IEnumerable<TItem>>> propertySelector, ISpecification<TItem> itemSpecification, int n)
    {
        _propertySelector = propertySelector;
        _itemSpecification = itemSpecification;
        _n = n;
    }
    /// <summary>
    /// Converts the specification into a LINQ expression.
    /// </summary>
    /// <returns>A LINQ expression that represents the specification.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var entityParameter = Expression.Parameter(typeof(T), "entity");
        var countMethod = typeof(Enumerable).GetMethods().First(m => m.Name == "Count" && m.GetParameters().Length == 2);
        countMethod = countMethod.MakeGenericMethod(typeof(TItem));
        var innerLambda = _itemSpecification.ToExpression();
        var countExpression = Expression.Call(countMethod, Expression.Invoke(_propertySelector, entityParameter), innerLambda);
        var atLeastExpression = Expression.GreaterThanOrEqual(countExpression, Expression.Constant(_n));
        return Expression.Lambda<Func<T, bool>>(atLeastExpression, entityParameter);
    }
}