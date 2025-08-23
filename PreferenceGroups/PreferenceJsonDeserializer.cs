using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for updating a
    /// <see cref="Preference"/> by deserializing from a <see cref="JValue"/>.
    /// </summary>
    public static class PreferenceJsonDeserializer
    {
        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="bool"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Boolean"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="bool"/> and returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is cast into a
        /// <see cref="Nullable{T}"/> <see cref="int"/> and only returns
        /// <see langword="true"/> if it is not equal to zero and
        /// <see langword="false"/> otherwise.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/> and the result of the
        /// <see cref="Convert.ToBoolean(string, IFormatProvider)"/> method is
        /// returned.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static bool? DeserializeAsBoolean(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Boolean)
            {
                return (bool?)jValue;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                var int32Value = (int?)jValue;

                if (int32Value is null)
                {
                    return null;
                }

                return int32Value.Value != 0;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }

                return Convert.ToBoolean(@string, CultureInfo.InvariantCulture);
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as a "
                + $"{nameof(Boolean)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="int"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="int"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToInt32(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToInt32(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static int? DeserializeAsInt32(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (int?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToInt32(@string, fromBase: 16)
                        : Convert.ToInt32(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Int32)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a <see cref="string"/>
        /// if the <see cref="JValue.Type"/> is <see cref="JTokenType.String"/>
        /// by casting it. If the <see cref="JValue.Type"/> of
        /// <paramref name="jValue"/> is <see cref="JTokenType.Null"/>, then
        /// <see langword="null"/> is returned.
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static string DeserializeAsString(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.String)
            {
                return (string)jValue;
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(String)}.");
        }

        /// <summary>
        /// Attempts to upate <paramref name="booleanPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsBoolean(JValue)"/> is called and
        /// its result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="booleanPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="booleanPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(BooleanPreference booleanPreference,
            JValue jValue)
        {
            if (booleanPreference is null)
            {
                throw new ArgumentNullException(nameof(booleanPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                booleanPreference.SetValueToNull();

                return;
            }

            var @bool = DeserializeAsBoolean(jValue);

            if (@bool is null)
            {
                booleanPreference.SetValueToNull();
            }
            else
            {
                booleanPreference.Value = @bool;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="int32Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsInt32(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="int32Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="int32Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(Int32Preference int32Preference,
            JValue jValue)
        {
            if (int32Preference is null)
            {
                throw new ArgumentNullException(nameof(int32Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                int32Preference.SetValueToNull();

                return;
            }

            var @int = DeserializeAsInt32(jValue);

            if (@int is null)
            {
                int32Preference.SetValueToNull();
            }
            else
            {
                int32Preference.Value = @int;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="stringPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsString(JValue)"/> is called and
        /// its result is used to set
        /// <see cref="StringPreference.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="stringPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stringPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(StringPreference stringPreference,
            JValue jValue)
        {
            if (stringPreference is null)
            {
                throw new ArgumentNullException(nameof(stringPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                stringPreference.SetValueToNull();

                return;
            }

            var @string = DeserializeAsString(jValue);

            if (@string is null)
            {
                stringPreference.SetValueToNull();
            }
            else
            {
                stringPreference.Value = @string;
            }
        }

        /// <summary>
        /// Attempts to update <paramref name="preference"/> from
        /// <paramref name="jObject"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jObject"/> is <see langword="null"/> or its
        /// <see cref="JObject.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueFromObject(object)"/> is called with
        /// <see langword="null"/>.</item>
        /// <item>The <see cref="Preference.Name"/> is called with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method, and its result is used to check with the
        /// <see cref="JObject.ContainsKey(string)"/> method if
        /// <paramref name="jObject"/> has a property with that <c>name</c>. If
        /// it does, then that property is cast to a <see cref="JValue"/> and
        /// the <see cref="UpdateFrom(Preference, JValue)"/> method is
        /// called.</item>
        /// </list>
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="jObject"></param>
        /// <returns><see langword="true"/> if <paramref name="preference"/> was
        /// updated from <paramref name="jObject"/>, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static bool UpdateFrom(Preference preference, JObject jObject)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (jObject is null || jObject.Type == JTokenType.Null)
            {
                return false;
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name, nameof(preference));

            if (!jObject.ContainsKey(processedName))
            {
                return false;
            }

            UpdateFrom(preference, (JValue)jObject[processedName]);

            return true;
        }

        /// <summary>
        /// Attempts to update <paramref name="preference"/> from
        /// <paramref name="jToken"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jToken"/> is <see langword="null"/> or its
        /// <see cref="JToken.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>If the <see cref="JToken.Type"/> of <paramref name="jToken"/>
        /// is <see cref="JTokenType.Object"/>, then <paramref name="jToken"/>
        /// is cast to a <see cref="JObject"/> and the
        /// <see cref="UpdateFrom(Preference, JObject)"/> method is
        /// called.</item>
        /// <item>Otherwise, <paramref name="jToken"/> is cast as a
        /// <see cref="JValue"/> and the
        /// <see cref="UpdateFrom(Preference, JValue)"/> method is
        /// called.</item>
        /// </list>
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="jToken"></param>
        /// <returns><see langword="true"/> if <paramref name="preference"/> was
        /// updated from <paramref name="jToken"/>, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static bool UpdateFrom(Preference preference, JToken jToken)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (jToken is null || jToken.Type == JTokenType.Null)
            {
                preference.SetValueToNull();

                return true;
            }
            else if (jToken.Type == JTokenType.Object)
            {
                return UpdateFrom(preference, (JObject)jToken);
            }

            // If jToken isn't an object, then assume it's a JValue.
            UpdateFrom(preference, (JValue)jToken);

            return true;
        }

        /// <summary>
        /// Attempts to update <paramref name="preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>If <see cref="Preference.GetValueType()"/> returns
        /// <see cref="Nullable{T}"/> of <see cref="bool"/>, then
        /// <paramref name="preference"/> is cast to
        /// <see cref="BooleanPreference"/>, and the
        /// <see cref="UpdateFrom(BooleanPreference, JValue)"/> is called.</item>
        /// <item>If <see cref="Preference.GetValueType()"/> returns
        /// <see cref="Nullable{T}"/> of <see cref="int"/>, then
        /// <paramref name="preference"/> is cast to
        /// <see cref="Int32Preference"/>, and the
        /// <see cref="UpdateFrom(Int32Preference, JValue)"/> is called.</item>
        /// <item>If <see cref="Preference.GetValueType()"/> returns
        /// <see cref="string"/>, then <paramref name="preference"/> is cast to
        /// <see cref="StringPreference"/>, and the
        /// <see cref="UpdateFrom(StringPreference, JValue)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="Preference.GetValueType()"/> returned an unexpected
        /// <see cref="Type"/>.</exception>
        public static void UpdateFrom(Preference preference, JValue jValue)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                preference.SetValueToNull();

                return;
            }

            var valueType = preference.GetValueType();

            if (valueType == typeof(bool?))
            {
                var booleanPreference = (BooleanPreference)preference;
                UpdateFrom(booleanPreference, jValue);
            }
            else if (valueType == typeof(int?))
            {
                var int32Preference = (Int32Preference)preference;
                UpdateFrom(int32Preference, jValue);
            }
            else if (valueType == typeof(string))
            {
                var stringPreference = (StringPreference)preference;
                UpdateFrom(stringPreference, jValue);
            }
            else
            {
                throw new InvalidOperationException("An unexpected value "
                    + $"{nameof(Type)} of \"{valueType.Name}\" to update.");
            }
        }
    }
}
