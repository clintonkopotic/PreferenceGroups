using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Indicates which step failed when setting either the value or default
    /// value of a <see cref="Preference"/>.
    /// </summary>
    public enum SetValueStepFailure
    {
        /// <summary>
        /// No failure was encountered.
        /// </summary>
        None,

        /// <summary>
        /// This is a catch-all step failure that an <see cref="Exception"/> was
        /// generated.
        /// </summary>
        Unknown,

        /// <summary>
        /// While ensuring that the <see cref="Preference.Name"/> was valid, by
        /// calling the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method, an <see cref="Exception"/> was generated.
        /// </summary>
        ProcessingName,

        /// <summary>
        /// While retrieving a <see cref="Preference"/> an
        /// <see cref="Exception"/> was generated.
        /// </summary>
        RetrievingPreference,

        /// <summary>
        /// While casting a value from one type to the the of the
        /// <see cref="Preference"/> value (see
        /// <see cref="Preference.GetValueType()"/>), especially in the
        /// <see cref="Preference.SetDefaultValueFromObject(object)"/> or the
        /// <see cref="Preference.SetValueFromObject(object)"/> methods, an
        /// <see cref="Exception"/> was generated.
        /// </summary>
        Casting,

        /// <summary>
        /// While parsing a value to be set for a <see cref="Preference"/> an
        /// <see cref="Exception"/> was generated.
        /// </summary>
        Parsing,

        /// <summary>
        /// While converting a value to be set for a <see cref="Preference"/> an
        /// <see cref="Exception"/> was generated.
        /// </summary>
        Converting,

        /// <summary>
        /// While calling the <c>Pre</c> process step an <see cref="Exception"/>
        /// was generated.
        /// </summary>
        PreProcessing,

        /// <summary>
        /// While calling the <c>IsValid</c> validity step an
        /// <see cref="Exception"/> was generated explaining why the value is
        /// not valid.
        /// </summary>
        ValidityCheck,

        /// <summary>
        /// While calling the <c>Post</c> process step an
        /// <see cref="Exception"/> was generated.
        /// </summary>
        PostProcessing,

        /// <summary>
        /// While setting the value an <see cref="Exception"/> was
        /// generated.
        /// </summary>
        SettingValue,
    }
}
