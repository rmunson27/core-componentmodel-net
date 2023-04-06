using System;
using Rem.Core.ComponentModel.Logic;

namespace Rem.CoreTest.ComponentModel.Logic;

/// <summary>
/// Tests of the <see cref="Ternary"/> struct.
/// </summary>
[TestClass]
public class TernaryTest
{
    /// <summary>
    /// The <see cref="Ternary.Unknown"/> value.
    /// </summary>
    private static readonly Ternary Unknown = Ternary.Unknown;

    /// <summary>
    /// Tests the conjunction (&) operator.
    /// </summary>
    [TestMethod]
    public void TestAnd()
    {
        var table = new TruthTable
        {
            True = new(True: true, False: false, Unknown: Unknown),
            False = new(True: false, False: false, Unknown: false),
            Unknown = new(True: Unknown, False: false, Unknown: Unknown),
        };

        foreach (var left in Ternary.All)
        { 
            foreach (var right in Ternary.All)
            {
                Assert.AreEqual(table[left][right], left & right, $"{left} & {right}");
            }
        }
    }

    /// <summary>
    /// Tests the disjunction (|) operator.
    /// </summary>
    [TestMethod]
    public void TestOr()
    {
        var table = new TruthTable
        {
            True = new(True: true, False: true, Unknown: true),
            False = new(True: true, False: false, Unknown: Unknown),
            Unknown = new(True: true, False: Unknown, Unknown: Unknown),
        };

        foreach (var left in Ternary.All)
        { 
            foreach (var right in Ternary.All)
            {
                Assert.AreEqual(table[left][right], left | right, $"{left} | {right}");
            }
        }
    }

    /// <summary>
    /// Tests the exclusive disjunction (^) operator.
    /// </summary>
    [TestMethod]
    public void TestXOr()
    {
        var table = new TruthTable
        {
            True = new(True: false, False: true, Unknown: Unknown),
            False = new(True: true, False: false, Unknown: Unknown),
            Unknown = new(True: Unknown, False: Unknown, Unknown: Unknown),
        };

        foreach (var left in Ternary.All)
        { 
            foreach (var right in Ternary.All)
            {
                Assert.AreEqual(table[left][right], left ^ right, $"{left} ^ {right}");
            }
        }
    }

    /// <summary>
    /// Represents a truth table.
    /// </summary>
    private sealed class TruthTable
    { 
        /// <summary>
        /// Gets the row of the truth table for <see cref="Ternary.True"/> on the left-hand side.
        /// </summary>
        public Row True { get; init; }

        /// <summary>
        /// Gets the row of the truth table for <see cref="Ternary.False"/> on the left-hand side.
        /// </summary>
        public Row False { get; init; }

        /// <summary>
        /// Gets the row of the truth table for <see cref="Ternary.Unknown"/> on the left-hand side.
        /// </summary>
        public Row Unknown { get; init; }

        /// <summary>
        /// Gets the row representing the result of applying the operation under test to <paramref name="left"/> and
        /// other values of <see cref="Ternary"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public Row this[Ternary left] => left.Value switch
        { 
            Ternary.Values.True => True,
            Ternary.Values.False => False,
            _ => Unknown,
        };

        /// <summary>
        /// Represents a row of a truth table.
        /// </summary>
        public readonly struct Row
        {
            /// <summary>
            /// Gets the expected result of applying the operator under test to the value under test
            /// and <see cref="Ternary.True"/>.
            /// </summary>
            public Ternary True { get; }

            /// <summary>
            /// Gets the expected result of applying the operator under test to the value under test
            /// and <see cref="Ternary.False"/>.
            /// </summary>
            public Ternary False { get; }

            /// <summary>
            /// Gets the expected result of applying the operator under test to the value under test
            /// and <see cref="Ternary.Unknown"/>.
            /// </summary>
            public Ternary Unknown { get; }

            /// <summary>
            /// Gets the result of the operator under test applied to the <see cref="Ternary"/> this row represents
            /// results for and <paramref name="right"/>.
            /// </summary>
            /// <param name="right"></param>
            /// <returns></returns>
            public Ternary this[Ternary right] => right.Value switch
            {
                Ternary.Values.False => False,
                Ternary.Values.True => True,
                _ => Unknown,
            };

            /// <summary>
            /// Constructs a row of the truth table with the expected results.
            /// </summary>
            /// <param name="True"></param>
            /// <param name="False"></param>
            /// <param name="Unknown"></param>
            public Row(Ternary True, Ternary False, Ternary Unknown)
            {
                this.True = True;
                this.False = False;
                this.Unknown = Unknown;
            }
        }
    }
}
