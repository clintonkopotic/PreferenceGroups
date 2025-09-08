using System;
using System.CodeDom.Compiler;
using System.Text;
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
        /// <see cref="Preference.GetAllowedValuesAsObjects()"/> method. If
        /// <c>AllowedValues</c> is <see langword="null"/>, then
        /// <see langword="null"/> is returned. On the other hand, for each
        /// allowed value, if it is <see langword="null"/> then the result of
        /// <see cref="JsonConvert.Null"/> is used; otherwise, the result of
        /// <see cref="GetObjectAsString(object)"/> method is used.
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

            var allowedValues = preference.GetAllowedValuesAsObjects();

            if (allowedValues is null)
            {
                return null;
            }

            var jsonAllowedValues = new string[allowedValues.Length];

            for (var i = 0; i < allowedValues.Length; i++)
            {
                jsonAllowedValues[i] = allowedValues[i] is null
                    ? JsonConvert.Null : GetObjectAsString(allowedValues[i]);
            }

            return jsonAllowedValues;
        }

        /// <summary>
        /// Returns the <c>DefaultValue</c> as valid JSON in a
        /// <see cref="string"/> by calling the
        /// <see cref="Preference.GetDefaultValueAsObject()"/> method and
        /// passing the result into the <see cref="GetObjectAsString(object)"/>
        /// and the result returned.
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

            return GetObjectAsString(preference.GetDefaultValueAsObject());
        }

        /// <summary>
        /// A wrapper method for the <see cref="JsonConvert.ToString(object)"/>
        /// method. If <paramref name="object"/> is an <see cref="Enum"/>, then
        /// the result of <see cref="Enum.ToString()"/> used with the
        /// <see cref="JsonConvert.ToString(object)"/> method.
        /// </summary>
        /// <param name="object">What to get as a JSON string.</param>
        /// <returns>A JSON string representation of
        /// <paramref name="object"/>.</returns>
        public static string GetObjectAsString(object @object)
        {
            if (@object is null)
            {
                return JsonConvert.Null;
            }

            if (@object is Enum @enum)
            {
                @object = @enum.ToString();
            }

            return JsonConvert.ToString(@object);
        }

        /// <summary>
        /// Returns the <c>Value</c> as valid JSON in a <see cref="string"/> by
        /// calling the <see cref="Preference.GetValueAsObject()"/> method and
        /// passing the result into the <see cref="GetObjectAsString(object)"/>
        /// and the result returned.
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

            return GetObjectAsString(preference.GetValueAsObject());
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
                var prefix = new StringBuilder()
                    .Append(preference.AllowUndefinedValues
                        ? "Suggested values" : "Allowed values")
                    .Append(preference.IsEnum && preference.HasEnumFlags
                        ? " are combinations of (separated by \',\'): "
                        : ": ")
                    .ToString();
                JsoncSerializerHelper.WriteListInComment(
                    indentedTextWriter: context.Writer,
                    list: allowedValues,
                    prefix: prefix,
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
            
            if (preference.DefaultValueIsNull)
            {
                return;
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
