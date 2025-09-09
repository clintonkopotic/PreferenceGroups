using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// An <see cref="sbyte"/> <see cref="Preference"/>.
    /// </summary>
    public class SBytePreference : StructPreference<sbyte>
    {
        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The name of the <see cref="SBytePreference"/>
        /// and must be not <see langword="null"/>, not empty and not consist
        /// only of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public SBytePreference(string name) : base(name)
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
        public SBytePreference(string name, string description,
            bool allowUndefinedValues,
            IEnumerable<sbyte?> allowedValues,
            StructValidityProcessor<sbyte> validityProcessor)
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
        public SBytePreference(string name, string description,
            bool allowUndefinedValues, IEnumerable<sbyte?> allowedValues,
            bool sortAllowedValues,
            StructValidityProcessor<sbyte> validityProcessor)
            : base(name, description, allowUndefinedValues, allowedValues,
                  sortAllowedValues, validityProcessor)
        { }

        /// <summary>
        /// Uses <see cref="Convert.ToSByte(object)"/> to convert
        /// <paramref name="value"/> to a <see cref="Nullable{T}"/> of
        /// <see cref="sbyte"/>. If <paramref name="value"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// converting.</exception>
        public override sbyte? ConvertObjectToValue(object value)
            => ConvertObjectToValueBase(value);

        /// <summary>
        /// Uses <see cref="Convert.ToSByte(object)"/> to convert
        /// <paramref name="value"/> to a <see cref="Nullable{T}"/> of
        /// <see cref="sbyte"/>. If <paramref name="value"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// converting.</exception>
        public static sbyte? ConvertObjectToValueBase(object value)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                if (value is sbyte sByteValue)
                {
                    return sByteValue;
                }

                return Convert.ToSByte(value);
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Converting);
            }
        }
    }
}
