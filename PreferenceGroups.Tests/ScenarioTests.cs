namespace PreferenceGroups.Tests;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

[TestClass]
public sealed class ScenarioTests
{
    [TestMethod]
    public void SimpleGroupInitWriteRead()
    {
        PreferenceGroup group = PreferenceGroupBuilder
            .Create()
            .AddInt32(name: "Number", b => b
                .WithDefaultValue(13))
            .AddString(name: "String", b => b
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

        var updatedNames = PreferenceFile.UpdateFromString(group,
            jsoncString);

        Assert.IsNotNull(updatedNames);
        Assert.AreEqual(2, updatedNames.Count);
        CollectionAssert.AreEqual(group.Names.ToArray(),
            updatedNames.ToArray());
    }
}
