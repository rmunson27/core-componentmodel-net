using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.ComponentModel;

/// <summary>
/// Tests the <see cref="Defaultable"/> class functionality.
/// </summary>
[TestClass]
public class DefaultableTest
{
    private static Class? ObjectRef = new();
    private static Class? NullRef = null;
    private static int? NullNullableVal = null;
    private static int? NonNullNullableVal = 4;
    private static DefaultableStruct NonDefaultDefaultableVal = new() { Object = new() };
    private static DefaultableStruct DefaultDefaultableVal = new();
    private static NonDefaultableStruct NonDefaultNonDefaultableVal = new() { Int = 4 };
    private static NonDefaultableStruct DefaultNonDefaultableVal = new();

    /// <summary>
    /// Tests the <see cref="Defaultable.IsDefault"/> and <see cref="Defaultable.RefsDefault"/> methods.
    /// </summary>
    [TestMethod]
    public void TestIsDefault()
    {
        Assert.IsFalse(Defaultable.IsDefault(ObjectRef));
        Assert.IsTrue(Defaultable.IsDefault(NullRef));
        Assert.IsFalse(Defaultable.IsDefault(NonDefaultDefaultableVal));
        Assert.IsTrue(Defaultable.IsDefault(DefaultDefaultableVal));
        Assert.IsFalse(Defaultable.IsDefault(NonNullNullableVal));
        Assert.IsTrue(Defaultable.IsDefault(NullNullableVal));
        Assert.IsFalse(Defaultable.IsDefault(NonDefaultNonDefaultableVal));
        Assert.IsFalse(Defaultable.IsDefault(DefaultNonDefaultableVal));

        Assert.IsFalse(Defaultable.RefsDefault(ref ObjectRef));
        Assert.IsTrue(Defaultable.RefsDefault(ref NullRef));
        Assert.IsFalse(Defaultable.RefsDefault(ref NonDefaultDefaultableVal));
        Assert.IsTrue(Defaultable.RefsDefault(ref DefaultDefaultableVal));
        Assert.IsFalse(Defaultable.RefsDefault(ref NonNullNullableVal));
        Assert.IsTrue(Defaultable.RefsDefault(ref NullNullableVal));
        Assert.IsFalse(Defaultable.RefsDefault(ref NonDefaultNonDefaultableVal));
        Assert.IsFalse(Defaultable.RefsDefault(ref DefaultNonDefaultableVal));
    }

    /// <summary>
    /// Tests the <see cref="Defaultable.ThrowIfDefault"/> and <see cref="Defaultable.ThrowIfRefsDefault"/> methods.
    /// </summary>
    [TestMethod]
    public void TestThrowIfDefault()
    {
        Assert.AreEqual(ObjectRef, Defaultable.ThrowIfDefault(ObjectRef));
        Assert.ThrowsException<NullReferenceException>(() => Defaultable.ThrowIfDefault(NullRef));
        Assert.AreEqual(NonDefaultDefaultableVal, Defaultable.ThrowIfDefault(NonDefaultDefaultableVal));
        Assert.ThrowsException<StructDefaultException>(() => Defaultable.ThrowIfDefault(DefaultDefaultableVal));
        Assert.AreEqual(NonNullNullableVal, Defaultable.ThrowIfDefault(NonNullNullableVal));
        Assert.ThrowsException<NullReferenceException>(() => Defaultable.ThrowIfDefault(NullNullableVal));
        Assert.AreEqual(NonDefaultNonDefaultableVal, Defaultable.ThrowIfDefault(NonDefaultNonDefaultableVal));
        Assert.AreEqual(DefaultNonDefaultableVal, Defaultable.ThrowIfDefault(DefaultNonDefaultableVal));

        Assert.AreEqual(ObjectRef, Defaultable.ThrowIfRefsDefault(ref ObjectRef));
        Assert.ThrowsException<NullReferenceException>(() => Defaultable.ThrowIfRefsDefault(ref NullRef));
        Assert.AreEqual(NonDefaultDefaultableVal, Defaultable.ThrowIfRefsDefault(ref NonDefaultDefaultableVal));
        Assert.ThrowsException<StructDefaultException>(
            () => Defaultable.ThrowIfRefsDefault(ref DefaultDefaultableVal));
        Assert.AreEqual(NonNullNullableVal, Defaultable.ThrowIfRefsDefault(ref NonNullNullableVal));
        Assert.ThrowsException<NullReferenceException>(() => Defaultable.ThrowIfRefsDefault(ref NullNullableVal));
        Assert.AreEqual(NonDefaultNonDefaultableVal, Defaultable.ThrowIfRefsDefault(ref NonDefaultNonDefaultableVal));
        Assert.AreEqual(DefaultNonDefaultableVal, Defaultable.ThrowIfRefsDefault(ref DefaultNonDefaultableVal));
    }

