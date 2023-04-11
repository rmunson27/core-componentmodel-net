using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Helper methods for determining whether values are invalid defaults.
/// </summary>
/// <remarks>
/// For the purposes of this class, the <see cref="ImmutableArray{T}"/> type will be treated as a defaultable struct
/// (although it does not implement <see cref="IDefaultableStruct"/>), and its default value will be treated
/// as invalid.
/// </remarks>
public static class Defaultable
{
    /// <param name="value">A reference to the value of the parameter to check for invalid defaults.</param>
    /// <returns>A readonly reference to the value passed in.</returns>
    /// <inheritdoc cref="ThrowIfArgumentDefault"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T ThrowIfArgumentRefsDefault<T>([NotNull, NotDefault] ref T? value, string paramName)
        => ref Defaultable<T>.DefaultInfo.ThrowIfArgumentRefsDefault(ref value, paramName);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the supplied <typeparamref name="T"/> represents an invalid
    /// default value of a parameter with the specified name.
    /// </summary>
    /// <param name="paramName">The name of the parameter <paramref name="value"/> represents.</param>
    /// <inheritdoc cref="ThrowIfDefault"/>
    /// <exception cref="ArgumentNullException">
    /// <typeparamref name="T"/> is a reference or nullable value type and <paramref name="value"/>
    /// was <see langword="null"/>.
    /// <exception cref="StructArgumentDefaultException">
    /// <typeparamref name="T"/> is <see cref="ImmutableArray{T}"/> or a value type implementing
    /// <see cref="IDefaultableStruct"/> and <paramref name="value"/><c>.IsDefault</c> returns <see langword="true"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T ThrowIfArgumentDefault<T>([NotNull, NotDefault] in T? value, string paramName)
        => ref Defaultable<T>.DefaultInfo.ThrowIfArgumentDefault(value, paramName);

    /// <summary>
    /// Throws an exception if the supplied <typeparamref name="T"/> is an invalid default value.
    /// </summary>
    /// <param name="value">A readonly reference to the value to check for invalid defaults.</param>
    /// <returns>A readonly reference to the value passed in.</returns>
    /// <inheritdoc cref="ThrowIfRefsDefault"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T ThrowIfDefault<T>([NotNull, NotDefault] in T? value)
        => ref Defaultable<T>.DefaultInfo.ThrowIfDefault(in value);

    /// <summary>
    /// Throws an exception if the supplied <typeparamref name="T"/> is an invalid default value.
    /// </summary>
    /// <returns>A reference to the value passed in.</returns>
    /// <inheritdoc cref="RefsDefault{T}(ref T)"/>
    /// <exception cref="NullReferenceException">
    /// <typeparamref name="T"/> is a reference or nullable value type and <paramref name="value"/>
    /// was <see langword="null"/>.
    /// </exception>
    /// <exception cref="StructDefaultException">
    /// <typeparamref name="T"/> is <see cref="ImmutableArray{T}"/> or a value type implementing
    /// <see cref="IDefaultableStruct"/> and <paramref name="value"/><c>.IsDefault</c> returns <see langword="true"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T ThrowIfRefsDefault<T>([NotNull, NotDefault] ref T? value)
        => ref Defaultable<T>.DefaultInfo.ThrowIfRefsDefault(ref value);

    /// <param name="value">A readonly reference to the value to check for invalid defaults.</param>
    /// <inheritdoc cref="RefsDefault"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDefault<T>([NotNullWhen(false), NotDefaultWhen(false)] in T? value)
        => Defaultable<T>.DefaultInfo.IsDefault(in value);

    /// <summary>
    /// Determines if the supplied <typeparamref name="T"/> is an invalid default value.
    /// </summary>
    /// <typeparam name="T">The type of value to check for invalid defaults.</typeparam>
    /// <param name="value">A reference to the value to check for invalid defaults.</param>
    /// <returns>
    /// <see langword="true"/> if one of the following conditions is met:
    /// <para/>
    /// <typeparamref name="T"/> is a reference or nullable value type and <paramref name="value"/>
    /// is <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// <typeparamref name="T"/> is <see cref="ImmutableArray{T}"/> or a value type implementing
    /// <see cref="IDefaultableStruct"/> and <paramref name="value"/><c>.IsDefault</c> returns <see langword="true"/>,
    /// <para/>
    /// otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool RefsDefault<T>([NotNullWhen(false), NotDefaultWhen(false)] ref T? value)
        => Defaultable<T>.DefaultInfo.RefsDefault(ref value);
}

