namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class EnumHelpersTests
{
    [TestMethod]
    public void IsEnumTests()
    {
        Dictionary<string, (Type?, bool, bool, Type?)> tests = new()
        {
            {"Null", (null, false, false, null) },
            { "Int32", (typeof(int), false, false, null) },
            { "Int32?", (typeof(int?), false, true, null) },
            { "SingleDay",
                (typeof(SingleDay), true, false, typeof(SingleDay)) },
            { "MultiDay",
                (typeof(MultiDay), true, false, typeof(MultiDay)) },
            { "SingleDay?",
                (typeof(SingleDay?), true, true, typeof(SingleDay)) },
            { "MultiDay?",
                (typeof(MultiDay?), true, true, typeof(MultiDay)) },
        };

        foreach ((var testName, (var type, var expectedIsEnum,
            var expectedTypeIsNullable, var expectedEnumType)) in tests)
        {
            var message = $"Test name: \"{testName}\".";

            if (type is null)
            {
                var argumentNullExpection = Assert
                    .ThrowsException<ArgumentNullException>(() =>
                    {
                        _ = EnumHelpers.IsEnum(type, out _, out _,
                            nameof(type));
                    });

                Assert.AreEqual(nameof(type), argumentNullExpection.ParamName,
                    message);

                continue;
            }

            var isEnum = EnumHelpers.IsEnum(type, out var typeIsNullable,
                out var enumType, nameof(type));

            Assert.AreEqual(expectedIsEnum, isEnum, message);
            Assert.AreEqual(expectedTypeIsNullable, typeIsNullable, message);

            if (!isEnum)
            {
                continue;
            }

            Assert.AreEqual(expectedEnumType, enumType, message);
        }
    }

    [TestMethod]
    public void GetZeroTest()
    {
        Assert.AreEqual(SingleDay.None, EnumHelpers.GetZero<SingleDay>());
        Assert.AreEqual(MultiDay.None, EnumHelpers.GetZero<MultiDay>());
        Assert.AreEqual((NoZeroEnum)0, EnumHelpers.GetZero<NoZeroEnum>());
    }

    [TestMethod]
    public void HasFlagsTest()
    {
        Assert.IsFalse(EnumHelpers.HasFlags<SingleDay>());
        Assert.IsTrue(EnumHelpers.HasFlags<MultiDay>());
        Assert.IsTrue(EnumHelpers.HasFlags<NoZeroEnum>());
    }

    [TestMethod]
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
            (MultiDay)Enum.ToObject(typeof(MultiDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsDefined(
            (MultiDay)Enum.ToObject(typeof(MultiDay), int.MaxValue)));

        Assert.IsTrue(EnumHelpers.IsDefined((Enum)SingleDay.None));
        Assert.IsTrue(EnumHelpers.IsDefined((Enum)SingleDay.Sunday));
        Assert.IsTrue(EnumHelpers.IsDefined((Enum)MultiDay.None));
        Assert.IsFalse(EnumHelpers.IsDefined((Enum)(NoZeroEnum)0));
        Assert.IsFalse(EnumHelpers.IsDefined((Enum)Enum.ToObject(
            typeof(SingleDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsDefined((Enum)Enum.ToObject(
            typeof(SingleDay), int.MaxValue)));
        Assert.IsFalse(EnumHelpers.IsDefined((Enum)Enum.ToObject(
            typeof(MultiDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsDefined((Enum)Enum.ToObject(
            typeof(MultiDay), int.MaxValue)));
    }

    [TestMethod]
    public void IsZeroTest()
    {
        Assert.IsTrue(EnumHelpers.IsZero(SingleDay.None));
        Assert.IsFalse(EnumHelpers.IsZero(SingleDay.Sunday));
        Assert.IsTrue(EnumHelpers.IsZero(MultiDay.None));
        Assert.IsTrue(EnumHelpers.IsZero((NoZeroEnum)0));
        Assert.IsFalse(EnumHelpers.IsZero(
            (SingleDay)Enum.ToObject(typeof(SingleDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsZero(
            (SingleDay)Enum.ToObject(typeof(SingleDay), int.MaxValue)));
        Assert.IsFalse(EnumHelpers.IsZero(
            (MultiDay)Enum.ToObject(typeof(MultiDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsZero(
            (MultiDay)Enum.ToObject(typeof(MultiDay), int.MaxValue)));

        Assert.IsTrue(EnumHelpers.IsZero((Enum)SingleDay.None));
        Assert.IsFalse(EnumHelpers.IsZero((Enum)SingleDay.Sunday));
        Assert.IsTrue(EnumHelpers.IsZero((Enum)MultiDay.None));
        Assert.IsTrue(EnumHelpers.IsZero((Enum)(NoZeroEnum)0));
        Assert.IsFalse(EnumHelpers.IsZero((Enum)Enum.ToObject(
            typeof(SingleDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsZero((Enum)Enum.ToObject(
            typeof(SingleDay), int.MaxValue)));
        Assert.IsFalse(EnumHelpers.IsZero((Enum)Enum.ToObject(
            typeof(MultiDay), int.MinValue)));
        Assert.IsFalse(EnumHelpers.IsZero((Enum)Enum.ToObject(
            typeof(MultiDay), int.MaxValue)));
    }

    [TestMethod]
    public void ThrowIfNullOrNotEnumTests()
    {
        EnumHelpers.ThrowIfNullOrNotEnum(typeof(SingleDay));
        EnumHelpers.ThrowIfNullOrNotEnum(typeof(MultiDay));

        var argumentNullException
            = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                EnumHelpers.ThrowIfNullOrNotEnum(null);
            });

        Assert.AreEqual("enumType", argumentNullException.ParamName);

        var argumentException = Assert.ThrowsException<ArgumentException>(() =>
        {
            EnumHelpers.ThrowIfNullOrNotEnum(typeof(int));
        });

        Assert.AreEqual("enumType", argumentException.ParamName);
    }

    [TestMethod]
    public void ThrowIfTypeNotEqualTests()
    {
        EnumHelpers.ThrowIfTypeNotEqual(typeof(MultiDay), MultiDay.Sunday);
        EnumHelpers.ThrowIfTypeNotEqual(typeof(SingleDay),
            SingleDay.Sunday.GetType());
        EnumHelpers.ThrowIfTypeNotEqual(typeof(MultiDay),
            MultiDay.Sunday.GetType());
        EnumHelpers.ThrowIfTypeNotEqual(typeof(SingleDay?), typeof(SingleDay));

        // Test if enumType is null.
        var argumentNullException
            = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                EnumHelpers.ThrowIfTypeNotEqual((Type?)null, SingleDay.Sunday);
            });

        Assert.AreEqual("enumType", argumentNullException.ParamName);

        // Test if enum is null.
        argumentNullException = Assert.ThrowsException<ArgumentNullException>(
            () =>
            {
                EnumHelpers.ThrowIfTypeNotEqual(typeof(SingleDay), (Enum?)null);
            });

        Assert.AreEqual("enum", argumentNullException.ParamName);

        // Test if enumType is not an Enum.
        var argumentException = Assert.ThrowsException<ArgumentException>(() =>
        {
            EnumHelpers.ThrowIfTypeNotEqual(typeof(int), typeof(SingleDay));
        });

        Assert.AreEqual("enumType", argumentException.ParamName);

        // Test if otherEnumType is not an Enum.
        argumentException = Assert.ThrowsException<ArgumentException>(() =>
        {
            EnumHelpers.ThrowIfTypeNotEqual(typeof(SingleDay), typeof(int));
        });

        Assert.AreEqual("otherEnumType", argumentException.ParamName);

        // Test if enum is a different Type than enumType.
        argumentException = Assert.ThrowsException<ArgumentException>(() =>
        {
            EnumHelpers.ThrowIfTypeNotEqual(typeof(MultiDay), SingleDay.Sunday);
        });

        Assert.AreEqual("enum", argumentException.ParamName);

        // Test if otherEnumType is not the same Type as enumType.
        argumentException = Assert.ThrowsException<ArgumentException>(() =>
        {
            EnumHelpers.ThrowIfTypeNotEqual(typeof(SingleDay),
                typeof(MultiDay));
        });

        Assert.AreEqual("otherEnumType", argumentException.ParamName);
    }
}
