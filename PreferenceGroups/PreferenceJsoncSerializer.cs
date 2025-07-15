using System;
using System.CodeDom.Compiler;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for serializing a
    /// <see cref="Preference"/> into JSONC data including the metadata being
    /// written in comments, like the <see cref="Preference.Description"/>,
    /// <c>AllowedValues</c> and the <c>DefaultValue</c>.
    /// </summary>
    /// <remarks>See <see href="https://www.jsonc.org">jsonc.org</see> for
    /// further details about a JSONC file format.</remarks>
    public static class PreferenceJsoncSerializer
    {
        /// <summary>
        /// Returns the <c>AllowedValues</c> as valid JSON in a
        /// <see cref="string"/> <see cref="Array"/>, using the
        /// <see cref="Preference.GetAllowedValuesAsStrings(IFormatProvider)"/>
        /// method (with <see cref="CultureInfo.InvariantCulture"/> as the
        /// <c>formatProvider</c>). If <c>AllowedValues</c> is
        /// <see langword="null"/>, then <see langword="null"/> is returned. On
        /// the other hand, for each allowed value, if it is
        /// <see langword="null"/> then it is replaced with
        /// <see cref="JsonConvert.Null"/>; otherwise, if the
        /// <see cref="ShouldValueBeInJsonString(Preference)"/> method returns
        /// <see langword="true"/> then it is replaced with the result of the
        /// <see cref="JsoncSerializerHelper.SerialzieStringValue(string)"/>
        /// method, where if it returns <see langword="false"/> then it is not
        /// replaced.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static string[] GetAllowedValuesAsStrings(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var allowedValues = preference.GetAllowedValuesAsStrings(
                CultureInfo.InvariantCulture);

            if (allowedValues is null)
            {
                return null;
            }

            if (allowedValues.Length > 0)
            {
                var shouldValueBeInJsonString = ShouldValueBeInJsonString(
                    preference);
                var jsonAllowedValues = new string[allowedValues.Length];

                for (var i = 0; i < allowedValues.Length; i++)
                {
                    var allowedValue = allowedValues[i];

                    if (allowedValue is null)
                    {
                        allowedValue = JsonConvert.Null;
                    }
                    else if (shouldValueBeInJsonString)
                    {
                        allowedValue = JsoncSerializerHelper
                            .SerialzieStringValue(allowedValue);
                    }

                    jsonAllowedValues[i] = allowedValue;
                }

                allowedValues = jsonAllowedValues;
            }

            return allowedValues;
        }

        /// <summary>
        /// Returns the <c>DefaultValue</c> as valid JSON in a
        /// <see cref="string"/> by calling the
        /// <see cref="Preference.GetDefaultValueAsString(IFormatProvider)"/>
        /// method (with <see cref="CultureInfo.InvariantCulture"/> as the
        /// <c>formatProvider</c>). If it is <see langword="null"/>, then
        /// <see langword="null"/> is returned; otherwise, if the
        /// <see cref="ShouldValueBeInJsonString(Preference)"/> method returns
        /// <see langword="true"/> then the result of the
        /// <see cref="JsoncSerializerHelper.SerialzieStringValue(string)"/>
        /// method is returned, where if it returns <see langword="false"/>
        /// then it is not altered. 
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static string GetDefaultValueAsString(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var defaultValue = preference.GetDefaultValueAsString(
                CultureInfo.InvariantCulture);

            if (!(defaultValue is null)
                && ShouldValueBeInJsonString(preference))
            {
                defaultValue = JsoncSerializerHelper.SerialzieStringValue(
                    defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the <c>tValue</c> as valid JSON in a
        /// <see cref="string"/> by calling the
        /// <see cref="Preference.GetValueAsString(IFormatProvider)"/>
        /// method (with <see cref="CultureInfo.InvariantCulture"/> as the
        /// <c>formatProvider</c>). If it is <see langword="null"/>, then
        /// <see cref="JsonConvert.Null"/> is returned; otherwise, if the
        /// <see cref="ShouldValueBeInJsonString(Preference)"/> method returns
        /// <see langword="true"/> then the result of the
        /// <see cref="JsoncSerializerHelper.SerialzieStringValue(string)"/>
        /// method is returned, where if it returns <see langword="false"/>
        /// then it is not altered. 
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static string GetValueAsString(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var value = preference.GetValueAsString(
                CultureInfo.InvariantCulture);

            if (value is null)
            {
                value = JsonConvert.Null;
            }
            else if (ShouldValueBeInJsonString(preference))
            {
                value = JsoncSerializerHelper.SerialzieStringValue(value);
            }

            return value;
        }

        /// <summary>
        /// Serializes <paramref name="preference"/> into JSONC using
        /// <paramref name="context"/> by calling <see
        /// cref="WriteAnyComments(JsoncSerializationContext, Preference)"/>,
        /// then if <see cref="JsoncSerializationContext.CurrentType"/> is
        /// <see cref="JTokenType.Object"/> for <paramref name="context"/> the
        /// <see cref="JsoncSerializerHelper.WritePropertyName(
        /// IndentedTextWriter, string)"/> is called, and finally the
        /// <see cref="IndentedTextWriter.Write(string)"/> method is called with
        /// the <see cref="JsoncSerializationContext.Writer"/> property of
        /// <paramref name="context"/> with the results of the
        /// <see cref="GetValueAsString(Preference)"/> method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public static void Serialize(JsoncSerializationContext context,
            Preference preference)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            WriteAnyComments(context, preference);

            if (context.CurrentType == JTokenType.Object)
            {
                JsoncSerializerHelper.WritePropertyName(context.Writer,
                    Preference.ProcessNameOrThrowIfInvalid(preference.Name));
            }

            context.Writer.Write(GetValueAsString(preference));
        }

        /// <summary>
        /// Determines if <see cref="Preference.GetValueType()"/> should be in a
        /// JSON string or not.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="Preference.GetValueType()"/> method of
        /// <paramref name="preference"/> returned
        /// <see langword="null"/>.</exception>
        public static bool ShouldValueBeInJsonString(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var type = preference.GetValueType()
                ?? throw new InvalidOperationException("The value type of "
                    + $"{nameof(preference)} cannot be null.");

            return ShouldValueBeInJsonString(type);
        }

        /// <summary>
        /// Determines if <paramref name="type"/> should be in a JSON string or
        /// not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is <see langword="null"/>.</exception>
        public static bool ShouldValueBeInJsonString(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type == typeof(string);
        }

        /// <summary>
        /// Writes the <c>AllowedValues</c> of <paramref name="preference"/>
        /// using <paramref name="context"/> by calling the
        /// <see cref="JsoncSerializerHelper.WriteListInComment(
        /// IndentedTextWriter, string[], string, string)"/> method with the
        /// results of calling the
        /// <see cref="GetAllowedValuesAsStrings(Preference)"/> method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteAllowedValuesInComment(
            JsoncSerializationContext context, Preference preference)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var allowedValues = GetAllowedValuesAsStrings(preference);

            if (!(allowedValues is null) && allowedValues.Length > 0)
            {
                context.WriteEmptyLineIfNeeded();
                JsoncSerializerHelper.WriteListInComment(
                    indentedTextWriter: context.Writer,
                    list: allowedValues,
                    prefix: preference.IsEnum && preference.HasEnumFlags
                        ? "Allowed values are combinations of: "
                        : "Allowed values: ",
                    postfix: ".");
            }
        }

        /// <summary>
        /// If <see cref="JsoncSerializationContext.CommentsWritten"/> is
        /// <see langword="false"/>, then the following are called in sequence:
        /// <list type="number">
        /// <item><see cref="JsoncSerializationContext.ResetNeedToWriteLine()"/>
        /// from <paramref name="context"/>.</item>
        /// <item><see cref="WriteDescriptionInComment(
        /// JsoncSerializationContext, Preference)"/>.</item>
        /// <item><see cref="WriteAllowedValuesInComment(
        /// JsoncSerializationContext, Preference)"/>.</item>
        /// <item><see cref="WriteDefaultValueInComment(
        /// JsoncSerializationContext, Preference)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.CommentsWritten"/> is set
        /// to <see langword="true"/>.</item>
        /// <item><see cref="JsoncSerializationContext.NeedToWriteLine"/> is set
        /// to <see langword="false"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteAnyComments(JsoncSerializationContext context,
            Preference preference)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (!context.CommentsWritten)
            {
                context.ResetNeedToWriteLine();
                WriteDescriptionInComment(context, preference);
                WriteAllowedValuesInComment(context, preference);
                WriteDefaultValueInComment(context, preference);
                context.CommentsWritten = true;
                context.NeedToWriteLine = false;
            }
        }

        /// <summary>
        /// If <see cref="GetDefaultValueAsString(Preference)"/> does not return
        /// <see langword="null"/> and is not an empty <see cref="string"/>,
        /// then its result is called with the
        /// <see cref="JsoncSerializerHelper.WriteLineComment(
        /// IndentedTextWriter, string)"/> method after the
        /// <see cref="JsoncSerializationContext.WriteEmptyLineIfNeeded()"/> method
        /// is called.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteDefaultValueInComment(
            JsoncSerializationContext context, Preference preference)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var defaultValue = GetDefaultValueAsString(preference);

            if (!string.IsNullOrEmpty(defaultValue))
            {
                context.WriteEmptyLineIfNeeded();
                JsoncSerializerHelper.WriteLineComment(
                    indentedTextWriter: context.Writer,
                    comment: $"Default value: {defaultValue}.");
            }
        }

        /// <summary>
        /// If <see cref="Preference.Description"/> does not return
        /// <see langword="null"/> and is not an empty <see cref="string"/>,
        /// then its result is called with the
        /// <see cref="JsoncSerializerHelper.WriteLineComment(
        /// IndentedTextWriter, string)"/> method after the
        /// <see cref="JsoncSerializationContext.WriteEmptyLineIfNeeded()"/> method
        /// is called.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteDescriptionInComment(
            JsoncSerializationContext context, Preference preference)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var description = preference.Description;

            if (!string.IsNullOrEmpty(description))
            {
                context.WriteEmptyLineIfNeeded();
                JsoncSerializerHelper.WriteLineComment(
                    indentedTextWriter: context.Writer,
                    comment: description);
            }
        }
    }
}
