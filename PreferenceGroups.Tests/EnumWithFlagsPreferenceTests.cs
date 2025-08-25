using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

namespace PreferenceGroups.Tests
{
    [TestClass]
    public sealed class EnumWithFlagsPreferenceTests
    {
        [DataTestMethod]
        [DataRow("Testing", null, "Desc:Testing", null, DisplayName = "Null")]
        [DataRow("Testing1", MultiDay.Sunday, "Desc:Testing1", MultiDay.Monday,
            DisplayName = "One")]
        [DataRow("Testing2", MultiDay.Monday, "Desc:Testing2", MultiDay.Tuesday,
            DisplayName = "Two")]
        [DataRow("TestingMax", MultiDay.Saturday, "Desc:TestingMax", null,
            DisplayName = "Max")]
        [DataRow("TestingMid", MultiDay.Wednesday, "Desc:TestingMid", null,
            DisplayName = "Mid")]
        public void SimpleBuildTest(string name, MultiDay? value,
            string? description, MultiDay? defaultValue)
        {
            EnumPreference<MultiDay> enumPreference
                = EnumPreferenceBuilder<MultiDay>
                    .Create(name)
                    .WithValue(value)
                    .WithDescription(description)
                    .WithDefaultValue(defaultValue)
                    .Build();

            Assert.IsTrue(enumPreference.IsEnum);
            Assert.IsTrue(enumPreference.HasEnumFlags);
            Assert.AreEqual(name, enumPreference.Name);
            Assert.AreEqual(value, enumPreference.Value);
            Assert.AreEqual(description, enumPreference.Description);
            Assert.AreEqual(defaultValue, enumPreference.DefaultValue);
            Assert.IsNotNull(enumPreference.AllowedValues);
            Assert.IsFalse(enumPreference.AllowUndefinedValues);

            Preference preference = enumPreference;

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.AreEqual(typeof(MultiDay?), preference.GetValueType());
            Assert.AreEqual(value is null, preference.ValueIsNull);
            Assert.AreEqual(value, (MultiDay?)preference.GetValueAsObject());
            Assert.AreEqual(description, preference.Description);
            Assert.AreEqual(defaultValue is null,
                preference.DefaultValueIsNull);
            Assert.AreEqual(defaultValue,
                (MultiDay?)preference.GetDefaultValueAsObject());
            Assert.IsFalse(preference.AllowUndefinedValues);

            preference.SetValueToNull();

            Assert.IsTrue(preference.ValueIsNull);
            Assert.IsNull((MultiDay?)preference.GetValueAsObject());

            preference.SetValueToDefault();

            Assert.AreEqual(defaultValue, (MultiDay?)preference.GetValueAsObject());

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
                        (MultiDay?)preference.GetValueAsObject());

                    valueIncremented--;

                    if (!preference.IsValueValid(valueIncremented.Value))
                    {
                        valueIncremented++;
                    }

