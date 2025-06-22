using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace PreferenceGroups
{
    /// <summary>
    /// <see cref="Preference"/> is the basic unit that encapsalates the
    /// <see cref="Name"/> of the preference and its Value. In addition, the
    /// metadata of the DefaultValue, <see cref="Description"/>, and a mechanism
    /// for validating values with AllowedValues,
    /// <see cref="AllowUndefinedValues"/> and SetValueProcessor. The Value by
    /// default is <see langword="null"/> and utilizes boxing for
    /// <see langword="struct"/> data types. When the DefaultValue is not
    /// <see langword="null"/> and Value is <see langword="null"/>, this means
    /// that Value is not set.
    /// </summary>
    public abstract class Preference
    {
        /// <summary>
        /// The name of the <see cref="Preference"/> and must be unique amoung
        /// other Preferences in a group. It must not be <see langword="null"/>,
        /// not empty, and not consist only of white-space characters, or in
        /// other words, the method <see cref="IsNameValid(string)"/> must
        /// return <see langword="true"/>.
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Is the Value of the <see cref="Preference"/> an <see cref="Enum"/>,
        /// where the underlying type (returned by
        /// <see cref="Type.GetEnumUnderlyingType()"/>) is the same as
        /// <see cref="GetValueType()"/>.
        /// </summary>
        public virtual bool IsEnum => false;

        /// <summary>
        /// The Value is an <see cref="Enum"/> and the
        /// <see cref="FlagsAttribute"/> is defined (using the
        /// <see cref="MemberInfo.IsDefined(Type, bool)"/> method).
        /// </summary>
        public virtual bool HasEnumFlags => false;

        /// <summary>
        /// A description of the <see cref="Preference"/> that is intended to be
        /// shown to the user and in the file as a comment.
        /// </summary>
        public virtual string Description { get; } = null;

        /// <summary>
        /// Whether or not the Value and DefaultValue properties can be set to
        /// something other than what is specified in AllowedValues. If
        /// AllowedValues is not used (it is <see langword="null"/> or empty),
        /// then <see cref="AllowUndefinedValues"/> must be set to
        /// <see langword="true"/>.
        /// </summary>
        public virtual bool AllowUndefinedValues { get; } = true;

        /// <summary>
        /// Is DefaultValue <see langword="null"/>.
        /// </summary>
        public virtual bool DefaultValueIsNull
            => GetDefaultValueAsObject() is null;

        /// <summary>
        /// Is Value <see langword="null"/>.
        /// </summary>
        public virtual bool ValueIsNull => GetValueAsObject() is null;

        /// <summary>
        /// Initializes <see cref="Name"/> with <paramref name="name"/> after
        /// it is processed with the
        /// <see cref="ProcessNameOrThrowIfInvalid(string)"/> method.
        /// </summary>
        /// <param name="name">The name of the <see cref="Preference"/> and
        /// must be not <see langword="null"/>, not empty and not consist only
        /// of white-space characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        protected Preference(string name)
        {
            Name = ProcessNameOrThrowIfInvalid(name);
        }

        /// <summary>
        /// Initializes <see cref="Name"/> with <paramref name="name"/> after
        /// it is processed with the
        /// <see cref="ProcessNameOrThrowIfInvalid(string)"/> method. It also
        /// initializes the <see cref="Description"/> and
        /// <see cref="AllowUndefinedValues"/> properties.
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
        /// <see cref="AllowUndefinedValues"/> must be set to
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
        /// Returns an <see cref="Array"/> of <see cref="string"/>s of formatted
        /// AllowedValues. The parameters are only used if
        /// <see cref="GetValueType()"/> implements <see cref="IFormattable"/>,
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
        public abstract string[] GetAllowedValuesAsStrings(string format,
            IFormatProvider formatProvider);

        /// <summary>
        /// Returns the DefaultValue as an <see cref="object"/> for versitility.
        /// A <see langword="null"/> means that the DefaultValue is not used,
        /// and a <see langword="null"/> Value means that it is set. On the
        /// other hand, if the DefaultValue is not <see langword="null"/>, then
        /// a <see langword="null"/> Value means that it has not been set.
        /// </summary>
        /// <returns>The DefaultValue as an <see cref="object"/>.</returns>
        public abstract object GetDefaultValueAsObject();

        /// <summary>
        /// Returns a <see cref="string"/> of the DefaultValue formatted. The
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
        /// <returns>DefaultValue as a <see cref="string"/>.</returns>
        public abstract string GetDefaultValueAsString(string format,
            IFormatProvider formatProvider);

        /// <summary>
        /// Returns the Value as an <see cref="object"/> for versitility. If
        /// DefaultValue is <see langword="null"/>, then that means a
        /// DefaultValue is not used, and a <see langword="null"/> Value means
        /// that it is set. On the other hand, if the DefaultValue is not
        /// <see langword="null"/>, then a <see langword="null"/> Value means
        /// that it has not been set.
        /// </summary>
        /// <returns>The Value as an <see cref="object"/>.</returns>
        public abstract object GetValueAsObject();

        /// <summary>
        /// Returns a <see cref="string"/> of the Value formatted. The
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
        /// <returns>Value as a <see cref="string"/>.</returns>
        public abstract string GetValueAsString(string format,
            IFormatProvider formatProvider);

        /// <summary>
        /// Returns the <see cref="Type"/> of Value.
        /// </summary>
        /// <returns></returns>
        public abstract Type GetValueType();

        /// <summary>
        /// Sets DefaultValue from <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">What to set Value to.</param>
        public abstract void SetDefaultValueFromObject(object defaultValue);

        /// <summary>
        /// Sets Value from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">What to set Value to.</param>
        public abstract void SetValueFromObject(object value);

        /// <summary>
        /// Tests whether or not <paramref name="name"/> is valid.
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
        /// <param name="allowedValuesCount"></param>
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
        /// <returns>The processed <paramref name="name"/> if it is
        /// valid.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public static string ProcessNameOrThrowIfInvalid(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(paramName: nameof(name),
                    message: "Cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(paramName: nameof(name),
                    message: "Cannot consist only of white-space characters.");
            }

            return name.Trim();
        }
    }
}
