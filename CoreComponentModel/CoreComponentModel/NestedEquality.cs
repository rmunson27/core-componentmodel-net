using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// An equality comparer that determines if instances of a generic type are equal based on an equality comparer
/// supplied for the given generic type parameter.
/// </summary>
/// <remarks>
/// The <see cref="EqualityComparer{T}"/> methods are overridden to call the
/// <see cref="INestedEqualityComparer{TGeneric, TParameter}"/> method implementations.
/// </remarks>
/// <typeparam name="TGeneric">The generic type.</typeparam>
/// <typeparam name="TParameter">The parameter of the generic type.</typeparam>
public abstract class NestedEqualityComparer<TGeneric, TParameter>
    : EqualityComparer<TGeneric>, INestedEqualityComparer<TGeneric, TParameter>
{
    /// <summary>
    /// Determines if two objects of type <typeparamref name="TGeneric"/> are equal according to the default equality
    /// comparer on type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public sealed override bool Equals(
        [AllowNull, AllowDefault] TGeneric x, [AllowNull, AllowDefault] TGeneric y)
        => Equals(x, y, EqualityComparer<TParameter>.Default);

    /// <summary>
    /// When overridden in a derived class, determines if two objects of type <typeparamref name="TGeneric"/> are
    /// equal according to the supplied equality comparer on type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="nestedComparer"></param>
    /// <returns></returns>
    public abstract bool Equals(
        [AllowNull, AllowDefault] TGeneric x, [AllowNull, AllowDefault] TGeneric y,
        IEqualityComparer<TParameter> nestedComparer);

    /// <summary>
    /// Serves as a hash function for type <typeparamref name="TGeneric"/> based on the hash implementation
    /// of the supplied equality comparer for type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
#pragma warning disable CS8765, CS8767 // Attributes are designed to match .NET 5 (do not exist in .NET 4.6.1)
    public sealed override int GetHashCode([DisallowNull, DisallowDefault] TGeneric obj)
#pragma warning restore CS8765, CS8767
        => GetHashCode(obj, EqualityComparer<TParameter>.Default);

    /// <summary>
    /// When overridden in a derived class, serves as a hash function for type <typeparamref name="TGeneric"/> based
    /// on the hash implementation of the supplied equality comparer for type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="nestedComparer"></param>
    /// <returns></returns>
    public abstract int GetHashCode(
        [DisallowNull, DisallowDefault] TGeneric obj, IEqualityComparer<TParameter> nestedComparer);
}

/// <summary>
/// An equality comparer interface that determines if instances of a generic type are equal based on an equality
/// comparer supplied for the given generic type parameter.
/// </summary>
/// <typeparam name="TGeneric">The generic type.</typeparam>
/// <typeparam name="TParameter">The parameter of the generic type.</typeparam>
public interface INestedEqualityComparer<TGeneric, TParameter> : IEqualityComparer<TGeneric>
{
    /// <summary>
    /// Determines if two objects of type <typeparamref name="TGeneric"/> are equal according to the specified equality
    /// comparer on type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="nestedComparer"></param>
    /// <returns></returns>
    public bool Equals(
        [AllowNull, AllowDefault] TGeneric x, [AllowNull, AllowDefault] TGeneric y,
        IEqualityComparer<TParameter> nestedComparer);

    /// <summary>
    /// Serves as a hash function for type <typeparamref name="TGeneric"/> using the hash implementation of the
    /// supplied equality comparer for type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="nestedComparer"></param>
    /// <returns></returns>
    public int GetHashCode([DisallowNull, DisallowDefault] TGeneric obj, IEqualityComparer<TParameter> nestedComparer);
}
