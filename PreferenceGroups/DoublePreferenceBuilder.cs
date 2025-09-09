using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Uses the fluent builder pattern to build a
    /// <see cref="DoublePreference"/>.
    /// </summary>
    public class DoublePreferenceBuilder
    {
        private string _description;

        private string _name = string.Empty;

        private double? _value = null;

        private double? _defaultValue = null;

        private List<double?> _allowedValues = null;

        private bool _allowUndefinedValues = true;

        private bool _sortAllowedValues = false;

        private StructValidityProcessor<double> _validityProcessor
            = new StructValidityProcessor<double>();

        private DoublePreferenceBuilder() { }

        /// <summary>
        /// Specifies that only values defined by the
        /// <see cref="WithAllowedValues(double?[])"/> methods (or related
        /// methods) will be allowed.<br/>
        /// <b>NOTE</b>: If <see cref="WithAllowedValues(double?[])"/> is not
        /// used or <see cref="WithNoAllowedValues()"/> is used, then the
        /// <see cref="Preference.AllowUndefinedValues"/> will be set to
        /// <see langword="true"/> (which is the default behavior).
        /// </summary>
        /// <returns></returns>
        public DoublePreferenceBuilder AllowOnlyDefinedValues()
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
        public DoublePreferenceBuilder AllowUndefinedValues()
        {
            _allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="DoublePreference"/>.
        /// </summary>
        /// <returns></returns>
        public DoublePreference Build() =>
            new DoublePreference(_name, _description, _allowUndefinedValues,
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
        public DoublePreferenceBuilder DoNotSortAllowedValues()
        {
            _sortAllowedValues = false;

            return this;
        }

        /// <summary>
        /// Specifies whether or not to allow values that are not specified by
        /// <see cref="StructPreference{T}.AllowedValues"/>.
        /// </summary>
        /// <returns></returns>
        public DoublePreferenceBuilder SetAllowUndefinedValues(
            bool allowUndefinedValues)
        {
            _allowUndefinedValues = allowUndefinedValues;

            return this;
        }

        /// <summary>
        /// Specifies whether or not to sort
        /// <see cref="StructPreference{T}.AllowedValues"/> when the
        /// <see cref="BooleanPreference"/> is built.
        /// </summary>
        /// <returns></returns>
        public DoublePreferenceBuilder SetSortAllowedValues(
            bool sortAllowedValues)
        {
            _sortAllowedValues = sortAllowedValues;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public DoublePreferenceBuilder SetValidityProcessor(
            DoubleValidityProcessor processor)
            => SetValidityProcessor((StructValidityProcessor<double>)processor);

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public DoublePreferenceBuilder SetValidityProcessor(
            StructValidityProcessor<double> processor)
        {
            _validityProcessor = processor
                ?? new StructValidityProcessor<double>();

            return this;
        }

        /// <summary>
        /// If <c>AllowedValues</c> is not <see langword="null"/> and is not
        /// empty, then it will be sorted (using a <see cref="SortedSet{T}"/>)
        /// upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public DoublePreferenceBuilder SortAllowedValues()
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
        public DoublePreferenceBuilder WithAllowedValues(
            IEnumerable<double?> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            _allowedValues = new List<double?>(allowedValues);

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
        public DoublePreferenceBuilder WithAllowedValues(
            IEnumerable<double> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            var values = new List<double?>();

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
        /// <see cref="Build()"/>, by calling the
        /// <see cref="DoublePreference.ConvertObjectToValueBase(object)"/>
        /// method on each item of <paramref name="allowedValues"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public DoublePreferenceBuilder WithAllowedValues(
            IEnumerable<object> allowedValues)
        {
            if (allowedValues is null)
            {
                return this;
            }

            var values = new List<double?>();

            foreach (var allowedValue in allowedValues)
            {
                try
                {
                    var value = DoublePreference.ConvertObjectToValueBase(
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
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedValues"/> is
        /// <see langword="null"/>.</exception>
        public DoublePreferenceBuilder WithAllowedValues(
            params double?[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues((IEnumerable<double?>)allowedValues);
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
        public DoublePreferenceBuilder WithAllowedValues(
            params double[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues((IEnumerable<double>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>, by calling the
        /// <see cref="DoublePreference.ConvertObjectToValueBase(object)"/>
        /// method on each item of <paramref name="allowedValues"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public DoublePreferenceBuilder WithAllowedValues(
            params object[] allowedValues)
        {
            if (allowedValues is null)
            {
                return this;
            }

            return WithAllowedValues((IEnumerable<object>)allowedValues);
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
        public DoublePreferenceBuilder WithAllowedValuesAndDoNotSort(
            IEnumerable<double?> allowedValues)
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
        public DoublePreferenceBuilder WithAllowedValuesAndDoNotSort(
            IEnumerable<double> allowedValues)
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
        public DoublePreferenceBuilder WithAllowedValuesAndDoNotSort(
            params double?[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndDoNotSort(
                (IEnumerable<double?>)allowedValues);
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
        public DoublePreferenceBuilder WithAllowedValuesAndDoNotSort(
            params double[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndDoNotSort(
                (IEnumerable<double>)allowedValues);
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
        public DoublePreferenceBuilder WithAllowedValuesAndSort(
            IEnumerable<double?> allowedValues)
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
        public DoublePreferenceBuilder WithAllowedValuesAndSort(
            IEnumerable<double> allowedValues)
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
        public DoublePreferenceBuilder WithAllowedValuesAndSort(
            params double?[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndSort((IEnumerable<double?>)allowedValues);
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
        public DoublePreferenceBuilder WithAllowedValuesAndSort(
            params double[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndSort((IEnumerable<double>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.DefaultValue"/> to
        /// <paramref name="defaultValue"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DoublePreferenceBuilder WithDefaultValue(double? defaultValue)
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
        public DoublePreferenceBuilder WithDescription(string description)
        {
            _description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.AllowedValues"/> to
        /// <see langword="null"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public DoublePreferenceBuilder WithNoAllowedValues()
        {
            _allowedValues = null;

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
        public DoublePreferenceBuilder WithValidityProcessor(
            DoubleValidityProcessor processor)
        {
            if (processor is null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            return WithValidityProcessor(
                (StructValidityProcessor<double>)processor);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.ValidityProcessor"/> to
        /// <paramref name="processor"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/>
        /// is <see langword="null"/>.</exception>
        public DoublePreferenceBuilder WithValidityProcessor(
            StructValidityProcessor<double> processor)
        {
            if (processor is null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            _validityProcessor = processor;

            return this;
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.Value"/> to
        /// <paramref name="value"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DoublePreferenceBuilder WithValue(double? value)
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
        public DoublePreferenceBuilder WithValueAndAsDefault(
            double? valueAndAsDefault)
            => WithValue(valueAndAsDefault)
                .WithDefaultValue(valueAndAsDefault);

        /// <summary>
        /// Builds a <see cref="DoublePreference"/> with
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
        public static DoublePreference Build(string name)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)))
                .Build();

        /// <summary>
        /// Builds a <see cref="DoublePreference"/> with
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
        public static DoublePreference Build(string name, double? value)
            => Create(Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)))
                .WithValue(value)
                .Build();

        /// <summary>
        /// Creates a <see cref="DoublePreferenceBuilder"/> with
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
        public static DoublePreferenceBuilder Create(string name)
            => new DoublePreferenceBuilder()
            {
                _name = Preference.ProcessNameOrThrowIfInvalid(name,
                    nameof(name)),
            };
    }
}
