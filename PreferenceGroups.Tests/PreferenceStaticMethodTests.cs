namespace PreferenceGroups.Tests;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class PreferenceStaticMethodTests
{
    [DataTestMethod]
    [DataRow(null, false, DisplayName = "null")]
    [DataRow("", false, DisplayName = "empty")]
    [DataRow("\t", false, DisplayName = "whitespace")]
    [DataRow("null", true, DisplayName = "\"null\"")]
    [DataRow("5", true, DisplayName = "\"5\"")]
    [DataRow("A", true, DisplayName = "\"A\"")]
    [DataRow("ABC", true, DisplayName = "\"ABC\"")]
    [DataRow(" ABC", true, DisplayName = "\" ABC\"")]
    [DataRow("ABC ", true, DisplayName = "\"ABC \"")]
    [DataRow(" ABC ", true, DisplayName = "\" ABC \"")]
    public void IsNameValidTests(string? name, bool expected)
    {
        Assert.AreEqual(expected, Preference.IsNameValid(name));
    }

    [DataTestMethod]
    [DataRow(false, null, true,
        DisplayName = "Only defined values and null AllowedValuesCount")]
    [DataRow(false, 0, true,
        DisplayName = "Only defined values and 0 AllowedValuesCount")]
    [DataRow(false, 1, false,
        DisplayName = "Only defined values and 1 AllowedValuesCount")]
    [DataRow(true, null, true,
        DisplayName = "Allow undefined values and null AllowedValuesCount")]
    [DataRow(true, 0, true,
        DisplayName = "Allow undefined values and 0 AllowedValuesCount")]
    [DataRow(true, 1, true,
        DisplayName = "Allow undefined values and 1 AllowedValuesCount")]
    public void ProcessAllowUndefinedValuesTest(bool allowUndefinedValuesIn,
        int? allowedValuesCount, bool expectedAllowUndefinedValuesOut)
        => Assert.AreEqual(expected: expectedAllowUndefinedValuesOut,
            actual: Preference.ProcessAllowUndefinedValues(
                allowUndefinedValuesIn, allowedValuesCount));

    [DataTestMethod]
    [DataRow(null, null, DisplayName = "null")]
    [DataRow("", null, DisplayName = "empty")]
    [DataRow("\t", null, DisplayName = "whitespace")]
    [DataRow("null", "null", DisplayName = "\"null\"")]
    [DataRow("5", "5", DisplayName = "\"5\"")]
    [DataRow("A", "A", DisplayName = "\"A\"")]
    [DataRow("ABC", "ABC", DisplayName = "\"ABC\"")]
    [DataRow(" ABC", "ABC", DisplayName = "\" ABC\"")]
    [DataRow("ABC ", "ABC", DisplayName = "\"ABC \"")]
    [DataRow(" ABC ", "ABC", DisplayName = "\" ABC \"")]
    public void ProcessNameThrowHelperTests(string? name, string expected)
    {
        if (expected is not null)
        {
            Assert.AreEqual(expected,
                Preference.ProcessNameOrThrowIfInvalid(name, nameof(name)));
        }
        else
        {
            if (name is null)
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    _ = Preference.ProcessNameOrThrowIfInvalid(name,
                        nameof(name));
                });
            }
            else
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    _ = Preference.ProcessNameOrThrowIfInvalid(name,
                        nameof(name));
                });
            }
        }
    }
}
