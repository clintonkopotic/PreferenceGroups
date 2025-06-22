using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Uses the fluent builder pattern to build a
    /// <see cref="StringPreference"/>.
    /// </summary>
    public class StringPreferenceBuilder
    {
        private string description;

        private string name = string.Empty;

        private string value = null;

        private string defaultValue = null;

        private bool allowUndefinedValues = true;

        private bool sortAllowedValues = false;

        private ClassValueValidityProcessor<string> validityProcessor
            = new ClassValueValidityProcessor<string>();

        private List<string> allowedValues = null;

        private StringPreferenceBuilder() { }

        /// <summary>
        /// Specifies that only values defined by the
        /// <see cref="WithAllowedValues(string[])"/> methods (or related
        /// methods) will be allowed.<br/>
        /// <b>NOTE</b>: If <see cref="WithAllowedValues(string[])"/> is not
        /// used or <see cref="WithNoAllowedValues()"/> is used, then the
        /// <see cref="Preference.AllowUndefinedValues"/> will be set to
        /// <see langword="true"/> (which is the default behavior).
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder AllowOnlyDefinedValues()
        {
            allowUndefinedValues = false;

            return this;
        }

        /// <summary>
        /// Specifies that any values that pass the
        /// <see cref="ClassPreference{T}.ValidityProcessor"/> will be allowed.
        /// This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder AllowUndefinedValues()
        {
            allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="StringPreference"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreference Build()
            => new StringPreference(name, description, allowUndefinedValues,
                allowedValues, sortAllowedValues, validityProcessor)
            {
                Value = value,
                DefaultValue = defaultValue,
            };

        /// <summary>
        /// The <c>AllowedValues</c> collection will not be sorted upon
        /// <see cref="Build()"/>. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder DoNotSortAllowedValues()
        {
            sortAllowedValues = false;

            return this;
        }

        /// <summary>
        /// If <c>AllowedValues</c> is not <see langword="null"/> and is not
        /// empty, then it will be sorted (using a <see cref="SortedSet{T}"/>)
        /// upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder SortAllowedValues()
        {
            sortAllowedValues = true;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValues(
            IEnumerable<string> allowedValues)
        {
            this.allowedValues = new List<string>(allowedValues);

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValues(
            params string[] allowedValues)
            => WithAllowedValues((IEnumerable<string>)allowedValues);

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValuesAndDoNotSort(
            IEnumerable<string> allowedValues)
            => WithAllowedValues(allowedValues)
            .DoNotSortAllowedValues();

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValuesAndDoNotSort(
            params string[] allowedValues)
            => WithAllowedValuesAndDoNotSort(
                (IEnumerable<string>)allowedValues);

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValuesAndSort(
            IEnumerable<string> allowedValues)
            => WithAllowedValues(allowedValues)
            .SortAllowedValues();

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValuesAndSort(
            params string[] allowedValues)
            => WithAllowedValuesAndSort((IEnumerable<string>)allowedValues);

        /// <summary>
        /// Will set <see cref="StringPreference.DefaultValue"/> to
        /// <paramref name="defaultValue"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithDefaultValue(string defaultValue)
        {
            this.defaultValue = defaultValue;

            return this;
        }

        /// <summary>
        /// Will set <see cref="Preference.Description"/> to
        /// <paramref name="description"/> after it is trimmed (by using
        /// <see cref="string.Trim()"/>) if it is not
        /// <see langword="null"/> upon <see cref="Build()"/>. The default
        /// behavior is for <see cref="Preference.Description"/> to be
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithDescription(string description)
        {
            this.description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> to
        /// <see langword="null"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder WithNoAllowedValues()
        {
            allowedValues = null;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithValidityProcessor(
            StringValueValidityProcessor processor)
            => WithValidityProcessor(
                (ClassValueValidityProcessor<string>)processor);

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithValidityProcessor(
            ClassValueValidityProcessor<string> processor)
        {
            validityProcessor = processor;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StringPreference.Value"/> to
        /// <paramref name="value"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithValue(string value)
        {
            this.value = value;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StringPreference.Value"/> and
        /// <see cref="StringPreference.DefaultValue"/> to
        /// <paramref name="valueAndAsDefault"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="valueAndAsDefault"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithValueAndAsDefault(
            string valueAndAsDefault)
            => WithValue(valueAndAsDefault)
            .WithDefaultValue(valueAndAsDefault);

        /// <summary>
        /// Builds a <see cref="StringPreference"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static StringPreference Build(string name)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name)).Build();

        /// <summary>
        /// Builds a <see cref="StringPreference"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/>) and
        /// with <paramref name="value"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static StringPreference Build(string name, string value)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name))
                .WithValue(value)
                .Build();

        /// <summary>
        /// Creates a <see cref="StringPreferenceBuilder"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static StringPreferenceBuilder Create(string name)
            => new StringPreferenceBuilder()
            {
                name = Preference.ProcessNameOrThrowIfInvalid(name),
            };
    }
}
