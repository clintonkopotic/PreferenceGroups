using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups.Tests
{
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
    }
}
