using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rem.Core.Attributes;

namespace Rem.Core.ComponentModel;

/// <summary>
/// Stores helper functionality for handling objects that notify of property changes.
/// </summary>
/// <remarks>
/// The functionality in this class can be used to subscribe to, and unsubscribe from, property change events of
/// a given type when the type (and therefore the property change notifications supported by the type) may not be
/// known, for instance when designing a generic class.
/// </remarks>
public static class PropertyChangeNotifier
{
    #region Notification Subscription And Unsubscription
    /// <summary>
    /// Subscribes to events of <paramref name="value"/> for any property change notifications supported by
    /// type <typeparamref name="TNotifier"/>.
    /// </summary>
    /// <typeparam name="TNotifier">The type of the value to subscribe to.</typeparam>
    /// <param name="value">The value to subscribe to.</param>
    /// <param name="nestedPropertyChanging">
    /// The (optional) <see cref="NestedPropertyChangingEventHandler"/> to add to the
    /// <see cref="INotifyNestedPropertyChanging.NestedPropertyChanging"/> event.
    /// </param>
    /// <param name="propertyChanging">
    /// The (optional) <see cref="PropertyChangingEventHandler"/> to add to the
    /// <see cref="INotifyPropertyChanging.PropertyChanging"/> event.
    /// </param>
    /// <param name="nestedChangingOnly">
    /// Whether or not to ignore the <paramref name="propertyChanging"/> parameter if <typeparamref name="TNotifier"/>
    /// implements the <see cref="INotifyNestedPropertyChanging"/> interface.
    /// <para/>
    /// Defaults to <see langword="true"/>, as property changing notifications should cause a nested property changing
    /// notification to be fired if implementations are designed correctly.
    /// </param>
    /// <param name="nestedPropertyChanged">
    /// The (optional) <see cref="NestedPropertyChangedEventHandler"/> to add to the
    /// <see cref="INotifyNestedPropertyChanged.NestedPropertyChanged"/> event.
    /// </param>
    /// <param name="propertyChanged">
    /// The (optional) <see cref="PropertyChangedEventHandler"/> to add to the
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
    /// </param>
    /// <param name="nestedChangedOnly">
    /// Whether or not to ignore the <paramref name="propertyChanged"/> parameter if <typeparamref name="TNotifier"/>
    /// implements the <see cref="INotifyNestedPropertyChanged"/> interface.
    /// <para/>
    /// Defaults to <see langword="true"/>, as property changed notifications should cause a nested property changed
    /// notification to be fired if implementations are designed correctly.
    /// </param>
    public static void SubscribeTo<TNotifier>(
        TNotifier value,

        NestedPropertyChangingEventHandler? nestedPropertyChanging = null,
        PropertyChangingEventHandler? propertyChanging = null,
        bool nestedChangingOnly = true,

        NestedPropertyChangedEventHandler? nestedPropertyChanged = null,
        PropertyChangedEventHandler? propertyChanged = null,
        bool nestedChangedOnly = true)
    {
        // Handle property changing
        switch ((PropertyChangeNotifier<TNotifier>.Type & PropertyChangeNotifierType.NestedPropertyChanging).Case)
        {
            case PropertyChangeNotifierType.Cases.NestedPropertyChanging:
                var notifier = CastToInterface<TNotifier, INotifyNestedPropertyChanging>(value);
                SubscribeToNestedPropertyChanging(notifier, nestedPropertyChanging);
                if (!nestedChangingOnly) SubscribeToPropertyChanging(notifier, propertyChanging);
                break;

            case PropertyChangeNotifierType.Cases.PropertyChanging:
                SubscribeToPropertyChanging(CastToInterface<TNotifier, INotifyPropertyChanging>(value),
                                            propertyChanging);
                break;
        }

        // Handle property changed
        switch ((PropertyChangeNotifier<TNotifier>.Type & PropertyChangeNotifierType.NestedPropertyChanged).Case)
        {
            case PropertyChangeNotifierType.Cases.NestedPropertyChanged:
                var notifier = CastToInterface<TNotifier, INotifyNestedPropertyChanged>(value);
                SubscribeToNestedPropertyChanged(notifier, nestedPropertyChanged);
                if (!nestedChangedOnly) SubscribeToPropertyChanged(notifier, propertyChanged);
                break;

            case PropertyChangeNotifierType.Cases.PropertyChanged:
                SubscribeToPropertyChanged(CastToInterface<TNotifier, INotifyPropertyChanged>(value), propertyChanged);
                break;
        }
    }

