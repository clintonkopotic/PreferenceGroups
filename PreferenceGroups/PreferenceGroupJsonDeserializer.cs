using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for updating a
    /// <see cref="PreferenceGroup"/> by deserializing from a <see cref="JObject"/>.
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

            List<string> names = new List<string>();

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
    }
}
