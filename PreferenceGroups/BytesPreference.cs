using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreferenceGroups
{
    /// <summary>
    /// An <see cref="Array"/> of <see cref="byte"/> <see cref="Preference"/>.
    /// </summary>
    public class BytesPreference : ClassPreference<byte[]>
    {
        /// <summary>
        /// The private backing store for <see cref="DefaultValue"/>.
        /// </summary>
        private byte[] _defaultValue;

        /// <summary>
        /// The private backing store for <see cref="Value"/>.
        /// </summary>
        private byte[] _value;

        /// <summary>
        /// The default value to set the <see cref="Value"/> with.
        /// </summary>
        public override byte[] DefaultValue
        {
            get => _defaultValue;
            set
            {
                try
                {
                    _defaultValue = ValidityProcessForSetValue(Name, value,
                        ValidityProcessor, AllowUndefinedValues, AllowedValues);
                }
                catch (SetValueException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new SetValueException(ex,
                        SetValueStepFailure.SettingValue);
                }
            }
        }

        /// <inheritdoc/>
        public override byte[] Value
        {
            get => _value;
            set
            {
                try
                {
                    _value = ValidityProcessForSetValue(Name, value,
                        ValidityProcessor, AllowUndefinedValues, AllowedValues);
                }
                catch (SetValueException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new SetValueException(ex,
                        SetValueStepFailure.SettingValue);
                }
            }
        }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The name of the <see cref="BytesPreference"/>
        /// and must be not <see langword="null"/>, not empty and not consist
        /// only of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public BytesPreference(string name) : base(name)
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
        /// for the <see cref="Value"/> and <see cref="DefaultValue"/> to be set
        /// to. If <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.</param>
        /// <param name="validityProcessor">Used for setting the
        /// <see cref="Value"/> and <see cref="DefaultValue"/> to ensure that
        /// only <see cref="ClassValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public BytesPreference(string name, string description,
            bool allowUndefinedValues,
            IEnumerable<byte[]> allowedValues,
            ClassValidityProcessor<byte[]> validityProcessor)
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
        /// for the <see cref="Value"/> and <see cref="DefaultValue"/> to be set
        /// to. If <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.</param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="validityProcessor">Used for setting the
        /// <see cref="Value"/> and <see cref="DefaultValue"/> to ensure that
        /// only <see cref="ClassValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public BytesPreference(string name, string description,
            bool allowUndefinedValues,
            IEnumerable<byte[]> allowedValues, bool sortAllowedValues,
            ClassValidityProcessor<byte[]> validityProcessor)
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
        public override byte[] ConvertObjectToValue(object value)
            => ConvertObjectToValueBase(value);

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/> of the
        /// <see cref="ClassPreference{T}.AllowedValues"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns><see cref="ClassPreference{T}.AllowedValues"/> as an
        /// <see cref="Array"/> of <see cref="string"/>s.</returns>
        public override string[] GetAllowedValuesAsStrings(string format,
            IFormatProvider formatProvider)
        {
            if (AllowedValues is null)
            {
                return null;
            }

            var strings = new List<string>();

            foreach (var allowedValue in AllowedValues)
            {
                strings.Add(GetArrayAsString(allowedValue, format,
                    formatProvider));
            }

            return strings.ToArray();
        }

        /// <summary>
        /// Returns <see cref="ClassPreference{T}.DefaultValue"/>. The
        /// parameters are ignored since <see cref="string"/> does not implement
        /// <see cref="IFormattable"/>.
        /// </summary>
        /// <param name="format">This parameter is ignored.</param>
        /// <param name="formatProvider">This parameter is ignored.</param>
        /// <returns></returns>
        public override string GetDefaultValueAsString(string format,
            IFormatProvider formatProvider)
            => GetArrayAsString(DefaultValue, format, formatProvider);

        /// <summary>
        /// Returns <see cref="ClassPreference{T}.Value"/>. The parameters are
        /// ignored since <see cref="string"/> does not implement
        /// <see cref="IFormattable"/>.
        /// </summary>
        /// <param name="format">This parameter is ignored.</param>
        /// <param name="formatProvider">This parameter is ignored.</param>
        /// <returns></returns>
        public override string GetValueAsString(string format,
            IFormatProvider formatProvider)
            => GetArrayAsString(Value, format, formatProvider);

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
        public static byte[] ConvertObjectToValueBase(object value)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                if (value is byte[] byteArray)
                {
                    return byteArray;
                }
                else if (value is char[] characters)
                {
                    return Convert.FromBase64CharArray(characters, 0,
                        characters.Length);
                }
                else if (value is string @string)
                {
                    return Convert.FromBase64String(@string);
                }
                
                throw new InvalidOperationException("Cannot convert to an "
                        + $"{nameof(Array)} of {nameof(Byte)}.");
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Converting);
            }
        }

        /// <summary>
        /// Formats <paramref name="array"/> into a <see cref="string"/> using
        /// <paramref name="format"/> and <paramref name="formatProvider"/> for
        /// each element.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is
        /// <see langword="null"/>.</exception>
        public static string GetArrayAsString(byte[] array,
            string format = null, IFormatProvider formatProvider = null)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var stringBuilder = new StringBuilder('[');

            for (var i = 0; i < array.Length; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(array[i].ToString(format, formatProvider));
            }

            return stringBuilder.Append(']').ToString();
        }

        /// <summary>
        /// A helper <see langword="static"/> method for setting the
        /// <see cref="Value"/> and the <see cref="DefaultValue"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/>. This is
        /// used for context with any possible excepctions.</param>
        /// <param name="valueIn">What is to be set.</param>
        /// <param name="validityProcessor">Contains the functions for processing
        /// and checking the validity of <paramref name="valueIn"/>.</param>
        /// <param name="allowUndefinedValues">Whether or not the value can be
        /// set to something other than what is specified in
        /// <paramref name="allowedValues"/>. If
        /// <paramref name="allowedValues"/> is not used (it is
        /// <see langword="null"/> or empty), then this must be set to
        /// <see langword="true"/>.</param>
        /// <param name="allowedValues">A collection of values that are allowed
        /// for the <see cref="Value"/> and <see cref="DefaultValue"/> to be set
        /// to. If <paramref name="allowedValues"/> is <see langword="true"/>,
        /// then this property is ignored. On the other hand, if
        /// <paramref name="allowedValues"/> is <see langword="false"/>, then
        /// this must not be empty or <see langword="null"/>.</param>
        /// <returns>Processed value ready to be set.</returns>
        /// <exception cref="SetValueException">A failure occured and
        /// <paramref name="valueIn"/> cannot be processed or is not
        /// valid.</exception>
        public static new byte[] ValidityProcessForSetValue(string name,
            byte[] valueIn,
            ClassValidityProcessor<byte[]> validityProcessor,
            bool allowUndefinedValues,
            IReadOnlyCollection<byte[]> allowedValues)
        {
            if (validityProcessor is null)
            {
                throw new SetValueException(
                    new ArgumentNullException(nameof(validityProcessor)),
                    SetValueStepFailure.Unknown);
            }

            string processedName;

            try
            {
                processedName = ProcessNameOrThrowIfInvalid(name);
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex,
                    SetValueStepFailure.ProcessingName);
            }

            if (valueIn is null)
            {
                return valueIn;
            }

            byte[] valueOut = valueIn;
            Exception exception = null;

            try
            {
                var result = validityProcessor.Pre(valueOut);
                valueOut = result.ValueOut;
                exception = result.Exception;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (!(exception is null))
            {
                throw new SetValueException(exception,
                    SetValueStepFailure.PreProcessing);
            }

            try
            {

                var isAllowedValue = false;

                if (!(allowedValues is null) && allowedValues.Count > 0)
                {
                    // See if the value is already a member of allowedValues.
                    isAllowedValue = allowedValues.Contains(valueOut);

                    if (!isAllowedValue && !allowUndefinedValues)
                    {
                        exception = new InvalidOperationException("For the "
                            + $"preference named \"{processedName}\", the "
                            + "following is not an allowed value: "
                            + $"\"{valueOut}\".");
                    }
                }

                if (allowedValues is null || allowedValues.Count <= 0
                    || (!isAllowedValue && exception is null))
                {
                    var result = validityProcessor.IsValid(valueOut);

                    if (!result.Valid && !(result.Exception is null))
                    {
                        exception = result.Exception;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (!(exception is null))
            {
                throw new SetValueException(exception,
                    SetValueStepFailure.ValidityCheck);
            }

            try
            {
                var result = validityProcessor.Post(valueOut);
                valueOut = result.ValueOut;
                exception = result.Exception;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (!(exception is null))
            {
                throw new SetValueException(exception,
                    SetValueStepFailure.PostProcessing);
            }

            return valueOut;
        }
    }
}
