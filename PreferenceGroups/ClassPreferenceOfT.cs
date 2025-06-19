using System;
using System.Collections.Generic;
using System.Linq;

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
        /// The validity processor used for setting the <see cref="Value"/> and
        /// <see cref="DefaultValue"/> to ensure that only
        /// <see cref="ClassValueValidityResult{T}.IsValid"/> values are used.
        /// </summary>
        public virtual ClassValueValidityProcessor<T> ValidityProcessor { get; }

        /// <inheritdoc/>
        public override object GetDefaultValueAsObject() => DefaultValue;

        /// <inheritdoc/>
        public override string GetDefaultValueAsString(string format,
            IFormatProvider formatProvider)
            => DefaultValue is IFormattable formattable
                ? formattable.ToString(format, formatProvider)
                : DefaultValue?.ToString();

        /// <inheritdoc/>
        public override object GetValueAsObject() => Value;

        /// <inheritdoc/>
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
        /// Sets Value from <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">What to set Value to.</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="defaultValue"/> is not <see langword="null"/> and
        /// cannot be cast to the
        /// <c>typeof(<typeparamref name="T"/>)</c>.</exception>
        public override void SetDefaultValueFromObject(object defaultValue)
        {
            DefaultValue = defaultValue is null ? null : (T)defaultValue;
        }

        /// <summary>
        /// Sets Value from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">What to set Value to.</param>
        /// <exception cref="InvalidCastException"><paramref name="value"/> is
        /// not <see langword="null"/> and cannot be cast to the
        /// <c>typeof(<typeparamref name="T"/>)</c>.</exception>
        public override void SetValueFromObject(object value)
        {
            Value = value is null ? null : (T)value;
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
