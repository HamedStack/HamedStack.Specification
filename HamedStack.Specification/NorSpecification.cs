using System.Linq.Expressions;

namespace HamedStack.Specification;

/// <summary>
/// Represents the logical NOR (Not Or) combination of two specifications.
/// </summary>
/// <typeparam name="T">The type of objects to be queried using the specification.</typeparam>
public class NorSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    /// Initializes a new instance of the <see cref="NorSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The left side specification.</param>
    /// <param name="right">The right side specification.</param>
    public NorSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));
    }

    /// <summary>
    /// Returns the combined expression of the left and right specifications using logical NOR.
    /// </summary>
    /// <returns>The combined expression.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var norExpression = Expression.Not(Expression.OrElse(leftExpression.Body, rightExpression.Body));

        return Expression.Lambda<Func<T, bool>>(norExpression, leftExpression.Parameters[0]);
    }
}