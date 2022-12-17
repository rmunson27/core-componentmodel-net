using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Extension methods for the <see cref="Exception"/> class.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Throws the current <see cref="Exception"/>, typing the method call return as an expression of
    /// type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// This method can be used to chain potentially <see langword="null"/> exception references into simple
    /// short-circuiting <see langword="null"/>-check expressions, for example:
    /// <code>
    /// Exception? e;
    /// int i = e?.Throw&lt;int&gt;() ?? 4;
    /// </code>
    /// which can be compared with the (potentially more verbose)
    /// <code>
    /// Exception? e;
    /// int i = e is not null ? throw e : 4;
    /// </code>
    /// <para/>
    /// This method can also be used as a simple analog for <see langword="throw"/> expressions in a C# language
    /// version that does not support them.
    /// </remarks>
    /// <typeparam name="T">The return type the method call should be typed as.</typeparam>
    /// <param name="e">The exception to throw.</param>
    /// <returns>This method never returns.</returns>
    [DoesNotReturn]
    public static T Throw<T>(this Exception e) => throw e;

    /// <summary>
    /// Throws the current <see cref="Exception"/> if it is not a <see langword="null"/> reference.
    /// </summary>
    /// <param name="e">The exception to throw.</param>
    public static void ThrowIfNotNull(this Exception? e)
    {
        if (e is not null) throw e;
    }
}
