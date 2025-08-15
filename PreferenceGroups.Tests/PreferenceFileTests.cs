namespace PreferenceGroups.Tests;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class PreferenceFileTests
{
    [TestMethod]
    public void EmptyPreferenceGroupToStringTest()
    {
        var jsoncString = PreferenceFile.WriteToString(
            PreferenceGroupBuilder.BuildEmpty());

        Assert.AreEqual("{}", jsoncString);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(0, jObject.Count);
    }

    [TestMethod]
    public void EmptyPreferenceGroupArrayToStringTest()
    {
        var jsoncString = PreferenceFile.WriteToString(
            Array.Empty<PreferenceGroup>());

        Assert.AreEqual("[]", jsoncString);

        var jArray = PreferenceFile.ReadStringAsJArray(jsoncString);

        Assert.IsNotNull(jArray);
        Assert.AreEqual(0, jArray.Count);
    }

    [TestMethod]
    public void SimplePreferenceGroupToStringTest()
    {
        var numberName = "Number";
        var stringName = "String";
        PreferenceGroup group = PreferenceGroupBuilder
            .Create()
            .AddInt32(name: numberName, b => b
                .WithDefaultValue(13))
            .AddString(name: stringName, b => b
                .WithDescription("A string prefence."))
            .Build();

        var jsoncString = PreferenceFile.WriteToString(group);
        var expected = """
            {
                // Default value: 13.
                "Number": null,

                // A string prefence.
                "String": null
            }
            """;

        Assert.AreEqual(expected, jsoncString);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(2, jObject.Count);
        Assert.IsTrue(jObject.ContainsKey(numberName));
        var numberJToken = jObject[numberName];
        Assert.IsNotNull(numberJToken);
        Assert.AreEqual(JTokenType.Null, numberJToken.Type);
        Assert.IsTrue(jObject.ContainsKey(stringName));
        var stringJToken = jObject[stringName];
        Assert.IsNotNull(stringJToken);
        Assert.AreEqual(JTokenType.Null, stringJToken.Type);
    }

    [TestMethod]
    public void WithAttributesToStringTest()
    {
        int? int32Value = 10;
        string? stringValue = "Testing…1…2…3";
        PreferenceGroupWithAttributes test = new()
        {
            Int32 = int32Value,
            String = stringValue,
        };

        PreferenceGroup group = PreferenceGroupBuilder.BuildFrom(test);

        Assert.AreEqual(PreferenceGroupWithAttributes.Int32DefaultValue,
            group.GetDefaultValueAs<int?>(
                nameof(PreferenceGroupWithAttributes.Int32)));
        Assert.AreEqual(PreferenceGroupWithAttributes.StringDefaultValue,
            group.GetDefaultValueAs<string?>(
                PreferenceGroupWithAttributes.StringName));

        var jsoncString = PreferenceFile.WriteToString(group);
        var expected = """
            // A group of preferences.
            {
                // An integer.
                // Default value: 4.
                "Int32": 10,

                // A string.
                // Default value: "".
                "String123": "Testing…1…2…3"
            }
            """;

        Assert.AreEqual(expected, jsoncString);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(2, jObject.Count);
        Assert.IsTrue(jObject.ContainsKey(nameof(PreferenceGroupWithAttributes
            .Int32)));
        var numberJToken = jObject[nameof(PreferenceGroupWithAttributes
            .Int32)];
        Assert.IsNotNull(numberJToken);
        Assert.AreEqual(JTokenType.Integer, numberJToken.Type);
        Assert.IsTrue(jObject.ContainsKey(
            PreferenceGroupWithAttributes.StringName));
        var stringJToken = jObject[PreferenceGroupWithAttributes.StringName];
        Assert.IsNotNull(stringJToken);
        Assert.AreEqual(JTokenType.String, stringJToken.Type);

        var changedJsoncString = """
            // A group of preferences.
            {
                // An integer.
                // Default value: 4.
                "Int32": 11,

                // A string.
                // Default value: "".
                "String123": "Testing…1…2…3…4"
            }
            """;

        PreferenceFile.UpdateFromString(group, changedJsoncString);

        Assert.AreEqual(11, test.Int32);
        Assert.AreEqual("Testing…1…2…3…4", test.String);
    }
}
