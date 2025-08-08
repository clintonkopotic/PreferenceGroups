namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class PreferenceGroupsTests
{
    [TestMethod]
    public void BuildTests()
    {
        var group = PreferenceGroupBuilder.BuildEmpty();

        Assert.IsNotNull(group);
        Assert.AreEqual(0, group.Count);
        Assert.IsNull(group.Description);

        var groupDescription = "Test group.";
        group = PreferenceGroupBuilder.Create()
            .WithDescription(groupDescription)
            .Build();

        Assert.IsNotNull(group);
        Assert.AreEqual(0, group.Count);
        Assert.AreEqual(groupDescription, group.Description);

        var emptyStringPreferenceName = "EmptyString";
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

        group = PreferenceGroupBuilder.Create()
            .WithDescription(groupDescription)
            .AddString(emptyStringPreferenceName)
            .AddInt32(int32PreferenceWithDescriptionName, b => b
                .WithValue(int32PreferenceWithDescriptionValue)
                .WithDescription(int32PreferenceDescription))
            .AddInt32(emptyInt32PreferenceName)
            .AddString(stringPreferenceWithDescriptionName, b => b
                .WithValue(stringPreferenceWithDescriptionValue)
                .WithDescription(stringPreferenceDescription))
            .AddInt32(int32PreferenceName, int32PreferenceValue)
            .AddString(stringPreferenceName, stringPreferenceValue)
            .Build();

        Assert.IsNotNull(group);
        Assert.AreEqual(groupDescription, group.Description);
        Assert.AreEqual(6, group.Count);
        Assert.AreEqual(6, group.Names.Count);
        Assert.IsFalse(((ICollection<Preference>)group).IsReadOnly);
        CollectionAssert.AreEqual(
            expected: new string[]
            {
                emptyStringPreferenceName,
                int32PreferenceWithDescriptionName,
                emptyInt32PreferenceName,
                stringPreferenceWithDescriptionName,
                int32PreferenceName,
                stringPreferenceName
            },
            actual: group.Names.ToArray());

        Assert.AreEqual(expected: null,
            actual: group.GetValueAs<string?>(emptyStringPreferenceName));
        Assert.AreEqual(expected: null,
            actual: group.GetValueAs<int?>(emptyInt32PreferenceName));
        Assert.AreEqual(expected: int32PreferenceWithDescriptionValue,
            actual: group.GetValueAs<int?>(int32PreferenceWithDescriptionName));
        Assert.AreEqual(expected: int32PreferenceValue,
            actual: group.GetValueAs<int?>(int32PreferenceName));
        Assert.AreEqual(expected: stringPreferenceWithDescriptionValue,
            actual: group.GetValueAs<string?>(
                stringPreferenceWithDescriptionName));
        Assert.AreEqual(expected: stringPreferenceValue,
            actual: group.GetValueAs<string?>(stringPreferenceName));
    }

    [TestMethod]
    public void ConstructorTests()
    {
        var description = "Test group.";
        var group = new PreferenceGroup(description);

        Assert.IsNotNull(group);
        Assert.AreEqual(description, group.Description);
        Assert.AreEqual(0, group.Count);
        Assert.AreEqual(0, group.Names.Count);
        Assert.IsFalse(((ICollection<Preference>)group).IsReadOnly);

        var stringPreferenceName = "String";
        var stringPreference = StringPreferenceBuilder.Build(
            stringPreferenceName);

        Assert.IsFalse(group.ContainsName(stringPreferenceName));

        _ = Assert.ThrowsException<KeyNotFoundException>(() =>
        {
            _ = group[stringPreferenceName];
        });

        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            group.Add(preference: null);
        });

        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            group.Add(preferences: (Preference[]?)null);
        });

        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            group.Add(preferences: (IEnumerable<Preference>?)null);
        });

        group.Add(stringPreference);

        Assert.AreEqual(1, group.Count);
        CollectionAssert.AreEqual(
            expected: new string[] { stringPreferenceName },
            actual: group.Names.ToArray());
        Assert.IsTrue(group.ContainsName(stringPreferenceName));

        Preference? tempPreference = group[stringPreferenceName];

        Assert.IsNotNull(tempPreference);
        Assert.AreEqual(stringPreference, tempPreference);

        tempPreference = StringPreferenceBuilder.Build(
            stringPreferenceName);

        Assert.IsNotNull(tempPreference);
        Assert.IsFalse(group.Contains(tempPreference));
        Assert.IsFalse(group.Remove(tempPreference));
        Assert.IsTrue(group.ContainsName(tempPreference.Name));
        Assert.IsTrue(group.RemoveByName(tempPreference.Name));
        Assert.AreEqual(0, group.Count);
        Assert.IsFalse(group.ContainsName(tempPreference.Name));

        group.Add([stringPreference]);

        Assert.AreEqual(1, group.Count);
        CollectionAssert.AreEqual(
            expected: new string[] { stringPreferenceName },
            actual: group.Names.ToArray());
        Assert.IsTrue(group.Contains(stringPreference));
        Assert.IsTrue(group.Remove(stringPreference));
        Assert.IsFalse(group.Remove(tempPreference));
        Assert.AreEqual(0, group.Count);

        var int32PreferenceName = "Int32";
        var int32PreferenceValue = 123;
        var int32PreferenceDescription = "Test int.";
        var int32Preference = Int32PreferenceBuilder
            .Create(int32PreferenceName)
            .WithValue(int32PreferenceValue)
            .WithDescription(int32PreferenceDescription)
            .Build();

        Assert.IsFalse(group.ContainsName(int32PreferenceName));

        group.Add((IEnumerable<Preference>)
            [stringPreference, int32Preference]);

        Assert.AreEqual(2, group.Count);
        CollectionAssert.AreEqual(
            expected: new string[]
            {
                stringPreferenceName,
                int32PreferenceName
            },
            actual: group.Names.ToArray());
        Assert.IsTrue(group.ContainsName(int32PreferenceName));
        Assert.AreEqual(
            expected: int32PreferenceValue,
            actual: (int?)group.GetValue(int32PreferenceName));

        int32PreferenceValue = 456;
        group.SetValue(int32PreferenceName, int32PreferenceValue);

        Assert.AreEqual(
            expected: int32PreferenceValue,
            actual: (int?)group.GetValue(int32PreferenceName));

        group.SetValue(stringPreferenceName, int32PreferenceValue);
        Assert.AreEqual(
            expected: int32PreferenceValue.ToString(),
            actual: (string?)group.GetValue(stringPreferenceName));
    }

    [TestMethod]
    public void ProcessNamesAndThrowIfNotEqualTests()
    {
        // name cannot be null.
        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: null,
                preference: null);
        });

        // preference cannot be null.
        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: "A",
                preference: null);
        });

        // name cannot be empty.
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: string.Empty,
                preference: null);
        });

        // name cannot consist only of white-space characters.
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: " ",
                preference: null);
        });

        // preference.Name cannot be null.
        _ = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: "A",
                preference: new NullNamePreference("B"));
        });

        // preference.Name cannot be empty.
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: "A",
                preference: new EmptyNamePreference("B"));
        });

        // preference.Name cannot consist only white-space characters.
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: "A",
                preference: new WhitespaceNamePreference("B"));
        });

        string name = "B";
        Preference preference = StringPreferenceBuilder.Build(name);

        // name and preference.Name cannot be different.
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            _ = PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name: "A",
                preference: preference);
        });

        // The processed name should be the same as name.
        Assert.AreEqual(
            expected: name,
            actual: PreferenceGroup.ProcessNamesAndThrowIfNotEqual(
                name, preference));
    }
}
