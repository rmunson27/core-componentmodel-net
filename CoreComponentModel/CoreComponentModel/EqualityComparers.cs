using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Extension methods and other static functionality for working with <see cref="IEqualityComparer{T}"/> instances.
/// </summary>
public static class EqualityComparers
{
    /// <summary>
    /// Gets the current <see cref="IEqualityComparer{T}"/>, or <see cref="EqualityComparer{T}.Default"/> if the
    /// current instance is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of value being compared.</typeparam>
    /// <param name="comparer"></param>
    /// <returns>
    /// The current instance, or <see cref="EqualityComparer{T}.Default"/> if the current instance
    /// is <see langword="null"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEqualityComparer<T> DefaultIfNull<T>(this IEqualityComparer<T>? comparer)
        => comparer ?? EqualityComparer<T>.Default;

    /// <summary>
    /// Gets the current <see cref="INestedEqualityComparer{TGeneric, TParameter}"/>, or
    /// <see cref="NestedEqualityComparer{TGeneric, TParameter}.Default"/> if the current instance
    /// is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TGeneric">The type of value being compared.</typeparam>
    /// <typeparam name="TParameter">
    /// A nested parameter for type <typeparamref name="TGeneric"/> that needs to be taken into account for
    /// equality comparisons.
    /// </typeparam>
    /// <param name="comparer"></param>
    /// <returns>
    /// The current instance, or <see cref="NestedEqualityComparer{TGeneric, TParameter}.Default"/> if the current
    /// instance is <see langword="null"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static INestedEqualityComparer<TGeneric, TParameter> DefaultIfNull<TGeneric, TParameter>(
            this INestedEqualityComparer<TGeneric, TParameter>? comparer)
        => comparer ?? NestedEqualityComparer<TGeneric, TParameter>.Default;
}
