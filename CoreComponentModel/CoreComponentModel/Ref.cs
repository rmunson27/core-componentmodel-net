using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Represents a reference to a value of any type.
/// </summary>
/// <remarks>
/// This class may be useful when handling situations in which the <see langword="ref"/> keyword is not permitted,
/// such as passing parameters to <see langword="async"/> methods.
/// </remarks>
/// <typeparam name="T">The type of value wrapped in the reference.</typeparam>
public sealed class Ref<T> : IReadOnlyRef<T>, IWriteOnlyRef<T>
{
    /// <summary>
    /// Gets the value wrapped in this reference.
    /// </summary>
    [AllowNull, MaybeNull] public T Value
    {
        get => _value;
        set
        {
            PropertyChanging?.Invoke(this, new(nameof(Value)));
            _value = value;
            PropertyChanged?.Invoke(this, new(nameof(Value)));
        }
    }
    [AllowNull, MaybeNull] private T _value;

    /// <inheritdoc/>
    public event PropertyChangingEventHandler? PropertyChanging;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Constructs a new instance of the <see cref="Ref{T}"/> class wrapping the value passed in.
    /// </summary>
    /// <param name="Value"></param>
    public Ref([AllowNull] T Value = default)
    {
        _value = Value;
    }
}

/// <summary>
/// An interface for types that act as a writeonly reference to a single value.
/// </summary>
/// <remarks>
/// This allows <typeparamref name="T"/> to be treated contravariantly in relevant situations.
/// </remarks>
/// <typeparam name="T">The type of value wrapped in the reference.</typeparam>
public interface IWriteOnlyRef<in T> : INotifyPropertyChanging, INotifyPropertyChanged
{
    /// <summary>
    /// Sets the value wrapped in this reference.
    /// </summary>
    [AllowNull] public T Value { set; }
}

/// <summary>
/// An interface for types that act as a readonly reference to a single value.
/// </summary>
/// <remarks>
/// This allows <typeparamref name="T"/> to be treated covariantly in relevant situations.
/// </remarks>
/// <typeparam name="T">The type of value wrapped in the reference.</typeparam>
public interface IReadOnlyRef<out T>
{
    /// <summary>
    /// Gets the value wrapped in this reference.
    /// </summary>
    [MaybeNull] public T Value { get; }
}
