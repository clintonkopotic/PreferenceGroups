using System;
using System.Collections.Generic;
using System.Threading;

namespace PreferenceGroups
{
    /// <summary>
    /// <see cref="Preference"/> is the basic unit that encapsalates the
    /// <see cref="Name"/> of the preference and its <c>Value</c>. In addition,
    /// the metadata of the <c>DefaultValue</c>, <see cref="Description"/>, and
    /// a mechanism for validating values with <c>AllowedValues</c>,
    /// <see cref="AllowUndefinedValues"/> and <c>ValidityProcessor</c>. The
    /// <c>Value</c> by default is <see langword="null"/> and utilizes boxing
    /// for <see langword="struct"/> data types. When the <c>DefaultValue</c> is
    /// not <see langword="null"/> and <c>Value</c> is <see langword="null"/>,
    /// this means that <c>Value</c> is not set.
    /// </summary>
    public abstract class Preference
    {
        /// <summary>
        /// The default value for <see cref="AllowUndefinedValues"/>.
        /// </summary>
        public const bool DefaultAllowUndefinedValues = true;

        /// <summary>
        /// The default behavior for whether the <c>AllowedValues</c> are
        /// sorted.
        /// </summary>
        public const bool DefaultSortAllowedValues = true;

        /// <summary>
        /// The name of the <see cref="Preference"/> and must be unique amoung
        /// other Preferences in a group. It must not be <see langword="null"/>,
        /// not empty, and not consist only of white-space characters, or in
        /// other words, the method <see cref="IsNameValid(string)"/> must
        /// return <see langword="true"/>.
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Is the <c>Value</c> of the <see cref="Preference"/> an
        /// <see cref="Enum"/>, where the underlying type (returned by
        /// <see cref="Type.GetEnumUnderlyingType()"/>) is the same as
        /// <see cref="GetValueType()"/>.
        /// </summary>
        public virtual bool IsEnum => false;

        /// <summary>
        /// The <c>Value</c> is an <see cref="Enum"/> and the
        /// <see cref="FlagsAttribute"/> is applied to it.
        /// </summary>
        public virtual bool HasEnumFlags => false;

        /// <summary>
        /// A description of the <see cref="Preference"/> that is intended to be
        /// shown to the user and in the file as a comment.
        /// </summary>
        public virtual string Description { get; } = null;

        /// <summary>
        /// Whether or not the <c>Value</c> and <c>DefaultValue</c> properties
        /// can be set to something other than what is specified in
        /// <c>AllowedValues</c>. If <c>AllowedValues</c> is not used (it is
        /// <see langword="null"/> or empty), then
        /// <see cref="AllowUndefinedValues"/> must be set to
        /// <see langword="true"/>.
        /// </summary>
        public virtual bool AllowUndefinedValues { get; }
            = DefaultAllowUndefinedValues;

        /// <summary>
        /// Is <c>DefaultValue</c> <see langword="null"/>.
        /// </summary>
        public virtual bool DefaultValueIsNull
            => GetDefaultValueAsObject() is null;

        /// <summary>
        /// Is <c>Value</c> <see langword="null"/>.
        /// </summary>
        public virtual bool ValueIsNull => GetValueAsObject() is null;

        /// <summary>
        /// Initializes <see cref="Name"/> with <paramref name="name"/> after
        /// it is processed with the
        /// <see cref="ProcessNameOrThrowIfInvalid(string, string)"/> method.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        protected Preference(string name)
        {
            Name = ProcessNameOrThrowIfInvalid(name);
        }