    /// <summary>
    /// Unsubscribes from events of <paramref name="value"/> for any property change notifications supported by
    /// type <typeparamref name="TNotifier"/>.
    /// </summary>
    /// <typeparam name="TNotifier">The type of the value to unsubscribe from.</typeparam>
    /// <param name="value">The value to unsubscribe from.</param>
    /// <param name="nestedPropertyChanging">
    /// The (optional) <see cref="NestedPropertyChangingEventHandler"/> to remove from the
    /// <see cref="INotifyNestedPropertyChanging.NestedPropertyChanging"/> event.
    /// </param>
    /// <param name="propertyChanging">
    /// The (optional) <see cref="PropertyChangingEventHandler"/> to remove from the
    /// <see cref="INotifyPropertyChanging.PropertyChanging"/> event.
    /// </param>
    /// <param name="nestedPropertyChanged">
    /// The (optional) <see cref="NestedPropertyChangedEventHandler"/> to remove from the
    /// <see cref="INotifyNestedPropertyChanged.NestedPropertyChanged"/> event.
    /// </param>
    /// <param name="propertyChanged">
    /// The (optional) <see cref="PropertyChangedEventHandler"/> to remove from the
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
    /// </param>
    public static void UnsubscribeFrom<TNotifier>(
        TNotifier value,

        NestedPropertyChangingEventHandler? nestedPropertyChanging = null,
        PropertyChangingEventHandler? propertyChanging = null,

        NestedPropertyChangedEventHandler? nestedPropertyChanged = null,
        PropertyChangedEventHandler? propertyChanged = null)
    {
        // Handle property changing
        switch ((PropertyChangeNotifier<TNotifier>.Type & PropertyChangeNotifierType.NestedPropertyChanging).Case)
        {
            case PropertyChangeNotifierType.Cases.NestedPropertyChanging:
                var nestedNotifier = CastToInterface<TNotifier, INotifyNestedPropertyChanging>(value);
                UnsubscribeFromNestedPropertyChanging(nestedNotifier, nestedPropertyChanging);
                UnsubscribeFromPropertyChanging(nestedNotifier, propertyChanging);
                break;

            case PropertyChangeNotifierType.Cases.PropertyChanging:
                UnsubscribeFromPropertyChanging(CastToInterface<TNotifier, INotifyPropertyChanging>(value),
                                                propertyChanging);
                break;
        }

        // Handle property changed
        switch ((PropertyChangeNotifier<TNotifier>.Type & PropertyChangeNotifierType.NestedPropertyChanged).Case)
        {
            case PropertyChangeNotifierType.Cases.NestedPropertyChanged:
                var nestedNotifier = CastToInterface<TNotifier, INotifyNestedPropertyChanged>(value);
                UnsubscribeFromNestedPropertyChanged(nestedNotifier, nestedPropertyChanged);
                UnsubscribeFromPropertyChanged(nestedNotifier, propertyChanged);
                break;

            case PropertyChangeNotifierType.Cases.PropertyChanged:
                UnsubscribeFromPropertyChanged(CastToInterface<TNotifier, INotifyPropertyChanged>(value),
                                               propertyChanged);
                break;
        }
    }

