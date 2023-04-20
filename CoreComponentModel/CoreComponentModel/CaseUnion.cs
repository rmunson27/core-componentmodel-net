using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rem.Core.Attributes;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Static functionality for implementors of the <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/> interface.
/// </summary>
/// <remarks>
/// Some of this class consists of helper functionality that may help dependent projects implement the interface.
/// </remarks>
public static class CaseUnion
{
    /// <summary>
    /// Creates an appropriate exception that can be thrown to indicate that an <see langword="enum"/> value
    /// representing a case of a type implementing <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/> is not defined.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements <see cref="IEnumeratedCaseUnion{TSelf, TCases}"/>.</typeparam>
    /// <returns>
    /// An <see cref="InvalidEnumArgumentException"/> with an appropriate message describing the exceptional case.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidEnumArgumentException InvalidCase<TSelf>()
        => new($"Invalid {typeof(TSelf)} enumeration case.");

    /// <summary>
    /// Indicates that the targeted <see langword="enum"/> definition nested within a type provides the case-backing
    /// enumeration for the type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum,
                    AllowMultiple = false,
                    Inherited = false)]
    public sealed class CasesAttribute : Attribute { }

    /// <summary>
    /// Marks the constructor of a type decorated with <see cref="CaseUnionAttribute"/> that should be used to
    /// construct instances of the type.
    /// </summary>
    /// <remarks>
    /// Exactly one <see cref="ConstructorAttribute"/> should apply to a case union type (and no more).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct,
                    AllowMultiple = false,
                    Inherited = false)]
    public sealed class ConstructorAttribute : Attribute { }

    /// <summary>
    /// Decorates the converter from a type decorated with <see cref="CaseUnionAttribute"/> to the underlying
    /// <see langword="enum"/> case.
    /// </summary>
    public sealed class ToCaseCastAttribute : CaseCastAttribute { }

    /// <summary>
    /// Decorates the converter from a type decorated with <see cref="CaseUnionAttribute"/> to the underlying
    /// <see langword="enum"/> case.
    /// </summary>
    public sealed class FromCaseCastAttribute : CaseCastAttribute { }

    /// <summary>
    /// Decorates a conversion operator between a type decorated with <see cref="CaseUnionAttribute"/> and its
    /// underlying <see langword="enum"/> case type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,
                    AllowMultiple = false,
                    Inherited = false)]
    public abstract class CaseCastAttribute : Attribute
    {
        private protected CaseCastAttribute() { }
    }

    /// <summary>
    /// Decorates the instance property of a type decorated with <see cref="CaseUnionAttribute"/> that stores the case
    /// of the instance.
    /// </summary>
    /// <remarks>
    /// This property should occur exactly once within a type decorated with <see cref="CaseUnionAttribute"/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property,
                    AllowMultiple = false,
                    Inherited = true)]
    public sealed class CaseAttribute : Attribute { }
}

/// <summary>
/// Indicates that the targeted definition has an <see langword="enum"/> type nested within it that enumerates all
/// the cases of the type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
                AllowMultiple = false,
                Inherited = true)]
public sealed class CaseUnionAttribute : Attribute { }
