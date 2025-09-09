namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class DoublePreferenceTests
{
    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", null, DisplayName = "Null")]
    [DataRow("Testing0", 0d, "Desc:Testing0", 0d, DisplayName = "Zero")]
    [DataRow("Testing1", 1d, "Desc:Testing1", 1d, DisplayName = "One")]
    [DataRow("TestingMax", double.MaxValue, "Desc:TestingMax", null,
        DisplayName = "Max")]
    [DataRow("TestingMin", double.MinValue, "Desc:TestingMin", null,
        DisplayName = "Min")]
    [DataRow("TestingEpsilon", double.Epsilon, "Desc:TestingEpsilon", null,
        DisplayName = "Epsilon")]
    [DataRow("Testing-∞", double.NegativeInfinity, "Desc:Testing-∞", null,
        DisplayName = "-∞")]
    [DataRow("Testing+∞", double.PositiveInfinity, "Desc:Testing+∞", null,
        DisplayName = "+∞")]
    public void SimpleBuildTest(string name, double? value,
        string? description, double? defaultValue)
    {
        DoublePreference doublePreference = DoublePreferenceBuilder
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(defaultValue)
            .Build();

        Assert.IsFalse(doublePreference.IsEnum);
        Assert.IsFalse(doublePreference.HasEnumFlags);
        Assert.AreEqual(name, doublePreference.Name);
        Assert.AreEqual(value, doublePreference.Value);
        Assert.AreEqual(description, doublePreference.Description);
        Assert.AreEqual(defaultValue, doublePreference.DefaultValue);
        Assert.IsNull(doublePreference.AllowedValues);
        Assert.IsTrue(doublePreference.AllowUndefinedValues);

        Preference preference = doublePreference;

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(typeof(double?), preference.GetValueType());
        Assert.AreEqual(value is null, preference.ValueIsNull);
        Assert.AreEqual(value, (double?)preference.GetValueAsObject());
        Assert.AreEqual(value, preference.GetValueAs<double?>());
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(defaultValue is null,
            preference.DefaultValueIsNull);
        Assert.AreEqual(defaultValue,
            (double?)preference.GetDefaultValueAsObject());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference.SetValueFromObject(null);

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((double?)preference.GetValueAsObject());
        Assert.IsNull(preference.GetValueAs<double?>());

        preference.SetValueToNull();

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((double?)preference.GetValueAsObject());
        Assert.IsNull(preference.GetValueAs<double?>());

        preference.SetValueFromObject(preference.GetDefaultValueAsObject());

        Assert.AreEqual(defaultValue, (double?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<double?>());

        preference.SetValueToDefault();

        Assert.AreEqual(defaultValue, (double?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<double?>());

        if (value is not null)
        {
            var valueIncremented = value + 1d;
            preference.SetValueFromObject(valueIncremented);
            Assert.AreEqual(valueIncremented,
                (double?)preference.GetValueAsObject());
            Assert.AreEqual(valueIncremented, preference.GetValueAs<double?>());

            valueIncremented += 1;
            preference.SetValueFromObject(valueIncremented);
            Assert.AreEqual(valueIncremented,
                (double?)preference.GetValueAsObject());
            Assert.AreEqual(valueIncremented, preference.GetValueAs<double?>());
        }

        doublePreference = DoublePreferenceBuilder.Build(name);

        Assert.IsFalse(doublePreference.IsEnum);
        Assert.IsFalse(doublePreference.HasEnumFlags);
        Assert.AreEqual(name, doublePreference.Name);
        Assert.IsNull(doublePreference.Value);
        Assert.IsNull(doublePreference.Description);
        Assert.IsNull(doublePreference.DefaultValue);
        Assert.IsNull(doublePreference.AllowedValues);
        Assert.IsTrue(doublePreference.AllowUndefinedValues);

        doublePreference = DoublePreferenceBuilder.Build(name, value);

        Assert.IsFalse(doublePreference.IsEnum);
        Assert.IsFalse(doublePreference.HasEnumFlags);
        Assert.AreEqual(name, doublePreference.Name);
        Assert.AreEqual(value, doublePreference.Value);
        Assert.IsNull(doublePreference.Description);
        Assert.IsNull(doublePreference.DefaultValue);
        Assert.IsNull(doublePreference.AllowedValues);
        Assert.IsTrue(doublePreference.AllowUndefinedValues);

        doublePreference.SetValueToDefault();

        Assert.IsNull(doublePreference.Value);
    }

    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", DisplayName = "Null")]
    [DataRow("Testing0", 0d, "Desc:Testing0", DisplayName = "Zero")]
    [DataRow("Testing1", 1d, "Desc:Testing1", DisplayName = "One")]
    [DataRow("TestingMax", double.MaxValue, "Desc:TestingMax",
        DisplayName = "Max")]
    [DataRow("TestingMin", double.MinValue, "Desc:TestingMin",
        DisplayName = "Min")]
    public void SameValueAndDefaultBuildTest(string name, double? value,
        string? description)
    {
        DoublePreference preference = DoublePreferenceBuilder
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

        preference = DoublePreferenceBuilder
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
        Assert.IsNull(StructPreference<double>.ProcessAllowedValues(null,
            sortAllowedValues: false));
        Assert.IsNull(StructPreference<double>.ProcessAllowedValues(null,
            sortAllowedValues: true));

        // When allowedValues is empty.
        IReadOnlyCollection<double?>? allowedValuesOut;
        allowedValuesOut = StructPreference<double>.ProcessAllowedValues([],
            sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<double?>>(allowedValuesOut);

        allowedValuesOut = StructPreference<double>.ProcessAllowedValues([],
            sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<double?>>(allowedValuesOut);

        // When allowedValues is a simple collection.
        IReadOnlyCollection<double?>? allowedValues = [3, 2, 1];
        IReadOnlyCollection<double?>? sortedAllowedValues = [1, 2, 3];
        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(allowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(allowedValues.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains repeats.
        allowedValues = [3, 2, 1, 2];
        IReadOnlyCollection<double?>? expectedAllowedValuesOut
            = [3, 2, 1];
        sortedAllowedValues = [1, 2, 3];
        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains null.
        allowedValues = [3, 2, null, 1];
        expectedAllowedValuesOut = [3, 2, 1];
        sortedAllowedValues = [1, 2, 3];
        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains nulls and repeats.
        allowedValues = [4, 6, null, 10, 8, null, 1, 2, 7, null, 3, 5, 9,
            null];
        expectedAllowedValuesOut = [4, 6, 10, 8, 1, 2, 7, 3, 5, 9];
        sortedAllowedValues = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<double>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<double?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());
    }

    [TestMethod]
    public void AllowedValuesTests()
    {
        // Test if no allowed values that the AllowUndefinedValues
        // properties is forced to true.

        var name = "name";
        DoublePreference preference = DoublePreferenceBuilder
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

        preference = DoublePreferenceBuilder
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
        double[] allowedValues = [3, 2, 1];
        double[] sortedAllowedValues = [1, 2, 3];
        preference = DoublePreferenceBuilder
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
            preference = DoublePreferenceBuilder
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
        preference = DoublePreferenceBuilder
            .Create(name)
            .WithValue(2)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues((IEnumerable<double>)allowedValues)
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

        preference = DoublePreferenceBuilder
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

        preference = DoublePreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort((IEnumerable<double>)allowedValues)
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

        preference = DoublePreferenceBuilder
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

        preference = DoublePreferenceBuilder
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

        preference = DoublePreferenceBuilder
            .Create(name)
            .WithValue(0)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(
                (IEnumerable<double>)allowedValues)
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

        preference = DoublePreferenceBuilder
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
        DoublePreference preference = DoublePreferenceBuilder
            .Create(name)
            .WithValue(1)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor(
                DoubleValidityProcessor.IsGreaterThanZero)
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

        preference = DoublePreferenceBuilder
            .Create(name)
            .WithValue(2)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor((StructValidityProcessor<double>)
                DoubleValidityProcessor.IsGreaterThanZero)
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
            preference = DoublePreferenceBuilder
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
            preference = DoublePreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    DoubleValidityProcessor.IsGreaterThanZero)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = DoublePreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    DoubleValidityProcessor.IsGreaterThan(null))
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentNullException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = DoublePreferenceBuilder
                .Create(name)
                .WithValue(0)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    DoubleValidityProcessor.IsGreaterThan(5))
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());
    }
}
