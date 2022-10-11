namespace Rem.CoreTest.ComponentModel;

/// <summary>
/// Tests of the <see cref="INotifyNestedPropertyChanging"/> and <see cref="INotifyNestedPropertyChanged"/> interfaces.
/// </summary>
[TestClass]
public class NestedPropertyChangeTest
{
    /// <summary>
    /// Tests property change event argument methods.
    /// </summary>
    [TestMethod]
    public void TestEventArgs()
    {
        var parentChange = new NestedPropertyChangedEventArgs(new[] { "A", "B" });
        var childChange = new NestedPropertyChangedEventArgs(new[] { "A", "B", "C" });
        var somethingElseChange = new NestedPropertyChangedEventArgs(new[] { "D", "E" });

        Assert.IsTrue(parentChange.ImpliesChangeOf(childChange.PropertyPath));
        Assert.IsFalse(parentChange.ChangeImpliedBy(childChange.PropertyPath));
        Assert.IsTrue(childChange.ChangeImpliedBy(parentChange.PropertyPath));
        Assert.IsFalse(childChange.ImpliesChangeOf(parentChange.PropertyPath));

        Assert.IsTrue(parentChange.ImpliesChangeOf(parentChange.PropertyPath));
        Assert.IsTrue(parentChange.ChangeImpliedBy(parentChange.PropertyPath));

        Assert.IsFalse(parentChange.ImpliesChangeOf(somethingElseChange.PropertyPath));
        Assert.IsFalse(parentChange.ChangeImpliedBy(somethingElseChange.PropertyPath));
    }
}