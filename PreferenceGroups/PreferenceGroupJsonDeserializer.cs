using System;
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
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(PreferenceGroup group, JObject jObject)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (jObject is null || jObject.Type == JTokenType.Null
                || jObject.Count <= 0)
            {
                return; // Assume nothing to update, leave as is.
            }

            foreach (var name in group.Names)
            {
                var preference = group[name];
                PreferenceJsonDeserializer.UpdateFrom(preference, jObject);
                group[name] = preference;
            }
        }
    }
}
