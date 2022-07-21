using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An interface for types that can be partially compared to instances of the same type.
/// </summary>
/// <typeparam name="T">The type implementing the interface.</typeparam>
/// <seealso cref="IComparable{T}"/>
public interface IPartialComparable<in T>
{
    /// <summary>
    /// Compares the current instance with another, returning an integer indicating the ordering of the two instances,
    /// or <see langword="null"/> if the two instances do not compare.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <seealso cref="IComparable{T}.CompareTo(T)"/>
    public int? CompareTo(T other);
}

/// <summary>
/// An interface for types that can represent partial comparison operations on a given type.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <seealso cref="IComparer{T}"/>
public interface IPartialComparer<in T>
{
    /// <summary>
    /// Compares two <typeparamref name="T"/> instances, returning an integer indicating the ordering of the instances,
    /// or <see langword="null"/> if the instances do not compare.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <seealso cref="IComparer{T}.Compare(T, T)"/>
    public int? Compare(T x, T y);
}
