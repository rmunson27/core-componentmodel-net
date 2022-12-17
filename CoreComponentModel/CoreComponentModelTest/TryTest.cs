using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.ComponentModel;

/// <summary>
/// Tests of the <see cref="Try"/> class functionality.
/// </summary>
[TestClass]
public class TryTest
{
    /// <summary>
    /// Tests the set-return combo methods of the <see cref="Try"/> class that can be used to implement standard
    /// "Try" methods.
    /// </summary>
    [TestMethod]
    public void TestSetReturnCombo()
    {
        var wrappedArray = new[] { 0, 1, 2, 3 };
        var wrappedString = "A String";
        ArrayOrString array = new(wrappedArray);
        ArrayOrString str = new(wrappedString);

#pragma warning disable IDE0018 // Cleaner to declare variables here for symmetry of the assertions
        int[]? outArray;
        string? outString;
#pragma warning restore IDE0018

        Assert.IsTrue(array.TryGetArray(out outArray));
        Assert.AreEqual(wrappedArray, outArray);

        Assert.IsTrue(array.TryGetArray(out outArray, out outString));
        Assert.AreEqual(wrappedArray, outArray);
        Assert.IsNull(outString);

        Assert.IsFalse(str.TryGetArray(out outArray));
        Assert.IsNull(outArray);

        Assert.IsFalse(str.TryGetArray(out outArray, out outString));
        Assert.IsNull(outArray);
        Assert.AreEqual(wrappedString, outString);
    }

    /// <summary>
    /// Used for testing the <see cref="Try"/> class methods.
    /// </summary>
    private sealed class ArrayOrString
    {
        public int[]? Array { get; }
        public string? String { get; }

        public ArrayOrString(int[] Array)
        {
            this.Array = Array ?? throw new ArgumentNullException(nameof(Array));
        }

        public ArrayOrString(string String)
        {
            this.String = String ?? throw new ArgumentNullException(nameof(String));
        }

        public bool TryGetArray([MaybeNullWhen(false)] out int[] Array)
            => this.Array is null ? Try.Failure(out Array) : Try.Success(out Array, this.Array);

        public bool TryGetArray([MaybeNullWhen(true)] out int[] Array, [MaybeNullWhen(false)] out string String)
            => this.Array is null
                ? Try.Failure(out Array, out String, this.String)
                : Try.Success(out Array, this.Array, out String);
    }
}
