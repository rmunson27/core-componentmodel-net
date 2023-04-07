using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

#region Full
/// <summary>
/// An interface for types that represent mutable versions of an immutable type <typeparamref name="TImmutable"/>,
/// and that can be queried for a <typeparamref name="TImmutable"/> instance representing the current state.
/// </summary>
/// <typeparam name="TImmutable"></typeparam>
public interface IMutableModelGetState<out TImmutable>
{
    /// <summary>
    /// Gets a <typeparamref name="TImmutable"/> instance representing the current state of this object.
    /// </summary>
    public TImmutable CurrentState { get; }
}

/// <summary>
/// An interface for types that represent mutable versions of an immutable type <typeparamref name="TImmutable"/>,
/// and that can have their state set to describe a given <typeparamref name="TImmutable"/> instance.
/// </summary>
/// <typeparam name="TImmutable"></typeparam>
public interface IMutableModelSetState<in TImmutable>
{
    /// <summary>
    /// Sets the current state of this object to describe the <typeparamref name="TImmutable"/> passed in.
    /// </summary>
    /// <param name="state"></param>
    public void SetState(TImmutable state);
}

/// <summary>
/// An interface for types that represent mutable versions of an immutable type <typeparamref name="TImmutable"/>,
/// can be queried for a <typeparamref name="TImmutable"/> instance representing the current state,
/// and can have their state set to describe a given <typeparamref name="TImmutable"/>.
/// </summary>
/// <typeparam name="TImmutable"></typeparam>
public interface IMutableModel<TImmutable>
    : IMutableModelGetState<TImmutable>, IMutableModelSetState<TImmutable>
{
    //
}
#endregion

#region Partial
/// <summary>
/// Extension methods for the <see cref="IMutablePartialModelGetState{TImmutable}"/> interface.
/// </summary>
public static class MutablePartialModelGetStateExtensions
{
    /// <summary>
    /// Throws an <see cref="InvalidMutableModelStateException"/> if the current
    /// <see cref="IMutablePartialModelGetState{TImmutable}"/> instance is not in a valid state.
    /// </summary>
    /// <typeparam name="TImmutable"></typeparam>
    /// <param name="model"></param>
    /// <exception cref="InvalidMutableModelStateException"></exception>
    public static void ThrowIfNotInValidState<TImmutable>(this IMutablePartialModelGetState<TImmutable> model)
    {
        if (!model.IsInValidState)
        {
            throw new InvalidMutableModelStateException(
                $"{model.GetType()} instance was in an invalid state (its state did not represent an instance of"
                    + $" type {typeof(TImmutable)}).");
        }
    }

    /// <summary>
    /// Gets the state of the current mutable model instance in an <see langword="out"/> parameter.
    /// </summary>
    /// <typeparam name="TImmutable"></typeparam>
    /// <param name="model"></param>
    /// <param name="State">
    /// An <see langword="out"/> parameter set to the state of the current instance if the method returns
    /// <see langword="true"/>, or the default value of type <typeparamref name="TImmutable"/> if the method
    /// returns <see langword="false"/>.
    /// </param>
    /// <returns>Whether or not the model was in a valid state.</returns>
    public static bool TryGetCurrentState<TImmutable>(
        this IMutablePartialModelGetState<TImmutable> model,
        [MaybeNullWhen(false), MaybeDefaultWhen(false)] out TImmutable State)
    {
        if (model.IsInValidState)
        {
            State = model.CurrentState;
            return true;
        }
        else
        {
            State = default;
            return false;
        }
    }

    /// <summary>
    /// Gets the state of the current mutable model instance, or the default value of type
    /// <typeparamref name="TImmutable"/> if the current instance is in an invalid state.
    /// </summary>
    /// <typeparam name="TImmutable"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    [return: MaybeNull, MaybeDefault]
    public static TImmutable GetCurrentStateOrDefault<TImmutable>(this IMutablePartialModelGetState<TImmutable> model)
        => model.IsInValidState ? model.CurrentState : default;
}

/// <summary>
/// An interface for types that represent mutable versions of an immutable type <typeparamref name="TImmutable"/>,
/// and that can be queried for a <typeparamref name="TImmutable"/> instance representing the current state, but may be
/// in an invalid state not representing an instance of <typeparamref name="TImmutable"/>.
/// </summary>
/// <typeparam name="TImmutable"></typeparam>
public interface IMutablePartialModelGetState<out TImmutable>
{
    /// <summary>
    /// Gets whether or not this object is in a valid state that represents an instance of
    /// <typeparamref name="TImmutable"/>.
    /// </summary>
    public bool IsInValidState { get; }

    /// <summary>
    /// Gets a <typeparamref name="TImmutable"/> instance representing the current state of this object, or throws
    /// an exception if this object is not in a valid state.
    /// </summary>
    /// <exception cref="InvalidMutableModelStateException">
    /// This object was not in a valid state.
    /// </exception>
    public TImmutable CurrentState { get; }
}

/// <summary>
/// An interface for types that represent mutable versions of an immutable type <typeparamref name="TImmutable"/>,
/// can be queried for a <typeparamref name="TImmutable"/> instance representing the current state,
/// and can have their state set to describe a given <typeparamref name="TImmutable"/>, but may be in an invalid state
/// not representing an instance of type <typeparamref name="TImmutable"/>.
/// </summary>
/// <typeparam name="TImmutable"></typeparam>
public interface IMutablePartialModel<TImmutable>
    : IMutablePartialModelGetState<TImmutable>, IMutableModelSetState<TImmutable>
{
    //
}

/// <summary>
/// An exception thrown when an attempt is made to access an <see cref="IMutablePartialModelGetState{TImmutable}"/>
/// instance that is in an invalid state.
/// </summary>
public class InvalidMutableModelStateException : InvalidOperationException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidMutableModelStateException"/> class.
    /// </summary>
    public InvalidMutableModelStateException() : base("Mutable model was in an invalid state.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidMutableModelStateException"/> class with the specified
    /// error message.
    /// </summary>
    /// <param name="message"></param>
    public InvalidMutableModelStateException(string message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidMutableModelStateException"/> class with the specified
    /// error message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public InvalidMutableModelStateException(string message, Exception innerException)
        : base(message, innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidMutableModelStateException"/> class from the serialization
    /// data passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected InvalidMutableModelStateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
#endregion
