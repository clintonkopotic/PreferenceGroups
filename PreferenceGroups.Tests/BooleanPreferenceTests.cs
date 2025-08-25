namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class BooleanPreferenceTests
{
    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", null, DisplayName = "Null")]
    [DataRow("Testing0", false, "Desc:Testing0", false, DisplayName = "False")]
    [DataRow("Testing1", true, "Desc:Testing1", true, DisplayName = "True")]
    [DataRow("Testing1", false, "Desc:Testing2", true,
        DisplayName = "False/True")]
    [DataRow("Testing1", false, "Desc:Testing2", null,
        DisplayName = "False/null")]
    [DataRow("Testing1", true, "Desc:Testing3", false,
        DisplayName = "True/False")]
    [DataRow("Testing1", true, "Desc:Testing3", null,
        DisplayName = "True/null")]
    public void SimpleBuildTest(string name, bool? value,
        string? description, bool? defaultValue)
    {
        BooleanPreference booleanPreference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(defaultValue)
            .Build();

        Assert.IsFalse(booleanPreference.IsEnum);
        Assert.IsFalse(booleanPreference.HasEnumFlags);
        Assert.AreEqual(name, booleanPreference.Name);
        Assert.AreEqual(value, booleanPreference.Value);
        Assert.AreEqual(description, booleanPreference.Description);
        Assert.AreEqual(defaultValue, booleanPreference.DefaultValue);
        Assert.IsNull(booleanPreference.AllowedValues);
        Assert.IsTrue(booleanPreference.AllowUndefinedValues);

        Preference preference = booleanPreference;

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(typeof(bool?), preference.GetValueType());
        Assert.AreEqual(value is null, preference.ValueIsNull);
        Assert.AreEqual(value, (bool?)preference.GetValueAsObject());
        Assert.AreEqual(value, preference.GetValueAs<bool?>());
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(defaultValue is null,
            preference.DefaultValueIsNull);
        Assert.AreEqual(defaultValue,
            (bool?)preference.GetDefaultValueAsObject());
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

        Assert.AreEqual(defaultValue, (bool?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<bool?>());

        preference.SetValueToDefault();

        Assert.AreEqual(defaultValue, (bool?)preference.GetValueAsObject());
        Assert.AreEqual(defaultValue, preference.GetValueAs<bool?>());

        if (value is not null)
        {
            var valueToggled = !value;
            preference.SetValueFromObject(valueToggled);
            Assert.AreEqual(valueToggled,
                (bool?)preference.GetValueAsObject());
            Assert.AreEqual(valueToggled, preference.GetValueAs<bool?>());

            valueToggled = !valueToggled;
            preference.SetValueFromObject(valueToggled);
            Assert.AreEqual(valueToggled,
                (bool?)preference.GetValueAsObject());
            Assert.AreEqual(valueToggled, preference.GetValueAs<bool?>());
        }

        booleanPreference = BooleanPreferenceBuilder.Build(name);

        Assert.IsFalse(booleanPreference.IsEnum);
        Assert.IsFalse(booleanPreference.HasEnumFlags);
        Assert.AreEqual(name, booleanPreference.Name);
        Assert.IsNull(booleanPreference.Value);
        Assert.IsNull(booleanPreference.Description);
        Assert.IsNull(booleanPreference.DefaultValue);
        Assert.IsNull(booleanPreference.AllowedValues);
        Assert.IsTrue(booleanPreference.AllowUndefinedValues);

        booleanPreference = BooleanPreferenceBuilder.Build(name, value);

        Assert.IsFalse(booleanPreference.IsEnum);
        Assert.IsFalse(booleanPreference.HasEnumFlags);
        Assert.AreEqual(name, booleanPreference.Name);
        Assert.AreEqual(value, booleanPreference.Value);
        Assert.IsNull(booleanPreference.Description);
        Assert.IsNull(booleanPreference.DefaultValue);
        Assert.IsNull(booleanPreference.AllowedValues);
        Assert.IsTrue(booleanPreference.AllowUndefinedValues);

        booleanPreference.SetValueToDefault();

        Assert.IsNull(booleanPreference.Value);
    }

    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", DisplayName = "Null")]
    [DataRow("Testing0", false, "Desc:Testing0", DisplayName = "False")]
    [DataRow("Testing1", true, "Desc:Testing1", DisplayName = "True")]
    public void SameValueAndDefaultBuildTest(string name, bool? value,
        string? description)
    {
        BooleanPreference preference = BooleanPreferenceBuilder
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

        preference = BooleanPreferenceBuilder
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
        Assert.IsNull(StructPreference<bool>.ProcessAllowedValues(null,
            sortAllowedValues: false));
        Assert.IsNull(StructPreference<bool>.ProcessAllowedValues(null,
            sortAllowedValues: true));

        // When allowedValues is empty.
        IReadOnlyCollection<bool?>? allowedValuesOut;
        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues([],
            sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<bool?>>(allowedValuesOut);

        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues([],
            sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(0, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<bool?>>(allowedValuesOut);

        // When allowedValues is a simple collection.
        IReadOnlyCollection<bool?>? allowedValues = [true, false];
        IReadOnlyCollection<bool?>? sortedAllowedValues = [false, true];
        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(allowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(allowedValues.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains repeats.
        allowedValues = [true, false, true];
        IReadOnlyCollection<bool?>? expectedAllowedValuesOut
            = [true, false];
        sortedAllowedValues = [false, true];
        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains null.
        allowedValues = [true, null, false];
        expectedAllowedValuesOut = [true, false];
        sortedAllowedValues = [false, true];
        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());

        // When allowedValues contains nulls and repeats.
        allowedValues = [true, false, null, false, false, null, null, true, true,
            null];
        expectedAllowedValuesOut = [true, false];
        sortedAllowedValues = [false, true];
        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: false);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(expectedAllowedValuesOut.Count,
            allowedValuesOut.Count);
        Assert.IsInstanceOfType<HashSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(expectedAllowedValuesOut.ToArray(),
            allowedValuesOut.ToArray());

        allowedValuesOut = StructPreference<bool>.ProcessAllowedValues(
            allowedValues, sortAllowedValues: true);

        Assert.IsNotNull(allowedValuesOut);
        Assert.AreEqual(sortedAllowedValues.Count, allowedValuesOut.Count);
        Assert.IsInstanceOfType<SortedSet<bool?>>(allowedValuesOut);
        CollectionAssert.AreEqual(sortedAllowedValues.ToArray(),
            allowedValuesOut.ToArray());
    }

    [TestMethod]
    public void AllowedValuesTests()
    {
        // Test if no allowed values that the AllowUndefinedValues
        // properties is forced to true.

        var name = "name";
        BooleanPreference preference = BooleanPreferenceBuilder
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

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        // Test if there are AllowedValues and AllowUndefinedValues is true.
        bool[] allowedValues = [true];
        bool[] sortedAllowedValues = [true];
        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
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
            preference = BooleanPreferenceBuilder
                .Create(name)
                .WithValue(false) // False is not an allowed value.
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
        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(true)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues((IEnumerable<bool>)allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(true, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort(allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort((IEnumerable<bool>)allowedValues)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .SortAllowedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort((IEnumerable<bool>)allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
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
        Assert.AreEqual(false, preference.Value);
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
        BooleanPreference preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(true)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor(BooleanValueValidityProcessor.IsTrue)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(true, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = BooleanPreferenceBuilder
            .Create(name)
            .WithValue(false)
            .WithDescription(null)
            .WithDefaultValue(null)
            .AllowOnlyDefinedValues()
            .WithValidityProcessor((StructValueValidityProcessor<bool>)
                BooleanValueValidityProcessor.IsFalse)
            .Build();

        Assert.IsFalse(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(false, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            preference = BooleanPreferenceBuilder
                .Create(name)
                .WithValue(false)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(null) // Cannot be null.
                .Build();
        });

        var exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = BooleanPreferenceBuilder
                .Create(name)
                .WithValue(false)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(BooleanValueValidityProcessor.IsTrue)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = BooleanPreferenceBuilder
                .Create(name)
                .WithValue(false)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    BooleanValueValidityProcessor.IsNotEqualTo(null))
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentNullException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = BooleanPreferenceBuilder
                .Create(name)
                .WithValue(false)
                .WithDescription(null)
                .WithDefaultValue(null)
                .AllowOnlyDefinedValues()
                .WithValidityProcessor(
                    BooleanValueValidityProcessor.IsEqualTo(true))
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());
    }
}
