namespace PreferenceGroups.Tests;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class PreferenceFileTests
{
    [TestMethod]
    public void SimpleFileTest()
    {
        // Create temporary directory to store the file in.
        var tempDirectoryInfo = Directory.CreateTempSubdirectory(
            prefix: "PreferenceGroups-");

        // Build the PreferenceGroup.
        PreferenceGroupWithAttributes @object = new();
        var group = PreferenceGroupBuilder.BuildFrom(@object);

        // Prepare for the Preferences.jsonc file.
        var preferencesFilePath = Path.Combine(tempDirectoryInfo.FullName,
            "Preferences.jsonc");
        PreferenceFile file = new(preferencesFilePath);

        Assert.IsFalse(File.Exists(preferencesFilePath));

        // Create the Preferences.jsonc file for first time.
        var updates = file.Update(group);

        // No updates occured.
        Assert.IsNull(updates);

        // Verify the file contents.
        var expected = """
            // A group of preferences.
            {
                // An integer.
                // Default value: 4.
                "Int32": null,
            
                // A string.
                // Default value: "".
                "String123": null,
            
                // Default value: false.
                "Boolean": false,
            
                // Testing
                // Suggested values are combinations of (separated by ','): "Sunday" | "Monday".
                // Default value: "None".
                "MultiDayEnum": "None",
            
                // Allowed values: "None" | "Sunday" | "Monday" | "Tuesday" | "Wednesday" | "Thursday" | "Friday" | "Saturday".
                // Default value: "None".
                "SingleDayEnum": "None"
            }
            """;
        Assert.AreEqual(expected, file.ReadAsString());

        // Overwrite the file with an empty file.
        File.Create(preferencesFilePath).Dispose();

        // Attempt to update again, but this should 
        updates = file.Update(group);

        // No updates occured.
        Assert.IsNull(updates);

        // Cleanup.
        tempDirectoryInfo.Delete(recursive: true);
    }

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
        var booleanName = "Boolean";
        var enumName = "Enum";
        PreferenceGroup group = PreferenceGroupBuilder
            .Create()
            .AddInt32(name: numberName, b => b
                .WithDefaultValue(13))
            .AddString(name: stringName, b => b
                .WithDescription("A string prefence."))
            .AddBoolean(name: booleanName, b => b
                .WithAllowedValuesAndSort(true, false))
            .AddEnum<MultiDay>(name: enumName, b => b
                .WithValue(MultiDay.Friday | MultiDay.Monday)
                .WithAllowedValuesAndDoNotSort(
                    MultiDay.Sunday,
                    MultiDay.Monday,
                    MultiDay.Tuesday,
                    MultiDay.Wednesday,
                    MultiDay.Thursday,
                    MultiDay.Friday,
                    MultiDay.Saturday,
                    MultiDay.Weekdays,
                    MultiDay.Weekend,
                    MultiDay.Week))
            .Build();

        var jsoncString = PreferenceFile.WriteToString(group);
        var expected = """
            {
                // Default value: 13.
                "Number": null,

                // A string prefence.
                "String": null,
            
                // Suggested values: false | true.
                "Boolean": null,

                // Allowed values are combinations of (separated by ','): "Sunday" | "Monday" | "Tuesday" | "Wednesday" | "Thursday" | "Friday" | "Saturday" | "Weekdays" | "Weekend" | "Week".
                "Enum": "Monday, Friday"
            }
            """;

        Assert.AreEqual(expected, jsoncString);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(4, jObject.Count);
        Assert.IsTrue(jObject.ContainsKey(numberName));
        var numberJToken = jObject[numberName];
        Assert.IsNotNull(numberJToken);
        Assert.AreEqual(JTokenType.Null, numberJToken.Type);
        Assert.IsTrue(jObject.ContainsKey(stringName));
        var stringJToken = jObject[stringName];
        Assert.IsNotNull(stringJToken);
        Assert.AreEqual(JTokenType.Null, stringJToken.Type);
        var booleanJToken = jObject[booleanName];
        Assert.IsNotNull(booleanJToken);
        Assert.AreEqual(JTokenType.Null, booleanJToken.Type);
        var enumJToken = jObject[enumName];
        Assert.IsNotNull(enumJToken);
        Assert.AreEqual(JTokenType.String, enumJToken.Type);
        Assert.AreEqual(expected: MultiDay.Friday | MultiDay.Monday,
            actual: group.GetValueAs<MultiDay?>(enumName));
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
        Assert.AreEqual(PreferenceGroupWithAttributes.BooleanDefaultValue,
            group.GetDefaultValueAs<bool?>(
                nameof(PreferenceGroupWithAttributes.Boolean)));

