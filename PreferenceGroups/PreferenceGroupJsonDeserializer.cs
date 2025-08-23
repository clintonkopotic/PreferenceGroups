using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for updating a
    /// <see cref="PreferenceGroup"/> by deserializing from a
    /// <see cref="JObject"/>.
    /// </summary>
    public static class PreferenceGroupJsonDeserializer
    {
        /// <summary>
        /// Attempts to update <paramref name="group"/> from
        /// <paramref name="jObject"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jObject"/> is <see langword="null"/> or its
        /// <see cref="JObject.Type"/> is <see cref="JTokenType.Null"/> or its
        /// <see cref="JContainer.Count"/> is zero, then there is nothing to
        /// update from and it just returns.</item>
        /// <item>For each <see cref="Preference.Name"/> in
        /// <paramref name="group"/>, by enumerating over
        /// <see cref="PreferenceGroup.Names"/>, the <see cref="Preference"/> is
        /// updated by calling the <see cref="PreferenceJsonDeserializer
        /// .UpdateFrom(Preference, JObject)"/> method.</item>
        /// </list>
        /// </summary>
        /// <param name="group"></param>
        /// <param name="jObject"></param>
        /// <returns>A <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the <see cref="Preference.Name"/>s that
        /// were updated from <paramref name="jObject"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public static IReadOnlyCollection<string> UpdateFrom(
            PreferenceGroup group, JObject jObject)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (jObject is null || jObject.Type == JTokenType.Null
                || jObject.Count <= 0)
            {
                return null; // Assume nothing to update, leave as is.
            }

            var names = new List<string>();

            foreach (var name in group.Names)
            {
                var preference = group[name];

                if (PreferenceJsonDeserializer.UpdateFrom(preference, jObject))
                {
                    names.Add(name);
                }

                group[name] = preference;
            }
            
            return names;
        }

        /// <summary>
        /// Attempts to update <paramref name="groups"/> from
        /// <paramref name="jArray"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jArray"/> is <see langword="null"/> or its
        /// <see cref="JArray.Type"/> is <see cref="JTokenType.Null"/> or its
        /// <see cref="JContainer.Count"/> is zero, then there is nothing to
        /// update from and it just returns.</item>
        /// <item>For each index of <paramref name="groups"/>, the
        /// <see cref="PreferenceGroup"/> is updated by calling the
        /// <see cref="UpdateFrom(PreferenceGroup, JObject)"/> method. It should
        /// be noted that the least number of indexes between
        /// <paramref name="groups"/> and <paramref name="jArray"/> will be
        /// updated.</item>
        /// </list>
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="jArray"></param>
        /// <returns>A <see cref="IReadOnlyDictionary{TKey, TValue}"/> of
        /// <see cref="int"/> and <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the index of the array and the
        /// <see cref="Preference.Name"/>s that
        /// were updated from the file in <paramref name="groups"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public static IReadOnlyDictionary<int, IReadOnlyCollection<string>>
            UpdateFrom(PreferenceGroup[] groups, JArray jArray)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            if (jArray is null || jArray.Type == JTokenType.Null
                || jArray.Count <= 0)
            {
                return null; // Assume nothing to update, leave as is.
            }

            var changes = new Dictionary<int, IReadOnlyCollection<string>>();

            for (var i = 0; i < groups.Length && i < jArray.Count; i++)
            {
                changes[i] = UpdateFrom(groups[i],
                    JsoncSerializerHelper.ReadAsJObject(jArray[i]));
            }

            return changes;
        }
    }
}
