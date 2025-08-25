using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

namespace PreferenceGroups.Tests
{
    [TestClass]
    public sealed class EnumHelpersTests
    {
        [DataTestMethod]
        public void IsDefinedTest()
        {
            Assert.IsTrue(EnumHelpers.IsDefined(SingleDay.None));
            Assert.IsTrue(EnumHelpers.IsDefined(SingleDay.Sunday));
            Assert.IsTrue(EnumHelpers.IsDefined(MultiDay.None));
            Assert.IsFalse(EnumHelpers.IsDefined((NoZeroEnum)0));
            Assert.IsFalse(EnumHelpers.IsDefined(
                (SingleDay)Enum.ToObject(typeof(SingleDay), int.MinValue)));
            Assert.IsFalse(EnumHelpers.IsDefined(
                (SingleDay)Enum.ToObject(typeof(SingleDay), int.MaxValue)));
            Assert.IsFalse(EnumHelpers.IsDefined(
                (SingleDay)Enum.ToObject(typeof(MultiDay), int.MinValue)));
            Assert.IsFalse(EnumHelpers.IsDefined(
                (SingleDay)Enum.ToObject(typeof(MultiDay), int.MaxValue)));
        }
    }
}
