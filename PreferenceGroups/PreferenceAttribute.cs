using System;

namespace PreferenceGroups
{
    /// <summary>
    /// An attribute for a <see langword="class"/>'s <see langword="public"/>
    /// property to identify that it is intended to be a
    /// <see cref="Preference"/>. In addition, it is expected that the enclosing
    /// <see langword="class"/> is to be a <see cref="PreferenceGroup"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PreferenceAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="Preference.Name"/> of the <see cref="Preference"/>.
        /// Defaults to the property name that this attribute is attached to.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// The <c>DefaultValue</c> of the <see cref="Preference"/>. Defaults to
        /// <see langword="null"/>.
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
        /// <see cref="Preference"/>.</param>
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
