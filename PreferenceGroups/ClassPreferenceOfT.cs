using System;
using System.Collections.Generic;

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
            // TODO: Implement validity checking.
            set => _defaultValue = value;
        }

        /// <summary>
        /// The value of the preference.
        /// </summary>
        public virtual T Value
        {
            get => _value;
            // TODO: Implement validity checking.
            set => _value = value;
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
    }
}
