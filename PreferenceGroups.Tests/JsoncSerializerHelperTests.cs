namespace PreferenceGroups.Tests;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    [TestMethod]
    public void GetTabStringExceptionTest()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            JsoncSerializerHelper.GetTabString(' ', -1);
        });
    }

    [DataTestMethod]
    [DataRow(' ', 0, "", DisplayName = "Space and zero indentDepth")]
    [DataRow(' ', 1, " ", DisplayName = "Space and one indentDepth")]
    [DataRow(' ', 4, "    ", DisplayName = "Space and four indentDepth")]
    [DataRow('a', 3, "aaa", DisplayName = "\'a\' and three indentDepth")]
    [DataRow('\t', 1, "\t", DisplayName = "Tab and one indentDepth")]
    public void GetTabStringTest(char indentChar, int indentDepth,
        string expectedTabString)
    {
        Assert.AreEqual(expectedTabString, JsoncSerializerHelper.GetTabString(
            indentChar, indentDepth));
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
