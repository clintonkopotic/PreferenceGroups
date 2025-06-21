using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains helper methods for any <see cref="Preference"/> builder
    /// classes, such as <see cref="StringPreferenceBuilder"/>.
    /// </summary>
    public static class PreferenceBuilderHelper
    {
        /// <summary>
        /// Processes <paramref name="allowedValuesIn"/> by instatiating either
        /// a <see cref="SortedSet{T}"/>, if
        /// <paramref name="sortAllowedValues"/> is <see langword="true"/>, or a
        /// <see cref="HashSet{T}"/> if <paramref name="sortAllowedValues"/> is
        /// <see langword="false"/>. If <paramref name="allowedValuesIn"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned. Any
        /// elements of <paramref name="allowedValuesIn"/> that are
        /// <see langword="null"/> will not be added to the returning
        /// <see cref="IReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<T> ProcessAllowedValuesForClass<T>(
            IEnumerable<T> allowedValuesIn, bool sortAllowedValues)
            where T : class
        {
            IReadOnlyCollection<T> allowedValuesOut = null;

            if (!(allowedValuesIn is null))
            {
                if (sortAllowedValues)
                {
                    var set = new SortedSet<T>();

                    foreach (var allowedValue in allowedValuesIn)
                    {
                        if (!(allowedValue is null))
                        {
                            set.Add(allowedValue);
                        }
                    }

                    allowedValuesOut = set;
                }
                else
                {
                    var set = new HashSet<T>();

                    foreach (var allowedValue in allowedValuesIn)
                    {
                        if (!(allowedValue is null))
                        {
                            set.Add(allowedValue);
                        }
                    }

                    allowedValuesOut = set;
                }
            }

            return allowedValuesOut;
        }
    }
}
