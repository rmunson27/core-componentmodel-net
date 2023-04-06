using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.ComponentModel;

/// <summary>
/// Tests the <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> class functionality.
/// </summary>
[TestClass]
public class NestedEqualityComparerTest
{
    private static readonly ParityEqualityComparer<int> IntParityEqualityComparer = new();

    /// <summary>
    /// Tests the <see cref="NestedEqualityComparer{TGeneric, TParameter}.Default"/> property.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        ReadOnlyCollection<int> low = new(new[] { 2, 4, 6 }),
                                high = new(new[] { 4, 8, 12 });

        EquatableTestCollection<int> lowEquatable = new() { Elements = low },
                                     highEquatable = new() { Elements = high };
        NestedEquatableTestCollection<int> lowNestedEquatable = new() { Elements = low },
                                           highNestedEquatable = new() { Elements = high };

        var nestedEquatableDefault = NestedEqualityComparer<NestedEquatableTestCollection<int>, int>.Default;
        var equatableDefault = NestedEqualityComparer<EquatableTestCollection<int>, int>.Default;

        // Control:
        // Should use the default equality comparer for ints, ignoring the specified comparer, determining that the
        // objects are equal
        Assert.IsTrue(equatableDefault.Equals(lowEquatable, lowEquatable, IntParityEqualityComparer));

        // Should ignore the nested equality comparer since the type isn't nested equatable
        Assert.IsFalse(equatableDefault.Equals(lowEquatable, highEquatable, IntParityEqualityComparer));

        // Control:
        // Will use the supplied equality comparer, but all instances are really equal so their parities will be
        // as well
        Assert.IsTrue(
            nestedEquatableDefault.Equals(lowNestedEquatable, lowNestedEquatable, IntParityEqualityComparer));

        // Should take the supplied equality comparer into account, comparing the parities of the elements and
        // determining that the two instances are equal
        Assert.IsTrue(
            nestedEquatableDefault.Equals(lowNestedEquatable, highNestedEquatable, IntParityEqualityComparer));
    }

    private sealed class ParityEqualityComparer<TNumber> : IEqualityComparer<TNumber> where TNumber : INumber<TNumber>
    {
        private static readonly TNumber Two = TNumber.One + TNumber.One;

        public bool Equals(TNumber? x, TNumber? y) => x is null ? y is null : y is not null && Parity(x) == Parity(y);

        public int GetHashCode([DisallowNull] TNumber obj) => Parity(obj).GetHashCode();

        private static TNumber Parity(TNumber x) => TNumber.Min(x % Two, TNumber.Zero); // To collapse -1 to 1
    }

    private sealed class EquatableTestCollection<TElement>
        : TestCollection<TElement>, IEquatable<EquatableTestCollection<TElement>>
    {
        public override bool Equals(object? obj) => obj is EquatableTestCollection<TElement> other && Equals(other);

        public bool Equals(EquatableTestCollection<TElement>? other)
            => other is not null
                && Elements.SequenceEqual(other.Elements);

        public override int GetHashCode()
        {
            if (Elements is null) throw new NullReferenceException($"{nameof(Elements)} property was null.");

            HashCode hashCode = new();
            foreach (var element in Elements)
            {
                hashCode.Add(element);
            }
            return hashCode.ToHashCode();
        }
    }

    private sealed class NestedEquatableTestCollection<TElement>
        : TestCollection<TElement>, INestedEquatable<NestedEquatableTestCollection<TElement>, TElement>
    {
        public override bool Equals(object? obj)
            => obj is NestedEquatableTestCollection<TElement> other && Equals(other);

        public bool Equals(NestedEquatableTestCollection<TElement>? other) => Equals(other, null);

        public bool Equals(NestedEquatableTestCollection<TElement>? other, IEqualityComparer<TElement>? nestedComparer)
            => other is not null
                && Elements.SequenceEqual(other.Elements, nestedComparer);

        public override int GetHashCode() => GetHashCode(null);

        public int GetHashCode(IEqualityComparer<TElement>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();

            HashCode hashCode = new();
            foreach (var element in Elements)
            {
                hashCode.Add(nestedComparer.GetHashCode(element!));
            }
            return hashCode.ToHashCode();
        }
    }

    private abstract class TestCollection<TElement>
    {
        public required ReadOnlyCollection<TElement> Elements { get; init; }
    }
}
