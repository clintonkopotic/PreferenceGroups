using System;

namespace PreferenceGroups
{
    /// <summary>
    /// An attribute for a <see langword="class"/>'s <see langword="public"/>
    /// property to identify that it is intended to be a
    /// <see cref="Preference"/>. In addition, it is expected that the enclosing
    /// <see langword="class"/> is to be a <see cref="PreferenceGroup"/> when
    /// the <see cref="PreferenceGroupBuilder.BuildFrom(object, bool)"/> method
    /// is called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PreferenceAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="Preference.Name"/> of the <see cref="Preference"/>.
        /// Defaults to the property name that this attribute is attached to. If
        /// the <see cref="PreferenceAttribute(string)"/> constructor is used,
        /// then this will be set to the <c>name</c> parameter.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// The <c>DefaultValue</c> of the <see cref="Preference"/>. Defaults to
        /// <see langword="null"/>. When the
        /// <see cref="PreferenceGroupBuilder.BuildFrom(object, bool)"/> method
        /// is used, the <c>useValuesAsDefault</c> parameter set to
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
        /// The <see cref="Preference.AllowUndefinedValues"/> of the
        /// <see cref="Preference"/>. Defaults to <see langword="true"/>.
        /// </summary>
        public bool AllowUndefinedValues { get; set; } = true;

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public PreferenceAttribute() { }

        /// <summary>
        /// Initializes the attribute with <paramref name="name"/> where it is
        /// valid according to the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// </summary>
        /// <param name="name">The <see cref="Preference.Name"/> of the
        /// <see cref="Preference"/> and overrides the value and behavior of
        /// <see cref="Name"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceAttribute(string name)
        {
            Name = Preference.ProcessNameOrThrowIfInvalid(name);
        }
    }
}
