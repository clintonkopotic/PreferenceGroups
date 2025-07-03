using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups.Tests
{
    [TestClass]
    public sealed class JsoncSerializationContextTests
    {
        [TestMethod]
        public void EmptyGroupTest()
        {
            var jsonString = JsoncSerializationContext.SerializeToString(
                PreferenceGroupBuilder.BuildEmpty());

            Assert.AreEqual("{}", jsonString);

            var jObject = JsonPeeler.Create(jsonString).AsJObject();

            Assert.IsNotNull(jObject);
            Assert.AreEqual(0, jObject.Count);
        }

        [TestMethod]
        public void EmptyGroupArrayTest()
        {
            var jsonString = JsoncSerializationContext.SerializeToString(
                groups: []);

            Assert.AreEqual("[]", jsonString);

            var jArray = JsonPeeler.Create(jsonString).AsJArray();

            Assert.IsNotNull(jArray);
            Assert.AreEqual(0, jArray.Count);
        }

        [TestMethod]
        public void SimpleInt32PreferenceTest()
        {
            Preference preference = Int32PreferenceBuilder
                .Create("TestInt32")
                .WithValue(1)
                .Build();

            var jsoncString = JsoncSerializationContext.SerializeToString(
                preference);
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

            var jsoncString = JsoncSerializationContext.SerializeToString(
                preference);
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

            var jsoncString = JsoncSerializationContext.SerializeToString(
                group);
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
