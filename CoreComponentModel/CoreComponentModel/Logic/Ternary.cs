using System;
using System.Collections.ObjectModel;
using Rem.Core.Attributes;

namespace Rem.Core.ComponentModel.Logic;

/// <summary>
/// Represents ternary logical values, either <see langword="true"/>, <see langword="false"/>
/// or an unknown.
/// </summary>
/// <remarks>
/// This struct uses a backing enum, which can be used to pass ternary values to attribute constructors that would not
/// allow either <see cref="Ternary"/> values or nullable <see cref="bool"/> values.
/// The backing enum is implicitly convertible to <see cref="Ternary"/>.  The conversion will map all unnamed enum
/// values to <see cref="Unknown"/>.
/// </remarks>
[CaseUnion<Ternary.Cases>]
public readonly record struct Ternary
{
    #region Constants
    /// <inheritdoc cref="Cases.False"/>
    public static readonly Ternary False = new(Cases.False);

    /// <inheritdoc cref="Cases.Unknown"/>
    public static readonly Ternary Unknown = new(Cases.Unknown);

    /// <inheritdoc cref="Cases.True"/>
    public static readonly Ternary True = new(Cases.True);

    /// <summary>
    /// A collection of all possible <see cref="Ternary"/> values.
    /// </summary>
    /// <remarks>
    /// The order of the elements of the array are chosen so that less surety is equated with a lower index.
    /// As a result, the elements of the collection are <see langword="false"/>, <see cref="Unknown"/> and
    /// <see langword="true"/>, in that order.
    /// </remarks>
    public static readonly ReadOnlyCollection<Ternary> All = new(new[] { False, Unknown, True });
    #endregion

    #region Properties
    /// <summary>
    /// Gets this instance as a <see cref="bool"/> value, treating <see cref="Unknown"/> <i>pessimistically</i>,
    /// collapsing it to <see langword="false"/>.
    /// </summary>
    public bool Pessimistic => _storedCase == StoredTrue;

    /// <summary>
    /// Gets this instance as a <see cref="bool"/> value, treating <see cref="Unknown"/> <i>optimistically</i>,
    /// collapsing it to <see langword="true"/>.
    /// </summary>
    public bool Optimistic => _storedCase != StoredFalse;

    /// <summary>
    /// Determines if this instance is <see langword="true"/>.
    /// </summary>
    public bool IsTrue => _storedCase == StoredTrue;

    /// <summary>
    /// Determines if this instance is <see langword="false"/>.
    /// </summary>
    public bool IsFalse => _storedCase == StoredFalse;

    /// <summary>
    /// Determines if this instance is <see cref="Unknown"/>.
    /// </summary>
    public bool IsUnknown => _storedCase == StoredUnknown;

    /// <inheritdoc cref="Case"/>
    [Obsolete("Will be removed in an upcoming version. Use `Case` instead.")]
    [NameableEnum] public Cases Value { get; }

    /// <inheritdoc/>
    [NameableEnum] public Cases Case => unchecked((Cases)((byte)_storedCase + Cases.Unknown));
    private readonly Cases _storedCase;
    #endregion

    /// <summary>
    /// Constructs this struct with the value passed in.
    /// </summary>
    /// <param name="Case"></param>
    private Ternary([NameableEnum] Cases Case)
    {
        _storedCase = unchecked((Cases)(Case - Cases.Unknown));
    }

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Ternary other) => _storedCase == other._storedCase;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _storedCase.GetHashCode();
    #endregion

    /// <summary>
    /// Gets a string representation of this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => IsBool(out var b) ? b.ToString() : "Unknown";

    #region Logic
    /// <summary>
    /// Computes the logical conjunction of the two values passed in.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Ternary operator &(Ternary left, Ternary right) => new(left.Case & right.Case);

    /// <summary>
    /// Computes the logical disjunction of the two values passed in.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Ternary operator |(Ternary left, Ternary right) => new(left.Case | right.Case);

    /// <summary>
    /// Computes the logical exclusive disjunction of the two values passed in.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Ternary operator ^(Ternary left, Ternary right)
        => left.IsBool(out var leftB) && right.IsBool(out var rightB)
            ? leftB ^ rightB
            : Unknown; // Any unknown value creates an unknown
    #endregion

    #region Conversions
    /// <summary>
    /// Implicitly converts a <see cref="Ternary"/> to a <see cref="Cases"/>.
    /// </summary>
    /// <param name="t"></param>
    public static implicit operator Cases(Ternary t) => t.Case;

    /// <summary>
    /// Implicitly converts a <see cref="Cases"/> to a <see cref="Ternary"/>.
    /// </summary>
    /// <remarks>
    /// This method will return <see cref="Unknown"/> for any unnamed <see cref="Cases"/> instance.
    /// </remarks>
    /// <param name="value"></param>
    public static implicit operator Ternary(Cases value) => value switch
    {
        Cases.False => False,
        Cases.True => True,
        _ => Unknown,
    };

    /// <summary>
    /// Implicitly converts a nullable <see cref="bool"/> to a <see cref="Ternary"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Ternary(bool? value) => value is bool b ? b : Unknown;

    /// <summary>
    /// Implicitly converts a <see cref="Ternary"/> to a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="t"></param>
    public static implicit operator bool?(Ternary t) => t._storedCase == StoredUnknown
                                                            ? null
                                                            : t._storedCase != StoredFalse;

    /// <summary>
    /// Explicitly converts a <see cref="Ternary"/> to a <see cref="bool"/>.
    /// </summary>
    /// <param name="t"></param>
    /// <exception cref="InvalidCastException"><paramref name="t"/> was <see cref="Unknown"/>.</exception>
    public static explicit operator bool(Ternary t)
        => t._storedCase == StoredUnknown
            ? throw new InvalidCastException($"Cannot convert \"{nameof(Unknown)}\" to a known boolean.")
            : t._storedCase != StoredFalse;

    /// <summary>
    /// Implicitly converts a <see cref="bool"/> to a <see cref="Ternary"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Ternary(bool value) => value ? True : False;

    /// <summary>
    /// Determines if this instance represents a known boolean value (i.e. is not <see cref="Unknown"/>), setting the
    /// value in a <see langword="out"/> parameter if so.
    /// </summary>
    /// <remarks>
    /// This method is equivalent to <see cref="IsBool(out bool)"/>.
    /// </remarks>
    /// <param name="knownCase"></param>
    /// <returns></returns>
    public bool IsKnown(out bool knownCase) => IsUnknown
                                                    ? Try.Failure(out knownCase)
                                                    : Try.Success(out knownCase, _storedCase == StoredTrue);

    /// <summary>
    /// Determines if this instance represents a boolean value, setting the value in an <see langword="out"/>
    /// parameter if so.
    /// </summary>
    /// <remarks>
    /// This method is equivalent to <see cref="IsKnown(out bool)"/>.
    /// </remarks>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsBool(out bool b) => IsUnknown ? Try.Failure(out b) : Try.Success(out b, _storedCase == StoredTrue);

    /// <summary>
    /// Determines if this instance represents a known boolean value (i.e. is not <see cref="Unknown"/>).
    /// </summary>
    /// <remarks>
    /// This method is equivalent to <see cref="IsBool()"/>.
    /// </remarks>
    /// <returns></returns>
    public bool IsKnown() => !IsUnknown;

    /// <summary>
    /// Determines if this instance represents a boolean value.
    /// </summary>
    /// <remarks>
    /// This method is equivalent to <see cref="IsKnown()"/>.
    /// </remarks>
    /// <returns></returns>
    public bool IsBool() => !IsUnknown;
    #endregion

    // Stored values of the cases - need "Unknown" to be the default to match "bool?"
    private const Cases StoredFalse = (Cases)unchecked(Cases.False - Cases.Unknown);
    private const Cases StoredUnknown = 0;
    private const Cases StoredTrue = (Cases)unchecked(Cases.True - Cases.Unknown);

    /// <summary>
    /// Represents all values of the <see cref="Ternary"/> type as <see langword="enum"/> values.
    /// </summary>
    public enum Cases : byte
    {
        /// <summary>
        /// Represents <see langword="false"/>.
        /// </summary>
        False = 0,

        /// <summary>
        /// Represents an unknown.
        /// </summary>
        Unknown = 1,

        /// <summary>
        /// Represents <see langword="true"/>.
        /// </summary>
        True = 3,
    }
}
