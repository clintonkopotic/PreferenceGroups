using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Uses the fluent builder pattern to build a
    /// <see cref="StringPreference"/>.
    /// </summary>
    public class StringPreferenceBuilder
    {
        private string _description;

        private string _name = string.Empty;

        private string _value = null;

        private string _defaultValue = null;

        private List<string> _allowedValues = null;

        private bool _allowUndefinedValues = true;

        private bool _sortAllowedValues = false;

        private ClassValidityProcessor<string> _validityProcessor
            = new ClassValidityProcessor<string>();

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
            _allowUndefinedValues = false;

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
            _allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="StringPreference"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreference Build() =>
            new StringPreference(_name, _description, _allowUndefinedValues,
                _allowedValues, _sortAllowedValues, _validityProcessor)
            {
                Value = _value,
                DefaultValue = _defaultValue,
            };

        /// <summary>
        /// The <c>AllowedValues</c> collection will not be sorted upon
        /// <see cref="Build()"/>. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder DoNotSortAllowedValues()
        {
            _sortAllowedValues = false;

            return this;
        }

        /// <summary>
        /// Specifies whether or not to allow values that are not specified by
        /// <see cref="ClassPreference{T}.AllowedValues"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder SetAllowUndefinedValues(
            bool allowUndefinedValues)
        {
            _allowUndefinedValues = allowUndefinedValues;

            return this;
        }

        /// <summary>
        /// Specifies whether or not to sort
        /// <see cref="ClassPreference{T}.AllowedValues"/> when the
        /// <see cref="StringPreference"/> is built.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder SetSortAllowedValues(
            bool sortAllowedValues)
        {
            _sortAllowedValues = sortAllowedValues;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public StringPreferenceBuilder SetValidityProcessor(
            ClassValidityProcessor<string> processor)
        {
            _validityProcessor = processor
                ?? new ClassValidityProcessor<string>();

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public StringPreferenceBuilder SetValidityProcessor(
            StringValidityProcessor processor)
            => SetValidityProcessor((ClassValidityProcessor<string>)processor);

        /// <summary>
        /// If <c>AllowedValues</c> is not <see langword="null"/> and is not
        /// empty, then it will be sorted (using a <see cref="SortedSet{T}"/>)
        /// upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder SortAllowedValues()
        {
            _sortAllowedValues = true;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithAllowedValues(
            IEnumerable<string> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            _allowedValues = new List<string>(allowedValues);

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>, by calling the
        /// <see cref="StringPreference.ConvertObjectToValueBase(object)"/>
        /// method on each item of <paramref name="allowedValues"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValues(
            IEnumerable<object> allowedValues)
        {
            if (allowedValues is null)
            {
                return this;
            }

            var values = new List<string>();

            foreach (var allowedValue in allowedValues)
            {
                try
                {
                    var value = StringPreference.ConvertObjectToValueBase(
                        allowedValue);

                    if (!(value is null))
                    {
                        values.Add(value);
                    }
                }
                catch
                { }
            }

            _allowedValues = values;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithAllowedValues(
            params string[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues((IEnumerable<string>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>, by calling the
        /// <see cref="StringPreference.ConvertObjectToValueBase(object)"/>
        /// method on each item of <paramref name="allowedValues"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithAllowedValues(
            params object[] allowedValues)
        {
            if (allowedValues is null)
            {
                return this;
            }

            return WithAllowedValues((IEnumerable<object>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithAllowedValuesAndDoNotSort(
            IEnumerable<string> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues(allowedValues).DoNotSortAllowedValues();
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will not be
        /// sorted (by using <see cref="HashSet{T}"/>) upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithAllowedValuesAndDoNotSort(
            params string[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndDoNotSort(
                (IEnumerable<string>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithAllowedValuesAndSort(
            IEnumerable<string> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues(allowedValues).SortAllowedValues();
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> and they will be sorted
        /// (by using <see cref="SortedSet{T}"/>) upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithAllowedValuesAndSort(
            params string[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndSort((IEnumerable<string>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StringPreference.DefaultValue"/> to
        /// <paramref name="defaultValue"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithDefaultValue(string defaultValue)
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
        public StringPreferenceBuilder WithDescription(string description)
        {
            _description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.AllowedValues"/> to
        /// <see langword="null"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public StringPreferenceBuilder WithNoAllowedValues()
        {
            _allowedValues = null;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithValidityProcessor(
            ClassValidityProcessor<string> processor)
        {
            if (processor is null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            _validityProcessor = processor;

            return this;
        }

        /// <summary>
        /// Will set <see cref="ClassPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public StringPreferenceBuilder WithValidityProcessor(
            StringValidityProcessor processor)
        {
            if (processor is null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            return WithValidityProcessor(
                (ClassValidityProcessor<string>)processor);
        }

        /// <summary>
        /// Will set <see cref="StringPreference.Value"/> to
        /// <paramref name="value"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public StringPreferenceBuilder WithValue(string value)
        {
            _value = value;

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
        public static StringPreference Build(string name)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)))
                .Build();

        /// <summary>
        /// Builds a <see cref="StringPreference"/> with
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
        public static StringPreference Build(string name, string value)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)))
                .WithValue(value)
                .Build();

        /// <summary>
        /// Creates a <see cref="StringPreferenceBuilder"/> with
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
        public static StringPreferenceBuilder Create(string name)
            => new StringPreferenceBuilder()
            {
                _name = Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)),
            };
    }
}