                    if (preference.IsValueValid(valueIncremented.Value))
                    {
                        preference.SetValueFromObject(valueIncremented);
                        Assert.AreEqual(valueIncremented,
                            (MultiDay?)preference.GetValueAsObject());
                    }
                }
            }

            enumPreference = EnumPreferenceBuilder<MultiDay>.Build(name);

            Assert.IsTrue(enumPreference.IsEnum);
            Assert.IsTrue(enumPreference.HasEnumFlags);
            Assert.AreEqual(name, enumPreference.Name);
            Assert.IsNull(enumPreference.Value);
            Assert.IsNull(enumPreference.Description);
            Assert.IsNull(enumPreference.DefaultValue);
            Assert.IsNotNull(enumPreference.AllowedValues);
            Assert.IsFalse(enumPreference.AllowUndefinedValues);

            enumPreference = EnumPreferenceBuilder<MultiDay>.Build(name, value);

            Assert.IsTrue(enumPreference.IsEnum);
            Assert.IsTrue(enumPreference.HasEnumFlags);
            Assert.AreEqual(name, enumPreference.Name);
            Assert.AreEqual(value, enumPreference.Value);
            Assert.IsNull(enumPreference.Description);
            Assert.IsNull(enumPreference.DefaultValue);
            Assert.IsNotNull(enumPreference.AllowedValues);
            Assert.IsFalse(enumPreference.AllowUndefinedValues);
        }

        [DataTestMethod]
        [DataRow("Testing", null, "Desc:Testing", DisplayName = "Null")]
        [DataRow("Testing1", MultiDay.Sunday, "Desc:Testing1",
            DisplayName = "One")]
        [DataRow("Testing1", MultiDay.Monday, "Desc:Testing2",
            DisplayName = "Two")]
        [DataRow("TestingMax", MultiDay.Saturday, "Desc:TestingMax",
            DisplayName = "Max")]
        [DataRow("TestingMid", MultiDay.Wednesday, "Desc:TestingMid",
            DisplayName = "Mid")]
        public void SameValueAndDefaultBuildTest(string name, MultiDay? value,
            string? description)
        {
            EnumPreference<MultiDay> preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(value)
                .WithDescription(description)
                .WithDefaultValue(value)
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.AreEqual(value, preference.Value);
            Assert.AreEqual(description, preference.Description);
            Assert.AreEqual(value, preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            Assert.IsFalse(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValueAndAsDefault(value)
                .WithDescription(description)
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
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
            EnumPreference<MultiDay> preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(null)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithNoAllowedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNull(preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNull(preference.AllowedValues);
            Assert.IsTrue(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.Sunday)
                .WithDescription(null)
                .WithDefaultValue(null)
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.Sunday, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            Assert.IsFalse(preference.AllowUndefinedValues);

            // Test if there are AllowedValues and AllowUndefinedValues is true.
            MultiDay[] allowedValues = [MultiDay.Tuesday, MultiDay.Monday,
            MultiDay.Sunday];
            MultiDay[] sortedAllowedValues = [MultiDay.Sunday, MultiDay.Monday,
            MultiDay.Tuesday];
            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValues(allowedValues)
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(allowedValues,
                preference.AllowedValues.ToList());
            Assert.IsTrue(preference.AllowUndefinedValues);

            // Test if there are AllowedValues and AllowUndefinedValues is false.
            var exception = Assert.ThrowsException<SetValueException>(() =>
            {
                preference = EnumPreferenceBuilder<MultiDay>
                    .Create(name)
                    .WithValue(MultiDay.None) // Zero is not an allowed value.
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
            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.Monday)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValues((IEnumerable<MultiDay>)allowedValues)
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.Monday, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(allowedValues,
                preference.AllowedValues.ToList());
            Assert.IsFalse(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValuesAndSort(allowedValues)
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(sortedAllowedValues,
                preference.AllowedValues.ToList());
            Assert.IsTrue(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValuesAndSort((IEnumerable<MultiDay>)allowedValues)
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(sortedAllowedValues,
                preference.AllowedValues.ToList());
            Assert.IsTrue(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValues(allowedValues)
                .SortAllowedValues()
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(sortedAllowedValues,
                preference.AllowedValues.ToList());
            Assert.IsTrue(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValuesAndDoNotSort(allowedValues)
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(allowedValues,
                preference.AllowedValues.ToList());
            Assert.IsTrue(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValuesAndDoNotSort(
                    (IEnumerable<MultiDay>)allowedValues)
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNotNull(preference.AllowedValues);
            CollectionAssert.AreEqual(allowedValues,
                preference.AllowedValues.ToList());
            Assert.IsTrue(preference.AllowUndefinedValues);

            preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.None)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithAllowedValues(allowedValues)
                .DoNotSortAllowedValues()
                .AllowUndefinedValues()
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.None, preference.Value);
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
            EnumPreference<MultiDay> preference = EnumPreferenceBuilder<MultiDay>
                .Create(name)
                .WithValue(MultiDay.Week)
                .WithDescription(null)
                .WithDefaultValue(null)
                .WithNoAllowedValues()
                .WithValidityProcessor(
                    EnumValueValidityProcessor<MultiDay>.IsDefinedAndNotZero)
                .Build();

            Assert.IsTrue(preference.IsEnum);
            Assert.IsTrue(preference.HasEnumFlags);
            Assert.AreEqual(name, preference.Name);
            Assert.IsNotNull(preference.Value);
            Assert.AreEqual(MultiDay.Week, preference.Value);
            Assert.IsNull(preference.Description);
            Assert.IsNull(preference.DefaultValue);
            Assert.IsNull(preference.AllowedValues);
            Assert.IsTrue(preference.AllowUndefinedValues);

            var exception = Assert.ThrowsException<SetValueException>(() =>
            {
                preference = EnumPreferenceBuilder<MultiDay>
                    .Create(name)
                    .WithValue(MultiDay.Sunday)
                    .WithDescription(null)
                    .WithDefaultValue(null)
                    .WithNoAllowedValues()
                    .WithValidityProcessor(
                        new StructValueValidityProcessor<MultiDay>()
                        {
                            IsValid = dayValue => dayValue != MultiDay.Sunday
                                ? StructValueValidityResult<MultiDay>.IsValid()
                                : StructValueValidityResult<MultiDay>.NotValid(
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
                preference = EnumPreferenceBuilder<MultiDay>
                    .Create(name)
                    .WithValue(MultiDay.None)
                    .WithDescription(null)
                    .WithDefaultValue(null)
                    .WithNoAllowedValues()
                    .WithValidityProcessor(
                        EnumValueValidityProcessor<MultiDay>
                        .IsDefinedAndNotZero)
                    .Build();
            });

            Assert.IsNotNull(exception);
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(typeof(ArgumentException),
                exception.InnerException.GetType());

            exception = Assert.ThrowsException<SetValueException>(() =>
            {
                preference = EnumPreferenceBuilder<MultiDay>
                    .Create(name)
                    .WithValue((MultiDay)0xFF)
                    .WithDescription(null)
                    .WithDefaultValue(null)
                    .WithNoAllowedValues()
                    .WithValidityProcessor(
                        EnumValueValidityProcessor<MultiDay>.IsDefinedAndNotZero)
                    .Build();
            });

            Assert.IsNotNull(exception);
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(typeof(ArgumentException),
                exception.InnerException.GetType());
        }
    }
}
