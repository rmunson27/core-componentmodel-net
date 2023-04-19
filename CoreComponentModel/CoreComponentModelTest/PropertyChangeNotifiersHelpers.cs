/// <summary>
/// Extension methods for the <see cref="ChangeSubscriptionOption"/> enum.
/// </summary>
internal static class ChangeSubscriptionOptions
{
    /// <summary>
    /// Applies the current option to a set of event flags, getting the expected resulting flags.
    /// </summary>
    /// <param name="option"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static PropertyChangeEventFlags ApplyTo(
        this ChangeSubscriptionOption option, PropertyChangeEventFlags flags)
    {
        // Remove options as necessary to describe the expected result
        if (option.HasOption(ChangeSubscriptionOption.NestedChangingOnly)
                && flags.HasEventFlag(PropertyChangeEventFlags.NestedPropertyChanging))
        {
            flags &= ~PropertyChangeEventFlags.PropertyChanging;
        }

        if (option.HasOption(ChangeSubscriptionOption.NestedChangedOnly)
                && flags.HasEventFlag(PropertyChangeEventFlags.NestedPropertyChanged))
        {
            flags &= ~PropertyChangeEventFlags.PropertyChanged;
        }

        return flags;
    }

    /// <summary>
    /// Determines if the given option is present in the current option set.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static bool HasOption(this ChangeSubscriptionOption current, ChangeSubscriptionOption option)
        => (current & option) == option;
}

/// <summary>
/// Extension methods for the <see cref="PropertyChangeEventFlags"/> enum.
/// </summary>
internal static class PropertyChangeEventFlagsExtensions
{
    /// <summary>
    /// Determines if the given flag is present in the current flag set.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static bool HasEventFlag(this PropertyChangeEventFlags value, PropertyChangeEventFlags flag)
        => (value & flag) == flag;
}

/// <summary>
/// Flags representing property change events.
/// </summary>
/// <remarks>
/// This differs from <see cref="PropertyChangeNotifications"/> because this type provides info about the actual
/// events fired rather than the notifications implemented by a given type.
/// </remarks>
[Flags]
internal enum PropertyChangeEventFlags
{
    /// <summary>
    /// Indicates that no property change events were fired.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that a non-nested property changed event was fired.
    /// </summary>
    PropertyChanged = 1,

    /// <summary>
    /// Indicates that a non-nested property changing event was fired.
    /// </summary>
    PropertyChanging = 2,

    /// <summary>
    /// Indicates that a nested property changed event was fired.
    /// </summary>
    NestedPropertyChanged = 4,

    /// <summary>
    /// Indicates that a nested property changing event was fired.
    /// </summary>
    NestedPropertyChanging = 8,
}

/// <summary>
/// Represents the options for handling of subscriptions to nested change events.
/// </summary>
[Flags]
internal enum ChangeSubscriptionOption : byte
{
    /// <summary>
    /// Indicates that all available events will be subscribed to.
    /// </summary>
    AllSupported = 0,

    /// <summary>
    /// Indicates that only nested changed events will be subscribed to, ignoring non-nested changed events.
    /// </summary>
    NestedChangedOnly = 1,

    /// <summary>
    /// Indicates that only nested changing events will be subscribed to, ignoring non-nested changing events.
    /// </summary>
    NestedChangingOnly = 2,

    /// <summary>
    /// Indicates that only nested change events will be subscribed to, ignoring their non-nested counterparts.
    /// </summary>
    NestedChangeOnly = NestedChangingOnly | NestedChangedOnly,
}
