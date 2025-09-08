using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Uses the fluent builder pattern to build an
    /// <see cref="EnumPreference"/> with the <typeparamref name="TEnum"/> which
    /// must be a specific <see cref="Enum"/>.
    /// </summary>
    public class EnumPreferenceBuilder<TEnum> where TEnum : struct, Enum
    {
        private string _description;

        private string _name = string.Empty;

        private TEnum? _value = null;

        private TEnum? _defaultValue = null;

        private List<Enum> _allowedValues = null;

        private bool _allowUndefinedValues = false;

        private bool _sortAllowedValues = false;

        private ClassValidityProcessor<Enum> _validityProcessor
            = new ClassValidityProcessor<Enum>();

        private EnumPreferenceBuilder() { }

        /// <summary>
        /// Specifies that only values defined by the
        /// <see cref="WithAllowedValues(TEnum?[])"/> methods (or related
        /// methods) will be allowed.<br/>
        /// <b>NOTE</b>: If <see cref="WithAllowedValues(TEnum?[])"/> is not
        /// used or <see cref="WithNoAllowedValues()"/> is used, then the
        /// <see cref="Preference.AllowUndefinedValues"/> will be set to
        /// <see langword="true"/> (which is the default behavior).
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> AllowOnlyDefinedValues()
        {
            _allowUndefinedValues = false;

            return this;
        }

        /// <summary>
        /// Specifies that any values that pass the
        /// <see cref="StructPreference{T}.ValidityProcessor"/> will be allowed.
        /// This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> AllowUndefinedValues()
        {
            _allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="EnumPreference"/>.
        /// </summary>
        /// <returns></returns>
        public EnumPreference Build() =>
            new EnumPreference(_name, new EnumTypeInfo(typeof(TEnum)),
                _description, _allowUndefinedValues, _allowedValues,
                _sortAllowedValues, _validityProcessor)
            {
                Value = _value,
                DefaultValue = _defaultValue,
            };

        /// <summary>
        /// The <c>AllowedValues</c> collection will not be sorted upon
        /// <see cref="Build()"/>. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> DoNotSortAllowedValues()
        {
            _sortAllowedValues = false;

            return this;
        }

        /// <summary>
        /// If <c>AllowedValues</c> is not <see langword="null"/> and is not
        /// empty, then it will be sorted (using a <see cref="SortedSet{T}"/>)
        /// upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> SortAllowedValues()
        {
            _sortAllowedValues = true;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValues(
            IEnumerable<TEnum> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            var values = new List<Enum>();

            foreach (var allowedValue in allowedValues)
            {
                values.Add(allowedValue);
            }

            _allowedValues = values;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValues(
            IEnumerable<TEnum?> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            var values = new List<Enum>();

            foreach (var allowedValue in allowedValues)
            {
                if (!(allowedValue is null))
                {
                    values.Add(allowedValue);
                }
            }

            _allowedValues = values;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValues(
            params TEnum?[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues((IEnumerable<TEnum?>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValues(
            params TEnum[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues((IEnumerable<TEnum>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndDoNotSort(
            IEnumerable<TEnum?> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues(allowedValues).DoNotSortAllowedValues();
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndDoNotSort(
            IEnumerable<TEnum> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues(allowedValues).DoNotSortAllowedValues();
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndDoNotSort(
            params TEnum?[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndDoNotSort(
                (IEnumerable<TEnum?>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndDoNotSort(
            params TEnum[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndDoNotSort(
                (IEnumerable<TEnum>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndSort(
            IEnumerable<TEnum> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues(allowedValues).SortAllowedValues();
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndSort(
            IEnumerable<TEnum?> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues(allowedValues).SortAllowedValues();
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndSort(
            params TEnum?[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndSort((IEnumerable<TEnum?>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithAllowedValuesAndSort(
            params TEnum[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndSort((IEnumerable<TEnum>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.DefaultValue"/> to
        /// <paramref name="defaultValue"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> WithDefaultValue(TEnum? defaultValue)
        {
            _defaultValue = defaultValue;

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
        public EnumPreferenceBuilder<TEnum> WithDescription(string description)
        {
            _description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> to
        /// <see langword="null"/> and forces
        /// <see cref="Preference.AllowUndefinedValues"/> to
        /// <see langword="true"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> WithNoAllowedValues()
        {
            _allowedValues = null;
            _allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithValidityProcessor(
            ClassValidityProcessor<Enum> processor)
        {
            if (processor is null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            _validityProcessor = processor;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public EnumPreferenceBuilder<TEnum> WithValidityProcessor(
            EnumValidityProcessor processor)
        {
            if (processor is null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            return WithValidityProcessor(
                (ClassValidityProcessor<Enum>)processor);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.Value"/> to
        /// <paramref name="value"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> WithValue(TEnum? value)
        {
            _value = value;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.Value"/> and
        /// <see cref="StructPreference{T}.DefaultValue"/> to
        /// <paramref name="valueAndAsDefault"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="valueAndAsDefault"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder<TEnum> WithValueAndAsDefault(
            TEnum? valueAndAsDefault)
            => WithValue(valueAndAsDefault)
                .WithDefaultValue(valueAndAsDefault);

        /// <summary>
        /// Builds a <see cref="EnumPreference"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreference Build(string name)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)))
                .Build();

        /// <summary>
        /// Builds a <see cref="EnumPreference"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>) and with <paramref name="value"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreference Build(string name, TEnum? value)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)))
                .WithValue(value)
                .Build();

        /// <summary>
        /// Creates a <see cref="EnumPreferenceBuilder{TEnum}"/> with
        /// <paramref name="name"/> (after it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreferenceBuilder<TEnum> Create(string name)
            => new EnumPreferenceBuilder<TEnum>()
            {
                _name = Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)),
            };
    }
}