    /// <summary>
    /// Shuffles events of <paramref name="value"/> for any property change notifications supported by
    /// type <typeparamref name="TNotifier"/>, subscribing to new events and unsubscribing from old events.
    /// </summary>
    /// <typeparam name="TNotifier">The type of the value to shuffle events for.</typeparam>
    /// <param name="value">The value to shuffle events for.</param>
    /// <param name="oldNestedPropertyChanging">
    /// The (optional) old <see cref="NestedPropertyChangingEventHandler"/> to remove from the
    /// <see cref="INotifyNestedPropertyChanging.NestedPropertyChanging"/> event.
    /// </param>
    /// <param name="newNestedPropertyChanging">
    /// The (optional) new <see cref="NestedPropertyChangingEventHandler"/> to add to the
    /// <see cref="INotifyNestedPropertyChanging.NestedPropertyChanging"/> event.
    /// </param>
    /// <param name="oldPropertyChanging">
    /// The (optional) old <see cref="PropertyChangingEventHandler"/> to remove from the
    /// <see cref="INotifyPropertyChanging.PropertyChanging"/> event.
    /// </param>
    /// <param name="newPropertyChanging">
    /// The (optional) new <see cref="PropertyChangingEventHandler"/> to add to the
    /// <see cref="INotifyPropertyChanging.PropertyChanging"/> event.
    /// </param>
    /// <param name="nestedChangingOnly">
    /// Whether or not to ignore the <paramref name="newPropertyChanging"/> parameter if
    /// <typeparamref name="TNotifier"/> implements the <see cref="INotifyNestedPropertyChanging"/> interface.
    /// </param>
    /// <param name="oldNestedPropertyChanged">
    /// The (optional) old <see cref="NestedPropertyChangedEventHandler"/> to remove from the
    /// <see cref="INotifyNestedPropertyChanged.NestedPropertyChanged"/> event.
    /// </param>
    /// <param name="newNestedPropertyChanged">
    /// The (optional) new <see cref="NestedPropertyChangedEventHandler"/> to add to the
    /// <see cref="INotifyNestedPropertyChanged.NestedPropertyChanged"/> event.
    /// </param>
    /// <param name="oldPropertyChanged">
    /// The (optional) old <see cref="PropertyChangedEventHandler"/> to remove from the
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
    /// </param>
    /// <param name="newPropertyChanged">
    /// The (optional) new <see cref="PropertyChangedEventHandler"/> to add to the
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
    /// </param>
    /// <param name="nestedChangedOnly">
    /// Whether or not to ignore the <paramref name="newPropertyChanged"/> parameter if
    /// <typeparamref name="TNotifier"/> implements the <see cref="INotifyNestedPropertyChanged"/> interface.
    /// </param>
    public static void Shuffle<TNotifier>(
        TNotifier value,

        NestedPropertyChangingEventHandler? oldNestedPropertyChanging = null,
        NestedPropertyChangingEventHandler? newNestedPropertyChanging = null,
        PropertyChangingEventHandler? oldPropertyChanging = null,
        PropertyChangingEventHandler? newPropertyChanging = null,
        bool nestedChangingOnly = true,

        NestedPropertyChangedEventHandler? oldNestedPropertyChanged = null,
        NestedPropertyChangedEventHandler? newNestedPropertyChanged = null,
        PropertyChangedEventHandler? oldPropertyChanged = null,
        PropertyChangedEventHandler? newPropertyChanged = null,
        bool nestedChangedOnly = true)
    {
        // Handle property changing
        switch ((PropertyChangeNotifier<TNotifier>.Type & PropertyChangeNotifierType.NestedPropertyChanging).Case)
        {
            case PropertyChangeNotifierType.Cases.NestedPropertyChanging:
                var notifier = CastToInterface<TNotifier, INotifyNestedPropertyChanging>(value);
                ShuffleNestedPropertyChanging(notifier, oldNestedPropertyChanging, newNestedPropertyChanging);
                if (!nestedChangingOnly) ShufflePropertyChanging(notifier, oldPropertyChanging, newPropertyChanging);
                break;

            case PropertyChangeNotifierType.Cases.PropertyChanging:
                ShufflePropertyChanging(
                    CastToInterface<TNotifier, INotifyPropertyChanging>(value),
                    oldPropertyChanging, newPropertyChanging);
                break;
        }

        // Handle property changed
        switch ((PropertyChangeNotifier<TNotifier>.Type & PropertyChangeNotifierType.NestedPropertyChanged).Case)
        {
            case PropertyChangeNotifierType.Cases.NestedPropertyChanged:
                var notifier = CastToInterface<TNotifier, INotifyNestedPropertyChanged>(value);
                ShuffleNestedPropertyChanged(notifier, oldNestedPropertyChanged, newNestedPropertyChanged);
                if (!nestedChangedOnly) ShufflePropertyChanged(notifier, oldPropertyChanged, newPropertyChanged);
                break;

            case PropertyChangeNotifierType.Cases.PropertyChanged:
                ShufflePropertyChanged(
                    CastToInterface<TNotifier, INotifyPropertyChanged>(value), oldPropertyChanged, newPropertyChanged);
                break;
        }
    }
    #endregion

