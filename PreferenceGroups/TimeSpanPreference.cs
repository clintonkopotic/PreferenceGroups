using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// An <see cref="TimeSpan"/> <see cref="Preference"/>.
    /// </summary>
    public class TimeSpanPreference : StructPreference<TimeSpan>
    {
        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The name of the <see cref="TimeSpanPreference"/>
        /// and must be not <see langword="null"/>, not empty and not consist
        /// only of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public TimeSpanPreference(string name) : base(name)
        { }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method. It also initializes the
        /// <see cref="Preference.Description"/>,
        /// <see cref="Preference.AllowUndefinedValues"/>,
        /// <see cref="StructPreference{T}.AllowedValues"/> and
        /// <see cref="StructPreference{T}.ValidityProcessor"/> properties.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="description">A description of the
        /// <see cref="Preference"/> that is intended to be shown to the user
        /// and in the file as a comment.</param>
        /// <param name="allowUndefinedValues">Whether or not the Value and
        /// DefaultValue properties can be set to something other than what is
        /// specified in AllowedValues. If AllowedValues is not used (it is
        /// <see langword="null"/> or empty), then
        /// <see cref="Preference.AllowUndefinedValues"/> must be set to
        /// <see langword="true"/>.</param>
        /// <param name="allowedValues">A collection of values that are allowed
        /// for the <see cref="StructPreference{T}.Value"/> and
        /// <see cref="StructPreference{T}.DefaultValue"/> to be set to. If
        /// <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.</param>
        /// <param name="validityProcessor">Used for setting the
        /// <see cref="StructPreference{T}.Value"/> and
        /// <see cref="StructPreference{T}.DefaultValue"/> to ensure that
        /// only <see cref="StructValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public TimeSpanPreference(string name, string description,
            bool allowUndefinedValues,
            IEnumerable<TimeSpan?> allowedValues,
            StructValidityProcessor<TimeSpan> validityProcessor)
            : base(name, description, allowUndefinedValues, allowedValues,
                  validityProcessor)
        { }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method. It also initializes the
        /// <see cref="Preference.Description"/>,
        /// <see cref="Preference.AllowUndefinedValues"/>,
        /// <see cref="StructPreference{T}.AllowedValues"/> and
        /// <see cref="StructPreference{T}.ValidityProcessor"/> properties.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="description">A description of the
        /// <see cref="Preference"/> that is intended to be shown to the user
        /// and in the file as a comment.</param>
        /// <param name="allowUndefinedValues">Whether or not the Value and
        /// DefaultValue properties can be set to something other than what is
        /// specified in AllowedValues. If AllowedValues is not used (it is
        /// <see langword="null"/> or empty), then
        /// <see cref="Preference.AllowUndefinedValues"/> must be set to
        /// <see langword="true"/>.</param>
        /// <param name="allowedValues">A collection of values that are allowed
        /// for the <see cref="StructPreference{T}.Value"/> and
        /// <see cref="StructPreference{T}.DefaultValue"/> to be set to. If
        /// <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.</param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="validityProcessor">Used for setting the
        /// <see cref="StructPreference{T}.Value"/> and
        /// <see cref="StructPreference{T}.DefaultValue"/> to ensure that only
        /// <see cref="StructValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public TimeSpanPreference(string name, string description,
            bool allowUndefinedValues, IEnumerable<TimeSpan?> allowedValues,
            bool sortAllowedValues,
            StructValidityProcessor<TimeSpan> validityProcessor)
            : base(name, description, allowUndefinedValues, allowedValues,
                  sortAllowedValues, validityProcessor)
        { }

        /// <summary>
        /// Uses <see cref="Convert.ToInt32(object)"/> to convert
        /// <paramref name="value"/> to a <see cref="Nullable{T}"/> of
        /// <see cref="TimeSpan"/>. If <paramref name="value"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// converting.</exception>
        public override TimeSpan? ConvertObjectToValue(object value)
            => ConvertObjectToValueBase(value);

        /// <summary>
        /// Uses <see cref="Convert.ToInt32(object)"/> to convert
        /// <paramref name="value"/> to a <see cref="Nullable{T}"/> of
        /// <see cref="TimeSpan"/>. If <paramref name="value"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// converting.</exception>
        public static TimeSpan? ConvertObjectToValueBase(object value)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                if (value is TimeSpan timeSpanValue)
                {
                    return timeSpanValue;
                }
                else if (value is long ticks)
                {
                    return new TimeSpan(ticks);
                }
                else if (value is string @string)
                {
                    return TimeSpan.Parse(@string);
                }

                throw new ArgumentException(paramName: nameof(value),
                    message: $"An unexpected {nameof(Type)} of "
                    + $"{value.GetType()}");
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Converting);
            }
        }
    }
}
