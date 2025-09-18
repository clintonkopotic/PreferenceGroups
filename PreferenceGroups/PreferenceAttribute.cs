using System;

namespace PreferenceGroups
{
    /// <summary>
    /// An attribute for a <see langword="class"/>'s <see langword="public"/>
    /// property to identify that it is intended to be a
    /// <see cref="Preference"/>. In addition, it is expected that the enclosing
    /// <see langword="class"/> is to be a <see cref="PreferenceGroup"/> when
    /// the <see cref="PreferenceGroupBuilder.BuildFrom(object, bool, bool)"/>
    /// method is called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PreferenceAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="Preference.Name"/> of the <see cref="Preference"/>.
        /// Defaults to the property name that this attribute is attached to. If
        /// the <see cref="PreferenceAttribute(string, bool, object[])"/>
        /// constructor is used, then this will be set to the <c>name</c>
        /// parameter.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// The <c>DefaultValue</c> of the <see cref="Preference"/>. Defaults to
        /// <see langword="null"/>. When the
        /// <see cref="PreferenceGroupBuilder.BuildFrom(object, bool, bool)"/>
        /// method is used, the <c>useValuesAsDefault</c> parameter set to
        /// <see langword="true"/> will set the <c>DefaultValue</c> of the
        /// <see cref="Preference"/> to the current value of property unless
        /// this is set to a non-<see langword="null"/> value. If the
        /// <c>useValuesAsDefault</c> parameter is set to
        /// <see langword="false"/>, then the <c>DefaultValue</c> of the
        /// <see cref="Preference"/> will be set to this value.
        /// </summary>
        public object DefaultValue { get; set; } = null;

        /// <summary>
        /// The <see cref="Preference.Description"/> of the
        /// <see cref="Preference"/>. Defaults to <see langword="null"/>.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// Any <c>AllowedValues</c> for the <see cref="Preference"/>.
        /// </summary>
        public object[] AllowedValues { get; }

        /// <summary>
        /// The <see cref="Preference.AllowUndefinedValues"/> of the
        /// <see cref="Preference"/>. Defaults to <see langword="null"/> which
        /// will attempt to use the default behavior of the type of
        /// <see cref="Preference"/>.
        /// </summary>
        public bool? AllowUndefinedValues { get; }

        /// <summary>
        /// Allows for the <see cref="AllowedValues"/> to be sorted during the
        /// build process.
        /// </summary>
        public bool? SortAllowValues { get; }

        /// <summary>
        /// Allows for specifiying the <see cref="Type"/> of the
        /// <see langword="class"/> that is for the <c>ValidityProcessor</c>.
        /// For <see cref="ClassPreference{T}"/> inherit from
        /// <see cref="ClassValidityProcessor{T}"/>, and for
        /// <see cref="StructPreference{T}"/> inherit from
        /// <see cref="StructValidityProcessor{T}"/>.
        /// </summary>
        public Type ValueValidityProcessorClassType { get; set; }

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public PreferenceAttribute() { }

        /// <summary>
        /// Initializes the attribute with <paramref name="name"/> where it is
        /// valid according to the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The <see cref="Preference.Name"/> of the
        /// <see cref="Preference"/> and overrides the value and behavior of
        /// <see cref="Name"/>.</param>
        /// <param name="allowedValues">Any <c>AllowedValues</c> for the
        /// <see cref="Preference"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceAttribute(string name, params object[] allowedValues)
        {
            Name = Preference.ProcessNameOrThrowIfInvalid(name, nameof(name));
            AllowedValues = allowedValues;
        }

        /// <summary>
        /// Initializes the attribute with <paramref name="name"/> where it is
        /// valid according to the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The <see cref="Preference.Name"/> of the
        /// <see cref="Preference"/> and overrides the value and behavior of
        /// <see cref="Name"/>.</param>
        /// <param name="allowUndefinedValues">Specifies whether or not to allow
        /// values that are not specified by
        /// <paramref name="allowedValues"/>.</param>
        /// <param name="allowedValues">Any <c>AllowedValues</c> for the
        /// <see cref="Preference"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceAttribute(string name, bool allowUndefinedValues,
            params object[] allowedValues)
        {
            Name = Preference.ProcessNameOrThrowIfInvalid(name, nameof(name));
            AllowUndefinedValues = allowUndefinedValues;
            AllowedValues = allowedValues;
        }

        /// <summary>
        /// Initializes the attribute with <paramref name="name"/> where it is
        /// valid according to the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The <see cref="Preference.Name"/> of the
        /// <see cref="Preference"/> and overrides the value and behavior of
        /// <see cref="Name"/>.</param>
        /// <param name="allowUndefinedValues">Specifies whether or not to allow
        /// values that are not specified by
        /// <paramref name="allowedValues"/>.</param>
        /// <param name="sortAllowedValues">Specifies whether or not to sort
        /// <paramref name="allowedValues"/> when the <see cref="Preference"/>
        /// is built.</param>
        /// <param name="allowedValues">Any <c>AllowedValues</c> for the
        /// <see cref="Preference"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceAttribute(string name, bool allowUndefinedValues,
            bool sortAllowedValues, params object[] allowedValues)
        {
            Name = Preference.ProcessNameOrThrowIfInvalid(name, nameof(name));
            AllowUndefinedValues = allowUndefinedValues;
            SortAllowValues = sortAllowedValues;
            AllowedValues = allowedValues;
        }
    }
}
