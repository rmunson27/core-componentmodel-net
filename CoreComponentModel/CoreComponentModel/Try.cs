using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
///     Static helper methods for standard "Try" methods.
/// </summary>
/// 
/// <remarks>
///     The methods in this class can be used to write a standard "Try" method as a single expression.
///     <para/>
///     
///     For example, consider the following (rather contrived) example:
///     <code>
///         public bool TryGetPositiveString(int i, [MaybeNullWhen(false)] out string s)
///             => i > 0 ? Try.Success(out s, i.ToString()) : Try.Failure(out s);
///     </code>
/// 
///     as opposed to the more verbose standard equivalent:
///     <code>
///         public bool TryGetPositiveString(int i, [MaybeNullWhen(false)] out string s)
///         {
///             if (i > 0)
///             {
///                 s = i.ToString();
///                 return true;
///             }
///             else
///             {
///                 s = default;
///                 return false;
///             }
///         }
///     </code>
/// </remarks>
public static class Try
{
    #region Success Setters
    /// <summary>
    /// Sets <paramref name="successOut"/> to <paramref name="successValue"/> and <paramref name="failureOut"/> to
    /// <see langword="default"/> and returns <see langword="true"/>.
    /// </summary>
    /// <typeparam name="TSuccess">The type of <paramref name="successOut"/>.</typeparam>
    /// <typeparam name="TFailure">The type of <paramref name="failureOut"/>.</typeparam>
    /// <param name="successOut">
    /// The <see langword="out"/> parameter to set to <paramref name="successValue"/>.
    /// </param>
    /// <param name="successValue">The value to set <paramref name="successOut"/> to.</param>
    /// <param name="failureOut">
    /// The <see langword="out"/> parameter to set to <see langword="default"/>.
    /// </param>
    /// <returns><see langword="true"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Success<TSuccess, TFailure>(out TSuccess successOut, TSuccess successValue,
                                                   [MaybeNull, MaybeDefault] out TFailure failureOut)
        => Success(out successOut, successValue, out failureOut, failureValue: default);

    /// <summary>
    /// Sets <paramref name="successOut"/> to <paramref name="successValue"/> and <paramref name="failureOut"/> to
    /// <paramref name="failureValue"/> and returns <see langword="true"/>.
    /// </summary>
    /// <typeparam name="TSuccess">The type of <paramref name="successOut"/>.</typeparam>
    /// <typeparam name="TFailure">The type of <paramref name="failureOut"/>.</typeparam>
    /// <param name="successOut">
    /// The <see langword="out"/> parameter to set to <paramref name="successValue"/>.
    /// </param>
    /// <param name="successValue">The value to set <paramref name="successOut"/> to.</param>
    /// <param name="failureOut">
    /// The <see langword="out"/> parameter to set to <paramref name="failureValue"/>.
    /// </param>
    /// <param name="failureValue">The value to set <paramref name="failureOut"/> to.</param>
    /// <returns><see langword="true"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Success<TSuccess, TFailure>(out TSuccess successOut, TSuccess successValue,
                                                   [MaybeNull, MaybeDefault] out TFailure failureOut,
                                                   [AllowNull, AllowDefault] TFailure failureValue)
    {
        successOut = successValue;
        failureOut = failureValue;
        return true;
    }

    /// <summary>
    /// Sets <paramref name="outParameter"/> to <paramref name="value"/> and returns <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="outParameter"/>.</typeparam>
    /// <param name="value">The value to set <paramref name="outParameter"/> to.</param>
    /// <param name="outParameter">The <see langword="out"/> parameter to set to <paramref name="value"/>.</param>
    /// <returns><see langword="true"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Success<T>(out T outParameter, T value)
    {
        outParameter = value;
        return true;
    }
    #endregion

    #region Failure Setters
    /// <summary>
    /// Sets <paramref name="successOut"/> to <see langword="default"/> and <paramref name="failureOut"/> to
    /// <paramref name="failureValue"/> and returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="TSuccess">The type of <paramref name="successOut"/>.</typeparam>
    /// <typeparam name="TFailure">The type of <paramref name="failureOut"/>.</typeparam>
    /// <param name="successOut">
    /// The <see langword="out"/> parameter to set to <see langword="default"/>.
    /// </param>
    /// <param name="failureOut">
    /// The <see langword="out"/> parameter to set to <paramref name="failureValue"/>.
    /// </param>
    /// <param name="failureValue">The value to set <paramref name="failureOut"/> to.</param>
    /// <returns><see langword="false"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Failure<TSuccess, TFailure>([MaybeNull, MaybeDefault] out TSuccess successOut,
                                                   out TFailure failureOut, TFailure failureValue)
        => Failure(out successOut, successValue: default, out failureOut, failureValue);

    /// <summary>
    /// Sets <paramref name="successOut"/> to <paramref name="successValue"/> and <paramref name="failureOut"/> to
    /// <paramref name="failureValue"/> and returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="TSuccess">The type of <paramref name="successOut"/>.</typeparam>
    /// <typeparam name="TFailure">The type of <paramref name="failureOut"/>.</typeparam>
    /// <param name="successOut">
    /// The <see langword="out"/> parameter to set to <paramref name="successValue"/>.
    /// </param>
    /// <param name="successValue">The value to set <paramref name="successOut"/> to.</param>
    /// <param name="failureOut">
    /// The <see langword="out"/> parameter to set to <paramref name="failureValue"/>.
    /// </param>
    /// <param name="failureValue">The value to set <paramref name="failureOut"/> to.</param>
    /// <returns><see langword="false"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Failure<TSuccess, TFailure>([MaybeNull, MaybeDefault] out TSuccess successOut,
                                                   [AllowNull, AllowDefault] TSuccess successValue,
                                                   out TFailure failureOut, TFailure failureValue)
    {
        successOut = successValue;
        failureOut = failureValue;
        return false;
    }

    /// <summary>
    /// Sets <paramref name="outParameter"/> to <see langword="default"/> and returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="outParameter"/>.</typeparam>
    /// <param name="outParameter">The <see langword="out"/> parameter to set to <see langword="default"/>.</param>
    /// <returns><see langword="false"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Failure<T>([MaybeNull, MaybeDefault] out T outParameter) => Failure(out outParameter, default);

    /// <summary>
    /// Sets <paramref name="outParameter"/> to <paramref name="value"/> and returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="outParameter"/>.</typeparam>
    /// <param name="outParameter">The <see langword="out"/> parameter to set to <paramref name="value"/>.</param>
    /// <param name="value">The value to set <paramref name="outParameter"/> to.</param>
    /// <returns><see langword="false"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Failure<T>([MaybeNull, MaybeDefault] out T outParameter, [AllowNull, AllowDefault] T value)
    {
        outParameter = value;
        return false;
    }
    #endregion
}

