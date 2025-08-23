namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class PreferenceStoreTests
{
    [TestMethod]
    public void BuildTests()
    {
        var store = PreferenceStoreBuilder.BuildEmpty();

        Assert.IsNotNull(store);
        Assert.AreEqual(0, store.Count);
        Assert.IsNull(store.Description);

        var storeDescription = "Test store.";

        store = PreferenceStoreBuilder.Create()
            .WithDescription(storeDescription)
            .Build();

        Assert.IsNotNull(store);
        Assert.AreEqual(0, store.Count);
        Assert.AreEqual(storeDescription, store.Description);

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

        store = PreferenceStoreBuilder.Create()
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

        Assert.IsNotNull(store);
        Assert.AreEqual(storeDescription, store.Description);
        Assert.AreEqual(2, store.Count);
        Assert.AreEqual(2, store.Names.Count);
        Assert.IsFalse(
            ((ICollection<KeyValuePair<string, PreferenceStoreItem>>)store)
            .IsReadOnly);
        CollectionAssert.AreEqual(
            expected: new string[]
            {
                emptyStringPreferenceName,
                groupName
            },
            actual: store.Names.ToArray());

        var group = store.GetItemAsPreferenceGroup(groupName);

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

        group.SetValuesToNull();

        foreach (var preference in group)
        {
            Assert.IsTrue(preference.ValueIsNull);
        }

        group.SetValuesToDefault();

        foreach (var preference in group)
        {
            Assert.IsTrue(preference.ValueIsNull);
        }
    }
}
