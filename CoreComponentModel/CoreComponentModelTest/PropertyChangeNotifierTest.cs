using System;
using System.ComponentModel;

namespace Rem.CoreTest.ComponentModel;

/// <summary>
/// Tests the <see cref="PropertyChangeNotifier"/> and <see cref="PropertyChangeNotifier{TNotifier}"/> classes.
/// </summary>
[TestClass]
public class PropertyChangeNotifierTest
{
    #region Tests
    #region Metadata
    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier{TNotifier}.Type"/>
    /// </summary>
    [TestMethod]
    public void TestType()
    {
        Assert.AreEqual(PropertyChangeNotifierType.None,
                        PropertyChangeNotifier<NonNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.PropertyChanging,
                        PropertyChangeNotifier<ChangingOnlyNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.PropertyChanged,
                        PropertyChangeNotifier<ChangedOnlyNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.PropertyChange,
                        PropertyChangeNotifier<ChangeNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.NestedPropertyChanging,
                        PropertyChangeNotifier<NestedChangingOnlyNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.NestedPropertyChanged,
                        PropertyChangeNotifier<NestedChangedOnlyNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.PreNestedPropertyChange,
                        PropertyChangeNotifier<NestedChangingAndChangedNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.PostNestedPropertyChange,
                        PropertyChangeNotifier<NestedChangedAndChangingNotifier>.Type);

        Assert.AreEqual(PropertyChangeNotifierType.NestedPropertyChange,
                        PropertyChangeNotifier<NestedChangeNotifier>.Type);
    }
    #endregion

    #region Non-Nested
    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that do not implement any property change notifications.
    /// </summary>
    [TestMethod]
    public void TestNonNotifier() => RunSubscriptionTest(new NonNotifier(), PropertyChangeEventFlags.None);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement only non-nested property changing notifications.
    /// </summary>
    [TestMethod]
    public void TestChangingOnly()
        => RunSubscriptionTest(new ChangingOnlyNotifier(), PropertyChangeEventFlags.PropertyChanging);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement only non-nested property changed notifications.
    /// </summary>
    [TestMethod]
    public void TestChangedOnly()
        => RunSubscriptionTest(new ChangedOnlyNotifier(), PropertyChangeEventFlags.PropertyChanged);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement both non-nested property change notifications.
    /// </summary>
    [TestMethod]
    public void TestChange()
        => RunSubscriptionTest(
            new ChangeNotifier(),
            PropertyChangeEventFlags.PropertyChanging | PropertyChangeEventFlags.PropertyChanged);
    #endregion

    #region Nested
    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement only nested and non-nested property changed notifications.
    /// </summary>
    [TestMethod]
    public void TestNestedChangedOnly()
        => RunNestedTest(
            new NestedChangedOnlyNotifier(),
            PropertyChangeEventFlags.PropertyChanged | PropertyChangeEventFlags.NestedPropertyChanged);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement only nested and non-nested property changing notifications.
    /// </summary>
    [TestMethod]
    public void TestNestedChangingOnly()
        => RunNestedTest(
            new NestedChangingOnlyNotifier(),
            PropertyChangeEventFlags.PropertyChanging | PropertyChangeEventFlags.NestedPropertyChanging);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement only nested and non-nested property changing and non-nested property changed notifications.
    /// </summary>
    [TestMethod]
    public void TestNestedChangingAndChangedOnly()
        => RunNestedTest(
            new NestedChangingAndChangedNotifier(),
            PropertyChangeEventFlags.PropertyChanging | PropertyChangeEventFlags.PropertyChanged
                | PropertyChangeEventFlags.NestedPropertyChanging);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement only nested and non-nested property changed and non-nested property changing notifications.
    /// </summary>
    [TestMethod]
    public void TestNestedChangedAndChangingOnly()
        => RunNestedTest(
            new NestedChangedAndChangingNotifier(),
            PropertyChangeEventFlags.PropertyChanged | PropertyChangeEventFlags.PropertyChanging
                | PropertyChangeEventFlags.NestedPropertyChanged);

    /// <summary>
    /// Tests the <see cref="PropertyChangeNotifier"/> subscription, unsubscription and shuffling behavior for objects
    /// that implement all available nested and non-nested property change notifications.
    /// </summary>
    [TestMethod]
    public void TestNestedChange()
        => RunNestedTest(
            new NestedChangeNotifier(),
            PropertyChangeEventFlags.PropertyChanged | PropertyChangeEventFlags.PropertyChanging
                | PropertyChangeEventFlags.NestedPropertyChanged | PropertyChangeEventFlags.NestedPropertyChanging);

