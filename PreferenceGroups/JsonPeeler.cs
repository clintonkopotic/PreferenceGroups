using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Facilitates peeling a JSON data object (via <see cref="JToken"/>) down
    /// to the desired data object, either a <see cref="JObject"/> or
    /// <see cref="JValue"/>. Then further deserializing activities can happen
    /// with <see cref="PreferenceGroupJsonDeserializer"/> and
    /// <see cref="PreferenceJsonDeserializer"/>.
    /// </summary>
    public class JsonPeeler
    {
        /// <summary>
        /// The root or beginning <see cref="JToken"/> used when creating the
        /// peeler.
        /// </summary>
        public JToken Root { get; }

        /// <summary>
        /// The current <see cref="JToken"/> layer that the peeler has peeled
        /// to.
        /// </summary>
        public JToken Token { get; }

        /// <summary>
        /// Instantiates the start of a peeler where both <see cref="Root"/> and
        /// <see cref="Token"/> are set to <paramref name="root"/>.
        /// </summary>
        /// <param name="root"></param>
        protected JsonPeeler(JToken root) : this(root, root)
        { }

        /// <summary>
        /// Instantiates the next layer of a peeler with the
        /// <paramref name="root"/> and the new current
        /// <paramref name="token"/>.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="token"></param>
        protected JsonPeeler(JToken root, JToken token)
        {
            Root = root;
            Token = token;
        }

        /// <summary>
        /// Attempts to cast <see cref="Token"/> as a <see cref="JArray"/>,
        /// otherwise it will return <see langword="null"/>.
        /// </summary>
        /// <returns></returns>
        public JArray AsJArray() => JsoncSerializerHelper.ReadAsJArray(Token);

        /// <summary>
        /// Attempts to cast <see cref="Token"/> as a <see cref="JObject"/>,
        /// otherwise it will return <see langword="null"/>.
        /// </summary>
        /// <returns></returns>
        public JObject AsJObject()
            => JsoncSerializerHelper.ReadAsJObject(Token);

        /// <summary>
        /// Attempts to cast <see cref="Token"/> as a <see cref="JValue"/>,
        /// otherwise it will return <see langword="null"/>.
        /// </summary>
        /// <remarks><see cref="Token"/> will be cast as a <see cref="JValue"/>
        /// if its <see cref="JTokenType"/> is one of the following:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Integer"/></item>
        /// <item><see cref="JTokenType.Float"/></item>
        /// <item><see cref="JTokenType.String"/></item>
        /// <item><see cref="JTokenType.Boolean"/></item>
        /// <item><see cref="JTokenType.Date"/></item>
        /// <item><see cref="JTokenType.Bytes"/></item>
        /// <item><see cref="JTokenType.Guid"/></item>
        /// <item><see cref="JTokenType.Uri"/></item>
        /// <item><see cref="JTokenType.TimeSpan"/></item>
        /// </list>
        /// </remarks>
        /// <returns></returns>
        public JValue AsJValue() => JsoncSerializerHelper.ReadAsJValue(Token);

        /// <summary>
        /// Creates a new <see cref="JsonPeeler"/> with <see cref="Root"/> and
        /// the result of <see cref="GetArrayItemAsJToken(JArray, int)"/> with
        /// the result of <see cref="AsJArray()"/> and <paramref name="index"/>
        /// as the next layer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonPeeler GetArrayIndex(int index)
            => new JsonPeeler(Root,
                token: GetArrayItemAsJToken(
                    jArray: AsJArray(),
                    index: index));

        /// <summary>
        /// Creates a new <see cref="JsonPeeler"/> with <see cref="Root"/> and
        /// the result of <see cref="FindPropertyValueAsJToken(JObject, string,
        /// StringComparison)"/> with the result of <see cref="AsJObject()"/>,
        /// <paramref name="propertyName"/> and
        /// <paramref name="stringComparison"/> as the next layer.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName"/> is
        /// <see langword="null"/>.</exception>
        public JsonPeeler GetPropertyValue(string propertyName,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            return new JsonPeeler(Root,
                token: FindPropertyValueAsJToken(
                    jObject: AsJObject(),
                    propertyName: propertyName,
                    stringComparison: stringComparison));
        }

        /// <summary>
        /// Creates a <see cref="JsonPeeler"/> with <paramref name="token"/>,
        /// which will become the <see cref="Root"/> and <see cref="Token"/>.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JsonPeeler Create(JToken token)
            => new JsonPeeler(root: token);

        /// <summary>
        /// Creates a <see cref="JsonPeeler"/> by first creating a
        /// <see cref="StreamReader"/> with the
        /// <see cref="StreamReader(Stream, Encoding)"/> constructor with
        /// <paramref name="stream"/> and
        /// <paramref name="encoding"/>, where if
        /// <paramref name="encoding"/> is <see langword="null"/> the
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> will be used.
        /// The resulting <see cref="StreamReader"/> will be used along with
        /// <paramref name="jsonLoadSettings"/>, where if it is
        /// <see langword="null"/> then the
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> will be used
        /// when calling
        /// <see cref="Create(TextReader, JsonLoadSettings)"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is
        /// <see langword="null"/>.</exception>
        public static JsonPeeler Create(Stream stream,
            Encoding encoding = null, JsonLoadSettings jsonLoadSettings = null)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            JsonPeeler jsonPeeler;

            using (var reader = new StreamReader(stream,
                encoding ?? JsoncSerializerHelper.DefaultEncoding))
            {
                jsonPeeler = Create(reader, jsonLoadSettings);
            }

            return jsonPeeler;
        }

        /// <summary>
        /// Creates a <see cref="JsonPeeler"/> by first calling the
        /// <see cref="JToken.Parse(string, JsonLoadSettings)"/> method with
        /// <paramref name="jsonString"/> and
        /// <paramref name="jsonLoadSettings"/>, where if
        /// <paramref name="jsonLoadSettings"/> is <see langword="null"/> the
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> will be
        /// used. Then the <see cref="JsonPeeler"/> is created by calling
        /// <see cref="Create(JToken)"/>.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="jsonString"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="jsonString"/> is <see langword="null"/>.</exception>
        public static JsonPeeler Create(string jsonString,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (jsonString is null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(jsonString));
            }

            return Create(token: JToken.Parse(
                json: jsonString,
                settings: jsonLoadSettings
                    ?? JsoncSerializerHelper.DefaultLoadSettings));
        }

        /// <summary>
        /// Creates a <see cref="JsonPeeler"/> by first calling the
        /// <see cref="JToken.ReadFrom(JsonReader, JsonLoadSettings)"/> with
        /// <paramref name="textReader"/> and
        /// <paramref name="jsonLoadSettings"/>, where if
        /// <paramref name="jsonLoadSettings"/> is <see langword="null"/> the
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> will be
        /// used. Then the <see cref="JsonPeeler"/> is created by calling
        /// <see cref="Create(JToken)"/>.
        /// </summary>
        /// <param name="textReader"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static JsonPeeler Create(TextReader textReader,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            return Create(token: JToken.ReadFrom(
                reader: new JsonTextReader(textReader),
                settings: jsonLoadSettings
                    ?? JsoncSerializerHelper.DefaultLoadSettings));
        }

        /// <summary>
        /// Attempts to locate <paramref name="propertyName"/> (using
        /// <paramref name="stringComparison"/> within
        /// <paramref name="jObject"/> by using the
        /// <see cref="JObject.Property(string, StringComparison)"/> method and
        /// return it.
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName"/> is
        /// <see langword="null"/>.</exception>
        public static JProperty FindPropertyIn(JObject jObject,
            string propertyName,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(propertyName));
            }

            if (jObject is null || jObject.Type != JTokenType.Object)
            {
                return null;
            }

            return jObject.Property(propertyName, stringComparison);
        }

        /// <summary>
        /// Attempts to locate <paramref name="propertyName"/> (using
        /// <paramref name="stringComparison"/> within
        /// <paramref name="jObject"/> by using the
        /// <see cref="FindPropertyIn(JObject, string, StringComparison)"/> and
        /// return it as a <see cref="JToken"/>.
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName"/> is
        /// <see langword="null"/>.</exception>
        public static JToken FindPropertyValueAsJToken(JObject jObject,
            string propertyName,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(propertyName));
            }

            if (jObject is null || jObject.Type != JTokenType.Object)
            {
                return null;
            }

            JProperty property = FindPropertyIn(jObject, propertyName,
                stringComparison);

            if (property is null || property.Type != JTokenType.Property)
            {
                return null;
            }

            return property.Value;
        }

        /// <summary>
        /// Attempts to get the <paramref name="jArray"/> item at
        /// <paramref name="index"/>, otherwise return <see langword="null"/>.
        /// </summary>
        /// <param name="jArray"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static JToken GetArrayItemAsJToken(JArray jArray, int index)
        {
            if (jArray is null || jArray.Count <= 0)
            {
                return null;
            }

            return jArray[index];
        }
    }
}
