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
    /// <remarks>See <see href="https://www.jsonc.org">jsonc.org</see> for
    /// further details about a JSONC file format.</remarks>
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
        /// to <see cref="JsoncSerializerHelper.DefaultTabString"/>.
        /// </summary>
        public string TabString { get; }

        /// <summary>
        /// Tracks the <see cref="CurrentTypeAndCount"/> for formatting
        /// purposes.
        /// </summary>
        protected Stack<(JTokenType, int)> Stack { get; }
            = new Stack<(JTokenType, int)>();

        /// <summary>
        /// What is used to write the JSONC.
        /// </summary>
        public IndentedTextWriter Writer { get; }

        /// <summary>
        /// Instantiates using <paramref name="innerWriter"/> with a
        /// <see cref="TabString"/> being set to the result of calling the
        /// <see cref="JsoncSerializerHelper.GetTabString(char, int)"/> method
        /// with <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/>.
        /// </summary>
        /// <param name="innerWriter"></param>
        /// <param name="indentChar">The indent character for the tab string
        /// that will be repeated <paramref name="indentDepth"/> number of
        /// times.</param>
        /// <param name="indentDepth">The number of times to repeat
        /// <paramref name="indentChar"/> for the tab string. Must not be a
        /// negative number and if it is zero then <see cref="string.Empty"/> is
        /// returned.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerWriter"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public JsoncSerializationContext(TextWriter innerWriter,
            char indentChar, int indentDepth)
            : this(innerWriter,
                  JsoncSerializerHelper.GetTabString(indentChar, indentDepth))
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

        /// <summary>
        /// Instantiates using <paramref name="innerWriter"/> with a default
        /// <see cref="TabString"/> of
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/>.
        /// </summary>
        /// <param name="innerWriter"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerWriter"/> is
        /// <see langword="null"/>.</exception>
        public JsoncSerializationContext(TextWriter innerWriter)
            : this(innerWriter, JsoncSerializerHelper.DefaultTabString)
        { }

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
        /// Serializes <paramref name="store"/> by calling
        /// <see cref="PreferenceStoreJsoncSerializer.Serialize(
        /// JsoncSerializationContext, PreferenceStore)"/>.
        /// </summary>
        /// <param name="store"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="store"/> is <see langword="null"/>.</exception>
        public void Serialize(PreferenceStore store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            PreferenceStoreJsoncSerializer.Serialize(this, store);
        }

        /// <summary>
        /// Serializes <paramref name="stores"/> by calling
        /// <see cref="PreferenceStoreJsoncSerializer.Serialize(
        /// JsoncSerializationContext, PreferenceStore[])"/>.
        /// </summary>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void Serialize(PreferenceStore[] stores)
        {
            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            PreferenceStoreJsoncSerializer.Serialize(this, stores);
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
    }
}
