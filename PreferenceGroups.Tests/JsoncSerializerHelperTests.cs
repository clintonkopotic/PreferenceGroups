using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PreferenceGroups.Tests
{
    [TestClass]
    public sealed class JsoncSerializerHelperTests
    {
        [DataTestMethod]
        [DataRow(null, null, DisplayName = "null")]
        [DataRow("null", null, DisplayName = "\"null\"")]
        [DataRow("Null", null, DisplayName = "\"Null\"")]
        [DataRow("nulL", null, DisplayName = "\"nulL\"")]
        [DataRow("\" \"", " ", DisplayName = "\" \"")]
        [DataRow("\"\t\"", "\t", DisplayName = "\"\\t\"")]
        [DataRow("\"\\t\"", "\t", DisplayName = "\"\\\\t\"")]
        [DataRow("\"abc\"", "abc", DisplayName = "\"abc\"")]
        public void GetStringFromJsonStringValueTest(string? @string,
            string? expectedString)
        {
            Assert.AreEqual(expectedString, JsoncSerializerHelper
                .GetStringFromJsonStringValue(@string));
        }

        [DataTestMethod]
        [DataRow(null, "null", DisplayName = "null")]
        [DataRow("null", "\"null\"", DisplayName = "\"null\"")]
        [DataRow(" ", "\" \"", DisplayName = "\" \"")]
        [DataRow("\t", "\"\\t\"", DisplayName = "\"\\t\"")]
        [DataRow("\\t", "\"\\\\t\"", DisplayName = "\"\\\\t\"")]
        [DataRow("abc", "\"abc\"", DisplayName = "\"abc\"")]
        public void SerialzieStringValueTest(string? @string,
            string? expectedString)
        {
            Assert.AreEqual(expectedString, JsoncSerializerHelper
                .SerialzieStringValue(@string));
        }
    }
}
