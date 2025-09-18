using System;

namespace PreferenceGroups
{
    /// <summary>
    /// An attribute for a <see langword="class"/> where its
    /// <see langword="public"/> properties that have the
    /// <see cref="PreferenceAttribute"/> will each be a
    /// <see cref="Preference"/> in a <see cref="PreferenceGroup"/> when the
    /// <see cref="PreferenceGroupBuilder.BuildFrom(object, bool, bool)"/>
    /// method is called with an instantied object of the
    /// <see langword="class"/> that this attribute is attached to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PreferenceGroupAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="PreferenceGroup.Description"/> of the
        /// <see cref="PreferenceGroup"/>. Defaults to <see langword="null"/>.
        /// </summary>
        public string Description { get; set; } = null;
    }
}
