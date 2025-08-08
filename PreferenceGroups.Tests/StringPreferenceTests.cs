namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class StringPreferenceTests
{
    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", null, DisplayName = "Null")]
    [DataRow("Testing0", "0", "Desc:Testing0", "0", DisplayName = "Zero")]
    [DataRow("Testing1", "1", "Desc:Testing1", "1", DisplayName = "One")]
    [DataRow("TestingMax", "int.MaxValue", "Desc:TestingMax", null,
        DisplayName = "Max")]
    [DataRow("TestingMin", "int.MinValue", "Desc:TestingMin", null,
        DisplayName = "Min")]
    public void SimpleBuildTest(string name, string? value, string? description,
        string? defaultValue)
    {
        StringPreference stringPreference = StringPreferenceBuilder
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(defaultValue)
            .Build();

        Assert.IsFalse(stringPreference.IsEnum);
        Assert.IsFalse(stringPreference.HasEnumFlags);
        Assert.AreEqual(name, stringPreference.Name);
        Assert.AreEqual(value, stringPreference.Value);
        Assert.AreEqual(description, stringPreference.Description);
        Assert.AreEqual(defaultValue, stringPreference.DefaultValue);
        Assert.IsNull(stringPreference.AllowedValues);
        Assert.IsTrue(stringPreference.AllowUndefinedValues);

        Preference preference = stringPreference;

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(typeof(string), preference.GetValueType());
        Assert.AreEqual(value is null, preference.ValueIsNull);
        Assert.AreEqual(value, (string?)preference.GetValueAsObject());
        Assert.AreEqual(value, preference.GetValueAs<string?>());
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(defaultValue is null,
            preference.DefaultValueIsNull);
        Assert.AreEqual(defaultValue,
            (string?)preference.GetDefaultValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetDefaultValueAs<string?>());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference.SetValueFromObject(null);

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((string?)preference.GetValueAsObject());
        Assert.IsNull(preference.GetValueAs<string?>());

        preference.SetValueFromObject(preference.GetDefaultValueAsObject());

        Assert.AreEqual(defaultValue, (string?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<string?>());

        if (value is not null)
        {
            var valueIncremented = value + 1;
            preference.SetValueFromObject(valueIncremented);
            Assert.AreEqual(valueIncremented,
                (string?)preference.GetValueAsObject());
            Assert.AreEqual(valueIncremented, preference.GetValueAs<string?>());

            valueIncremented += 1;
            preference.SetValueFromObject(valueIncremented);
            Assert.AreEqual(valueIncremented,
                (string?)preference.GetValueAsObject());
            Assert.AreEqual(valueIncremented, preference.GetValueAs<string?>());
        }

        stringPreference = StringPreferenceBuilder.Build(name);

        Assert.IsFalse(stringPreference.IsEnum);
        Assert.IsFalse(stringPreference.HasEnumFlags);
        Assert.AreEqual(name, stringPreference.Name);
        Assert.IsNull(stringPreference.Value);
        Assert.IsNull(stringPreference.Description);
        Assert.IsNull(stringPreference.DefaultValue);
        Assert.IsNull(stringPreference.AllowedValues);
        Assert.IsTrue(stringPreference.AllowUndefinedValues);

        stringPreference = StringPreferenceBuilder.Build(name, value);

        Assert.IsFalse(stringPreference.IsEnum);
        Assert.IsFalse(stringPreference.HasEnumFlags);
        Assert.AreEqual(name, stringPreference.Name);
        Assert.AreEqual(value, stringPreference.Value);
        Assert.IsNull(stringPreference.Description);
        Assert.IsNull(stringPreference.DefaultValue);
        Assert.IsNull(stringPreference.AllowedValues);
        Assert.IsTrue(stringPreference.AllowUndefinedValues);
    }

    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", DisplayName = "Null")]
    [DataRow("Testing0", "0", "Desc:Testing0", DisplayName = "Zero")]
    [DataRow("Testing1", "1", "Desc:Testing1", DisplayName = "One")]
    [DataRow("TestingMax", "int.MaxValue", "Desc:TestingMax",
        DisplayName = "Max")]
    [DataRow("TestingMin", "int.MinValue", "Desc:TestingMin",
        DisplayName = "Min")]
    public void SameValueAndDefaultBuildTest(string name, string? value,
        string? description)
    {
        StringPreference preference = StringPreferenceBuilder
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(value)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(value, preference.Value);
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(value, preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValueAndAsDefault(value)
            .WithDescription(description)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(value, preference.Value);
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(value, preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);
    }

    [TestMethod]
    public void ProcessAllowedValuesTests()
    {
        // When allowedValues is null.
        Assert.IsNull(ClassPreference<string?>.ProcessAllowedValues(null,
            sortAllowedValues: false));
        Assert.IsNull(ClassPreference<string?>.ProcessAllowedValues(null,
            sortAllowedValues: true));

        // When allowedValues is empty.
        IReadOnlyCollection<string?>? allowedValuesOut;
        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues([],
            sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<string?>>(allowedValuesOut);

        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues([],
            sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<string?>>(allowedValuesOut);

        // When allowedValues is a simple collection.
        IReadOnlyCollection<string?>? allowedValues = ["3", "2", "1"];
        IReadOnlyCollection<string?>? sortedAllowedValues = ["1", "2", "3"];
        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(allowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(allowedValues.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains repeats.
        allowedValues = ["3", "2", "1", "2"];
        IReadOnlyCollection<string?>? expectedAllowedValuesOut
            = ["3", "2", "1"];
        sortedAllowedValues = ["1", "2", "3"];
        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains null.
        allowedValues = ["3", "2", null, "1"];
        expectedAllowedValuesOut = ["3", "2", "1"];
        sortedAllowedValues = ["1", "2", "3"];
        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains nulls and repeats.
        allowedValues = ["4", "6", null, "10", "8", null, "1", "2", "7",
            null, "3", "5", "9", null];
        expectedAllowedValuesOut = ["4", "6", "10", "8", "1", "2", "7", "3",
            "5", "9"];
        sortedAllowedValues = ["1", "10", "2", "3", "4", "5", "6", "7", "8",
            "9"];
        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = ClassPreference<string?>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<string?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());
    }

    [TestMethod]
    public void AllowedValuesTests()
    {
        // Test if no allowed values that the AllowUndefinedValues properties
        // is forced to true.

        var name = "name";
        StringPreference preference = StringPreferenceBuilder
            .Create(name)
            .WithValue(null)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithNoAllowedValues()
            .AllowOnlyDefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNull(preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        // Test if there are AllowedValues and AllowUndefinedValues is true.
        string[] allowedValues = ["3", "2", "1"];
        string[] sortedAllowedValues = ["1", "2", "3"];
        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        // Test if there are AllowedValues and AllowUndefinedValues is false.
        var exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = StringPreferenceBuilder
                .Create(name)
                .WithValue("0") // Zero is not an allowed value.
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValues(allowedValues)
                .AllowOnlyDefinedValues()
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(InvalidOperationException),
            exception.InnerException.GetType());

        // Test if there are AllowedValues and AllowUndefinedValues is false.
        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("2")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues((IEnumerable<string>)allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("2", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort(allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort((IEnumerable<string>)allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .SortAllowedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort((IEnumerable<string>)allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("0")
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .DoNotSortAllowedValues()
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("0", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);
    }

    [TestMethod]
    public void IsValidTests()
    {
        var name = "name";
        StringPreference preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("1")
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor(StringValueValidityProcessor.PreTrimIfNotNull)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("1", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = StringPreferenceBuilder
            .Create(name)
            .WithValue("2")
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor((ClassValueValidityProcessor<string>)
                StringValueValidityProcessor.EnsureNotNullOrWhiteSpaceAndPostTrim)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual("2", preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            preference = StringPreferenceBuilder
                .Create(name)
                .WithValue(" ")
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(null) // Cannot be null.
                .Build();
        });

        var exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = StringPreferenceBuilder
                .Create(name)
                .WithValue(" ")
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(StringValueValidityProcessor
                    .EnsureNotNullOrWhiteSpaceAndPostTrim)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());
    }
}