    #region Helper Methods
    #region Nullable Notifier Helpers
    #region Subscription
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SubscribeToNestedPropertyChanging(
        INotifyNestedPropertyChanging? notifier, NestedPropertyChangingEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.NestedPropertyChanging += eventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SubscribeToNestedPropertyChanged(
        INotifyNestedPropertyChanged? notifier, NestedPropertyChangedEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.NestedPropertyChanged += eventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SubscribeToPropertyChanging(
        INotifyPropertyChanging? notifier, PropertyChangingEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.PropertyChanging += eventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SubscribeToPropertyChanged(
        INotifyPropertyChanged? notifier, PropertyChangedEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.PropertyChanged += eventHandler;
        }
    }
    #endregion

    #region Unsubscription
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UnsubscribeFromNestedPropertyChanging(
        INotifyNestedPropertyChanging? notifier, NestedPropertyChangingEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.NestedPropertyChanging -= eventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UnsubscribeFromNestedPropertyChanged(
        INotifyNestedPropertyChanged? notifier, NestedPropertyChangedEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.NestedPropertyChanged -= eventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UnsubscribeFromPropertyChanging(
        INotifyPropertyChanging? notifier, PropertyChangingEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.PropertyChanging -= eventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UnsubscribeFromPropertyChanged(
        INotifyPropertyChanged? notifier, PropertyChangedEventHandler? eventHandler)
    {
        if (notifier is not null)
        {
            notifier.PropertyChanged -= eventHandler;
        }
    }
    #endregion

    #region Shuffling (Both Subscription and Unsubscription)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ShuffleNestedPropertyChanging(
        INotifyNestedPropertyChanging? notifier,
        NestedPropertyChangingEventHandler? oldEventHandler,
        NestedPropertyChangingEventHandler? newEventHandler)
    {
        if (notifier is not null)
        {
            notifier.NestedPropertyChanging -= oldEventHandler;
            notifier.NestedPropertyChanging += newEventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ShuffleNestedPropertyChanged(
        INotifyNestedPropertyChanged? notifier,
        NestedPropertyChangedEventHandler? oldEventHandler,
        NestedPropertyChangedEventHandler? newEventHandler)
    {
        if (notifier is not null)
        {
            notifier.NestedPropertyChanged -= oldEventHandler;
            notifier.NestedPropertyChanged += newEventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ShufflePropertyChanging(
        INotifyPropertyChanging? notifier,
        PropertyChangingEventHandler? oldEventHandler,
        PropertyChangingEventHandler? newEventHandler)
    {
        if (notifier is not null)
        {
            notifier.PropertyChanging -= oldEventHandler;
            notifier.PropertyChanging += newEventHandler;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ShufflePropertyChanged(
        INotifyPropertyChanged? notifier,
        PropertyChangedEventHandler? oldEventHandler,
        PropertyChangedEventHandler? newEventHandler)
    {
        if (notifier is not null)
        {
            notifier.PropertyChanged -= oldEventHandler;
            notifier.PropertyChanged += newEventHandler;
        }
    }
    #endregion
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TInterface? CastToInterface<T, TInterface>(T value) where TInterface : class
        => PropertyChangeEvents<T>.IsValueType ? (TInterface?)(object?)value : Unsafe.As<T, TInterface?>(ref value);
    #endregion
}

/// <summary>
/// Stores helper functionality for handling property change notifications implemented by <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="TNotifier">The type handled by this class.</typeparam>
public static class PropertyChangeNotifier<TNotifier>
{
    /// <summary>
    /// Gets a value describing the property change notifications <typeparamref name="T"/> supports.
    /// </summary>
    public static PropertyChangeNotifierType Type => PropertyChangeEvents<TNotifier>.Supported;
}

/// <summary>
/// Represents a valid set of property change notifications that can be implemented by a type.
/// </summary>
public readonly record struct PropertyChangeNotifierType
    : IEnumeratedCaseUnion<PropertyChangeNotifierType, PropertyChangeNotifierType.Cases>
{
    #region Constants
    /// <inheritdoc cref="Cases.None"/>
    public static readonly PropertyChangeNotifierType None = default;

    /// <inheritdoc cref="Cases.PropertyChanging"/>
    public static readonly PropertyChangeNotifierType PropertyChanging = new(Cases.PropertyChanging);

    /// <inheritdoc cref="Cases.PropertyChanged"/>
    public static readonly PropertyChangeNotifierType PropertyChanged = new(Cases.PropertyChanged);

    /// <summary>
    /// Indicates that a type implements both <see cref="INotifyPropertyChanging"/> and
    /// <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public static readonly PropertyChangeNotifierType PropertyChange = PropertyChanging | PropertyChanged;

    /// <inheritdoc cref="Cases.NestedPropertyChanging"/>
    public static readonly PropertyChangeNotifierType NestedPropertyChanging = new(Cases.NestedPropertyChanging);

    /// <inheritdoc cref="Cases.NestedPropertyChanged"/>
    public static readonly PropertyChangeNotifierType NestedPropertyChanged = new(Cases.NestedPropertyChanged);

    /// <summary>
    /// Indicates that the type implements both <see cref="INotifyPropertyChanged"/> and
    /// <see cref="INotifyNestedPropertyChanging"/> (implying that it also implements
    /// <see cref="INotifyPropertyChanging"/>).
    /// </summary>
    public static readonly PropertyChangeNotifierType PreNestedPropertyChange
        = NestedPropertyChanging | PropertyChanged;

    /// <summary>
    /// Indicates that the type implements both <see cref="INotifyPropertyChanging"/> and
    /// <see cref="INotifyNestedPropertyChanged"/> (implying that it also implements
    /// <see cref="INotifyPropertyChanged"/>).
    /// </summary>
    public static readonly PropertyChangeNotifierType PostNestedPropertyChange
        = PropertyChanging | NestedPropertyChanged;

    /// <summary>
    /// Indicates that the type implements both <see cref="INotifyNestedPropertyChanging"/> and
    /// <see cref="INotifyNestedPropertyChanged"/> (implying that it also implements both
    /// <see cref="INotifyPropertyChanging"/> and <see cref="INotifyPropertyChanged"/>).
    /// </summary>
    public static readonly PropertyChangeNotifierType NestedPropertyChange
        = NestedPropertyChanging | NestedPropertyChanged;
    #endregion

    #region Properties
    /// <inheritdoc/>
    public Cases Case { get; }
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PropertyChangeNotifierType(Cases Case) { this.Case = Case; }

    #region Operators
    /// <summary>
    /// Determines if the set of supported notifications <paramref name="first"/> represents is a strict subtype of
    /// the set of supported notifications <paramref name="second"/> represents.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator <(PropertyChangeNotifierType first, PropertyChangeNotifierType second)
        => first.Case != second.Case && first <= second;

    /// <summary>
    /// Determines if the set of supported notifications <paramref name="first"/> represents is a non-strict subtype of
    /// the set of supported notifications <paramref name="second"/> represents.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator <=(PropertyChangeNotifierType first, PropertyChangeNotifierType second)
        => (first.Case & second.Case) == first.Case;

    /// <summary>
    /// Determines if the set of supported notifications <paramref name="first"/> represents is a strict supertype of
    /// the set of supported notifications <paramref name="second"/> represents.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator >(PropertyChangeNotifierType first, PropertyChangeNotifierType second)
        => first.Case != second.Case && first >= second;

    /// <summary>
    /// Determines if the set of supported notifications <paramref name="first"/> represents is a non-strict supertype
    /// of the set of supported notifications <paramref name="second"/> represents.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator >=(PropertyChangeNotifierType first, PropertyChangeNotifierType second)
        => (first.Case & second.Case) == second.Case;

    /// <summary>
    /// Computes the logical union of two <see cref="PropertyChangeNotifierType"/> instances.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static PropertyChangeNotifierType operator |(PropertyChangeNotifierType first,
                                                            PropertyChangeNotifierType second)
        => new(first.Case | second.Case);

    /// <summary>
    /// Computes the logical intersection of two <see cref="PropertyChangeNotifierType"/> instances.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static PropertyChangeNotifierType operator &(PropertyChangeNotifierType first,
                                                            PropertyChangeNotifierType second)
        => new(first.Case & second.Case);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance represents the same set of property change notifications as another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PropertyChangeNotifierType other) => Case == other.Case;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Case.GetHashCode();
    #endregion

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Case.ToString();

    /// <summary>
    /// Implicitly converts a <see cref="Cases"/> value to the <see cref="PropertyChangeNotifierType"/> whose case
    /// it represents. 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="value"/> was unnamed.</exception>
    public static implicit operator PropertyChangeNotifierType(Cases value)
        => value.IsDefined()
            ? new(value)
            : throw EnumeratedCaseUnion.InvalidCase<PropertyChangeNotifierType>();

    /// <summary>
    /// Implicitly converts a <see cref="PropertyChangeNotifierType"/> to the <see cref="Cases"/> value
    /// representing its case.
    /// </summary>
    /// <param name="t"></param>
    public static implicit operator Cases(PropertyChangeNotifierType t) => t.Case;

    /// <summary>
    /// A degenerate <see cref="Cases"/> instance that represents an impossible type that implements
    /// <see cref="INotifyNestedPropertyChanging"/> but not <see cref="INotifyPropertyChanging"/>.
    /// </summary>
    /// <inheritdoc cref="DegenerateNestedPropertyChanged"/>
    private const Cases DegenerateNestedPropertyChanging = (Cases)4;

    /// <summary>
    /// A degenerate <see cref="Cases"/> instance that represents an impossible type that implements
    /// <see cref="INotifyNestedPropertyChanged"/> but not <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    /// <remarks>
    /// This is used to throw out invalid
    /// <see cref="operator ^(PropertyChangeNotifierType, PropertyChangeNotifierType)"/> results as
    /// <see langword="null"/>. We don't want to expose it to users because we don't want to encourage it's use - any
    /// attempt to cast it (implicitly) to <see cref="PropertyChangeNotifierType"/> will result in an exception, as
    /// it is an unnamed <see langword="enum"/> value.
    /// </remarks>
    private const Cases DegenerateNestedPropertyChanged = (Cases)8;

    /// <summary>
    /// Represents all possible cases of the <see cref="PropertyChangeNotifierType"/> type as an
    /// <see langword="enum"/> value.
    /// </summary>
    [Flags]
    [UnionCaseEnumerator]
    public enum Cases : byte
    {
        /// <summary>
        /// Indicates that a type implements no property change notifications.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that a type implements <see cref="INotifyPropertyChanging"/>.
        /// </summary>
        PropertyChanging = 1,

        /// <summary>
        /// Indicates that a type implements <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        PropertyChanged = 2,

        /// <summary>
        /// Indicates that a type implements <see cref="INotifyNestedPropertyChanging"/> (implying that it also
        /// implements <see cref="INotifyPropertyChanging"/>).
        /// </summary>
        NestedPropertyChanging = PropertyChanging | 4, // 5

        // DegenerateNestedPropertyChanging = 4, // (5 without PropertyChanging = 1)

        /// <summary>
        /// Indicates that a type implements <see cref="INotifyNestedPropertyChanged"/> (implying that it also
        /// implements <see cref="INotifyPropertyChanged"/>).
        /// </summary>
        NestedPropertyChanged = PropertyChanged | 8, // 10

        // DegenerateNestedPropertyChanged = 8, // (10 without PropertyChanged = 2)
    }
}

/// <summary>
/// File-scoped extension methods.
/// </summary>
file static class FileExtensions
{
    private const PropertyChangeNotifierType.Cases AllCases
        = PropertyChangeNotifierType.Cases.NestedPropertyChanging
            | PropertyChangeNotifierType.Cases.NestedPropertyChanged;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDefined(this PropertyChangeNotifierType.Cases e) => (e & ~AllCases) is 0;

    /// <summary>
    /// Efficient version of <see cref="Enum.HasFlag(Enum)"/> for the
    /// <see cref="PropertyChangeNotifierType.Cases"/> enum.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasType(this PropertyChangeNotifierType.Cases e,
                                     PropertyChangeNotifierType.Cases other)
        => (e & other) == other;
}
