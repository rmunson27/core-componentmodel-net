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
}
