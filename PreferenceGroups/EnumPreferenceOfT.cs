using System;
using System.Collections.Generic;
using System.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// An <typeparamref name="TEnum"/> <see cref="Preference"/>.
    /// </summary>
    /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
    [Obsolete(message: "Use EnumPreference instead.")]
    public class EnumPreference<TEnum> : StructPreference<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// The default value for <see cref="Preference.AllowUndefinedValues"/>.
        /// </summary>
        public new const bool DefaultAllowUndefinedValues = false;

        /// <summary>
        /// The default behavior for whether the
        /// <see cref="StructPreference{T}.AllowedValues"/> are sorted.
        /// </summary>
        public new const bool DefaultSortAllowedValues = true;

        private TEnum? _defaultValue = default;

        private TEnum? _value = default;

        /// <summary>
        /// The default value to set the <see cref="Value"/> with.
        /// </summary>
        public override TEnum? DefaultValue
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

        /// <summary>
        /// The value of the preference.
        /// </summary>
        public override TEnum? Value
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

        /// <inheritdoc/>
        public override bool HasEnumFlags => EnumHelpers.HasFlags<TEnum>();

        /// <summary>
        /// <see langword="true"/>.
        /// </summary>
        public override bool IsEnum => true;

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The name of the
        /// <see cref="EnumPreference{TEnum}"/> and must be not
        /// <see langword="null"/>, not empty and not consist only of
        /// white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public EnumPreference(string name) : this(name, null,
            DefaultAllowUndefinedValues, null, null)
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
        /// <param name="name">The name of the
        /// <see cref="EnumPreference{TEnum}"/> and must be not
        /// <see langword="null"/>, not empty and not consist only of
        /// white-space characters.</param>
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
        /// only <see cref="StructValueValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public EnumPreference(string name, string description,
            bool allowUndefinedValues, IEnumerable<TEnum?> allowedValues,
            StructValueValidityProcessor<TEnum> validityProcessor)
            : this(name, description, allowUndefinedValues, allowedValues,
                  DefaultSortAllowedValues, validityProcessor)
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
        /// <param name="name">The name of the
        /// <see cref="EnumPreference{TEnum}"/> and must be not
        /// <see langword="null"/>, not empty and not consist only of
        /// white-space characters.</param>
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
        /// <see cref="StructValueValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public EnumPreference(string name, string description,
            bool allowUndefinedValues, IEnumerable<TEnum?> allowedValues,
            bool sortAllowedValues,
            StructValueValidityProcessor<TEnum> validityProcessor)
            : base(name, description,
                  ProcessAllowUndefinedValuesAndAllowedValues(
                      allowUndefinedValues, allowedValues, sortAllowedValues,
                      out var allowedValuesOut),
                  allowedValuesOut, validityProcessor)
        { }

        /// <summary>
        /// Uses <see cref="Convert.ToInt32(object)"/> to convert
        /// <paramref name="value"/> to a <see cref="Nullable{T}"/> of
        /// <see cref="int"/>. If <paramref name="value"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">An exception was thrown while
        /// converting.</exception>
        public override TEnum? ConvertObjectToValue(object value)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                if (value is string @string)
                {
                    try
                    {
                        return (TEnum?)Enum.Parse(typeof(TEnum), @string,
                            ignoreCase: true);
                    }
                    catch (Exception ex)
                    {
                        throw new SetValueException(ex,
                            SetValueStepFailure.Parsing);
                    }
                }
                else
                {
                    try
                    {
                        return (TEnum?)Enum.ToObject(typeof(TEnum), value);
                    }
                    catch (Exception ex)
                    {
                        throw new SetValueException(ex,
                            SetValueStepFailure.Converting);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Converting);
            }
        }

        /// <summary>
        /// Processes both the <paramref name="allowUndefinedValues"/> and the
        /// <paramref name="allowedValuesIn"/> by calling the
        /// <c>StructPreference&lt;T&gt;.ProcessAllowedValues(IEnumerable&lt;T?&gt;,
        /// bool)</c> method with <paramref name="allowedValuesIn"/> and
        /// <paramref name="sortAllowedValues"/>. If
        /// <paramref name="allowUndefinedValues"/> is <see langword="false"/>
        /// and the processed <c>allowedValues</c> is empty, then the default
        /// behavior is enforced. The default behavior is for
        /// <paramref name="allowedValuesOut"/> to have all of the constant
        /// values of <typeparamref name="TEnum"/> but not any that are
        /// equivalent to zero of the
        /// <see cref="Enum.GetUnderlyingType(Type)"/> (see
        /// <see cref="EnumHelpers.GetValuesNotZeroAsNullable{TEnum}"/>). Then,
        /// the result of
        /// <see cref="Preference.ProcessAllowUndefinedValues(bool, int?)"/> is
        /// returned.
        /// </summary>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="allowedValuesOut">The processed
        /// <paramref name="allowedValuesIn"/>.</param>
        /// <returns>The processed
        /// <paramref name="allowUndefinedValues"/>.</returns>
        public static new bool ProcessAllowUndefinedValuesAndAllowedValues(
            bool allowUndefinedValues, IEnumerable<TEnum?> allowedValuesIn,
            bool sortAllowedValues,
            out IReadOnlyCollection<TEnum?> allowedValuesOut)
        {
            // Process what is there, in case none are left.
            allowedValuesOut = ProcessAllowedValues(allowedValuesIn,
                sortAllowedValues);

            // Ensure default behavior of if allowUndefinedValues is false and
            // there are not any allowedValues, then automattically populate
            // allowedValues with all of the values for TEnum (but not zero)
            // with the EnumHelpers.GetValuesNotZeroAsNullable<TEnum>() method.
            if (!allowUndefinedValues
                && (allowedValuesOut is null || allowedValuesOut.Count <= 0))
            {
                // Process the default behaivor to sort them if needed.
                allowedValuesOut = ProcessAllowedValues(
                    EnumHelpers.GetValuesNotZeroAsNullable<TEnum>(),
                    sortAllowedValues);
            }

            return ProcessAllowUndefinedValues(allowUndefinedValues,
                allowedValuesOut?.Count);
        }

        /// <summary>
        /// A helper <see langword="static"/> method for setting the
        /// <see cref="Value"/> and the <see cref="DefaultValue"/>.
        /// </summary>
        /// <param name="name">The name of the
        /// <see cref="EnumPreference{TEnum}"/>. This is used for context with
        /// any possible excepctions.</param>
        /// <param name="valueIn">What is to be set.</param>
        /// <param name="validityProcessor">Contains the functions for
        /// processing and checking the validity of
        /// <paramref name="valueIn"/>.</param>
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
        public static new TEnum? ValidityProcessForSetValue(string name,
            TEnum? valueIn,
            StructValueValidityProcessor<TEnum> validityProcessor,
            bool allowUndefinedValues,
            IReadOnlyCollection<TEnum?> allowedValues)
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
                processedName = ProcessNameOrThrowIfInvalid(name, nameof(name));
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

            TEnum? valueOut = valueIn;
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

                    // If it isn't a member, but it is using flags
                    if (!isAllowedValue && EnumHelpers.HasFlags<TEnum>())
                    {
                        isAllowedValue = EnumHelpers.IsDefinedAndNotZero(
                            valueOut);
                    }

                    if (!isAllowedValue && !allowUndefinedValues)
                    {
                        exception = new InvalidOperationException("For the "
                            + $"preference named \"{processedName}\", the "
                            + "following is not an allowed value: "
                            + $"{valueOut}.");
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