    private static void RunNestedTest<TNotifier>(TNotifier notifier, PropertyChangeEventFlags expectedFlags)
        where TNotifier : TestNotifier
    {
        // Run tests for each possible option
        RunSubscriptionTest(notifier, expectedFlags, ChangeSubscriptionOption.AllSupported);
        RunSubscriptionTest(notifier, expectedFlags, ChangeSubscriptionOption.NestedChangingOnly);
        RunSubscriptionTest(notifier, expectedFlags, ChangeSubscriptionOption.NestedChangedOnly);
        RunSubscriptionTest(notifier, expectedFlags, ChangeSubscriptionOption.NestedChangeOnly);
    }
    #endregion
    #endregion

    #region Helpers
    #region Methods
    private static void RunSubscriptionTest<TNotifier>(
        TNotifier notifier, PropertyChangeEventFlags expectedFlags,
        ChangeSubscriptionOption subscriptionOption = ChangeSubscriptionOption.NestedChangeOnly)
        where TNotifier : TestNotifier
    {
        var notifications = new TestNotifications();
        var otherNotifications = new TestNotifications();
        expectedFlags = subscriptionOption.ApplyTo(expectedFlags);

        notifier.B = false; // Make sure the notifier starts in a default state (just for consistency)

        // Subscribe
        notifications.SubscribeTo(notifier, subscriptionOption);
        notifier.B = true;
        Assert.AreEqual(expectedFlags, notifications.FiredFlags);

        // Shuffle
        notifications.ResetFiredFlags();
        notifications.ShuffleToOther(notifier, otherNotifications, subscriptionOption);
        notifier.B = false;
        Assert.AreEqual(PropertyChangeEventFlags.None, notifications.FiredFlags);
        Assert.AreEqual(expectedFlags, otherNotifications.FiredFlags);

        // Unsubscribe
        otherNotifications.ResetFiredFlags();
        otherNotifications.UnsubscribeFrom(notifier);
        notifier.B = true;
        Assert.AreEqual(PropertyChangeEventFlags.None, otherNotifications.FiredFlags);
    }
    #endregion

    #region Types
    #region Test Notifiers
    private sealed class NonNotifier : TestNotifier
    {
        public override bool B { get; set; }
    }

    private sealed class ChangingOnlyNotifier : TestNotifier, INotifyPropertyChanging
    {
        public override bool B
        {
            get => _b;
            set
            {
                PropertyChanging?.Invoke(this, new(nameof(B)));
                _b = value;
            }
        }
        private bool _b;

        public event PropertyChangingEventHandler? PropertyChanging;
    }