        var jsoncString = PreferenceFile.WriteToString(group);
        var expected = """
            // A group of preferences.
            {
                // An integer.
                // Default value: 4.
                "Int32": 10,

                // A string.
                // Default value: "".
                "String123": "Testing…1…2…3",
            
                // Default value: false.
                "Boolean": false,
            
                // Testing
                // Suggested values are combinations of (separated by ','): "Sunday" | "Monday".
                // Default value: "None".
                "MultiDayEnum": "None",
            
                // Allowed values: "None" | "Sunday" | "Monday" | "Tuesday" | "Wednesday" | "Thursday" | "Friday" | "Saturday".
                // Default value: "None".
                "SingleDayEnum": "None"
            }
            """;

        Assert.AreEqual(expected, jsoncString);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(5, jObject.Count);
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
        Assert.IsTrue(jObject.ContainsKey(nameof(PreferenceGroupWithAttributes
            .Boolean)));
        var booleanJToken = jObject[nameof(PreferenceGroupWithAttributes
            .Boolean)];
        Assert.IsNotNull(booleanJToken);
        Assert.AreEqual(JTokenType.Boolean, booleanJToken.Type);

        var changedJsoncString = """
            // A group of preferences.
            {
                // An integer.
                // Default value: 4.
                "Int32": 11,

                // A string.
                // Default value: "".
                "String123": "Testing…1…2…3…4",
                "Boolean": true
            }
            """;

        PreferenceFile.UpdateFromString(group, changedJsoncString);

