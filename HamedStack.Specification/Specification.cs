// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Linq.Expressions;

namespace HamedStack.Specification;

/// <summary>
/// Provides the base functionality for creating and combining query specifications.
/// </summary>
/// <typeparam name="T">The type of objects to be queried using the specification.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <summary>
    /// Converts this specification into an expression.
    /// </summary>
    /// <returns>An expression representing the query specification.</returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Determines if the given entity satisfies the current specification.
    /// </summary>
    /// <param name="entity">The entity to evaluate.</param>
    /// <returns>true if the entity satisfies the specification; otherwise, false.</returns>
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }

    /// <summary>
    /// Combines the current specification with the provided one using logical AND.
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>A new specification.</returns>
    public Specification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines the current specification with the provided one using logical OR.
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>A new specification.</returns>
    public Specification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    /// <summary>
    /// Negates the current specification.
    /// </summary>
    /// <returns>A new negated specification.</returns>
    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }

    /// <summary>
    /// Combines the current specification with the provided one using logical NAND (NOT AND).
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>A new specification.</returns>
    public Specification<T> Nand(ISpecification<T> specification)
    {
        return new NandSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines the current specification with the provided one using logical NOR (NOT OR).
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>A new specification.</returns>
    public Specification<T> Nor(ISpecification<T> specification)
    {
        return new NorSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines the current specification with the provided one using logical XOR (Exclusive OR).
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>A new specification.</returns>
    public Specification<T> Xor(ISpecification<T> specification)
    {
        return new XorSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines the current specification with the provided one using logical XNOR (Exclusive NOR or Equivalence).
    /// </summary>
    /// <param name="specification">The specification to combine with.</param>
    /// <returns>A new specification.</returns>
    public Specification<T> Xnor(ISpecification<T> specification)
    {
        return new XnorSpecification<T>(this, specification);
    }

    /// <summary>
    /// Creates a specification that checks if a property matches a regex pattern.
    /// </summary>
    /// <typeparam name="TValue">The type of the property being checked.</typeparam>
    /// <param name="propertySelector">A function to select the property from an entity.</param>
    /// <param name="pattern">The regex pattern to match against the property.</param>
    /// <returns>The specification.</returns>
    public static Specification<T> RegexMatch<TValue>(Func<T, TValue> propertySelector, string pattern)
    {
        return new RegexMatchSpecification<T, TValue>(propertySelector, pattern);
    }

    /// <summary>
    /// Creates a specification that checks if any item in a collection satisfies a given specification.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the collection.</typeparam>
    /// <param name="propertySelector">A function to select the collection from the entity.</param>
    /// <param name="itemSpecification">The specification to be satisfied by any item in the collection.</param>
    /// <returns>The specification.</returns>
    public Specification<T> Any<TItem>(Expression<Func<T, IEnumerable<TItem>>> propertySelector, ISpecification<TItem> itemSpecification)
    {
        return new AnySpecification<T, TItem>(propertySelector, itemSpecification);
    }

    /// <summary>
    /// Creates a specification that checks if all items in a collection satisfy a given specification.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the collection.</typeparam>
    /// <param name="propertySelector">A function to select the collection from the entity.</param>
    /// <param name="itemSpecification">The specification to be satisfied by all items in the collection.</param>
    /// <returns>The specification.</returns>
    public Specification<T> All<TItem>(Expression<Func<T, IEnumerable<TItem>>> propertySelector, ISpecification<TItem> itemSpecification)
    {
        return new AllSpecification<T, TItem>(propertySelector, itemSpecification);
    }

    /// <summary>
    /// Creates a specification that checks if at least 'n' items in a collection satisfy a given specification.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the collection.</typeparam>
    /// <param name="propertySelector">A function to select the collection from the entity.</param>
    /// <param name="itemSpecification">The specification to be satisfied by the items.</param>
    /// <param name="count">The minimum number of items that should satisfy the specification.</param>
    /// <returns>The specification.</returns>
    public Specification<T> AtLeast<TItem>(Expression<Func<T, IEnumerable<TItem>>> propertySelector, ISpecification<TItem> itemSpecification, int count)
    {
        return new AtLeastSpecification<T, TItem>(propertySelector, itemSpecification, count);
    }

    /// <summary>
    /// Creates a specification that checks if at most 'n' items in a collection satisfy a given specification.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the collection.</typeparam>
    /// <param name="propertySelector">A function to select the collection from the entity.</param>
    /// <param name="itemSpecification">The specification to be satisfied by the items.</param>
    /// <param name="count">The maximum number of items that should satisfy the specification.</param>
    /// <returns>The specification.</returns>
    public Specification<T> AtMost<TItem>(Expression<Func<T, IEnumerable<TItem>>> propertySelector, ISpecification<TItem> itemSpecification, int count)
    {
        return new AtMostSpecification<T, TItem>(propertySelector, itemSpecification, count);
    }
}