        /// <summary>
        /// Initializes <see cref="Name"/> with <paramref name="name"/> after
        /// it is processed with the
        /// <see cref="ProcessNameOrThrowIfInvalid(string, string)"/> method. It
        /// also initializes the <see cref="Description"/> and
        /// <see cref="AllowUndefinedValues"/> properties.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <param name="description">A description of the
        /// <see cref="Preference"/> that is intended to be shown to the user
        /// and in the file as a comment.</param>
        /// <param name="allowUndefinedValues">Whether or not the <c>Value</c>
        /// and <c>DefaultValue</c> properties can be set to something other
        /// than what is specified in <c>AllowedValues</c>. If
        /// <c>AllowedValues</c> is not used (it is <see langword="null"/> or
        /// empty), then <see cref="AllowUndefinedValues"/> must be set to
        /// <see langword="true"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        protected Preference(string name, string description,
            bool allowUndefinedValues)
            : this(name)
        {
            Description = description;
            AllowUndefinedValues = allowUndefinedValues;
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="T"/>s of the
        /// <c>AllowedValues</c> by casting.
        /// </summary>
        /// <returns><c>AllowedValues</c> as an <see cref="Array"/> of
        /// <typeparamref name="T"/>s.</returns>
        /// <exception cref="InvalidCastException">A member of
        /// <c>AllowedValues</c> failed to be cast to
        /// <typeparamref name="T"/>.</exception>
        public virtual T[] GetAllowedValuesAs<T>()
        {
            var list = new List<T>();

            foreach (var allowedValue in GetAllowedValuesAsObjects())
            {
                list.Add((T)allowedValue);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="object"/>s of the
        /// <c>AllowedValues</c>.
        /// </summary>
        /// <returns><c>AllowedValues</c> as an <see cref="Array"/> of
        /// <see cref="object"/>s.</returns>
        public abstract object[] GetAllowedValuesAsObjects();

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of formatted
        /// <c>AllowedValues</c> by calling the
        /// <see cref="GetAllowedValuesAsStrings(string, IFormatProvider)"/>
        /// method with a <see langword="null"/> for both parameters.
        /// </summary>
        /// <returns><c>AllowedValues</c> as an <see cref="Array"/> of
        /// <see cref="string"/>s.</returns>
        public string[] GetAllowedValuesAsStrings()
            => GetAllowedValuesAsStrings(null, null);

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of formatted
        /// <c>AllowedValues</c>. The parameter
        /// <paramref name="formatProvider"/> is only used if
        /// <see cref="GetValueType()"/> implements <see cref="IFormattable"/>
        /// (along with a <see langword="null"/> for the <c>format</c> parameter
        /// when calling the
        /// <see cref="GetAllowedValuesAsStrings(string, IFormatProvider)"/>
        /// method), otherwise the <see cref="object.ToString()"/> method is
        /// used.
        /// </summary>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns><c>AllowedValues</c> as an <see cref="Array"/> of
        /// <see cref="string"/>s.</returns>
        public string[] GetAllowedValuesAsStrings(
            IFormatProvider formatProvider)
            => GetAllowedValuesAsStrings(null, formatProvider);

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of formatted
        /// <c>AllowedValues</c>. The parameters are only used if
        /// <see cref="GetValueType()"/> implements <see cref="IFormattable"/>,
        /// otherwise the <see cref="object.ToString()"/> method is used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns><c>AllowedValues</c> as an <see cref="Array"/> of
        /// <see cref="string"/>s.</returns>
        public abstract string[] GetAllowedValuesAsStrings(string format,
            IFormatProvider formatProvider);

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of formatted
        /// <c>AllowedValues</c>. The parameter <paramref name="format"/> is
        /// only used if <see cref="GetValueType()"/> implements
        /// <see cref="IFormattable"/> (along with a <see langword="null"/> for
        /// the <c>formatProvider</c> parameter when calling the
        /// <see cref="GetAllowedValuesAsStrings(string, IFormatProvider)"/>
        /// method), otherwise the <see cref="object.ToString()"/> method is
        /// used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <returns><c>AllowedValues</c> as an <see cref="Array"/> of
        /// <see cref="string"/>s.</returns>
        public string[] GetAllowedValuesAsStrings(string format)
            => GetAllowedValuesAsStrings(format, null);

        /// <summary>
        /// Returns the <c>DefaultValue</c> as an <typeparamref name="T"/>. A
        /// <see langword="null"/> means that the <c>DefaultValue</c> is not
        /// used, and a <see langword="null"/> <c>Value</c> means that it is
        /// set. On the other hand, if the <c>DefaultValue</c> is not
        /// <see langword="null"/>, then a <see langword="null"/> <c>Value</c>
        /// means that it has not been set.
        /// </summary>
        /// <returns>The <c>DefaultValue</c> as an
        /// <typeparamref name="T"/>.</returns>
        public virtual T GetDefaultValueAs<T>() => (T)GetDefaultValueAsObject();

        /// <summary>
        /// Returns the <c>DefaultValue</c> as an <see cref="object"/>. A
        /// <see langword="null"/> means that the <c>DefaultValue</c> is not
        /// used, and a <see langword="null"/> <c>Value</c> means that it is
        /// set. On the other hand, if the <c>DefaultValue</c> is not
        /// <see langword="null"/>, then a <see langword="null"/> <c>Value</c>
        /// means that it has not been set.
        /// </summary>
        /// <returns>The <c>DefaultValue</c> as an
        /// <see cref="object"/>.</returns>
        public abstract object GetDefaultValueAsObject();

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>DefaultValue</c> formatted
        /// by calling the
        /// <see cref="GetDefaultValueAsString(string, IFormatProvider)"/>
        /// method with a <see langword="null"/> for both parameters.
        /// </summary>
        /// <returns><c>DefaultValue</c> as a <see cref="string"/>.</returns>
        public string GetDefaultValueAsString()
            => GetDefaultValueAsString(null, null);

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>DefaultValue</c> formatted.
        /// The parameter <paramref name="formatProvider"/> is only used if
        /// <see cref="GetValueType()"/> implements <see cref="IFormattable"/>
        /// (along with a <see langword="null"/> for the <c>format</c> parameter
        /// when calling the
        /// <see cref="GetDefaultValueAsString(string, IFormatProvider)"/>
        /// method), otherwise the <see cref="object.ToString()"/> method is
        /// used.
        /// </summary>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns><c>DefaultValue</c> as a <see cref="string"/>.</returns>
        public string GetDefaultValueAsString(IFormatProvider formatProvider)
            => GetDefaultValueAsString(null, formatProvider);

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>DefaultValue</c> formatted.
        /// The parameters are only used if <see cref="GetValueType()"/>
        /// implements <see cref="IFormattable"/>, otherwise the
        /// <see cref="object.ToString()"/> method is used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns><c>DefaultValue</c> as a <see cref="string"/>.</returns>
        public abstract string GetDefaultValueAsString(string format,
            IFormatProvider formatProvider);

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>DefaultValue</c> formatted.
        /// The parameter <paramref name="format"/> is only used if
        /// <see cref="GetValueType()"/> implements <see cref="IFormattable"/>
        /// (along with a <see langword="null"/> for the <c>formatProvider</c>
        /// parameter when calling the
        /// <see cref="GetDefaultValueAsString(string, IFormatProvider)"/>
        /// method), otherwise the <see cref="object.ToString()"/> method is
        /// used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <returns><c>DefaultValue</c> as a <see cref="string"/>.</returns>
        public string GetDefaultValueAsString(string format)
            => GetDefaultValueAsString(format, null);

        /// <summary>
        /// Returns the <c>Value</c> as a <typeparamref name="T"/>. If
        /// <c>DefaultValue</c> is <see langword="null"/>, then that means a
        /// <c>DefaultValue</c> is not used, and a <see langword="null"/>
        /// <c>Value</c> means that it is set. On the other hand, if the
        /// <c>DefaultValue</c> is not <see langword="null"/>, then a
        /// <see langword="null"/> <c>Value</c> means that it has not been set.
        /// </summary>
        /// <returns>The <c>Value</c> as a <typeparamref name="T"/>.</returns>
        public virtual T GetValueAs<T>() => (T)GetValueAsObject();

        /// <summary>
        /// Returns the <c>Value</c> as an <see cref="object"/>. If
        /// <c>DefaultValue</c> is <see langword="null"/>, then that means a
        /// <c>DefaultValue</c> is not used, and a <see langword="null"/>
        /// <c>Value</c> means that it is set. On the other hand, if the
        /// <c>DefaultValue</c> is not <see langword="null"/>, then a
        /// <see langword="null"/> <c>Value</c> means that it has not been set.
        /// </summary>
        /// <returns>The <c>Value</c> as an <see cref="object"/>.</returns>
        public abstract object GetValueAsObject();

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>Value</c> formatted by
        /// calling the <see cref="GetValueAsString(string, IFormatProvider)"/>
        /// method with a <see langword="null"/> for both parameters.
        /// </summary>
        /// <returns><c>Value</c> as a <see cref="string"/>.</returns>
        public string GetValueAsString() => GetValueAsString(null, null);

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>Value</c> formatted. The
        /// parameter <paramref name="formatProvider"/> is only used if
        /// <see cref="GetValueType()"/> implements <see cref="IFormattable"/>
        /// (along with a <see langword="null"/> for the <c>format</c> parameter
        /// when calling the
        /// <see cref="GetValueAsString(string, IFormatProvider)"/>
        /// method), otherwise the <see cref="object.ToString()"/> method is
        /// used.
        /// </summary>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns><c>Value</c> as a <see cref="string"/>.</returns>
        public string GetValueAsString(IFormatProvider formatProvider)
            => GetValueAsString(null, formatProvider);

        /// <summary>
        /// Returns a <see cref="string"/> of the <c>Value</c> formatted. The
        /// parameters are only used if <see cref="GetValueType()"/> implements
        /// <see cref="IFormattable"/>, otherwise the
        /// <see cref="object.ToString()"/> method is used.
        /// </summary>
        /// <param name="format">The format to use. If it is
        /// <see langword="null"/>, then the default format will be
        /// used.</param>
        /// <param name="formatProvider">The provider to use to format the
        /// value. If it is <see langword="null"/>, then the
        /// <see cref="Thread.CurrentCulture"/> will be used.</param>
        /// <returns><c>Value</c> as a <see cref="string"/>.</returns>
        public abstract string GetValueAsString(string format,
            IFormatProvider formatProvider);

        /// <summary>
        /// Returns the <see cref="Type"/> of <c>Value</c>.
        /// </summary>
        /// <returns></returns>
        public abstract Type GetValueType();

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a valid value.
        /// This takes into account the <c>AllowedValues</c>,
        /// <see cref="AllowUndefinedValues"/>, and the
        /// <c>ValidityProcessor</c>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool IsValueValid(object value);

        /// <summary>
        /// Sets <c>DefaultValue</c> from <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">What to set <c>DefaultValue</c>
        /// to.</param>
        public abstract void SetDefaultValueFromObject(object defaultValue);

        /// <summary>
        /// Sets <c>Value</c> from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">What to set <c>Value</c> to.</param>
        public abstract void SetValueFromObject(object value);

        /// <summary>
        /// Sets <c>Value</c> to <c>DefaultValue</c>.
        /// </summary>
        public void SetValueToDefault() => SetValueFromObject(
            GetDefaultValueAsObject());

        /// <summary>
        /// Sets <c>Value</c> to <see langword="null"/>.
        /// </summary>
        public void SetValueToNull() => SetValueFromObject(null);

        /// <summary>
        /// Attempts to get the <c>AllowedValues</c> as an <see cref="Array"/>
        /// of <typeparamref name="T"/>s.
        /// </summary>
        /// <param name="allowedValues">The <c>AllowedValues</c>, as an
        /// <see cref="Array"/> of <typeparamref name="T"/>s.</param>
        /// <returns><see langword="true"/> if <paramref name="allowedValues"/>
        /// is the found <c>AllowedValues</c>; otherwise,
        /// <see langword="false"/>.</returns>
        public virtual bool TryGetAllowedValuesAs<T>(out T[] allowedValues)
        {
            allowedValues = null;

            try
            {
                allowedValues = GetAllowedValuesAs<T>();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to get the <c>DefaultValue</c> as a
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <param name="defaultValue">The <c>DefaultValue</c>, as a
        /// <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="defaultValue"/>
        /// is the found <c>DefaultValue</c>; otherwise,
        /// <see langword="false"/>.</returns>
        public virtual bool TryGetDefaultValueAs<T>(out T defaultValue)
        {
            defaultValue = default;

            try
            {
                defaultValue = GetDefaultValueAs<T>();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to get the <c>Value</c> as a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The <c>Value</c>, as a
        /// <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is the
        /// found <c>Value</c>; otherwise, <see langword="false"/>.</returns>
        public virtual bool TryGetValueAs<T>(out T value)
        {
            value = default;

            try
            {
                value = GetValueAs<T>();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tests whether or not <paramref name="name"/> is a valid
        /// <see cref="Name"/> for a <see cref="Preference"/>.
        /// </summary>
        /// <param name="name">What to check is a valid name or not.</param>
        /// <returns><see langword="true"/> if <paramref name="name"/> is not
        /// <see langword="null"/>, empty, or consist only of white-space
        /// characters, otherwise <see langword="false"/>.</returns>
        public static bool IsNameValid(string name)
            => !string.IsNullOrWhiteSpace(name);

        /// <summary>
        /// Processes <paramref name="allowUndefinedValues"/> by ensuring that
        /// <see cref="AllowUndefinedValues"/> is only
        /// <see langword="true"/> when <paramref name="allowUndefinedValues"/>
        /// is already <see langword="true"/>, or it is <see langword="false"/>
        /// and <paramref name="allowedValuesCount"/> is <see langword="null"/>
        /// or not positive.
        /// </summary>
        /// <param name="allowUndefinedValues"></param>
        /// <param name="allowedValuesCount">How many <c>AllowedValues</c> there
        /// are.</param>
        /// <returns>The processed
        /// <paramref name="allowUndefinedValues"/>.</returns>
        public static bool ProcessAllowUndefinedValues(
            bool allowUndefinedValues, int? allowedValuesCount)
            => allowUndefinedValues || (!allowUndefinedValues
            && (allowedValuesCount is null || allowedValuesCount <= 0));

        /// <summary>
        /// Processes <paramref name="name"/> by trimming it (by calling
        /// <see cref="string.Trim()"/>), or throw an exception
        /// if it is not valid.
        /// </summary>
        /// <param name="name">What to process.</param>
        /// <param name="paramName">The name of the <paramref name="name"/>
        /// parameter. If it is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="name"/> will be
        /// used.</param>
        /// <returns>The processed <paramref name="name"/> if it is
        /// valid.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static string ProcessNameOrThrowIfInvalid(string name,
            string paramName = null)
        {
            if (name is null)
            {
                throw new ArgumentNullException(paramName ?? nameof(name));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    paramName: paramName ?? nameof(name),
                    message: "Cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    paramName: paramName ?? nameof(name),
                    message: "Cannot consist only of white-space characters.");
            }

            return name.Trim();
        }
    }
}
