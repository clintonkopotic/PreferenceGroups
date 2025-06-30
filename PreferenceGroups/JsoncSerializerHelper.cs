using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PreferenceGroups
{
    /// <summary>
    /// A <see langword="static"/> <see langword="class"/> of helper methods for
    /// JSONC (JSON with Comments) files.
    /// </summary>
    public static class JsoncSerializerHelper
    {
        /// <summary>
        /// The default character encoding for a JSONC file, namely
        /// <see cref="Encoding.UTF8"/>.
        /// </summary>
        public static Encoding DefaultEncoding => Encoding.UTF8;

        /// <summary>
        /// The end of an array character, namely <c>']'</c>.
        /// </summary>
        public static char EndArrayChar => ']';

        /// <summary>
        /// The end of an object character, namely <c>'}'</c>.
        /// </summary>
        public static char EndObjectChar => '}';

        /// <summary>
        /// The array and object item separtor character, namely <c>','</c>.
        /// </summary>
        public static char ItemSeparator => ',';

        /// <summary>
        /// The line comment string, namely <c>"// "</c>.
        /// </summary>
        public static string LineCommentPrefix => "// ";

        /// <summary>
        /// The list item seperator string in a comment, namely <c>" | "</c>.
        /// </summary>
        public static string ListInCommentSeparator => " | ";

        /// <summary>
        /// The property name seperator character (what goes after the property
        /// name), namely <c>':'</c>.
        /// </summary>
        public static char PropertyNameSeparator => ':';

        /// <summary>
        /// The start of an array character, namely <c>'['</c>.
        /// </summary>
        public static char StartArrayChar => '[';

        /// <summary>
        /// The start of an object character, namely <c>'{'</c>.
        /// </summary>
        public static char StartObjectChar => '{';

        /// <summary>
        /// Deserializes a JSON string value into a <see cref="string"/> by
        /// checking if <paramref name="jsonStringValue"/> is
        /// <see langword="null"/> or equals <see cref="JsonConvert.Null"/>,
        /// then it returns <see langword="null"/>; otherwise it will use
        /// <see cref="JsonConvert.DeserializeObject{T}(string)"/> with the
        /// generic type of <see cref="string"/>.
        /// </summary>
        /// <param name="jsonStringValue"></param>
        /// <returns></returns>
        public static string GetStringFromJsonStringValue(
            string jsonStringValue)
        {
            if (jsonStringValue is null || JsonConvert.Null.Equals(
                jsonStringValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<string>(jsonStringValue);
        }

        /// <summary>
        /// Serializes <paramref name="string"/> to safely write to a JSON file,
        /// where if <paramref name="string"/> is <see langword="null"/> then
        /// <see cref="JsonConvert.Null"/> is returned; otherwise, the
        /// <see cref="JsonConvert.ToString(string)"/> is used.
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string SerialzieStringValue(string @string)
        {
            if (@string is null)
            {
                return JsonConvert.Null;
            }

            return JsonConvert.ToString(@string);
        }

        /// <summary>
        /// Writes the JSON end of an array to
        /// <paramref name="indentedTextWriter"/>, by first decrementing its
        /// <see cref="IndentedTextWriter.Indent"/> property and then
        /// writing the <see cref="EndArrayChar"/> using the
        /// <see cref="IndentedTextWriter.Write(char)"/> method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteEndArray(IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Indent--;
            indentedTextWriter.Write(EndArrayChar);
        }

        /// <summary>
        /// Writes the JSON end of an object to
        /// <paramref name="indentedTextWriter"/>, by first decrementing its
        /// <see cref="IndentedTextWriter.Indent"/> property and then
        /// writing the <see cref="EndObjectChar"/> using the
        /// <see cref="IndentedTextWriter.Write(char)"/> method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteEndObject(IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Indent--;
            indentedTextWriter.Write(EndObjectChar);
        }

        /// <summary>
        /// Writes <paramref name="comment"/>, line by line and prefixed with
        /// <see cref="LineCommentPrefix"/> for each, to
        /// <paramref name="indentedTextWriter"/> using the
        /// <see cref="IndentedTextWriter.WriteLine(string)"/> method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <param name="comment"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteLineComment(
            IndentedTextWriter indentedTextWriter, string comment)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            if (string.IsNullOrEmpty(comment))
            {
                return;
            }

            var lineReader = new StringReader(comment);
            string line;

            while (!((line = lineReader.ReadLine()) is null))
            {
                indentedTextWriter.WriteLine($"{LineCommentPrefix}{line}");
            }
        }

        /// <summary>
        /// Writes <paramref name="list"/> to
        /// <paramref name="indentedTextWriter"/> by:
        /// <list type="number">
        /// <item>Writing the <see cref="LineCommentPrefix"/>.</item>
        /// <item>Writing <paramref name="prefix"/>, if any.</item>
        /// <item>Writing <paramref name="list"/> by using the
        /// <see cref="string.Join(string, string[])"/> method with
        /// <see cref="ListInCommentSeparator"/>.</item>
        /// <item>Writing <paramref name="postfix"/>, if any.</item>
        /// <item>Writing the end of the line.</item>
        /// </list>
        /// This occurs all on one line.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <param name="list"></param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteListInComment(
            IndentedTextWriter indentedTextWriter, string[] list,
            string prefix = null, string postfix = null)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            if (list is null || list.Length <= 0)
            {
                return;
            }

            indentedTextWriter.Write(LineCommentPrefix);
            indentedTextWriter.Write(prefix);
            indentedTextWriter.Write(string.Join(
                separator: ListInCommentSeparator,
                value: list));
            indentedTextWriter.WriteLine(postfix);
        }

        /// <summary>
        /// Writes the JSON end of an array to
        /// <paramref name="indentedTextWriter"/>, by first decrementing its
        /// <see cref="IndentedTextWriter.Indent"/> property and then
        /// writing the <see cref="EndArrayChar"/> using the
        /// <see cref="IndentedTextWriter.WriteLine(char)"/> method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteLineEndArray(
            IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Indent--;
            indentedTextWriter.WriteLine(EndArrayChar);
        }

        /// <summary>
        /// Writes the JSON end of an object to
        /// <paramref name="indentedTextWriter"/>, by first decrementing its
        /// <see cref="IndentedTextWriter.Indent"/> property and then
        /// writing the <see cref="EndObjectChar"/> using the
        /// <see cref="IndentedTextWriter.WriteLine(char)"/> method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteLineEndObject(
            IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Indent--;
            indentedTextWriter.WriteLine(EndObjectChar);
        }

        /// <summary>
        /// Writes the JSON start of an array to
        /// <paramref name="indentedTextWriter"/>, by first writing the
        /// <see cref="StartArrayChar"/> using the
        /// <see cref="IndentedTextWriter.WriteLine(char)"/> method and then
        /// incrementing its <see cref="IndentedTextWriter.Indent"/> property.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteLineStartArray(
            IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }
            
            indentedTextWriter.WriteLine(StartArrayChar);
            indentedTextWriter.Indent++;
        }

        /// <summary>
        /// Writes the JSON start of an object to
        /// <paramref name="indentedTextWriter"/>, by first writing the
        /// <see cref="StartObjectChar"/> using the
        /// <see cref="IndentedTextWriter.WriteLine(char)"/> method and then
        /// incrementing its <see cref="IndentedTextWriter.Indent"/> property.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteLineStartObject(
            IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.WriteLine(StartObjectChar);
            indentedTextWriter.Indent++;
        }

        /// <summary>
        /// Writes <paramref name="propertyName"/> as a JSON property name to
        /// <paramref name="indentedTextWriter"/> by first using the
        /// <see cref="WriteString(IndentedTextWriter, string)"/> method,
        /// followed by writing <see cref="PropertyNameSeparator"/> and space
        /// characters using the <see cref="IndentedTextWriter.Write(string)"/>
        /// method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <param name="propertyName"></param>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/>
        /// is empty or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> or
        /// <paramref name="propertyName"/> is
        /// <see langword="null"/>.</exception>
        public static void WritePropertyName(
            IndentedTextWriter indentedTextWriter, string propertyName)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(paramName: nameof(propertyName),
                    message: "Cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException(paramName: nameof(propertyName),
                    message: "Cannot constist only of white-space characters.");
            }

            WriteString(indentedTextWriter, propertyName);
            indentedTextWriter.Write($"{PropertyNameSeparator} ");
        }

        /// <summary>
        /// Writes the JSON start of an array to
        /// <paramref name="indentedTextWriter"/>, by first writing the
        /// <see cref="StartArrayChar"/> using the
        /// <see cref="IndentedTextWriter.Write(char)"/> method and then
        /// incrementing its <see cref="IndentedTextWriter.Indent"/> property.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteStartArray(
            IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Write(StartArrayChar);
            indentedTextWriter.Indent++;
        }

        /// <summary>
        /// Writes the JSON start of an object to
        /// <paramref name="indentedTextWriter"/>, by first writing the
        /// <see cref="StartObjectChar"/> using the
        /// <see cref="IndentedTextWriter.Write(char)"/> method and then
        /// incrementing its <see cref="IndentedTextWriter.Indent"/> property.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteStartObject(
            IndentedTextWriter indentedTextWriter)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Write(StartObjectChar);
            indentedTextWriter.Indent++;
        }

        /// <summary>
        /// Writes <paramref name="string"/> to
        /// <paramref name="indentedTextWriter"/> by first using the
        /// <see cref="SerialzieStringValue(string)"/> method and then the
        /// <see cref="IndentedTextWriter.Write(string)"/> method.
        /// </summary>
        /// <param name="indentedTextWriter"></param>
        /// <param name="string"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indentedTextWriter"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteString(IndentedTextWriter indentedTextWriter,
            string @string)
        {
            if (indentedTextWriter is null)
            {
                throw new ArgumentNullException(nameof(indentedTextWriter));
            }

            indentedTextWriter.Write(SerialzieStringValue(@string));
        }
    }
}
