using System;
using System.ComponentModel;
using Rem.Core.Attributes;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Static functionality for implementors of the <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/> interface.
/// </summary>
/// <remarks>
/// Some of this class consists of helper functionality that may help dependent projects implement the interface.
/// </remarks>
public static class EnumeratedCaseUnion
{
    /// <summary>
    /// Creates an appropriate exception that can be thrown to indicate that an <see langword="enum"/> value
    /// representing a case of a type implementing <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/> is not defined.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/>.</typeparam>
    /// <returns>
    /// An <see cref="InvalidEnumArgumentException"/> with an appropriate message describing the exceptional case.
    /// </returns>
    public static InvalidEnumArgumentException InvalidCase<TSelf>()
        => new($"Invalid {typeof(TSelf)} enumeration case.");
}

/// <summary>
/// Indicates that a nested <see langword="enum"/> definition within a type provides the case-backing enumeration
/// for the type.
/// </summary>
/// <seealso cref="IEnumeratedCaseUnion{TSelf, TCases}"/>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
public sealed class UnionCaseEnumeratorAttribute : Attribute { }

/// <summary>
/// An interface for types with different cases that are uniquely defined by the named constants of a given
/// <see langword="enum"/> type.
/// </summary>
/// <remarks>
/// Typically <typeparamref name="TCases"/> should be defined nested within <see cref="TSelf"/> and used to uniquely
/// identify the cases of type <typeparamref name="TSelf"/>.
/// <para/>
/// It should never be possible for <see cref="Case"/> to be an unnamed <typeparamref name="TCases"/> value.
/// </remarks>
/// <typeparam name="TSelf">The type implementing <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/>.</typeparam>
/// <typeparam name="TCases">The <see langword="enum"/> type enumerating the cases.</typeparam>
public interface IEnumeratedCaseUnion<TSelf, TCases> where TSelf : IEnumeratedCaseUnion<TSelf, TCases>
                                                     where TCases : struct, Enum
{
    /// <summary>
    /// Gets a named value of type <typeparamref name="TCases"/> uniquely representing this instance.
    /// </summary>
    [NameableEnum] public TCases Case { get; }
}
