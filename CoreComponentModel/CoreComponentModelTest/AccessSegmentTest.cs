using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.ComponentModel;

/// <summary>
/// Tests of the <see cref="AccessSegment32"/> and <see cref="AccessSegment64"/> structs.
/// </summary>
[TestClass]
public class AccessSegmentTest
{
    /// <summary>
    /// Asserts that the default value is as advertised in the documentation.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(new AccessSegment32(0), default);
    }

    /// <summary>
    /// Tests construction of a segment.
    /// </summary>
    [TestMethod]
    public void TestConstruct()
    {
        var indexSegment = new AccessSegment32(3);
        Assert.AreEqual(3, indexSegment.IndexValue);

        var propertySegment = new AccessSegment32("sss");
        Assert.AreEqual("sss", propertySegment.PropertyName);

        Assert.ThrowsException<ArgumentNullException>(() => new AccessSegment32(null!));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new AccessSegment32(-1));
    }

    /// <summary>
    /// Tests try methods of segments.
    /// </summary>
    [TestMethod]
    public void TestTry()
    {
#pragma warning disable IDE0018 // The declaration is cleaner up here since the method is separated into regions
        int indexValue;
        string? propertyName;
#pragma warning restore IDE0018

        #region Index
        var indexSegment = new AccessSegment32(4);

        Assert.IsTrue(indexSegment.TryIndex(out indexValue));
        Assert.AreEqual(indexSegment.IndexValue, indexValue);

        Assert.IsTrue(indexSegment.TryIndex(out indexValue, out propertyName));
        Assert.AreEqual(indexSegment.IndexValue, indexValue);
        Assert.IsNull(propertyName);

        Assert.IsFalse(indexSegment.TryProperty(out propertyName));
        Assert.IsNull(propertyName);

        Assert.IsFalse(indexSegment.TryProperty(out propertyName, out indexValue));
        Assert.IsNull(propertyName);
        Assert.AreEqual(indexSegment.IndexValue, indexValue);
        #endregion

        #region Property
        var propSegment = new AccessSegment32("AProperty");

        Assert.IsTrue(propSegment.TryProperty(out propertyName));
        Assert.AreEqual(propSegment.PropertyName, propertyName);

        Assert.IsTrue(propSegment.TryProperty(out propertyName, out indexValue));
        Assert.AreEqual(propSegment.PropertyName, propertyName);
        Assert.AreEqual(0, indexValue);

        Assert.IsFalse(propSegment.TryIndex(out indexValue));
        Assert.AreEqual(0, indexValue);

        Assert.IsFalse(propSegment.TryIndex(out indexValue, out propertyName));
        Assert.AreEqual(0, indexValue);
        Assert.AreEqual(propSegment.PropertyName, propertyName);
        #endregion
    }

    /// <summary>
    /// Tests the <c>IndexOrNull</c> property of access segments.
    /// </summary>
    [TestMethod]
    public void TestIndexOrNull()
    {
        var indexSegment = new AccessSegment32(4);

        Assert.AreEqual(indexSegment.IndexValue, indexSegment.IndexValueOrNull);
        Assert.IsNull(new AccessSegment32("Prop").IndexValueOrNull);
    }
}
