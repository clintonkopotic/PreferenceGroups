using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains helper methods for any <see cref="Preference"/> builder
    /// classes, such as <see cref="StringPreferenceBuilder"/>.
    /// </summary>
    internal static class PreferenceBuilderHelpers
    {
        /// <summary>
        /// Processes <paramref name="allowedValuesIn"/> by instatiating either
        /// a <see cref="SortedSet{T}"/>, if
        /// <paramref name="sortAllowedValues"/> is <see langword="true"/>, or a
        /// <see cref="HashSet{T}"/> if <paramref name="sortAllowedValues"/> is
        /// <see langword="false"/>. If <paramref name="allowedValuesIn"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<T> ProcessAllowedValues<T>(
            IEnumerable<T> allowedValuesIn, bool sortAllowedValues)
        {
            IReadOnlyCollection<T> allowedValuesOut = null;

            if (!(allowedValuesIn is null))
            {
                if (sortAllowedValues)
                {
                    allowedValuesOut = new SortedSet<T>(allowedValuesIn);
                }
                else
                {
                    allowedValuesOut = new HashSet<T>(allowedValuesIn);
                }
            }

            return allowedValuesOut;
        }
    }
}
