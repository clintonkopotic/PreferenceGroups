using System;
using System.CodeDom.Compiler;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for serializing a
    /// <see cref="PreferenceStore"/> into JSONC data including the metadata
    /// being written in comments, like the
    /// <see cref="PreferenceStore.Description"/> property.
    /// </summary>
    /// <remarks>See <see href="https://www.jsonc.org">jsonc.org</see> for
    /// further details about a JSONC file format.</remarks>
    public static class PreferenceStoreJsoncSerializer
    {
        /// <summary>
        /// Serializes <paramref name="store"/> into JSONC using
        /// <paramref name="context"/> by calling <see cref="WriteAnyComments(
        /// JsoncSerializationContext, PreferenceStore)"/>,
        /// then if <see cref="PreferenceStore.Count"/> of
        /// <paramref name="store"/> is zero then the
        /// <see cref="JsoncSerializationContext.WriteEmptyObject()"/> method is
        /// called, otherwise the following are called in sequence:
        /// <list type="number">
        /// <item><see cref="JsoncSerializationContext.StartObject()"/></item>
        /// <item>For each <see cref="Preference"/> in <paramref name="store"/>
        /// the <see cref="PreferenceJsoncSerializer.Serialize(
        /// JsoncSerializationContext, Preference)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.EndObject()"/></item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="store"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="store"/> is
        /// <see langword="null"/>.</exception>
        public static void Serialize(JsoncSerializationContext context,
            PreferenceStore store)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            WriteAnyComments(context, store);

            if (store.Count <= 0)
            {
                context.WriteEmptyObject();

                return;
            }

            context.StartObject();

            foreach (var itemWithName in store)
            {
                var name = itemWithName.Key;
                var item = itemWithName.Value;
                context.WriteItemSeparator();
                PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(item);
                context.CommentsWritten = false;

                if (item.Kind == PreferenceStoreItemKind.Preference)
                {
                    PreferenceJsoncSerializer.Serialize(context,
                        item.GetAsPreference());
                }
                else
                {
                    context.ResetNeedToWriteLine();
                    var description = item.Description;

                    if (!string.IsNullOrEmpty(description))
                    {
                        context.WriteEmptyLineIfNeeded();
                        JsoncSerializerHelper.WriteLineComment(
                            indentedTextWriter: context.Writer,
                            comment: description);
                    }

                    context.NeedToWriteLine = false;
                    context.CommentsWritten = true;
                    JsoncSerializerHelper.WritePropertyName(
                        indentedTextWriter: context.Writer,
                        propertyName: Preference.ProcessNameOrThrowIfInvalid(
                            name, nameof(name)));

                    switch (item.Kind)
                    {
                        case PreferenceStoreItemKind.PreferenceGroup:
                            PreferenceGroupJsoncSerializer.Serialize(context,
                                item.GetAsPreferenceGroup());

                            break;
                        case PreferenceStoreItemKind.ArrayOfPreferenceGroups:
                            PreferenceGroupJsoncSerializer.Serialize(context,
                                item.GetAsArrayOfPreferenceGroups());

                            break;
                        case PreferenceStoreItemKind.PreferenceStore:
                            Serialize(context, item.GetAsPreferenceStore());

                            break;
                        case PreferenceStoreItemKind.ArrayOfPreferenceStores:
                            Serialize(context,
                                item.GetAsArrayOfPreferenceStores());

                            break;
                        default:
                            throw new InvalidOperationException("The following "
                                + "is an unexpected "
                                + $"{nameof(PreferenceStoreItemKind)} of "
                                + $"{item.Kind}.");
                    }
                }

                context.IncrementCurrentCount();
            }

            context.EndObject();
        }

        /// <summary>
        /// Serializes <paramref name="stores"/> into JSONC using
        /// <paramref name="context"/> where if <see cref="Array.Length"/> of
        /// <paramref name="stores"/> is zero then the
        /// <see cref="JsoncSerializationContext.WriteEmptyArray()"/> method is
        /// called, otherwise the following are called in sequence:
        /// <list type="number">
        /// <item><see cref="JsoncSerializationContext.StartArray()"/></item>
        /// <item>For each <see cref="PreferenceStore"/> in
        /// <paramref name="stores"/> the <see cref="Serialize(
        /// JsoncSerializationContext, PreferenceStore)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.EndArray()"/></item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="stores"/> is
        /// <see langword="null"/>.</exception>
        public static void Serialize(JsoncSerializationContext context,
            PreferenceStore[] stores)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            if (stores.Length <= 0)
            {
                context.WriteEmptyArray();

                return;
            }

            context.StartArray();

            foreach (var group in stores)
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
        /// JsoncSerializationContext, PreferenceStore)"/>.</item>
        /// <item><see cref="JsoncSerializationContext.CommentsWritten"/> is set
        /// to <see langword="true"/>.</item>
        /// <item><see cref="JsoncSerializationContext.NeedToWriteLine"/> is set
        /// to <see langword="false"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="store"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="store"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteAnyComments(JsoncSerializationContext context,
            PreferenceStore store)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            if (!context.CommentsWritten)
            {
                context.ResetNeedToWriteLine();
                WriteDescriptionInComment(context, store);
                context.CommentsWritten = true;
                context.NeedToWriteLine = false;
            }
        }

        /// <summary>
        /// If <see cref="PreferenceStore.Description"/> does not return
        /// <see langword="null"/> and is not an empty <see cref="string"/>,
        /// then its result is called with the
        /// <see cref="JsoncSerializerHelper.WriteLineComment(
        /// IndentedTextWriter, string)"/> method after the
        /// <see cref="JsoncSerializationContext.WriteEmptyLineIfNeeded()"/>
        /// method is called.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="store"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> or <paramref name="store"/> is
        /// <see langword="null"/>.</exception>
        public static void WriteDescriptionInComment(
            JsoncSerializationContext context, PreferenceStore store)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            var description = store.Description;

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
