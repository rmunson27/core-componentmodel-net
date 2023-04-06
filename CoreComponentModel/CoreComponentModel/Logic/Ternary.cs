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
public readonly record struct Ternary
{
    #region Constants
    /// <inheritdoc cref="Values.False"/>
    public static readonly Ternary False = new(Values.False);

    /// <inheritdoc cref="Values.Unknown"/>
    public static readonly Ternary Unknown = new(Values.Unknown);

    /// <inheritdoc cref="Values.True"/>
    public static readonly Ternary True = new(Values.True);

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
    public bool Pessimistic => Value == Values.True;

    /// <summary>
    /// Gets this instance as a <see cref="bool"/> value, treating <see cref="Unknown"/> <i>optimistically</i>,
    /// collapsing it to <see langword="true"/>.
    /// </summary>
    public bool Optimistic => Value != Values.False;

    /// <summary>
    /// Determines if this instance is <see langword="true"/>.
    /// </summary>
    public bool IsTrue => Value == Values.True;

    /// <summary>
    /// Determines if this instance is <see langword="false"/>.
    /// </summary>
    public bool IsFalse => Value == Values.False;

    /// <summary>
    /// Determines if this instance is <see cref="Unknown"/>.
    /// </summary>
    public bool IsUnknown => Value == Values.Unknown;

    /// <summary>
    /// Gets an <see langword="enum"/> value uniquely representing this instance.
    /// </summary>
    [NameableEnum] public Values Value { get; }
    #endregion

    /// <summary>
    /// Constructs this struct with the value passed in.
    /// </summary>
    /// <param name="Value"></param>
    private Ternary([NameableEnum] Values Value)
    {
        this.Value = Value;
    }

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Ternary other) => Value == other.Value;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => (int)Value;
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
    public static Ternary operator &(Ternary left, Ternary right) => new(left.Value & right.Value);

    /// <summary>
    /// Computes the logical disjunction of the two values passed in.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Ternary operator |(Ternary left, Ternary right) => new(left.Value | right.Value);

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
    /// Implicitly converts a <see cref="Ternary"/> to a <see cref="Values"/>.
    /// </summary>
    /// <param name="t"></param>
    public static implicit operator Values(Ternary t) => t.Value;

    /// <summary>
    /// Implicitly converts a <see cref="Values"/> to a <see cref="Ternary"/>.
    /// </summary>
    /// <remarks>
    /// This method will return <see cref="Unknown"/> for any unnamed <see cref="Values"/> instance.
    /// </remarks>
    /// <param name="value"></param>
    public static implicit operator Ternary(Values value) => value switch
    {
        Values.False => False,
        Values.True => True,
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
    public static implicit operator bool?(Ternary t) => t.Value == Unknown ? null : t.Value != Values.False;

    /// <summary>
    /// Implicitly converts a <see cref="bool"/> to a <see cref="Ternary"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Ternary(bool value) => value ? True : False;

    /// <summary>
    /// Determines if this instance represents a boolean value, setting the value in an <see langword="out"/>
    /// parameter if so.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsBool(out bool b) => IsUnknown ? Try.Failure(out b) : Try.Success(out b, Value != Values.False);

    /// <summary>
    /// Determines if this instance represents a boolean value.
    /// </summary>
    /// <returns></returns>
    public bool IsBool() => !IsUnknown;
    #endregion

    /// <summary>
    /// Represents all values of the <see cref="Ternary"/> type as <see langword="enum"/> values.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents <see langword="false"/>.
        /// </summary>
        False = 0,

        /// <summary>
        /// Represents an unknown.
        /// </summary>
        Unknown = 135, // Closest possible to 128 with half of bits set

        /// <summary>
        /// Represents <see langword="true"/>.
        /// </summary>
        True = 255,
    }
}
