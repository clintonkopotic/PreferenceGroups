namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class EnumPreferenceTests
{
    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", null, DisplayName = "Null")]
    [DataRow("Testing1", SingleDay.Sunday, "Desc:Testing1",
        SingleDay.Monday, DisplayName = "One")]
    [DataRow("Testing2", SingleDay.Monday, "Desc:Testing2",
        SingleDay.Tuesday, DisplayName = "Two")]
    [DataRow("TestingMax", SingleDay.Saturday, "Desc:TestingMax", null,
        DisplayName = "Max")]
    [DataRow("TestingMid", SingleDay.Wednesday, "Desc:TestingMid", null,
        DisplayName = "Mid")]
    public void SimpleBuildTest(string name, SingleDay? value,
        string? description, SingleDay? defaultValue)
    {
        var enumPreference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(defaultValue)
            .Build();

        Assert.IsTrue(enumPreference.IsEnum);
        Assert.IsFalse(enumPreference.HasEnumFlags);
        Assert.AreEqual(name, enumPreference.Name);
        Assert.AreEqual(value, enumPreference.Value);
        Assert.AreEqual(description, enumPreference.Description);
        Assert.AreEqual(defaultValue, enumPreference.DefaultValue);
        Assert.IsNotNull(enumPreference.AllowedValues);
        Assert.IsFalse(enumPreference.AllowUndefinedValues);

        Preference preference = enumPreference;

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(typeof(SingleDay?), preference.GetValueType());
        Assert.AreEqual(value is null, preference.ValueIsNull);
        Assert.AreEqual(value, (SingleDay?)preference.GetValueAsObject());
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(defaultValue is null,
            preference.DefaultValueIsNull);
        Assert.AreEqual(defaultValue,
            (SingleDay?)preference.GetDefaultValueAsObject());
        Assert.IsFalse(preference.AllowUndefinedValues);

        preference.SetValueToNull();

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((SingleDay?)preference.GetValueAsObject());

        preference.SetValueToDefault();

        Assert.AreEqual(defaultValue,
            (SingleDay?)preference.GetValueAsObject());

        if (value is not null)
        {
            var valueIncremented = value + 1;

            if (!preference.IsValueValid(valueIncremented.Value))
            {
                valueIncremented = value - 1;
            }

            if (preference.IsValueValid(valueIncremented.Value))
            {
                preference.SetValueFromObject(valueIncremented);
                Assert.AreEqual(valueIncremented,
                    (SingleDay?)preference.GetValueAsObject());

                valueIncremented--;

                if (!preference.IsValueValid(valueIncremented.Value))
                {
                    valueIncremented++;
                }

                if (preference.IsValueValid(valueIncremented.Value))
                {
                    preference.SetValueFromObject(valueIncremented);
                    Assert.AreEqual(valueIncremented,
                        (SingleDay?)preference.GetValueAsObject());
                    Assert.AreEqual(valueIncremented,
                        (SingleDay?)preference.GetValueAsObject());
                    Assert.AreEqual(valueIncremented,
                        preference.GetValueAs<SingleDay?>());
                }
            }
        }

        enumPreference = EnumPreferenceBuilder<SingleDay>.Build(name);

        Assert.IsTrue(enumPreference.IsEnum);
        Assert.AreEqual(name, enumPreference.Name);
        Assert.IsNull(enumPreference.Value);
        Assert.IsNull(enumPreference.Description);
        Assert.IsNull(enumPreference.DefaultValue);
        Assert.IsNotNull(enumPreference.AllowedValues);
        Assert.IsFalse(enumPreference.AllowUndefinedValues);

        enumPreference = EnumPreferenceBuilder<SingleDay>.Build(name,
            value);

        Assert.IsTrue(enumPreference.IsEnum);
        Assert.IsFalse(enumPreference.HasEnumFlags);
        Assert.AreEqual(name, enumPreference.Name);
        Assert.AreEqual(value, enumPreference.Value);
        Assert.IsNull(enumPreference.Description);
        Assert.IsNull(enumPreference.DefaultValue);
        Assert.IsNotNull(enumPreference.AllowedValues);
        Assert.IsFalse(enumPreference.AllowUndefinedValues);

        // Test using new EnumPreferenceBuilder
        EnumPreference enumPreferenceNew = EnumPreferenceBuilder
            .Create<SingleDay>(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(defaultValue)
            .Build();

        Assert.IsTrue(enumPreferenceNew.IsEnum);
        Assert.IsFalse(enumPreferenceNew.HasEnumFlags);
        Assert.AreEqual(name, enumPreferenceNew.Name);
        Assert.AreEqual(value, enumPreferenceNew.Value);
        Assert.AreEqual(description, enumPreferenceNew.Description);
        Assert.AreEqual(defaultValue, enumPreferenceNew.DefaultValue);
        Assert.IsNotNull(enumPreferenceNew.AllowedValues);
        Assert.IsFalse(enumPreferenceNew.AllowUndefinedValues);

        preference = enumPreferenceNew;

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(typeof(SingleDay?), preference.GetValueType());
        Assert.AreEqual(value is null, preference.ValueIsNull);
        Assert.AreEqual(value, (SingleDay?)preference.GetValueAsObject());
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(defaultValue is null,
            preference.DefaultValueIsNull);
        Assert.AreEqual(defaultValue,
            (SingleDay?)preference.GetDefaultValueAsObject());
        Assert.IsFalse(preference.AllowUndefinedValues);

        preference.SetValueToNull();

        Assert.IsTrue(preference.ValueIsNull);
        Assert.IsNull((SingleDay?)preference.GetValueAsObject());

        preference.SetValueToDefault();

        Assert.AreEqual(defaultValue,
            (SingleDay?)preference.GetValueAsObject());

        if (value is not null)
        {
            var valueIncremented = value + 1;

            if (!preference.IsValueValid(valueIncremented.Value))
            {
                valueIncremented = value - 1;
            }

            if (preference.IsValueValid(valueIncremented.Value))
            {
                preference.SetValueFromObject(valueIncremented);
                Assert.AreEqual(valueIncremented,
                    (SingleDay?)preference.GetValueAsObject());

                valueIncremented--;

                if (!preference.IsValueValid(valueIncremented.Value))
                {
                    valueIncremented++;
                }

                if (preference.IsValueValid(valueIncremented.Value))
                {
                    preference.SetValueFromObject(valueIncremented);
                    Assert.AreEqual(valueIncremented,
                        (SingleDay?)preference.GetValueAsObject());
                    Assert.AreEqual(valueIncremented,
                        (SingleDay?)preference.GetValueAsObject());
                    Assert.AreEqual(valueIncremented,
                        preference.GetValueAs<SingleDay?>());
                }
            }
        }

        enumPreferenceNew = EnumPreferenceBuilder.Build<SingleDay>(name);

        Assert.IsTrue(enumPreferenceNew.IsEnum);
        Assert.AreEqual(name, enumPreferenceNew.Name);
        Assert.IsNull(enumPreferenceNew.Value);
        Assert.IsNull(enumPreferenceNew.Description);
        Assert.IsNull(enumPreferenceNew.DefaultValue);
        Assert.IsNotNull(enumPreferenceNew.AllowedValues);
        Assert.IsFalse(enumPreferenceNew.AllowUndefinedValues);

        enumPreferenceNew = EnumPreferenceBuilder.Build(name, value);

        Assert.IsTrue(enumPreferenceNew.IsEnum);
        Assert.IsFalse(enumPreferenceNew.HasEnumFlags);
        Assert.AreEqual(name, enumPreferenceNew.Name);
        Assert.AreEqual(value, enumPreferenceNew.Value);
        Assert.IsNull(enumPreferenceNew.Description);
        Assert.IsNull(enumPreferenceNew.DefaultValue);
        Assert.IsNotNull(enumPreferenceNew.AllowedValues);
        Assert.IsFalse(enumPreferenceNew.AllowUndefinedValues);
    }

    [DataTestMethod]
    [DataRow("Testing", null, "Desc:Testing", DisplayName = "Null")]
    [DataRow("Testing1", SingleDay.Sunday, "Desc:Testing1",
        DisplayName = "One")]
    [DataRow("Testing1", SingleDay.Monday, "Desc:Testing2",
        DisplayName = "Two")]
    [DataRow("TestingMax", SingleDay.Saturday, "Desc:TestingMax",
        DisplayName = "Max")]
    [DataRow("TestingMid", SingleDay.Wednesday, "Desc:TestingMid",
        DisplayName = "Mid")]
    public void SameValueAndDefaultBuildTest(string name, SingleDay? value,
        string? description)
    {
        var preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(value)
            .WithDescription(description)
            .WithDefaultValue(value)
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(value, preference.Value);
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(value, preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        Assert.IsFalse(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValueAndAsDefault(value)
            .WithDescription(description)
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.AreEqual(value, preference.Value);
        Assert.AreEqual(description, preference.Description);
        Assert.AreEqual(value, preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        Assert.IsFalse(preference.AllowUndefinedValues);
    }

    [TestMethod]
    public void AllowedValuesTests()
    {
        // Test if no allowed values is set that the AllowUndefinedValues
        // property is forced to true and AllowedValues defaults to null.
        var name = "name";
        var preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(null)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithNoAllowedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNull(preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.Sunday)
            .WithDescription(null)
            .WithDefaultValue(null)
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.Sunday, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        Assert.IsFalse(preference.AllowUndefinedValues);

        // Test if there are AllowedValues and AllowUndefinedValues is true.
        SingleDay[] allowedValues = [SingleDay.Tuesday, SingleDay.Monday,
        SingleDay.Sunday];
        SingleDay[] sortedAllowedValues = [SingleDay.Sunday, SingleDay.Monday,
        SingleDay.Tuesday];
        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
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
            preference = EnumPreferenceBuilder<SingleDay>
                .Create(name)
                .WithValue(SingleDay.None) // Zero is not an allowed value.
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValues(allowedValues)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(InvalidOperationException),
            exception.InnerException.GetType());

        // Test if there are AllowedValues and AllowUndefinedValues is false.
        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.Monday)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.Monday, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsFalse(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort(allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndSort((IEnumerable<SingleDay>)allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .SortAllowedValues()
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(sortedAllowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValuesAndDoNotSort(
                (IEnumerable<SingleDay>)allowedValues)
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNotNull(preference.AllowedValues);
        CollectionAssert.AreEqual(allowedValues,
            preference.AllowedValues.ToList());
        Assert.IsTrue(preference.AllowUndefinedValues);

        preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.None)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithAllowedValues(allowedValues)
            .DoNotSortAllowedValues()
            .AllowUndefinedValues()
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.None, preference.Value);
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
        EnumPreference preference = EnumPreferenceBuilder<SingleDay>
            .Create(name)
            .WithValue(SingleDay.Sunday)
            .WithDescription(null)
            .WithDefaultValue(null)
            .WithNoAllowedValues()
            .WithValidityProcessor(
                EnumValueValidityProcessor.IsDefinedAndNotZero)
            .Build();

        Assert.IsTrue(preference.IsEnum);
        Assert.IsFalse(preference.HasEnumFlags);
        Assert.AreEqual(name, preference.Name);
        Assert.IsNotNull(preference.Value);
        Assert.AreEqual(SingleDay.Sunday, preference.Value);
        Assert.IsNull(preference.Description);
        Assert.IsNull(preference.DefaultValue);
        Assert.IsNull(preference.AllowedValues);
        Assert.IsTrue(preference.AllowUndefinedValues);

        var exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = EnumPreferenceBuilder<SingleDay>
                .Create(name)
                .WithValue(SingleDay.Sunday)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithNoAllowedValues()
                .WithValidityProcessor(
                    new ClassValueValidityProcessor<Enum>()
                    {
                        IsValid = dayValue =>
                            !dayValue.Equals(SingleDay.Sunday)
                            ? ClassValueValidityResult<Enum>.IsValid()
                            : ClassValueValidityResult<Enum>.NotValid(
                                new ArgumentException(
                                    paramName: nameof(dayValue),
                                    message: "Cannot be Sunday"))
                    })
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = EnumPreferenceBuilder<SingleDay>
                .Create(name)
                .WithValue(SingleDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithNoAllowedValues()
                .WithValidityProcessor(
                    EnumValueValidityProcessor.IsDefinedAndNotZero)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());

        exception = Assert.ThrowsException<SetValueException>(() =>
        {
            preference = EnumPreferenceBuilder<SingleDay>
                .Create(name)
                .WithValue((SingleDay)(-1))
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithNoAllowedValues()
                .WithValidityProcessor(
                    EnumValueValidityProcessor.IsDefinedAndNotZero)
                .Build();
        });

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(typeof(ArgumentException),
            exception.InnerException.GetType());
    }
}
