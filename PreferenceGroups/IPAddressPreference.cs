using System;
using System.Collections.Generic;
using System.Net;

namespace PreferenceGroups
{
    /// <summary>
    /// An <see cref="IPAddress"/> <see cref="Preference"/>.
    /// </summary>
    public class IPAddressPreference : ClassPreference<IPAddress>
    {
        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The name of the <see cref="IPAddressPreference"/>
        /// and must be not <see langword="null"/>, not empty and not consist
        /// only of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public IPAddressPreference(string name) : base(name)
        { }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method. It also initializes the
        /// <see cref="Preference.Description"/>,
        /// <see cref="Preference.AllowUndefinedValues"/>,
        /// <see cref="ClassPreference{T}.AllowedValues"/> and
        /// <see cref="ClassPreference{T}.ValidityProcessor"/> properties.
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
        /// for the <see cref="ClassPreference{T}.Value"/> and
        /// <see cref="ClassPreference{T}.DefaultValue"/> to be set
        /// to. If <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.</param>
        /// <param name="validityProcessor">Used for setting the
        /// <see cref="ClassPreference{T}.Value"/> and
        /// <see cref="ClassPreference{T}.DefaultValue"/> to ensure that
        /// only <see cref="ClassValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public IPAddressPreference(string name, string description,
            bool allowUndefinedValues,
            IEnumerable<IPAddress> allowedValues,
            ClassValidityProcessor<IPAddress> validityProcessor)
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
        /// <see cref="ClassPreference{T}.AllowedValues"/> and
        /// <see cref="ClassPreference{T}.ValidityProcessor"/> properties.
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
        /// for the <see cref="ClassPreference{T}.Value"/> and
        /// <see cref="ClassPreference{T}.DefaultValue"/> to be set
        /// to. If <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.</param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="validityProcessor">Used for setting the
        /// <see cref="ClassPreference{T}.Value"/> and
        /// <see cref="ClassPreference{T}.DefaultValue"/> to ensure that
        /// only <see cref="ClassValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public IPAddressPreference(string name, string description,
            bool allowUndefinedValues,
            IEnumerable<IPAddress> allowedValues, bool sortAllowedValues,
            ClassValidityProcessor<IPAddress> validityProcessor)
            : base(name, description, allowUndefinedValues, allowedValues,
                  sortAllowedValues, validityProcessor)
        { }

        /// <summary>
        /// If <paramref name="value"/> is an <see cref="Array"/> of
        /// <see cref="byte"/>, then it is returned. If <paramref name="value"/>
        /// is an <see cref="Array"/> of <see cref="char"/>, then the result of
        /// calling the
        /// <see cref="Convert.FromBase64CharArray(char[], int, int)"/> method,
        /// for the whole <see cref="Array"/>, is returned. If
        /// <paramref name="value"/> is a <see cref="string"/>, then the result
        /// of calling the <see cref="Convert.FromBase64String(string)"/> method
        /// is returned. If <paramref name="value"/> is <see langword="null"/>,
        /// then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// calling <see cref="object.ToString()"/>.</exception>
        public override IPAddress ConvertObjectToValue(object value)
            => ConvertObjectToValueBase(value);

        /// <summary>
        /// If <paramref name="value"/> is an <see cref="Array"/> of
        /// <see cref="byte"/>, then it is returned. If <paramref name="value"/>
        /// is an <see cref="Array"/> of <see cref="char"/>, then the result of
        /// calling the
        /// <see cref="Convert.FromBase64CharArray(char[], int, int)"/> method,
        /// for the whole <see cref="Array"/>, is returned. If
        /// <paramref name="value"/> is a <see cref="string"/>, then the result
        /// of calling the <see cref="Convert.FromBase64String(string)"/> method
        /// is returned. If <paramref name="value"/> is <see langword="null"/>,
        /// then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// calling <see cref="object.ToString()"/>.</exception>
        public static IPAddress ConvertObjectToValueBase(object value)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                if (value is IPAddress byteArray)
                {
                    return byteArray;
                }
                else if (value is string @string)
                {
                    return IPAddress.Parse(@string);
                }

                throw new InvalidOperationException("Cannot convert to an "
                    + $"{nameof(IPAddress)} from a {nameof(Type)} of "
                    + $"{value.GetType()}.");
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Converting);
            }
        }
    }
}
