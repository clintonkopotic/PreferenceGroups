namespace PreferenceGroups.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreferenceGroups.Tests.HelperClasses;

[TestClass]
public sealed class PreferenceStoreItemTests
{
    [TestMethod]
    public void ItemKindTests()
    {
        var exception = Assert.ThrowsException<ArgumentException>(() =>
        {
            PreferenceStoreItem.ThrowIfItemKindIsUnknown(
                PreferenceStoreItemKind.None, "abc");
        });

        Assert.AreEqual("abc", exception.ParamName);

        exception = Assert.ThrowsException<ArgumentException>(() =>
        {
            PreferenceStoreItem.ThrowIfItemKindIsUnknown(
                (PreferenceStoreItemKind)(-1), "abc");
        });

        Assert.AreEqual("abc", exception.ParamName);

        exception = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(
                null, "abc");
        });

        Assert.AreEqual("abc", exception.ParamName);
    }
}
