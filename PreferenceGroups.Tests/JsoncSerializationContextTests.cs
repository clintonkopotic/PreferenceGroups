using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PreferenceGroups.Tests
{
    [TestClass]
    public sealed class JsoncSerializationContextTests
    {
        [TestMethod]
        public void EmptyGroupTest()
        {
            using StringWriter stringWriter = new();
            using JsoncSerializationContext context = new(stringWriter);
            context.Serialize(PreferenceGroupBuilder.BuildEmpty());
            context.Flush();
            var jsoncString = stringWriter.ToString();

            Assert.AreEqual("{}", jsoncString);

            var jObject = JsonPeeler.Create(jsoncString).AsJObject();

            Assert.IsNotNull(jObject);
            Assert.AreEqual(0, jObject.Count);
        }

        [TestMethod]
        public void SimpleInt32PreferenceTest()
        {
            Preference preference = Int32PreferenceBuilder
                .Create("TestInt32")
                .WithValue(1)
                .Build();

            using StringWriter stringWriter = new();
            using JsoncSerializationContext context = new(stringWriter);
            context.Serialize(preference);
            context.Flush();
            var jsoncString = stringWriter.ToString();
            var expected = "1";

            Assert.AreEqual(expected, jsoncString);

            var jValue = JsonPeeler.Create(jsoncString).AsJValue();

            Assert.IsNotNull(jValue);
            Assert.AreEqual(JTokenType.Integer, jValue.Type);

            var value = (long?)jValue.Value;
            Assert.IsNotNull(value);
            Assert.AreEqual(1L, value.Value);
            preference.SetValueFromObject(jValue.Value);
        }

        [TestMethod]
        public void SimpleStringPreference()
        {
            Preference preference = StringPreferenceBuilder
                .Create("TestString")
                .WithValue("1")
                .Build();

            using StringWriter stringWriter = new();
            using JsoncSerializationContext context = new(stringWriter);
            context.Serialize(preference);
            context.Flush();
            var jsoncString = stringWriter.ToString();
            var expected = "\"1\"";

            Assert.AreEqual(expected, jsoncString);

            var jValue = JsonPeeler.Create(jsoncString).AsJValue();

            Assert.IsNotNull(jValue);
            Assert.AreEqual(JTokenType.String, jValue.Type);

            var value = (string?)jValue.Value;
            Assert.IsNotNull(value);
            Assert.AreEqual("1", value);
            preference.SetValueFromObject(jValue.Value);
        }

        [TestMethod]
        public void SimplePreferenceGroupTest()
        {
            PreferenceGroup group = PreferenceGroupBuilder
                .Create()
                .AddInt32(name: "Number", b => b
                    .WithDefaultValue(13))
                .AddString(name: "String", b => b
                    .WithDescription("A string prefence."))
                .Build();

            using StringWriter stringWriter = new();
            using JsoncSerializationContext context = new(stringWriter);
            context.Serialize(group);
            context.Flush();
            var jsoncString = stringWriter.ToString();
            var expected = """
                {
                    // Default value: 13.
                    "Number": null,

                    // A string prefence.
                    "String": null
                }
                """;

            Assert.AreEqual(expected, jsoncString);

            var jObject = JsonPeeler.Create(jsoncString).AsJObject();

            Assert.IsNotNull(jObject);
            Assert.AreEqual(2, jObject.Count);
        }
    }
}
