using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

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
    /// Gets a default <see cref="NestedEqualityComparer{TGeneric, TParameter}"/>.
    /// </summary>
    /// <remarks>
    /// This will use the implementation of <see cref="INestedEquatable{TGeneric, TParameter}"/> on
    /// <typeparamref name="TGeneric"/> in order to satisfy the requirements of a
    /// <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> if possible, otherwise it will use the default
    /// <see cref="EqualityComparer{T}"/> for type <typeparamref name="TGeneric"/>,
    /// ignoring <typeparamref name="TParameter"/>.
    /// </remarks>
    public static new NestedEqualityComparer<TGeneric, TParameter> Default { get; }

    static NestedEqualityComparer()
    {
        if (typeof(INestedEquatable<TGeneric, TParameter>).IsAssignableFrom(typeof(TGeneric)))
        {
            // We can use the implementation of INestedEquatable to determine nested equality for the type
            Default = (NestedEqualityComparer<TGeneric, TParameter>)
                        typeof(NestedEquatableEqualityComparer<,>)
                            .MakeGenericType(typeof(TGeneric), typeof(TParameter))
                            .GetConstructor(new Type[] { })
                            .Invoke(new object[] { });
        }
        else
        {
            Default = new EquatableEqualityComparer<TGeneric, TParameter>();
        }
    }

    /// <summary>
    /// Determines if two objects of type <typeparamref name="TGeneric"/> are equal according to the default equality
    /// comparer on type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="x">The first instance of <typeparamref name="TGeneric"/> to compare.</param>
    /// <param name="y">The second instance of <typeparamref name="TGeneric"/> to compare.</param>
    /// <returns>Whether or not <paramref name="x"/> and <paramref name="y"/> were equal.</returns>
    public sealed override bool Equals(TGeneric? x, TGeneric? y) => Equals(x, y, null);

    /// <inheritdoc/>
    public abstract bool Equals(TGeneric? x, TGeneric? y, IEqualityComparer<TParameter>? nestedComparer);

    /// <summary>
    /// Serves as a hash function for type <typeparamref name="TGeneric"/> based on the hash implementation
    /// of the default equality comparer for type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="obj">The instance of <typeparamref name="TGeneric"/> to get a hash code for.</param>
    /// <returns>A hash code for <paramref name="obj"/>.</returns>
#pragma warning disable CS8765, CS8767 // Attributes are designed to match .NET 5+ (do not exist in .NET 4.6.1)
    public sealed override int GetHashCode([DisallowNull, DisallowDefault] TGeneric obj)
#pragma warning restore CS8765, CS8767
        => GetHashCode(obj, EqualityComparer<TParameter>.Default);

    /// <inheritdoc/>
    public abstract int GetHashCode(
        [DisallowNull, DisallowDefault] TGeneric obj, IEqualityComparer<TParameter>? nestedComparer);
}

/// <summary>
/// Provides the default <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> when
/// <typeparamref name="TGeneric"/> implements <see cref="INestedEquatable{TGeneric, TParameter}"/>.
/// </summary>
/// <typeparam name="TGeneric"></typeparam>
/// <typeparam name="TParameter"></typeparam>
file sealed class NestedEquatableEqualityComparer<TGeneric, TParameter> : NestedEqualityComparer<TGeneric, TParameter>
    where TGeneric : INestedEquatable<TGeneric, TParameter>
{
    public override bool Equals(TGeneric? x, TGeneric? y, IEqualityComparer<TParameter>? nestedComparer)
        => x is null
            ? y is null
            : y is not null && x.Equals(y, nestedComparer);
    public override int GetHashCode([DisallowDefault, DisallowNull] TGeneric obj,
                                    IEqualityComparer<TParameter>? nestedComparer)
        => obj is null
            ? throw new ArgumentNullException(nameof(obj))
            : obj.GetHashCode(nestedComparer);
}

/// <summary>
/// Provides the default <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> when
/// <typeparamref name="TGeneric"/> does not implement <see cref="INestedEquatable{TGeneric, TParameter}"/>.
/// </summary>
/// <typeparam name="TGeneric"></typeparam>
/// <typeparam name="TParameter"></typeparam>
file sealed class EquatableEqualityComparer<TGeneric, TParameter> : NestedEqualityComparer<TGeneric, TParameter>
{
    public override bool Equals(TGeneric? x, TGeneric? y, IEqualityComparer<TParameter>? nestedComparer)
        => EqualityComparer<TGeneric?>.Default.Equals(x, y);

    public override int GetHashCode([DisallowDefault, DisallowNull] TGeneric obj,
                                    IEqualityComparer<TParameter>? nestedComparer)
        => EqualityComparer<TGeneric>.Default.GetHashCode();
}

/// <summary>
/// An equality comparer interface that determines if instances of a generic type are equal based on an equality
/// comparer supplied for the given generic type parameter.
/// </summary>
/// <typeparam name="TGeneric">The generic type.</typeparam>
/// <typeparam name="TParameter">The parameter of the generic type.</typeparam>
public interface INestedEqualityComparer<in TGeneric, out TParameter> : IEqualityComparer<TGeneric>
{
    /// <summary>
    /// Determines if two objects of type <typeparamref name="TGeneric"/> are equal according to the specified equality
    /// comparer on type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="nestedComparer"></param>
    /// <returns></returns>
    public bool Equals(TGeneric? x, TGeneric? y, IEqualityComparer<TParameter>? nestedComparer);

    /// <summary>
    /// Serves as a hash function for type <typeparamref name="TGeneric"/> using the hash implementation of the
    /// supplied equality comparer for type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="nestedComparer"></param>
    /// <returns></returns>
    public int GetHashCode([DisallowNull, DisallowDefault] TGeneric obj,
                           IEqualityComparer<TParameter>? nestedComparer);
}

/// <summary>
/// An interface for types that can be compared for equality by specifying the equality comparer used for
/// <typeparamref name="TParameter"/> parameters.
/// </summary>
/// <typeparam name="TGeneric">The generic type.</typeparam>
/// <typeparam name="TParameter">The parameter of the generic type.</typeparam>
public interface INestedEquatable<TGeneric, out TParameter> : IEquatable<TGeneric>
{
    /// <summary>
    /// Determines if this instance is equal to the <typeparamref name="TGeneric"/> passed in, using the specified
    /// <see cref="IEqualityComparer{T}"/> to equate members of type <typeparamref name="TParameter"/>.
    /// </summary>
    /// <param name="other">The <typeparamref name="TGeneric"/> value to check for equality.</param>
    /// <param name="nestedComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to equate <typeparamref name="TParameter"/> members, or
    /// <see langword="null"/> to use the default equality comparer for type <typeparamref name="TParameter"/>.
    /// </param>
    /// <returns>Whether or not the two instances are equal.</returns>
    public bool Equals(TGeneric? other, IEqualityComparer<TParameter>? nestedComparer);

    /// <summary>
    /// Gets a hash code for this instance using the specified <see cref="IEqualityComparer{T}"/>.
    /// </summary>
    /// <param name="nestedComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to generate the hash code using members of type
    /// <typeparamref name="TParameter"/>, or <see langword="null"/> to use the default equality comparer for
    /// type <typeparamref name="TParameter"/>.
    /// </param>
    /// <returns>A hash code for this instance.</returns>
    public int GetHashCode(IEqualityComparer<TParameter>? nestedComparer);
}