        Assert.AreEqual(11, test.Int32);
        Assert.AreEqual("Testing…1…2…3…4", test.String);
        Assert.IsTrue(test.Boolean);
    }

    [TestMethod]
    public void SimplePreferenceStoreToStringTest()
    {
        var storeDescription = "Test store.";
        var emptyStringPreferenceName = "EmptyString";
        var groupName = "group123";
        var emptyInt32PreferenceName = "EmptyInt32";
        var int32PreferenceWithDescriptionName = "Int32WithDescription";
        int? int32PreferenceWithDescriptionValue = 123;
        var int32PreferenceDescription = "Test int.";
        var int32PreferenceName = "Int32";
        int? int32PreferenceValue = 456;
        var stringPreferenceWithDescriptionName = "StringWithDescription";
        string? stringPreferenceWithDescriptionValue
            = "String with description";
        var stringPreferenceDescription = "Test string.";
        var stringPreferenceName = "String";
        string? stringPreferenceValue = "String";

        var store = PreferenceStoreBuilder.Create()
            .WithDescription(storeDescription)
            .AddString(emptyStringPreferenceName)
            .AddGroup(groupName, b => b
                .AddInt32(int32PreferenceWithDescriptionName, b => b
                    .WithValue(int32PreferenceWithDescriptionValue)
                    .WithDescription(int32PreferenceDescription))
                .AddInt32(emptyInt32PreferenceName)
                .AddString(stringPreferenceWithDescriptionName, b => b
                    .WithValue(stringPreferenceWithDescriptionValue)
                    .WithDescription(stringPreferenceDescription))
                .AddInt32(int32PreferenceName, int32PreferenceValue)
                .AddString(stringPreferenceName, stringPreferenceValue))
            .Build();

        var jsoncString = PreferenceFile.WriteToString(store);
        var expected = """
            // Test store.
            {
                "EmptyString": null,
                "group123": {
                    // Test int.
                    "Int32WithDescription": 123,
                    "EmptyInt32": null,

                    // Test string.
                    "StringWithDescription": "String with description",
                    "Int32": 456,
                    "String": "String"
                }
            }
            """;

        Assert.AreEqual(expected, jsoncString);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(2, jObject.Count);
        Assert.IsTrue(jObject.ContainsKey(emptyStringPreferenceName));
    }

    [TestMethod]
    public void SimpleStoreFileTest()
    {
        // Create temporary directory to store the file in.
        var tempDirectoryInfo = Directory.CreateTempSubdirectory(
            prefix: "PreferenceGroups-");

        // Build the PreferenceGroup.
        var storeDescription = "Test store.";
        var emptyStringPreferenceName = "EmptyString";
        var groupName = "group123";
        var emptyInt32PreferenceName = "EmptyInt32";
        var int32PreferenceWithDescriptionName = "Int32WithDescription";
        int? int32PreferenceWithDescriptionValue = 123;
        var int32PreferenceDescription = "Test int.";
        var int32PreferenceName = "Int32";
        int? int32PreferenceValue = 456;
        var stringPreferenceWithDescriptionName = "StringWithDescription";
        string? stringPreferenceWithDescriptionValue
            = "String with description";
        var stringPreferenceDescription = "Test string.";
        var stringPreferenceName = "String";
        string? stringPreferenceValue = "String";

        var store = PreferenceStoreBuilder.Create()
            .WithDescription(storeDescription)
            .AddString(emptyStringPreferenceName)
            .AddGroup(groupName, b => b
                .AddInt32(int32PreferenceWithDescriptionName, b => b
                    .WithValue(int32PreferenceWithDescriptionValue)
                    .WithDescription(int32PreferenceDescription))
                .AddInt32(emptyInt32PreferenceName)
                .AddString(stringPreferenceWithDescriptionName, b => b
                    .WithValue(stringPreferenceWithDescriptionValue)
                    .WithDescription(stringPreferenceDescription))
                .AddInt32(int32PreferenceName, int32PreferenceValue)
                .AddString(stringPreferenceName, stringPreferenceValue))
            .Build();

        // Prepare for the Preferences.jsonc file.
        var preferencesFilePath = Path.Combine(tempDirectoryInfo.FullName,
            "Preferences.jsonc");
        PreferenceFile file = new(preferencesFilePath);

        Assert.IsFalse(File.Exists(preferencesFilePath));

        // Create the Preferences.jsonc file for first time.
        var updates = file.Update(store);

        // No updates occured.
        Assert.IsNull(updates);

        var jsoncString = PreferenceFile.WriteToString(store);
        var expected = """
            // Test store.
            {
                "EmptyString": null,
                "group123": {
                    // Test int.
                    "Int32WithDescription": 123,
                    "EmptyInt32": null,
            
                    // Test string.
                    "StringWithDescription": "String with description",
                    "Int32": 456,
                    "String": "String"
                }
            }
            """;

        Assert.AreEqual(expected, file.ReadAsString());

        // Overwrite the file with an empty file.
        File.Create(preferencesFilePath).Dispose();

        // Attempt to update again, but this should 
        updates = file.Update(store);

        // No updates occured.
        Assert.IsNull(updates);

        var jObject = PreferenceFile.ReadStringAsJObject(jsoncString);

        Assert.IsNotNull(jObject);
        Assert.AreEqual(2, jObject.Count);
        Assert.IsTrue(jObject.ContainsKey(emptyStringPreferenceName));

        // Cleanup.
        tempDirectoryInfo.Delete(recursive: true);
    }

    [TestMethod]
    public void EmptyStringTests()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            PreferenceFile.ReadStringAsJToken(null);
        });

        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            PreferenceFile.ReadStringAsJToken(string.Empty);
        });

        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            PreferenceFile.ReadStringAsJToken(" ");
        });

        JsonReaderException jsonReaderException;

        jsonReaderException = Assert.ThrowsException<JsonReaderException>(() =>
        {
            PreferenceFile.ReadAsJToken(new StringReader(string.Empty));
        });

        Assert.AreEqual(0, jsonReaderException.LineNumber);
        Assert.AreEqual(0, jsonReaderException.LinePosition);

        jsonReaderException = Assert.ThrowsException<JsonReaderException>(() =>
        {
            PreferenceFile.ReadAsJToken(new StringReader(" "));
        });

        Assert.AreEqual(1, jsonReaderException.LineNumber);
        Assert.AreEqual(1, jsonReaderException.LinePosition);

        jsonReaderException = Assert.ThrowsException<JsonReaderException>(() =>
        {
            PreferenceFile.ReadAsJToken(new StringReader("    "));
        });

        Assert.AreEqual(1, jsonReaderException.LineNumber);
        Assert.AreEqual(4, jsonReaderException.LinePosition);
    }
}
