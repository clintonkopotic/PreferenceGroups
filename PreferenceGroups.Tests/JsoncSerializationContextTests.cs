using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }

        [TestMethod]
        public void EmptyGroupArrayTest()
        {
            var jsonString = JsoncSerializationContext.SerializeToString(
                groups: []);

            Assert.AreEqual("[]", jsonString);
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
        }
    }
}