    private sealed class ChangedOnlyNotifier : TestNotifier, INotifyPropertyChanged
    {
        public override bool B
        {
            get => _b;
            set
            {
                _b = value;
                PropertyChanged?.Invoke(this, new(nameof(B)));
            }
        }
        private bool _b;

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    private sealed class ChangeNotifier : TestNotifier, INotifyPropertyChanging, INotifyPropertyChanged
    {
        public override bool B
        {
            get => _b;
            set
            {
                PropertyChanging?.Invoke(this, new(nameof(B)));
                _b = value;
                PropertyChanged?.Invoke(this, new(nameof(B)));
            }
        }
        private bool _b;

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    private sealed class NestedChangingOnlyNotifier : TestNotifier, INotifyNestedPropertyChanging
    {
        public override bool B
        {
            get => _b;
            set
            {
                NestedPropertyChanging?.Invoke(this, new(nameof(B)));
                PropertyChanging?.Invoke(this, new(nameof(B)));
                _b = value;
            }
        }
        private bool _b;

        public event NestedPropertyChangingEventHandler? NestedPropertyChanging;
        public event PropertyChangingEventHandler? PropertyChanging;
    }

    private sealed class NestedChangedOnlyNotifier : TestNotifier, INotifyNestedPropertyChanged
    {
        public override bool B
        {
            get => _b;
            set
            {
                _b = value;
                NestedPropertyChanged?.Invoke(this, new(nameof(B)));
                PropertyChanged?.Invoke(this, new(nameof(B)));
            }
        }
        private bool _b;

        public event NestedPropertyChangedEventHandler? NestedPropertyChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    private sealed class NestedChangingAndChangedNotifier
        : TestNotifier, INotifyNestedPropertyChanging, INotifyPropertyChanged
    {
        public override bool B
        {
            get => _b;
            set
            {
                NestedPropertyChanging?.Invoke(this, new(nameof(B)));
                PropertyChanging?.Invoke(this, new(nameof(B)));
                _b = value;
                PropertyChanged?.Invoke(this, new(nameof(B)));
            }
        }
        private bool _b;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NestedPropertyChangingEventHandler? NestedPropertyChanging;
        public event PropertyChangingEventHandler? PropertyChanging;
    }

    private sealed class NestedChangedAndChangingNotifier
        : TestNotifier, INotifyPropertyChanging, INotifyNestedPropertyChanged
    {
        public override bool B
        {
            get => _b;
            set
            {
                PropertyChanging?.Invoke(this, new(nameof(B)));
                _b = value;
                NestedPropertyChanged?.Invoke(this, new(nameof(B)));
                PropertyChanged?.Invoke(this, new(nameof(B)));
            }
        }
        private bool _b;

        public event NestedPropertyChangedEventHandler? NestedPropertyChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;
    }

    private sealed class NestedChangeNotifier
        : TestNotifier, INotifyNestedPropertyChanging, INotifyNestedPropertyChanged
    {
        public override bool B
        {
            get => _b;
            set
            {
                NestedPropertyChanging?.Invoke(this, new(nameof(B)));
                PropertyChanging?.Invoke(this, new(nameof(B)));
                _b = value;
                NestedPropertyChanged?.Invoke(this, new(nameof(B)));
                PropertyChanged?.Invoke(this, new(nameof(B)));
            }
        }
        private bool _b;

        public event NestedPropertyChangedEventHandler? NestedPropertyChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NestedPropertyChangingEventHandler? NestedPropertyChanging;
        public event PropertyChangingEventHandler? PropertyChanging;
    }

    private abstract class TestNotifier
    {
        public abstract bool B { get; set; }
    }
    #endregion

    #region Test Notifications
    private sealed class TestNotifications
    {
        /// <summary>
        /// Stores flags indicating which events were fired.
        /// </summary>
        public PropertyChangeEventFlags FiredFlags { get; private set; }

        public void PropertyChangingEventHandler(object? sender, PropertyChangingEventArgs e)
        {
            FiredFlags |= PropertyChangeEventFlags.PropertyChanging;
        }

        public void PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e)
        {
            FiredFlags |= PropertyChangeEventFlags.PropertyChanged;
        }

        public void NestedPropertyChangingEventHandler(object? sender, NestedPropertyChangingEventArgs e)
        {
            FiredFlags |= PropertyChangeEventFlags.NestedPropertyChanging;
        }

        public void NestedPropertyChangedEventHandler(object? sender, NestedPropertyChangedEventArgs e)
        {
            FiredFlags |= PropertyChangeEventFlags.NestedPropertyChanged;
        }

        public void ResetFiredFlags() => FiredFlags = PropertyChangeEventFlags.None;

        public void SubscribeTo<T>(
            T value, ChangeSubscriptionOption option = ChangeSubscriptionOption.NestedChangeOnly)
            => PropertyChangeNotifier.SubscribeTo(
                value,
                NestedPropertyChangingEventHandler, PropertyChangingEventHandler,
                option.HasOption(ChangeSubscriptionOption.NestedChangingOnly),
                NestedPropertyChangedEventHandler, PropertyChangedEventHandler,
                option.HasOption(ChangeSubscriptionOption.NestedChangedOnly));

        public void UnsubscribeFrom<T>(T value)
            => PropertyChangeNotifier.UnsubscribeFrom(
                value,
                NestedPropertyChangingEventHandler, PropertyChangingEventHandler,
                NestedPropertyChangedEventHandler, PropertyChangedEventHandler);

        public void ShuffleToOther<T>(
            T value,
            TestNotifications other,
            ChangeSubscriptionOption option = ChangeSubscriptionOption.NestedChangeOnly)
            => PropertyChangeNotifier.Shuffle(
                value,
                NestedPropertyChangingEventHandler, other.NestedPropertyChangingEventHandler,
                PropertyChangingEventHandler, other.PropertyChangingEventHandler,
                option.HasOption(ChangeSubscriptionOption.NestedChangingOnly),
                NestedPropertyChangedEventHandler, other.NestedPropertyChangedEventHandler,
                PropertyChangedEventHandler, other.PropertyChangedEventHandler,
                option.HasOption(ChangeSubscriptionOption.NestedChangedOnly));
    }
    #endregion
    #endregion
    #endregion
}
