using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PreferenceGroups.Tests
{
    [TestClass]
    public class PreferenceBuilderHelperTests
    {
        [TestMethod]
        public void ProcessAllowedValuesTest_NullAllowedValues()
        {
            string?[]? allowedValuesIn = null;

            var allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: false);

            Assert.IsNull(allowedValuesOut);

            allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: true);

            Assert.IsNull(allowedValuesOut);
        }

        [TestMethod]
        public void ProcessAllowedValuesTest_EmptyAllowedValues()
        {
            string?[]? allowedValuesIn = [];
            string?[]? expectedAllowedValuesOut = [];
            string?[]? expectedSortedAllowedValuesOut = [];
            var allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: false);

            Assert.IsNotNull(allowedValuesOut);
            var unsortedAllowedValues = allowedValuesOut as HashSet<string>;
            Assert.IsNotNull(unsortedAllowedValues);
            Assert.AreEqual(expectedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedAllowedValuesOut,
                unsortedAllowedValues.ToArray());

            allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: true);

            Assert.IsNotNull(allowedValuesOut);
            var sortedAllowedValues = allowedValuesOut as SortedSet<string>;
            Assert.IsNotNull(sortedAllowedValues);
            Assert.AreEqual(expectedSortedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedSortedAllowedValuesOut,
                sortedAllowedValues);
        }

        [TestMethod]
        public void ProcessAllowedValuesTest_SomeAllowedValues()
        {
            string?[]? allowedValuesIn = ["def", "abc"];
            string?[]? expectedAllowedValuesOut = ["def", "abc"];
            string?[]? expectedSortedAllowedValuesOut = ["abc", "def"];
            var allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: false);

            Assert.IsNotNull(allowedValuesOut);
            var unsortedAllowedValues = allowedValuesOut as HashSet<string>;
            Assert.IsNotNull(unsortedAllowedValues);
            Assert.AreEqual(expectedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedAllowedValuesOut,
                unsortedAllowedValues.ToArray());

            allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: true);

            Assert.IsNotNull(allowedValuesOut);
            var sortedAllowedValues = allowedValuesOut as SortedSet<string>;
            Assert.IsNotNull(sortedAllowedValues);
            Assert.AreEqual(expectedSortedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedSortedAllowedValuesOut,
                sortedAllowedValues);
        }

        [TestMethod]
        public void ProcessAllowedValuesTest_SomeWithNullAllowedValues()
        {
            string?[]? allowedValuesIn = ["def", null, "abc"];
            string?[]? expectedAllowedValuesOut = ["def", "abc"];
            string?[]? expectedSortedAllowedValuesOut = ["abc", "def"];
            var allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: false);

            Assert.IsNotNull(allowedValuesOut);
            var unsortedAllowedValues = allowedValuesOut as HashSet<string>;
            Assert.IsNotNull(unsortedAllowedValues);
            Assert.AreEqual(expectedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedAllowedValuesOut,
                unsortedAllowedValues.ToArray());

            allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: true);

            Assert.IsNotNull(allowedValuesOut);
            var sortedAllowedValues = allowedValuesOut as SortedSet<string>;
            Assert.IsNotNull(sortedAllowedValues);
            Assert.AreEqual(expectedSortedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedSortedAllowedValuesOut,
                sortedAllowedValues);
        }

        [TestMethod]
        public void ProcessAllowedValuesTest_SomeWithRepeatsAllowedValues()
        {
            string?[]? allowedValuesIn = ["def", "abc", "abc"];
            string?[]? expectedAllowedValuesOut = ["def", "abc"];
            string?[]? expectedSortedAllowedValuesOut = ["abc", "def"];
            var allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: false);

            Assert.IsNotNull(allowedValuesOut);
            var unsortedAllowedValues = allowedValuesOut as HashSet<string>;
            Assert.IsNotNull(unsortedAllowedValues);
            Assert.AreEqual(expectedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedAllowedValuesOut,
                unsortedAllowedValues.ToArray());

            allowedValuesOut = PreferenceBuilderHelper
                .ProcessAllowedValuesForClass(
                    allowedValuesIn: allowedValuesIn,
                    sortAllowedValues: true);

            Assert.IsNotNull(allowedValuesOut);
            var sortedAllowedValues = allowedValuesOut as SortedSet<string>;
            Assert.IsNotNull(sortedAllowedValues);
            Assert.AreEqual(expectedSortedAllowedValuesOut.Length,
                unsortedAllowedValues.Count);
            CollectionAssert.AreEqual(expectedSortedAllowedValuesOut,
                sortedAllowedValues);
        }
    }
}
