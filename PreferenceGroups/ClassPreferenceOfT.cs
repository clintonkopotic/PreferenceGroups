using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PreferenceGroups
{
    /// <summary>
    /// A <see cref="Preference"/> with a <see cref="Value"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ClassPreference<T> : Preference where T : class
    {
        /// <summary>
        /// The private backing store for <see cref="DefaultValue"/>.
        /// </summary>
        private T _defaultValue;

        /// <summary>
        /// The private backing store for <see cref="Value"/>.
        /// </summary>
        private T _value;

        /// <summary>
        /// The default value to set the <see cref="Value"/> with.
        /// </summary>
        public virtual T DefaultValue
        {
            get => _defaultValue;
            set => _defaultValue = ValidityProcessForSetValue(Name, value,
                ValidityProcessor, AllowUndefinedValues, AllowedValues);
        }

        /// <summary>
        /// The value of the preference.
        /// </summary>
        public virtual T Value
        {
            get => _value;
            set => _value = ValidityProcessForSetValue(Name, value,
                ValidityProcessor, AllowUndefinedValues, AllowedValues);
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
        public virtual IReadOnlyCollection<T> AllowedValues { get; }

        /// <summary>
        /// Used for setting the <see cref="Value"/> and
        /// <see cref="DefaultValue"/> to ensure that only
        /// <see cref="ClassValueValidityResult{T}.IsValid"/> values are used.
        /// </summary>
        public virtual ClassValueValidityProcessor<T> ValidityProcessor { get; }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        protected ClassPreference(string name) : base(name)
        { }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// It also initializes the <see cref="Preference.Description"/>,
        /// <see cref="Preference.AllowUndefinedValues"/>,
        /// <see cref="AllowedValues"/> and <see cref="ValidityProcessor"/>
        /// properties.
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
        protected ClassPreference(string name, string description,
            bool allowUndefinedValues, IEnumerable<T> allowedValues,
            ClassValueValidityProcessor<T> validityProcessor)
            : this(name, description, allowUndefinedValues, allowedValues,
                  sortAllowedValues: false, validityProcessor)
        { }

        /// <summary>
        /// Initializes <see cref="Preference.Name"/> with
        /// <paramref name="name"/> after it is processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// It also initializes the <see cref="Preference.Description"/>,
        /// <see cref="Preference.AllowUndefinedValues"/>,
        /// <see cref="AllowedValues"/> and <see cref="ValidityProcessor"/>
        /// properties.
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
        /// only <see cref="ClassValueValidityResult{T}.IsValid"/> values are
        /// used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        protected ClassPreference(string name, string description,
            bool allowUndefinedValues, IEnumerable<T> allowedValues,
            bool sortAllowedValues,
            ClassValueValidityProcessor<T> validityProcessor)
            : base(name, description,
                  ProcessAllowUndefinedValuesAndAllowedValues(
                      allowUndefinedValues, allowedValues, sortAllowedValues,
                      out var allowedValuesOut))
        {
            AllowedValues = allowedValuesOut;
            ValidityProcessor = validityProcessor;
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of formatted
        /// <see cref="AllowedValues"/>. The parameters are only used if
        /// <typeparamref name="T"/> implements <see cref="IFormattable"/>,
        /// otherwise the <see cref="object.ToString()"/> method is used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns>AllowedValues as an <see cref="Array"/> of
        /// <see cref="string"/>s.</returns>
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
                if (allowedValue is null)
                {
                    continue;
                }

                var @string = allowedValue is IFormattable formattable
                    ? formattable.ToString(format, formatProvider)
                    : allowedValue?.ToString();

                if (!string.IsNullOrWhiteSpace(@string))
                {
                    strings.Add(@string);
                }
            }

            return strings.ToArray();
        }

        /// <summary>
        /// Returns <see cref="DefaultValue"/> as an <see cref="object"/> for
        /// versitility. A <see langword="null"/> means that
        /// <see cref="DefaultValue"/> is not used, and a <see langword="null"/>
        /// <see cref="Value"/> means that it is set. On the other hand, if
        /// <see cref="DefaultValue"/> is not <see langword="null"/>, then
        /// a <see langword="null"/> <see cref="Value"/> means that it has not
        /// been set.
        /// </summary>
        /// <returns><see cref="DefaultValue"/> as an
        /// <see cref="object"/>.</returns>
        public override object GetDefaultValueAsObject() => DefaultValue;

        /// <summary>
        /// Returns a <see cref="string"/> of the <see cref="DefaultValue"/>
        /// formatted. The parameters are only used if <typeparamref name="T"/>
        /// implements <see cref="IFormattable"/>, otherwise the
        /// <see cref="object.ToString()"/> method is used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns>DefaultValue as a <see cref="string"/>.</returns>
        public override string GetDefaultValueAsString(string format,
            IFormatProvider formatProvider)
            => DefaultValue is IFormattable formattable
                ? formattable.ToString(format, formatProvider)
                : DefaultValue?.ToString();

        /// <summary>
        /// Returns <see cref="Value"/> as an <see cref="object"/> for
        /// versitility. If <see cref="DefaultValue"/> is
        /// <see langword="null"/>, then that means a <see cref="DefaultValue"/>
        /// is not used, and a <see langword="null"/> <see cref="Value"/> means
        /// that it is set. On the other hand, if the <see cref="DefaultValue"/>
        /// is not <see langword="null"/>, then a <see langword="null"/>
        /// <see cref="Value"/> means that it has not been set.
        /// </summary>
        /// <returns>The Value as an <see cref="object"/>.</returns>
        public override object GetValueAsObject() => Value;

        /// <summary>
        /// Returns a <see cref="string"/> of the <see cref="Value"/>
        /// formatted. The parameters are only used if <typeparamref name="T"/>
        /// implements <see cref="IFormattable"/>, otherwise the
        /// <see cref="object.ToString()"/> method is used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns>DefaultValue as a <see cref="string"/>.</returns>
        public override string GetValueAsString(string format,
            IFormatProvider formatProvider)
            => Value is IFormattable formattable
                ? formattable.ToString(format, formatProvider)
                : Value?.ToString();

        /// <summary>
        /// Returns the <see cref="Type"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        public override Type GetValueType() => typeof(T);

        /// <summary>
        /// Sets <see cref="DefaultValue"/> from
        /// <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">What to set Value to.</param>
        /// <exception cref="SetValueException">
        /// <paramref name="defaultValue"/> is not <see langword="null"/> and
        /// cannot be cast to the
        /// <c>typeof(<typeparamref name="T"/>)</c>.</exception>
        public override void SetDefaultValueFromObject(object defaultValue)
        {
            try
            {
                DefaultValue = defaultValue is null ? null : (T)defaultValue;
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Casting);
            }
        }

        /// <summary>
        /// Sets <see cref="Value"/> from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">What to set Value to.</param>
        /// <exception cref="SetValueException"><paramref name="value"/> is
        /// not <see langword="null"/> and cannot be cast to the
        /// <c>typeof(<typeparamref name="T"/>)</c>.</exception>
        public override void SetValueFromObject(object value)
        {
            try
            {
                Value = value is null ? null : (T)value;
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex, SetValueStepFailure.Casting);
            }
        }

        /// <summary>
        /// Processes <paramref name="allowedValues"/> by instatiating either
        /// a <see cref="SortedSet{T}"/>, if
        /// <paramref name="sortAllowedValues"/> is <see langword="true"/>, or a
        /// <see cref="HashSet{T}"/> if <paramref name="sortAllowedValues"/> is
        /// <see langword="false"/>. If <paramref name="allowedValues"/> is
        /// <see langword="null"/>, then <see langword="null"/> is returned. Any
        /// elements of <paramref name="allowedValues"/> that are
        /// <see langword="null"/> will not be added to the returning
        /// <see cref="IReadOnlyCollection{T}"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <param name="sortAllowedValues"></param>
        /// <returns>The processed <paramref name="allowedValues"/>.</returns>
        public static IReadOnlyCollection<T> ProcessAllowedValues(
            IEnumerable<T> allowedValues, bool sortAllowedValues)
        {
            IReadOnlyCollection<T> allowedValuesOut = null;

            if (!(allowedValues is null))
            {
                if (sortAllowedValues)
                {
                    var set = new SortedSet<T>();

                    foreach (var allowedValue in allowedValues)
                    {
                        if (!(allowedValue is null))
                        {
                            _ = set.Add(allowedValue);
                        }
                    }

                    allowedValuesOut = set;
                }
                else
                {
                    var set = new HashSet<T>();

                    foreach (var allowedValue in allowedValues)
                    {
                        if (!(allowedValue is null))
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
        /// Processes both the <paramref name="allowUndefinedValues"/> and the
        /// <paramref name="allowedValuesIn"/> by calling in sequence the
        /// <see cref="ProcessAllowedValues(IEnumerable{T}, bool)"/> and the
        /// <see cref="Preference.ProcessAllowUndefinedValues(bool, int?)"/>
        /// </summary>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValuesIn"></param>
        /// <param name="sortAllowedValues"></param>
        /// <param name="allowedValuesOut">The processed
        /// <paramref name="allowedValuesIn"/>.</param>
        /// <returns>The processed
        /// <paramref name="allowUndefinedValues"/>.</returns>
        public static bool ProcessAllowUndefinedValuesAndAllowedValues(
            bool allowUndefinedValues, IEnumerable<T> allowedValuesIn,
            bool sortAllowedValues, out IReadOnlyCollection<T> allowedValuesOut)
        {
            allowedValuesOut = ProcessAllowedValues(allowedValuesIn,
                sortAllowedValues);

            return ProcessAllowUndefinedValues(allowUndefinedValues,
                allowedValuesOut?.Count);
        }

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
        public static T ValidityProcessForSetValue(string name, T valueIn,
            ClassValueValidityProcessor<T> valueProcessor,
            bool allowUndefinedValues, IReadOnlyCollection<T> allowedValues)
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

            T valueOut = valueIn;
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
                            + $"{valueOut}.");
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
