using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for updating a
    /// <see cref="PreferenceStore"/> by deserializing from a
    /// <see cref="JObject"/>.
    /// </summary>
    public static class PreferenceStoreJsonDeserializer
    {
        /// <summary>
        /// Attempts to update <paramref name="store"/> from
        /// <paramref name="jObject"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jObject"/> is <see langword="null"/> or its
        /// <see cref="JObject.Type"/> is <see cref="JTokenType.Null"/> or its
        /// <see cref="JContainer.Count"/> is zero, then there is nothing to
        /// update from and it just returns.</item>
        /// <item>For each <see cref="Preference.Name"/> in
        /// <paramref name="store"/>, by enumerating over
        /// <see cref="PreferenceGroup.Names"/>, the <see cref="Preference"/> is
        /// updated by calling the <see cref="PreferenceJsonDeserializer
        /// .UpdateFrom(Preference, JObject)"/> method.</item>
        /// </list>
        /// </summary>
        /// <param name="store"></param>
        /// <param name="jObject"></param>
        /// <returns>A <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the <see cref="Preference.Name"/>s that
        /// were updated from <paramref name="jObject"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> is
        /// <see langword="null"/>.</exception>
        public static IReadOnlyCollection<string> UpdateFrom(
            PreferenceStore store, JObject jObject)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            if (jObject is null || jObject.Type == JTokenType.Null
                || jObject.Count <= 0)
            {
                return null; // Assume nothing to update, leave as is.
            }

            var names = new List<string>();

            foreach (var name in store.Names)
            {
                if (!jObject.ContainsKey(name))
                {
                    continue;
                }

                var item = store[name];
                var itemJToken = jObject[name];

                if (item.Kind == PreferenceStoreItemKind.Preference
                    && itemJToken.Type != JTokenType.None
                    && itemJToken.Type != JTokenType.Object
                    && itemJToken.Type != JTokenType.Array)
                {
                    if (PreferenceJsonDeserializer.UpdateFrom(
                        item.GetAsPreference(), itemJToken))
                    {
                        names.Add(name);
                    }
                }
                else if (item.Kind == PreferenceStoreItemKind.PreferenceGroup
                    && itemJToken.Type == JTokenType.Object)
                {
                    var updates = PreferenceGroupJsonDeserializer.UpdateFrom(
                        item.GetAsPreferenceGroup(),
                        JsoncSerializerHelper.ReadAsJObject(itemJToken));

                    if (updates != null && updates.Count > 0)
                    {
                        names.Add(name);
                    }
                }
                else if (item.Kind
                    == PreferenceStoreItemKind.ArrayOfPreferenceGroups
                    && itemJToken.Type == JTokenType.Array)
                {
                    var updates = PreferenceGroupJsonDeserializer.UpdateFrom(
                        item.GetAsArrayOfPreferenceGroups(),
                        JsoncSerializerHelper.ReadAsJArray(itemJToken));

                    if (updates != null && updates.Count > 0)
                    {
                        names.Add(name);
                    }
                }
                else if (item.Kind == PreferenceStoreItemKind.PreferenceStore
                    && itemJToken.Type == JTokenType.Object)
                {
                    var updates = UpdateFrom(item.GetAsPreferenceStore(),
                        JsoncSerializerHelper.ReadAsJObject(itemJToken));

                    if (updates != null && updates.Count > 0)
                    {
                        names.Add(name);
                    }
                }
                else if (item.Kind
                    == PreferenceStoreItemKind.ArrayOfPreferenceStores
                    && itemJToken.Type == JTokenType.Array)
                {
                    var updates = UpdateFrom(
                        item.GetAsArrayOfPreferenceStores(),
                        JsoncSerializerHelper.ReadAsJArray(itemJToken));

                    if (updates != null && updates.Count > 0)
                    {
                        names.Add(name);
                    }
                }

                store[name] = item;
            }

            return names;
        }

        /// <summary>
        /// Attempts to update <paramref name="stores"/> from
        /// <paramref name="jArray"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jArray"/> is <see langword="null"/> or its
        /// <see cref="JArray.Type"/> is <see cref="JTokenType.Null"/> or its
        /// <see cref="JContainer.Count"/> is zero, then there is nothing to
        /// update from and it just returns.</item>
        /// <item>For each index of <paramref name="stores"/>, the
        /// <see cref="PreferenceStore"/> is updated by calling the
        /// <see cref="UpdateFrom(PreferenceStore, JObject)"/> method. It should
        /// be noted that the least number of indexes between
        /// <paramref name="stores"/> and <paramref name="jArray"/> will be
        /// updated.</item>
        /// </list>
        /// </summary>
        /// <param name="stores"></param>
        /// <param name="jArray"></param>
        /// <returns>A <see cref="IReadOnlyDictionary{TKey, TValue}"/> of
        /// <see cref="int"/> and <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the index of the array and the
        /// <see cref="Preference.Name"/>s that
        /// were updated from the file in <paramref name="stores"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stores"/> is
        /// <see langword="null"/>.</exception>
        public static IReadOnlyDictionary<int, IReadOnlyCollection<string>>
            UpdateFrom(PreferenceStore[] stores, JArray jArray)
        {
            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            if (jArray is null || jArray.Type == JTokenType.Null
                || jArray.Count <= 0)
            {
                return null; // Assume nothing to update, leave as is.
            }

            var changes = new Dictionary<int, IReadOnlyCollection<string>>();

            for (var i = 0; i < stores.Length && i < jArray.Count; i++)
            {
                changes[i] = UpdateFrom(stores[i],
                    JsoncSerializerHelper.ReadAsJObject(jArray[i]));
            }

            return changes;
        }
    }
}