file static class Defaultable<T>
{
    public static readonly DefaultInfo<T> DefaultInfo;

    static Defaultable()
    {
        if (typeof(T).IsValueType)
        {
            if (typeof(IDefaultableStruct).IsAssignableFrom(typeof(T)))
            {
                DefaultInfo = (DefaultInfo<T>)typeof(DefaultableInfo<>).MakeGenericType(typeof(T))
                                                                       .GetConstructor(new Type[] { })
                                                                       .Invoke(new object[] { });
            }
            else if (typeof(T).IsGenericType)
            {
                var genericTypeDefinition = typeof(T).GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    var param = typeof(T).GetGenericArguments()[0];
#pragma warning disable CS8619 // Will always be nullable info
                    DefaultInfo = (DefaultInfo<T?>)typeof(NullableValInfo<>).MakeGenericType(param)
                                                                            .GetConstructor(new Type[] { })
                                                                            .Invoke(new object[] { });
#pragma warning restore CS8619
                }
                else if (genericTypeDefinition == typeof(ImmutableArray<>))
                {
                    var param = typeof(T).GetGenericArguments()[0];
                    DefaultInfo = (DefaultInfo<T>)typeof(ImmutableArrayInfo<>).MakeGenericType(param)
                                                                              .GetConstructor(new Type[] { })
                                                                              .Invoke(new object[] { });
                }
                else DefaultInfo = new NoDefaultDefaultableInfo<T>();
            }
            else DefaultInfo = new NoDefaultDefaultableInfo<T>();
        }
        else
        {
            DefaultInfo = (DefaultInfo<T>)typeof(NullableRefInfo<>).MakeGenericType(typeof(T))
                                                                   .GetConstructor(new Type[] { })
                                                                   .Invoke(new object[] { });
        }
    }
}

file sealed class NoDefaultDefaultableInfo<T> : DefaultInfo<T>
{
    // Never thrown
    public override Exception Exception() => throw new NotImplementedException();

    // Never thrown
    public override ArgumentException ArgumentException(string paramName) => throw new NotImplementedException();

#pragma warning disable CS8777 // This will never be null
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool IsDefault([NotNull, NotDefault] in T? value) => false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool RefsDefault([NotNull, NotDefault] ref T? value) => false;
#pragma warning restore CS8777
}

file sealed class ImmutableArrayInfo<T> : DefaultableInfoBase<ImmutableArray<T>>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool IsDefault([NotDefaultWhen(false)] in ImmutableArray<T> value) => value.IsDefault;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool RefsDefault([NotDefaultWhen(false)] ref ImmutableArray<T> value) => value.IsDefault;
}

file sealed class DefaultableInfo<T> : DefaultableInfoBase<T> where T : IDefaultableStruct
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool IsDefault([NotNullWhen(false), NotDefaultWhen(false)] in T? value) => value!.IsDefault;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool RefsDefault([NotNullWhen(false), NotDefaultWhen(false)] ref T? value) => value!.IsDefault;
}

file abstract class DefaultableInfoBase<T> : DefaultInfo<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sealed override Exception Exception() => new StructDefaultException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sealed override ArgumentException ArgumentException(string paramName)
        => new StructArgumentDefaultException(paramName);
}

file sealed class NullableRefInfo<T> : NullableInfo<T?> where T : class
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool IsDefault([NotNullWhen(false), NotDefaultWhen(false)] in T? value) => value is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool RefsDefault([NotNullWhen(false), NotDefaultWhen(false)] ref T? value) => value is null;
}

file sealed class NullableValInfo<T> : NullableInfo<T?> where T : struct
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool IsDefault([NotNullWhen(false), NotDefaultWhen(false)] in T? value) => value is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool RefsDefault([NotNullWhen(false), NotDefaultWhen(false)] ref T? value) => value is null;
}

file abstract class NullableInfo<T> : DefaultInfo<T?>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sealed override Exception Exception() => new NullReferenceException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sealed override ArgumentException ArgumentException(string paramName)
        => new ArgumentNullException(paramName);
}

file abstract class DefaultInfo<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T ThrowIfRefsDefault([NotNull, NotDefault] ref T? value)
    {
        if (IsDefault(in value)) throw Exception();
        else return ref value!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref readonly T ThrowIfDefault([NotNull, NotDefault] in T? value)
    {
        if (IsDefault(in value)) throw Exception();
        else return ref value!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T ThrowIfArgumentRefsDefault([NotNull, NotDefault] ref T? value, string paramName)
    {
        if (RefsDefault(ref value)) throw ArgumentException(paramName);
        else return ref value!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref readonly T ThrowIfArgumentDefault([NotNull, NotDefault] in T? value, string paramName)
    {
        if (IsDefault(in value)) throw ArgumentException(paramName);
        else return ref value!;
    }

    public abstract bool IsDefault([NotNullWhen(false), NotDefaultWhen(false)] in T? value);
    public abstract bool RefsDefault([NotNullWhen(false), NotDefaultWhen(false)] ref T? value);
    public abstract Exception Exception();
    public abstract ArgumentException ArgumentException(string paramName);
}