/// <inheritdoc cref="TryFunc{TParameter, TSuccess}"/>
public delegate bool TryFunc<TSuccess>([MaybeNullWhen(false), MaybeDefaultWhen(false)] out TSuccess success);

/// <summary>
/// A delegate representing a function that tries some failable operation, setting the
/// <paramref name="success"/> <see langword="out"/> parameter to the result on success, and returning whether or not
/// the operation succeeded.
/// </summary>
/// <inheritdoc cref="TryFailFunc{TParameter, TSuccess, TFailure}"/>
public delegate bool TryFunc<in TParameter, TSuccess>(
    TParameter parameter, [MaybeNullWhen(false), MaybeDefaultWhen(false)] out TSuccess success);

/// <inheritdoc cref="TryFailFunc{TParameter, TSuccess, TFailure}"/>
public delegate bool TryFailFunc<TSuccess, TFailure>(
    [MaybeNullWhen(false), MaybeDefaultWhen(false)] out TSuccess success,
    [MaybeNullWhen(true), MaybeDefaultWhen(true)] out TFailure failure);

/// <summary>
/// A delegate representing a function that tries some failable operation, setting the
/// <paramref name="success"/> <see langword="out"/> parameter to the result on success and
/// <typeparamref name="TFailure"/> to an error value on failure, and returning whether or not the
/// operation succeeded.
/// </summary>
/// <typeparam name="TParameter">The type of a parameter to the function.</typeparam>
/// <typeparam name="TSuccess">
/// The type of the <see langword="out"/> parameter that will be set on success.
/// <para/>
/// If this type is a defaultable struct or a nullable reference type, the <paramref name="success"/> parameter may be
/// set to <see langword="default"/> or <see langword="null"/> when the operation fails.
/// </typeparam>
/// <typeparam name="TFailure">
/// The type of the <see langword="out"/> parameter that will be set on failure.
/// <para/>
/// If this type is a defaultable struct or a nullable reference type, the <paramref name="failure"/> parameter may be
/// set to <see langword="default"/> or <see langword="null"/> when the operation succeeds.
/// </typeparam>
/// <param name="parameter">A parameter to use in the operation.</param>
/// <param name="success">The <see langword="out"/> parameter to set on success.</param>
/// <param name="failure">The <see langword="out"/> parameter to set on failure.</param>
/// <returns>Whether or not the operation succeeded.</returns>
public delegate bool TryFailFunc<in TParameter, TSuccess, TFailure>(
    TParameter parameter,
    [MaybeNullWhen(false), MaybeDefaultWhen(false)] out TSuccess success,
    [MaybeNullWhen(true), MaybeDefaultWhen(true)] out TFailure failure);
