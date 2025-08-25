namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class Int32PreferenceTests
{
    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", null, DisplayName = "Null")]
    [DataRow("Testing0", 0, "Desc:Testing0", 0, DisplayName = "Zero")]
    [DataRow("Testing1", 1, "Desc:Testing1", 1, DisplayName = "One")]
    [DataRow("TestingMax", int.MaxValue, "Desc:TestingMax", null,
        DisplayName = "Max")]
    [DataRow("TestingMin", int.MinValue, "Desc:TestingMin", null,
        DisplayName = "Min")]
    public void SimpleBuildTest(string name, int? value,
        string? description, int? defaultValue)
    {
        Int32Preference int32Preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(defaultValue)
            .Build();

        Assert.IsFalse(int32Preference.IsEnum);
        Assert.IsFalse(int32Preference.HasEnumFlags);
        Assert.AreEqual(name, int32Preference.Name);
        Assert.AreEqual(value, int32Preference.Value);
        Assert.AreEqual(description, int32Preference.Description);
        Assert.AreEqual(defaultValue, int32Preference.DefaultValue);
        Assert.IsNull(int32Preference.AllowedValues);
        Assert.IsTrue(int32Preference.AllowUndefinedValues);

        Preference preference = int32Preference;

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(typeof(int?), preference.GetValueType());
        Assert.AreEqual(value is null, preference.ValueIsNull);
        Assert.AreEqual(value, (int?)preference.GetValueAsObject());
        Assert.AreEqual(value, preference.GetValueAs<int?>());
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(defaultValue is null,
            preference.DefaultValueIsNull);
        Assert.AreEqual(defaultValue,
            (int?)preference.GetDefaultValueAsObject());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference.SetValueFromObject(null);

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((int?)preference.GetValueAsObject());
        Assert.IsNull(preference.GetValueAs<int?>());

        preference.SetValueToNull();

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((int?)preference.GetValueAsObject());
        Assert.IsNull(preference.GetValueAs<int?>());

        preference.SetValueFromObject(preference.GetDefaultValueAsObject());

        Assert.AreEqual(defaultValue, (int?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<int?>());

        preference.SetValueToDefault();

        Assert.AreEqual(defaultValue, (int?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<int?>());

        if (value is not null)
        {
            var valueIncremented = value + 1;
            preference.SetValueFromObject(valueIncremented);
            Assert.AreEqual(valueIncremented,
                (int?)preference.GetValueAsObject());
            Assert.AreEqual(valueIncremented, preference.GetValueAs<int?>());

            valueIncremented += 1;
            preference.SetValueFromObject(valueIncremented);
            Assert.AreEqual(valueIncremented,
                (int?)preference.GetValueAsObject());
            Assert.AreEqual(valueIncremented, preference.GetValueAs<int?>());
        }

        int32Preference = Int32PreferenceBuilder.Build(name);

        Assert.IsFalse(int32Preference.IsEnum);
        Assert.IsFalse(int32Preference.HasEnumFlags);
        Assert.AreEqual(name, int32Preference.Name);
        Assert.IsNull(int32Preference.Value);
        Assert.IsNull(int32Preference.Description);
        Assert.IsNull(int32Preference.DefaultValue);
        Assert.IsNull(int32Preference.AllowedValues);
        Assert.IsTrue(int32Preference.AllowUndefinedValues);

        int32Preference = Int32PreferenceBuilder.Build(name, value);

        Assert.IsFalse(int32Preference.IsEnum);
        Assert.IsFalse(int32Preference.HasEnumFlags);
        Assert.AreEqual(name, int32Preference.Name);
        Assert.AreEqual(value, int32Preference.Value);
        Assert.IsNull(int32Preference.Description);
        Assert.IsNull(int32Preference.DefaultValue);
        Assert.IsNull(int32Preference.AllowedValues);
        Assert.IsTrue(int32Preference.AllowUndefinedValues);

        int32Preference.SetValueToDefault();

        Assert.IsNull(int32Preference.Value);
    }

    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", DisplayName = "Null")]
    [DataRow("Testing0", 0, "Desc:Testing0", DisplayName = "Zero")]
    [DataRow("Testing1", 1, "Desc:Testing1", DisplayName = "One")]
    [DataRow("TestingMax", int.MaxValue, "Desc:TestingMax",
        DisplayName = "Max")]
    [DataRow("TestingMin", int.MinValue, "Desc:TestingMin",
        DisplayName = "Min")]
    public void SameValueAndDefaultBuildTest(string name, int? value,
        string? description)
    {
        Int32Preference preference = Int32PreferenceBuilder
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

        preference = Int32PreferenceBuilder
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
        Assert.IsNull(StructPreference<int>.ProcessAllowedValues(null,
            sortAllowedValues: false));
        Assert.IsNull(StructPreference<int>.ProcessAllowedValues(null,
            sortAllowedValues: true));

        // When allowedValues is empty.
        IReadOnlyCollection<int?>? allowedValuesOut;
        allowedValuesOut = StructPreference<int>.ProcessAllowedValues([],
            sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<int?>>(allowedValuesOut);

        allowedValuesOut = StructPreference<int>.ProcessAllowedValues([],
            sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<int?>>(allowedValuesOut);

        // When allowedValues is a simple collection.
        IReadOnlyCollection<int?>? allowedValues = [3, 2, 1];
        IReadOnlyCollection<int?>? sortedAllowedValues = [1, 2, 3];
        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(allowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(allowedValues.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains repeats.
        allowedValues = [3, 2, 1, 2];
        IReadOnlyCollection<int?>? expectedAllowedValuesOut
            = [3, 2, 1];
        sortedAllowedValues = [1, 2, 3];
        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains null.
        allowedValues = [3, 2, null, 1];
        expectedAllowedValuesOut = [3, 2, 1];
        sortedAllowedValues = [1, 2, 3];
        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains nulls and repeats.
        allowedValues = [4, 6, null, 10, 8, null, 1, 2, 7, null, 3, 5, 9,
            null];
        expectedAllowedValuesOut = [4, 6, 10, 8, 1, 2, 7, 3, 5, 9];
        sortedAllowedValues = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<int>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<int?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());
    }

    [TestMethod]
    public void AllowedValuesTests()
    {
        // Test if no allowed values that the AllowUndefinedValues
        // properties is forced to true.

        var name = "name";
        Int32Preference preference = Int32PreferenceBuilder
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

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        // Test if there are AllowedValues and AllowUndefinedValues is true.
        int[] allowedValues = [3, 2, 1];
        int[] sortedAllowedValues = [1, 2, 3];
        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        // Test if there are AllowedValues and AllowUndefinedValues is
        // false.
        var exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = Int32PreferenceBuilder
                .Create(name)
                .WithValue(0) // Zero is not an allowed value.
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

        // Test if there are AllowedValues and AllowUndefinedValues is
        // false.
        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(2)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues((IEnumerable<int>)allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(2, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort(allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort((IEnumerable<int>)allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .SortAllowedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(
                (IEnumerable<int>)allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(0, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(0)
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
        Assert.AreEqual(0, preference.Value);
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
        Int32Preference preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(1)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor(
                Int32ValueValidityProcessor.IsGreaterThanZero)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(1, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = Int32PreferenceBuilder
            .Create(name)
            .WithValue(2)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor((StructValueValidityProcessor<int>)
                Int32ValueValidityProcessor.IsGreaterThanZero)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(2, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            preference = Int32PreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(null) // Cannot be null.
                .Build();
        });

        var exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = Int32PreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    Int32ValueValidityProcessor.IsGreaterThanZero)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = Int32PreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    Int32ValueValidityProcessor.IsGreaterThan(null))
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentNullException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = Int32PreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    Int32ValueValidityProcessor.IsGreaterThan(5))
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());
    }
}