    /// <summary>
    /// Tests the <see cref="Defaultable.ThrowIfArgumentDefault"/> and
    /// <see cref="Defaultable.ThrowIfArgumentRefsDefault"/> methods.
    /// </summary>
    [TestMethod]
    public void TestThrowIfArgumentDefault()
    {
        const string ParamName = "parameter";

        Assert.AreEqual(ObjectRef, Defaultable.ThrowIfArgumentDefault(ObjectRef, ParamName));
        Assert.ThrowsException<ArgumentNullException>(() => Defaultable.ThrowIfArgumentDefault(NullRef, ParamName));
        Assert.AreEqual(NonDefaultDefaultableVal,
                        Defaultable.ThrowIfArgumentDefault(NonDefaultDefaultableVal, ParamName));
        Assert.ThrowsException<StructArgumentDefaultException>(
            () => Defaultable.ThrowIfArgumentDefault(DefaultDefaultableVal, ParamName));
        Assert.AreEqual(NonNullNullableVal, Defaultable.ThrowIfArgumentDefault(NonNullNullableVal, ParamName));
        Assert.ThrowsException<ArgumentNullException>(
            () => Defaultable.ThrowIfArgumentDefault(NullNullableVal, ParamName));
        Assert.AreEqual(NonDefaultNonDefaultableVal,
                        Defaultable.ThrowIfArgumentDefault(NonDefaultNonDefaultableVal, ParamName));
        Assert.AreEqual(DefaultNonDefaultableVal,
                        Defaultable.ThrowIfArgumentDefault(DefaultNonDefaultableVal, ParamName));

        Assert.AreEqual(ObjectRef, Defaultable.ThrowIfArgumentRefsDefault(ref ObjectRef, ParamName));
        Assert.ThrowsException<ArgumentNullException>(
            () => Defaultable.ThrowIfArgumentRefsDefault(ref NullRef, ParamName));
        Assert.AreEqual(NonDefaultDefaultableVal,
                        Defaultable.ThrowIfArgumentRefsDefault(ref NonDefaultDefaultableVal, ParamName));
        Assert.ThrowsException<StructArgumentDefaultException>(
            () => Defaultable.ThrowIfArgumentRefsDefault(ref DefaultDefaultableVal, ParamName));
        Assert.AreEqual(NonNullNullableVal, Defaultable.ThrowIfArgumentRefsDefault(ref NonNullNullableVal, ParamName));
        Assert.ThrowsException<ArgumentNullException>(
            () => Defaultable.ThrowIfArgumentRefsDefault(ref NullNullableVal, ParamName));
        Assert.AreEqual(NonDefaultNonDefaultableVal,
                        Defaultable.ThrowIfArgumentRefsDefault(ref NonDefaultNonDefaultableVal, ParamName));
        Assert.AreEqual(DefaultNonDefaultableVal,
                        Defaultable.ThrowIfArgumentRefsDefault(ref DefaultNonDefaultableVal, ParamName));
    }

    private readonly struct NonDefaultableStruct
    {
        public int Int { get; init; }
    }

    private readonly struct DefaultableStruct : IDefaultableStruct
    {
        [MemberNotNullWhen(false, nameof(Object))]
        public bool IsDefault => Object is null;
        public Class Object { get; init; }
    }

    public sealed class Class
    {
    }
}
