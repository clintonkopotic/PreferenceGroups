using System;

namespace PreferenceGroups
{
    /// <summary>
    /// The kind of <see cref="PreferenceStoreItem"/>.
    /// </summary>
    public enum PreferenceStoreItemKind
    {
        /// <summary>
        /// The default item that has yet to be set. Do not use.
        /// </summary>
        None,

        /// <summary>
        /// For a <see cref="PreferenceGroups.Preference"/>.
        /// </summary>
        Preference,

        /// <summary>
        /// For a <see cref="PreferenceGroups.PreferenceGroup"/>.
        /// </summary>
        PreferenceGroup,

        /// <summary>
        /// For an <see cref="Array"/> of
        /// <see cref="PreferenceGroups.PreferenceGroup"/>.
        /// </summary>
        ArrayOfPreferenceGroups,

        /// <summary>
        /// For a <see cref="PreferenceGroups.PreferenceStore"/>
        /// </summary>
        PreferenceStore,

        /// <summary>
        /// For an <see cref="Array"/> of
        /// <see cref="PreferenceGroups.PreferenceStore"/>.
        /// </summary>
        ArrayOfPreferenceStores,
    }
}
