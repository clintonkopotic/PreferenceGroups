using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Uses the fluent builder pattern to build a
    /// <see cref="Int32Preference"/>.
    /// </summary>
    public class Int32PreferenceBuilder
    {
        private string description;

        private string name = string.Empty;

        private int? value = null;

        private int? defaultValue = null;

        private bool allowUndefinedValues = true;

        private bool sortAllowedValues = false;

        private StructValueValidityProcessor<int> validityProcessor
            = new StructValueValidityProcessor<int>();

        private List<int?> allowedValues = null;

        private Int32PreferenceBuilder() { }

        /// <summary>
        /// Specifies that only values defined by the
        /// <see cref="WithAllowedValues(int?[])"/> methods (or related
        /// methods) will be allowed.<br/>
        /// <b>NOTE</b>: If <see cref="WithAllowedValues(int?[])"/> is not
        /// used or <see cref="WithNoAllowedValues()"/> is used, then the
        /// <see cref="Preference.AllowUndefinedValues"/> will be set to
        /// <see langword="true"/> (which is the default behavior).
        /// </summary>
        /// <returns></returns>
        public Int32PreferenceBuilder AllowOnlyDefinedValues()
        {
            allowUndefinedValues = false;

            return this;
        }

        /// <summary>
        /// Specifies that any values that pass the
        /// <see cref="StructPreference{T}.ValidityProcessor"/> will be allowed.
        /// This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public Int32PreferenceBuilder AllowUndefinedValues()
        {
            allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="Int32Preference"/>.
        /// </summary>
        /// <returns></returns>
        public Int32Preference Build()
            => new Int32Preference(name, description, allowUndefinedValues,
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
        public Int32PreferenceBuilder DoNotSortAllowedValues()
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
        public Int32PreferenceBuilder SortAllowedValues()
        {
            sortAllowedValues = true;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithAllowedValues(
            IEnumerable<int?> allowedValues)
        {
            this.allowedValues = new List<int?>(allowedValues);

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithAllowedValues(
            params int?[] allowedValues)
            => WithAllowedValues((IEnumerable<int?>)allowedValues);

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithAllowedValuesAndDoNotSort(
            IEnumerable<int?> allowedValues)
            => WithAllowedValues(allowedValues)
            .DoNotSortAllowedValues();

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithAllowedValuesAndDoNotSort(
            params int?[] allowedValues)
            => WithAllowedValuesAndDoNotSort(
                (IEnumerable<int?>)allowedValues);

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithAllowedValuesAndSort(
            IEnumerable<int?> allowedValues)
            => WithAllowedValues(allowedValues)
            .SortAllowedValues();

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithAllowedValuesAndSort(
            params int?[] allowedValues)
            => WithAllowedValuesAndSort((IEnumerable<int?>)allowedValues);

        /// <summary>
        /// Will set <see cref="StructPreference{T}.DefaultValue"/> to
        /// <paramref name="defaultValue"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithDefaultValue(int? defaultValue)
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
        public Int32PreferenceBuilder WithDescription(string description)
        {
            this.description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> to
        /// <see langword="null"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public Int32PreferenceBuilder WithNoAllowedValues()
        {
            allowedValues = null;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithValidityProcessor(
            Int32ValueValidityProcessor processor)
            => WithValidityProcessor(
                (StructValueValidityProcessor<int>)processor);

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithValidityProcessor(
            StructValueValidityProcessor<int> processor)
        {
            validityProcessor = processor;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.Value"/> to
        /// <paramref name="value"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithValue(int? value)
        {
            this.value = value;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.Value"/> and
        /// <see cref="StructPreference{T}.DefaultValue"/> to
        /// <paramref name="valueAndAsDefault"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="valueAndAsDefault"></param>
        /// <returns></returns>
        public Int32PreferenceBuilder WithValueAndAsDefault(
            int? valueAndAsDefault)
            => WithValue(valueAndAsDefault)
            .WithDefaultValue(valueAndAsDefault);

        /// <summary>
        /// Builds a <see cref="Int32Preference"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int32Preference Build(string name)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name)).Build();

        /// <summary>
        /// Builds a <see cref="Int32Preference"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/>) and
        /// with <paramref name="value"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32Preference Build(string name, int? value)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name))
                .WithValue(value)
                .Build();

        /// <summary>
        /// Creates a <see cref="Int32PreferenceBuilder"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int32PreferenceBuilder Create(string name)
            => new Int32PreferenceBuilder()
            {
                name = Preference.ProcessNameOrThrowIfInvalid(name),
            };
    }
}
