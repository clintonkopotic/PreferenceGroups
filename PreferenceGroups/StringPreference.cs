using System;
using System.Collections.Generic;
using System.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// A <see cref="string"/> <see cref="Preference"/>.
    /// </summary>
    public class StringPreference : ClassPreference<string>
    {
        /// <summary>
        /// The private backing store for <see cref="DefaultValue"/>.
        /// </summary>
        private string _defaultValue;

        /// <summary>
        /// The private backing store for <see cref="Value"/>.
        /// </summary>
        private string _value;

        /// <summary>
        /// The default value to set the <see cref="Value"/> with.
        /// </summary>
        public override string DefaultValue
        {
            get => _defaultValue;
            set => _defaultValue = ValidityProcessForSetValue(Name, value,
                ValidityProcessor, AllowUndefinedValues, AllowedValues);
        }

        /// <inheritdoc/>
        public override string Value
        {
            get => _value;
            set => _value = ValidityProcessForSetValue(Name, value,
                ValidityProcessor, AllowUndefinedValues, AllowedValues);
        }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// </summary>
        /// <param name="name">The name of the <see cref="StringPreference"/>
        /// and must be not <see langword="null"/>, not empty and not consist
        /// only of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public StringPreference(string name) : base(name)
        { }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// It also initializes the <see cref="Preference.Description"/>,
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
        /// only <see cref="ClassValueValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public StringPreference(string name, string description,
            bool allowUndefinedValues,
            IReadOnlyCollection<string> allowedValues,
            ClassValueValidityProcessor<string> validityProcessor)
            : base(name, description, allowUndefinedValues, allowedValues, validityProcessor)
        { }

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of the
        /// <see cref="ClassPreference{T}.AllowedValues"/>. The parameters are
        /// ignored since <see cref="string"/> does not implement
        /// <see cref="IFormattable"/> and only not <see langword="null"/>
        /// values are retured.
        /// </summary>
        /// <param name="format">This parameter is ignored.</param>
        /// <param name="formatProvider">This parameter is ignored.</param>
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
                if (!string.IsNullOrWhiteSpace(allowedValue))
                {
                    strings.Add(allowedValue);
                }
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
            => DefaultValue;

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
            => Value;

        /// <summary>
        /// A helper <see langword="static"/> method for setting the
        /// <see cref="Value"/> and the <see cref="DefaultValue"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/>. This is
        /// used for context with any possible excepctions.</param>
        /// <param name="valueIn">What is to be set.</param>
        /// <param name="valueProcessor">Contains the functions for processing
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
        public static new string ValidityProcessForSetValue(string name,
            string valueIn, ClassValueValidityProcessor<string> valueProcessor,
            bool allowUndefinedValues,
            IReadOnlyCollection<string> allowedValues)
        {
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

            string valueOut = valueIn;
            Exception exception = null;

            try
            {
                var result = valueProcessor.Pre(valueOut);
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
                    var result = valueProcessor.IsValid(valueOut);

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
                var result = valueProcessor.Post(valueOut);
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
