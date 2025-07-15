using System;
using System.CodeDom.Compiler;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for serializing a
    /// <see cref="PreferenceGroup"/> into JSONC data including the metadata
    /// being written in comments, like the
    /// <see cref="PreferenceGroup.Description"/> property.
    /// </summary>
    /// <remarks>See <see href="https://www.jsonc.org">jsonc.org</see> for
    /// further details about a JSONC file format.</remarks>
    public static class PreferenceGroupJsoncSerializer
    {
        /// <summary>
        /// Serializes <paramref name="group"/> into JSONC using
        /// <paramref name="context"/> by calling <see cref="WriteAnyComments(
        /// JsoncSerializationContext, PreferenceGroup)"/>,
        /// then if <see cref="PreferenceGroup.Count"/> of
        /// <paramref name="group"/> is zero then the
        /// <see cref="JsoncSerializationContext.WriteEmptyObject()"/> method is
        /// called, otherwise the following are called in sequence:
        /// <list type="number">
        /// <item><see cref="JsoncSerializationContext.StartObject()"/></item>
        /// <item>For each <see cref="Preference"/> in <paramref name="group"/>
        /// the <see cref="PreferenceJsoncSerializer.Serialize(
        /// JsoncSerializationContext, Preference)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.EndObject()"/></item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public static void Serialize(JsoncSerializationContext context,
            PreferenceGroup group)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            WriteAnyComments(context, group);

            if (group.Count <= 0)
            {
                context.WriteEmptyObject();

                return;
            }

            context.StartObject();

            foreach (var preference in group)
            {
                context.WriteItemSeparator();
                context.CommentsWritten = false;
                PreferenceJsoncSerializer.Serialize(context, preference);
                context.IncrementCurrentCount();
            }

            context.EndObject();
        }

        /// <summary>
        /// Serializes <paramref name="groups"/> into JSONC using
        /// <paramref name="context"/> where if <see cref="Array.Length"/> of
        /// <paramref name="groups"/> is zero then the
        /// <see cref="JsoncSerializationContext.WriteEmptyArray()"/> method is
        /// called, otherwise the following are called in sequence:
        /// <list type="number">
        /// <item><see cref="JsoncSerializationContext.StartArray()"/></item>
        /// <item>For each <see cref="PreferenceGroup"/> in
        /// <paramref name="groups"/> the <see cref="Serialize(
        /// JsoncSerializationContext, PreferenceGroup)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.EndArray()"/></item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public static void Serialize(JsoncSerializationContext context,
            PreferenceGroup[] groups)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            if (groups.Length <= 0)
            {
                context.WriteEmptyArray();

                return;
            }

            context.StartArray();

            foreach (var group in groups)
            {
                context.WriteItemSeparator();
                context.CommentsWritten = false;
                Serialize(context, group);
                context.IncrementCurrentCount();
            }

            context.EndArray();
        }

        /// <summary>
        /// If <see cref="JsoncSerializationContext.CommentsWritten"/> is
        /// <see langword="false"/>, then the following are called in sequence:
        /// <list type="number">
        /// <item><see cref="JsoncSerializationContext.ResetNeedToWriteLine()"/>
        /// from <paramref name="context"/>.</item>
        /// <item><see cref="WriteDescriptionInComment(
        /// JsoncSerializationContext, PreferenceGroup)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.CommentsWritten"/> is set
        /// to <see langword="true"/>.</item>
        /// <item><see cref="JsoncSerializationContext.NeedToWriteLine"/> is set
        /// to <see langword="false"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteAnyComments(JsoncSerializationContext context,
            PreferenceGroup group)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (!context.CommentsWritten)
            {
                context.ResetNeedToWriteLine();
                WriteDescriptionInComment(context, group);
                context.CommentsWritten = true;
                context.NeedToWriteLine = false;
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
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteDescriptionInComment(
            JsoncSerializationContext context, PreferenceGroup group)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            var description = group.Description;

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
