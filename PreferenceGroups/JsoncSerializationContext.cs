using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Facilitates serializing to formatted JSONC by using
    /// <see cref="IndentedTextWriter"/> with the <see cref="InnerWriter"/>.
    /// </summary>
    public class JsoncSerializationContext : IDisposable
    {
        private bool _disposedValue;

        /// <summary>
        /// Whether comments have already been written.
        /// </summary>
        public bool CommentsWritten { get; set; } = false;

        /// <summary>
        /// How many items in the <see cref="CurrentType"/> have been written.
        /// </summary>
        public int CurrentCount
        {
            get
            {
                (_, var count) = CurrentTypeAndCount;

                return count;
            }
        }

        /// <summary>
        /// What is the <see cref="JTokenType"/> being written, whether it is an
        /// <see cref="JTokenType.Array"/> or an
        /// <see cref="JTokenType.Object"/>. This defaults to
        /// <see cref="JTokenType.None"/>.
        /// </summary>
        public JTokenType CurrentType
        {
            get
            {
                (var type, _) = CurrentTypeAndCount;

                return type;
            }
        }

        /// <summary>
        /// A <see cref="Tuple{T1, T2}"/> with <see cref="CurrentType"/> and
        /// followed by <see cref="CurrentCount"/>.
        /// </summary>
        public (JTokenType, int) CurrentTypeAndCount
            => Stack.Count <= 0 ? (JTokenType.None, 0) : Stack.Peek();

        /// <summary>
        /// The underlying <see cref="TextWriter"/> that <see cref="Writer"/>
        /// uses to write out formatted JSONC.
        /// </summary>
        public TextWriter InnerWriter { get; }

        /// <summary>
        /// Signifies that a <see cref="IndentedTextWriter.WriteLine()"/> is
        /// pending.
        /// </summary>
        public bool NeedToWriteLine { get; set; } = false;

        /// <summary>
        /// The <see cref="string"/> used for each indentation level of
        /// formatting. Defaults
        /// to <see cref="IndentedTextWriter.DefaultTabString"/>.
        /// </summary>
        public string TabString { get; }

        /// <summary>
        /// Tracks the <see cref="CurrentTypeAndCount"/> for formatting
        /// purposes.
        /// </summary>
        private Stack<(JTokenType, int)> Stack { get; }
            = new Stack<(JTokenType, int)>();

        /// <summary>
        /// What is used to write the JSONC.
        /// </summary>
        public IndentedTextWriter Writer { get; }

        /// <summary>
        /// Instantiates using <paramref name="innerWriter"/> with a default
        /// <see cref="TabString"/> of
        /// <see cref="IndentedTextWriter.DefaultTabString"/>.
        /// </summary>
        /// <param name="innerWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerWriter"/> is
        /// <see langword="null"/>.</exception>
        public JsoncSerializationContext(TextWriter innerWriter)
            : this(innerWriter, IndentedTextWriter.DefaultTabString)
        { }

        /// <summary>
        /// Instantiates using <paramref name="innerWriter"/> with a
        /// <see cref="TabString"/> being set to the result of instatiating a
        /// <see cref="string"/> with <see cref="String(char, int)"/> with the
        /// provided <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/>.
        /// </summary>
        /// <param name="innerWriter"></param>
        /// <param name="indentChar"></param>
        /// <param name="indentDepth"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerWriter"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public JsoncSerializationContext(TextWriter innerWriter,
            char indentChar, int indentDepth)
            : this(innerWriter, new string(indentChar, indentDepth))
        { }

        /// <summary>
        /// Instantiates using <paramref name="innerWriter"/> with
        /// <paramref name="tabString"/>.
        /// </summary>
        /// <param name="innerWriter"></param>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerWriter"/> or <paramref name="tabString"/> is
        /// <see langword="null"/>.</exception>
        public JsoncSerializationContext(TextWriter innerWriter,
            string tabString)
        {
            if (innerWriter is null)
            {
                throw new ArgumentNullException(nameof(innerWriter));
            }

            if (tabString is null)
            {
                throw new ArgumentNullException(nameof(tabString));
            }

            InnerWriter = innerWriter;
            TabString = tabString;
            Writer = new IndentedTextWriter(InnerWriter, TabString)
            {
                Indent = 0
            };
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in
            // 'Dispose(bool disposing)' method.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Calls <see cref="TextWriter.Dispose()"/> on <see cref="Writer"/> if
        /// this method has not been called already and
        /// <paramref name="disposing"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Writer.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Writes the end of an array by using the <see cref
        /// ="JsoncSerializerHelper.WriteEndArray(IndentedTextWriter)"/> and
        /// maintains <see cref="CurrentTypeAndCount"/>.
        /// </summary>
        public void EndArray()
        {
            Writer.WriteLine();
            JsoncSerializerHelper.WriteEndArray(Writer);
            _ = Stack.Pop();
        }

        /// <summary>
        /// Writes the end of an array by using the <see cref
        /// ="JsoncSerializerHelper.WriteEndObject(IndentedTextWriter)"/> and
        /// maintains <see cref="CurrentTypeAndCount"/>.
        /// </summary>
        public void EndObject()
        {
            Writer.WriteLine();
            JsoncSerializerHelper.WriteEndObject(Writer);
            _ = Stack.Pop();
        }

        /// <summary>
        /// Flushes <see cref="Writer"/>.
        /// </summary>
        public void Flush() => Writer.Flush();

        /// <summary>
        /// Increments <see cref="CurrentCount"/> if <see cref="CurrentType"/>
        /// is not <see cref="JTokenType.None"/>.
        /// </summary>
        public void IncrementCurrentCount()
        {
            if (Stack.Count <= 0)
            {
                return;
            }

            (var type, var count) = Stack.Pop();
            count++;
            Stack.Push((type, count));
        }

        /// <summary>
        /// Resets <see cref="NeedToWriteLine"/> based on
        /// <see cref="CurrentTypeAndCount"/>, namely
        /// <see cref="NeedToWriteLine"/> is only <see langword="true"/> when
        /// <see cref="CurrentCount"/> is positve and <see cref="CurrentType"/>
        /// is either <see cref="JTokenType.Object"/> or
        /// <see cref="JTokenType.Array"/>.
        /// </summary>
        public void ResetNeedToWriteLine()
        {
            (var currentType, var currentCount) = CurrentTypeAndCount;
            NeedToWriteLine = currentCount > 0
                && (currentType == JTokenType.Object
                || currentType == JTokenType.Array);
        }

        /// <summary>
        /// Serializes <paramref name="preference"/> by calling
        /// <see cref="PreferenceJsoncSerializer.Serialize(
        /// JsoncSerializationContext, Preference)"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public void Serialize(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            PreferenceJsoncSerializer.Serialize(this, preference);
        }

        /// <summary>
        /// Serializes <paramref name="group"/> by calling
        /// <see cref="PreferenceGroupJsoncSerializer.Serialize(
        /// JsoncSerializationContext, PreferenceGroup)"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        public void Serialize(PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            PreferenceGroupJsoncSerializer.Serialize(this, group);
        }

        /// <summary>
        /// Serializes <paramref name="groups"/> by calling
        /// <see cref="PreferenceGroupJsoncSerializer.Serialize(
        /// JsoncSerializationContext, PreferenceGroup[])"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void Serialize(PreferenceGroup[] groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            PreferenceGroupJsoncSerializer.Serialize(this, groups);
        }

        /// <summary>
        /// Serializes <paramref name="preference"/> by calling
        /// <see cref="Serialize(Preference)"/>and then flushes
        /// <see cref="Writer"/> by calling <see cref="Flush()"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public void SerializeAndFlush(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            Serialize(preference);
            Flush();
        }

        /// <summary>
        /// Serializes <paramref name="group"/> by calling
        /// <see cref="Serialize(PreferenceGroup)"/>and then flushes
        /// <see cref="Writer"/> by calling <see cref="Flush()"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public void SerializeAndFlush(PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Serialize(group);
            Flush();
        }

        /// <summary>
        /// Serializes <paramref name="groups"/> by calling
        /// <see cref="Serialize(PreferenceGroup[])"/>and then flushes
        /// <see cref="Writer"/> by calling <see cref="Flush()"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public void SerializeAndFlush(PreferenceGroup[] groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            Serialize(groups);
            Flush();
        }

        /// <summary>
        /// Writes the start of an array by using the <see cref
        /// ="JsoncSerializerHelper.WriteStartArray(IndentedTextWriter)"/> and
        /// maintains <see cref="CurrentTypeAndCount"/>.
        /// </summary>
        public void StartArray()
        {
            Stack.Push((JTokenType.Array, 0));
            JsoncSerializerHelper.WriteLineStartArray(Writer);
        }

        /// <summary>
        /// Writes the start of an array by using the <see cref
        /// ="JsoncSerializerHelper.WriteStartObject(IndentedTextWriter)"/> and
        /// maintains <see cref="CurrentTypeAndCount"/>.
        /// </summary>
        public void StartObject()
        {
            Stack.Push((JTokenType.Object, 0));
            JsoncSerializerHelper.WriteLineStartObject(Writer);
        }

        /// <summary>
        /// Writes the an empty array by calling the <see cref
        /// ="JsoncSerializerHelper.WriteStartArray(IndentedTextWriter)"/>
        /// followed by <see cref="JsoncSerializerHelper.WriteEndArray(
        /// IndentedTextWriter)"/> and does not change
        /// <see cref="CurrentTypeAndCount"/>.
        /// </summary>
        public void WriteEmptyArray()
        {
            JsoncSerializerHelper.WriteStartArray(Writer);
            JsoncSerializerHelper.WriteEndArray(Writer);
        }

        /// <summary>
        /// Writes an empty line only if <see cref="NeedToWriteLine"/> is
        /// <see langword="true"/> and if so <see cref="NeedToWriteLine"/> will
        /// then be set to <see langword="false"/>.
        /// </summary>
        public void WriteEmptyLineIfNeeded()
        {
            if (NeedToWriteLine)
            {
                Writer.WriteLineNoTabs(null);
                NeedToWriteLine = false;
            }
        }

        /// <summary>
        /// Writes the an empty object by calling the <see cref
        /// ="JsoncSerializerHelper.WriteStartObject(IndentedTextWriter)"/>
        /// followed by <see cref="JsoncSerializerHelper.WriteEndObject(
        /// IndentedTextWriter)"/> and does not change
        /// <see cref="CurrentTypeAndCount"/>.
        /// </summary>
        public void WriteEmptyObject()
        {
            JsoncSerializerHelper.WriteStartObject(Writer);
            JsoncSerializerHelper.WriteEndObject(Writer);
        }

        /// <summary>
        /// Only writes <see cref="JsoncSerializerHelper.ItemSeparator"/> using
        /// <see cref="IndentedTextWriter.WriteLine(char)"/> when
        /// <see cref="CurrentCount"/> is positive.
        /// </summary>
        public void WriteItemSeparator()
        {
            if (CurrentCount > 0)
            {
                Writer.WriteLine(JsoncSerializerHelper.ItemSeparator);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="preference"/>
        /// serialized to JSONC using <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/> for the <see cref="TabString"/> using
        /// <see cref="String(char, int)"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="indentChar"></param>
        /// <param name="indentDepth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public static string SerializeToString(Preference preference,
            char indentChar, int indentDepth)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (indentDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indentDepth),
                    indentDepth, "Cannot be negative.");
            }

            return SerializeToString(preference,
                new string(indentChar, indentDepth));
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="preference"/>
        /// serialized to JSONC using <paramref name="tabString"/> for
        /// formatting.
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> or <paramref name="tabString"/> is
        /// <see langword="null"/>.</exception>
        public static string SerializeToString(Preference preference,
            string tabString)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (tabString is null)
            {
                throw new ArgumentNullException(nameof(tabString));
            }

            string result;

            using (var stringWriter = new StringWriter())
            {
                using (var context = new JsoncSerializationContext(
                    stringWriter, tabString))
                {
                    context.SerializeAndFlush(preference);
                    result = stringWriter.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="preference"/>
        /// serialized to JSONC using
        /// <see cref="IndentedTextWriter.DefaultTabString"/> for the
        /// <see cref="TabString"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static string SerializeToString(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            return SerializeToString(preference,
                IndentedTextWriter.DefaultTabString);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="group"/>
        /// serialized to JSONC using <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/> for the <see cref="TabString"/> using
        /// <see cref="String(char, int)"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="indentChar"></param>
        /// <param name="indentDepth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public static string SerializeToString(PreferenceGroup group,
            char indentChar, int indentDepth)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return SerializeToString(group,
                new string(indentChar, indentDepth));
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="group"/>
        /// serialized to JSONC using <paramref name="tabString"/> for
        /// formatting.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="group"/> or <paramref name="tabString"/> is
        /// <see langword="null"/>.</exception>
        public static string SerializeToString(PreferenceGroup group,
            string tabString)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            string result;

            using (var stringWriter = new StringWriter())
            {
                using (var context = new JsoncSerializationContext(
                    stringWriter, tabString))
                {
                    context.SerializeAndFlush(group);
                    result = stringWriter.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="group"/>
        /// serialized to JSONC using
        /// <see cref="IndentedTextWriter.DefaultTabString"/> for the
        /// <see cref="TabString"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        public static string SerializeToString(PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return SerializeToString(group,
                IndentedTextWriter.DefaultTabString);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="groups"/>
        /// serialized to JSONC using <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/> for the <see cref="TabString"/> using
        /// <see cref="String(char, int)"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="indentChar"></param>
        /// <param name="indentDepth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public static string SerializeToString(PreferenceGroup[] groups,
            char indentChar, int indentDepth)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            return SerializeToString(groups,
                new string(indentChar, indentDepth));
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="groups"/>
        /// serialized to JSONC using <paramref name="tabString"/> for
        /// formatting.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="groups"/> or <paramref name="tabString"/> is
        /// <see langword="null"/>.</exception>
        public static string SerializeToString(PreferenceGroup[] groups,
            string tabString)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            string result;

            using (var stringWriter = new StringWriter())
            {
                using (var context = new JsoncSerializationContext(
                    stringWriter, tabString))
                {
                    context.SerializeAndFlush(groups);
                    result = stringWriter.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="groups"/>
        /// serialized to JSONC using
        /// <see cref="IndentedTextWriter.DefaultTabString"/> for the
        /// <see cref="TabString"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public static string SerializeToString(PreferenceGroup[] groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            return SerializeToString(groups,
                IndentedTextWriter.DefaultTabString);
        }
    }
}
