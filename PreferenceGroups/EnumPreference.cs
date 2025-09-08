using System;
using System.Collections.Generic;
using System.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// An <see cref="Enum"/> <see cref="Preference"/>.
    /// </summary>
    public class EnumPreference : ClassPreference<Enum>
    {
        /// <summary>
        /// The default value for <see cref="Preference.AllowUndefinedValues"/>.
        /// </summary>
        public new const bool DefaultAllowUndefinedValues = false;

        /// <summary>
        /// The default behavior for whether the
        /// <see cref="ClassPreference{T}.AllowedValues"/> are sorted.
        /// </summary>
        public new const bool DefaultSortAllowedValues = true;

        /// <summary>
        /// The private backing store for <see cref="DefaultValue"/>.
        /// </summary>
        private Enum _defaultValue;

        /// <summary>
        /// The private backing store for
        /// <see cref="Preference.GetValueType()"/>.
        /// </summary>
        private readonly Type _enumType;

        private readonly bool _hasFlags;

        private readonly EnumTypeInfo _typeInfo;

        /// <summary>
        /// The private backing store for <see cref="Value"/>.
        /// </summary>
        private Enum _value;

        /// <summary>
        /// Is always <see langword="true"/>.
        /// </summary>
        public override bool IsEnum => true;

        /// <summary>
        /// The <see cref="Value"/> is an <see cref="Enum"/> and the
        /// <see cref="FlagsAttribute"/> is applied to it.
        /// </summary>
        public override bool HasEnumFlags => _hasFlags;

        /// <summary>
        /// The default value to set the <see cref="Value"/> with.
        /// </summary>
        public override Enum DefaultValue
        {
            get => _defaultValue;
            set
            {
                try
                {
                    _defaultValue = ValidityProcessForSetValue(Name, _typeInfo,
                        value, ValidityProcessor, AllowUndefinedValues,
                        AllowedValues);
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
        public override Enum Value
        {
            get => _value;
            set
            {
                try
                {
                    _value = ValidityProcessForSetValue(Name, _typeInfo, value,
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
        /// A collection of values that are allowed for the <see cref="Value"/>
        /// and <see cref="DefaultValue"/> to be set to. If
        /// <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="true"/>, then this property is ignored. On the other
        /// hand, if <see cref="Preference.AllowUndefinedValues"/> is
        /// <see langword="false"/>, then this must not be empty or
        /// <see langword="null"/>.
        /// </summary>
        public override IReadOnlyCollection<Enum> AllowedValues { get; }

        /// <summary>
        /// Used for setting the <see cref="Value"/> and
        /// <see cref="DefaultValue"/> to ensure that only
        /// <see cref="ClassValidityResult{T}.IsValid"/> values are used.
        /// </summary>
        public override ClassValidityProcessor<Enum> ValidityProcessor
            { get; }

        /// <summary>
        /// Initializes an <see cref="EnumPreference"/> with
        /// <paramref name="name"/> and the <paramref name="enumTypeInfo"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnumPreference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="enumTypeInfo">The <see cref="Enum"/> <see cref="Type"/>
        /// for this <see cref="EnumPreference"/>.</param>
        /// <param name="description"></param>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValues"></param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="validityProcessor"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumTypeInfo"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreference(string name, EnumTypeInfo enumTypeInfo,
            string description, bool allowUndefinedValues,
            IEnumerable<Enum> allowedValues, bool sortAllowedValues,
            ClassValidityProcessor<Enum> validityProcessor)
            : base(name, description,
                  ProcessAllowUndefinedValuesAndAllowedValues(enumTypeInfo,
                      allowUndefinedValues, allowedValues, sortAllowedValues,
                      out var allowedValuesOut))
        {
            if (enumTypeInfo is null)
            {
                throw new ArgumentNullException(nameof(enumTypeInfo));
            }

            _typeInfo = enumTypeInfo;
            _enumType = _typeInfo.EnumType;
            _hasFlags = _typeInfo.HasFlags;

            if (validityProcessor is null)
            {
                throw new ArgumentNullException(nameof(validityProcessor));
            }

            AllowedValues = allowedValuesOut;
            ValidityProcessor = validityProcessor;
        }

        /// <summary>
        /// Initializes an <see cref="EnumPreference"/> with
        /// <paramref name="name"/> and the <paramref name="enumTypeInfo"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnumPreference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="enumTypeInfo">The <see cref="Enum"/> <see cref="Type"/>
        /// for this <see cref="EnumPreference"/>.</param>
        /// <param name="description"></param>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValues"></param>
        /// <param name="validityProcessor"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumTypeInfo"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreference(string name, EnumTypeInfo enumTypeInfo,
            string description, bool allowUndefinedValues,
            IEnumerable<Enum> allowedValues,
            ClassValidityProcessor<Enum> validityProcessor)
            : this(name, enumTypeInfo, description, allowUndefinedValues,
                  allowedValues, DefaultSortAllowedValues, validityProcessor)
        { }

        /// <summary>
        /// Initializes an <see cref="EnumPreference"/> with
        /// <paramref name="name"/> and the <paramref name="enumTypeInfo"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnumPreference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="enumTypeInfo">The <see cref="Enum"/> <see cref="Type"/>
        /// for this <see cref="EnumPreference"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumTypeInfo"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreference(string name, EnumTypeInfo enumTypeInfo)
            : base(name)
        {
            if (enumTypeInfo is null)
            {
                throw new ArgumentNullException(nameof(enumTypeInfo));
            }

            _typeInfo = enumTypeInfo;
            _enumType = _typeInfo.EnumType;
            _hasFlags = _typeInfo.HasFlags;
        }

        /// <summary>
        /// Initializes an <see cref="EnumPreference"/> with
        /// <paramref name="name"/> and the <paramref name="enumType"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnumPreference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/>
        /// for this <see cref="EnumPreference"/>.</param>
        /// <param name="description"></param>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValues"></param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="validityProcessor"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters. Or <paramref name="enumType"/> does not represent an
        /// <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumType"/> is <see langword="null"/>.</exception>
        public EnumPreference(string name, Type enumType,
            string description, bool allowUndefinedValues,
            IEnumerable<Enum> allowedValues, bool sortAllowedValues,
            ClassValidityProcessor<Enum> validityProcessor)
            : this(name, new EnumTypeInfo(enumType), description,
                  allowUndefinedValues, allowedValues, sortAllowedValues,
                  validityProcessor)
        { }

        /// <summary>
        /// Initializes an <see cref="EnumPreference"/> with
        /// <paramref name="name"/> and the <paramref name="enumType"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnumPreference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/>
        /// for this <see cref="EnumPreference"/>.</param>
        /// <param name="description"></param>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValues"></param>
        /// <param name="validityProcessor"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters. Or <paramref name="enumType"/> does not represent an
        /// <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumType"/> is <see langword="null"/>.</exception>
        public EnumPreference(string name, Type enumType,
            string description, bool allowUndefinedValues,
            IEnumerable<Enum> allowedValues,
            ClassValidityProcessor<Enum> validityProcessor)
            : this(name, new EnumTypeInfo(enumType), description,
                  allowUndefinedValues, allowedValues, validityProcessor)
        { }

        /// <summary>
        /// Initializes an <see cref="EnumPreference"/> with
        /// <paramref name="name"/> and the <paramref name="enumType"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnumPreference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/>
        /// for this <see cref="EnumPreference"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters. Or <paramref name="enumType"/> does not represent an
        /// <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumType"/> is <see langword="null"/>.</exception>
        public EnumPreference(string name, Type enumType)
            : this(name, new EnumTypeInfo(enumType))
        { }

        /// <summary>
        /// Converts <paramref name="value"/> to <see cref="Enum"/> by calling
        /// the <see cref="EnumTypeInfo.ToEnum(object, string, bool)"/> method.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException">There was an
        /// <see cref="Exception"/> thrown by the
        /// <see cref="EnumTypeInfo.ToEnum(object, string, bool)"/>
        /// method.</exception>
        public override Enum ConvertObjectToValue(object value)
            => ConvertObjectToValueBase(_typeInfo, value);

        /// <summary>
        /// Returns a <see cref="Nullable{T}"/> of an <see cref="Enum"/>.
        /// </summary>
        /// <returns></returns>
        public override Type GetValueType() => _typeInfo.NullableType;

        /// <summary>
        /// Converts <paramref name="value"/> to <see cref="Enum"/> by calling
        /// the <see cref="EnumTypeInfo.ToEnum(object, string, bool)"/> method.
        /// </summary>
        /// <param name="enumTypeInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="SetValueException"><paramref name="enumTypeInfo"/>
        /// is <see langword="null"/> or there was an <see cref="Exception"/>
        /// thrown by the
        /// <see cref="EnumTypeInfo.ToEnum(object, string, bool)"/>
        /// method.</exception>
        public static Enum ConvertObjectToValueBase(EnumTypeInfo enumTypeInfo,
            object value)
        {
            if (enumTypeInfo is null)
            {
                throw new SetValueException(
                    new ArgumentNullException(nameof(enumTypeInfo)),
                    SetValueStepFailure.Converting);
            }

            if (value is null)
            {
                return null;
            }

            try
            {
                return enumTypeInfo.ToEnum(value, paramName: nameof(value));
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Converting);
            }
        }

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
        /// <param name="enumTypeInfo"></param>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <returns>The processed <paramref name="allowedValuesIn"/>.</returns>
        public static IReadOnlyCollection<Enum> ProcessAllowedValues(
            EnumTypeInfo enumTypeInfo, IEnumerable<Enum> allowedValuesIn,
            bool sortAllowedValues)
        {
            IReadOnlyCollection<Enum> allowedValuesOut = null;

            if (!(allowedValuesIn is null))
            {
                if (sortAllowedValues)
                {
                    var set = new SortedSet<Enum>();

                    foreach (var allowedValue in allowedValuesIn)
                    {
                        if (enumTypeInfo.Equals(allowedValue))
                        {
                            _ = set.Add(allowedValue);
                        }
                    }

                    allowedValuesOut = set;
                }
                else
                {
                    var set = new HashSet<Enum>();

                    foreach (var allowedValue in allowedValuesIn)
                    {
                        if (enumTypeInfo.Equals(allowedValue))
                        {
                            _ = set.Add(allowedValue);
                        }
                    }

                    allowedValuesOut = set;
                }
            }

            return allowedValuesOut;
        }

        /// <summary>
        /// Processes <paramref name="allowedValuesIn"/> by instatiating either
        /// a <see cref="SortedSet{T}"/>, if
        /// <paramref name="sortAllowedValues"/> is <see langword="true"/>, or a
        /// <see cref="HashSet{T}"/> if <paramref name="sortAllowedValues"/> is
        /// <see langword="false"/>. If <paramref name="allowedValuesIn"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned. Any
        /// elements of <paramref name="allowedValuesIn"/> that are
        /// <see langword="null"/> will not be added to the returning
        /// <see cref="IReadOnlyCollection{T}"/> of <see cref="Enum"/>. For each
        /// item in <paramref name="allowedValuesIn"/> is converted to
        /// <see cref="Enum"/> by calling the
        /// <see cref="ConvertObjectToValueBase(EnumTypeInfo, object)"/> method.
        /// </summary>
        /// <param name="enumTypeInfo"></param>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <returns>The processed <paramref name="allowedValuesIn"/>.</returns>
        public static IReadOnlyCollection<Enum> ProcessAllowedValues(
            EnumTypeInfo enumTypeInfo, IEnumerable<object> allowedValuesIn,
            bool sortAllowedValues)
        {
            IReadOnlyCollection<Enum> allowedValuesOut = null;

            if (!(allowedValuesIn is null))
            {
                if (sortAllowedValues)
                {
                    var set = new SortedSet<Enum>();

                    foreach (var allowedValue in allowedValuesIn)
                    {
                        try
                        {
                            var enumValue = ConvertObjectToValueBase(
                                enumTypeInfo, allowedValue);

                            if (!(enumValue is null))
                            {
                                _ = set.Add(enumValue);
                            }
                        }
                        catch
                        { }
                    }

                    allowedValuesOut = set;
                }
                else
                {
                    var set = new HashSet<Enum>();

                    foreach (var allowedValue in allowedValuesIn)
                    {
                        try
                        {
                            var enumValue = ConvertObjectToValueBase(
                                enumTypeInfo, allowedValue);

                            if (!(enumValue is null))
                            {
                                _ = set.Add(enumValue);
                            }
                        }
                        catch
                        { }
                    }

                    allowedValuesOut = set;
                }
            }

            return allowedValuesOut;
        }

        /// <summary>
        /// Processes both the <paramref name="allowUndefinedValues"/> and the
        /// <paramref name="allowedValuesIn"/> by calling in sequence the
        /// <see cref="ProcessAllowedValues(EnumTypeInfo, IEnumerable{Enum},
        /// bool)"/> and the
        /// <see cref="Preference.ProcessAllowUndefinedValues(bool, int?)"/>
        /// </summary>
        /// <param name="enumTypeInfo"></param>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="allowedValuesOut">The processed
        /// <paramref name="allowedValuesIn"/>.</param>
        /// <returns>The processed
        /// <paramref name="allowUndefinedValues"/>.</returns>
        public static bool ProcessAllowUndefinedValuesAndAllowedValues(
            EnumTypeInfo enumTypeInfo, bool allowUndefinedValues,
            IEnumerable<Enum> allowedValuesIn, bool sortAllowedValues,
            out IReadOnlyCollection<Enum> allowedValuesOut)
        {
            if (enumTypeInfo is null)
            {
                throw new ArgumentNullException(nameof(enumTypeInfo));
            }

            // Process what is there, in case none are left.
            allowedValuesOut = ProcessAllowedValues(enumTypeInfo,
                allowedValuesIn, sortAllowedValues);

            // Ensure default behavior of if allowUndefinedValues is false and
            // there are not any allowedValues, then automattically populate
            // allowedValues with all of the values for Enum (but not zero)
            // with the EnumHelpers.GetValuesNotZeroAsNullable(Type, string)
            // method.
            if (!allowUndefinedValues
                && (allowedValuesOut is null || allowedValuesOut.Count <= 0))
            {
                // Process the default behaivor to sort them if needed.
                allowedValuesOut = ProcessAllowedValues(enumTypeInfo,
                    enumTypeInfo.GetValues(), sortAllowedValues);
            }

            return ProcessAllowUndefinedValues(allowUndefinedValues,
                allowedValuesOut?.Count);
        }

        /// <summary>
        /// A helper <see langword="static"/> method for setting the
        /// <see cref="Value"/> and the <see cref="DefaultValue"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/>. This is
        /// used for context with any possible excepctions.</param>
        /// <param name="enumTypeInfo">The <see cref="Type"/> of the
        /// <see cref="Enum"/>.</param>
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
        public static Enum ValidityProcessForSetValue(string name,
            EnumTypeInfo enumTypeInfo, Enum valueIn,
            ClassValidityProcessor<Enum> validityProcessor,
            bool allowUndefinedValues,
            IReadOnlyCollection<Enum> allowedValues)
        {
            if (enumTypeInfo is null)
            {
                throw new SetValueException(
                    new ArgumentNullException(nameof(enumTypeInfo)),
                    SetValueStepFailure.Unknown);
            }

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

            try
            {
                EnumHelpers.ThrowIfTypeNotEqual(enumTypeInfo, valueIn,
                    nameof(enumTypeInfo), nameof(valueIn));
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex,
                    SetValueStepFailure.ProcessingType);
            }

            Enum valueOut = valueIn;
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
                    if (!isAllowedValue && enumTypeInfo.HasFlags)
                    {
                        isAllowedValue = EnumHelpers.IsDefinedAndNotZero(
                            valueOut);
                    }

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
