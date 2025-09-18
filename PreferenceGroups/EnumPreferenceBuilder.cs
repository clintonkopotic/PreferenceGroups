using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Uses the fluent builder pattern to build an
    /// <see cref="EnumPreference"/>.
    /// </summary>
    public class EnumPreferenceBuilder
    {
        private readonly string _name;

        private readonly Type _enumType;

        private readonly EnumTypeInfo _enumTypeInfo;

        private string _description;

        private Enum _value = null;

        private Enum _defaultValue = null;

        private IReadOnlyCollection<Enum> _allowedValues = null;

        private bool _allowUndefinedValues = false;

        private bool _sortAllowedValues = false;

        private ClassValidityProcessor<Enum> _validityProcessor
            = new ClassValidityProcessor<Enum>();

        private EnumPreferenceBuilder(string name, EnumTypeInfo enumTypeInfo)
        {
            _name = Preference.ProcessNameOrThrowIfInvalid(name, nameof(name));

            if (enumTypeInfo is null)
            {
                throw new ArgumentNullException(nameof(enumTypeInfo));
            }

            _enumTypeInfo = enumTypeInfo;
            _enumType = _enumTypeInfo.EnumType;
        }

        /// <summary>
        /// Specifies that only values defined by the
        /// <see cref="WithAllowedValues(Enum[])"/> methods (or related
        /// methods) will be allowed.<br/>
        /// <b>NOTE</b>: If <see cref="WithAllowedValues(Enum[])"/> is not
        /// used or <see cref="WithNoAllowedValues()"/> is used, then the
        /// <see cref="Preference.AllowUndefinedValues"/> will be set to
        /// <see langword="true"/> (which is the default behavior).
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder AllowOnlyDefinedValues()
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
        public EnumPreferenceBuilder AllowUndefinedValues()
        {
            _allowUndefinedValues = true;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="EnumPreference"/>.
        /// </summary>
        /// <returns></returns>
        public EnumPreference Build() =>
            new EnumPreference(_name, _enumTypeInfo, _description,
                _allowUndefinedValues, _allowedValues, _sortAllowedValues,
                _validityProcessor)
            {
                Value = _value,
                DefaultValue = _defaultValue,
            };

        /// <summary>
        /// The <c>AllowedValues</c> collection will not be sorted upon
        /// <see cref="Build()"/>. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder DoNotSortAllowedValues()
        {
            _sortAllowedValues = false;

            return this;
        }

        /// <summary>
        /// Specifies whether or not to allow values that are not specified by
        /// <see cref="ClassPreference{T}.AllowedValues"/>.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder SetAllowUndefinedValues(
            bool allowUndefinedValues)
        {
            _allowUndefinedValues = allowUndefinedValues;

            return this;
        }

        /// <summary>
        /// Specifies whether or not to sort
        /// <see cref="ClassPreference{T}.AllowedValues"/> when the
        /// <see cref="EnumPreference"/> is built.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder SetSortAllowedValues(
            bool sortAllowedValues)
        {
            _sortAllowedValues = sortAllowedValues;

            return this;
        }

        /// <summary>
        /// Sets the <see cref="EnumPreference.ValidityProcessor"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder SetValidityProcessor(
            ClassValidityProcessor<Enum> processor)
        {
            _validityProcessor = processor
                ?? new ClassValidityProcessor<Enum>();

            return this;
        }

        /// <summary>
        /// Sets the <see cref="EnumPreference.ValidityProcessor"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder SetValidityProcessor(
            EnumValidityProcessor processor)
        {
            _validityProcessor = processor;

            return this;
        }

        /// <summary>
        /// If <c>AllowedValues</c> is not <see langword="null"/> and is not
        /// empty, then it will be sorted (using a <see cref="SortedSet{T}"/>)
        /// upon <see cref="Build()"/>.
        /// </summary>
        /// <returns></returns>
        public EnumPreferenceBuilder SortAllowedValues()
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
        public EnumPreferenceBuilder WithAllowedValues(
            IEnumerable<Enum> allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }
            
            _allowedValues = EnumPreference.ProcessAllowedValues(_enumTypeInfo,
                allowedValues, false);

            return this;
        }

        /// <summary>
        /// Will set <see cref="EnumPreference.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>, by calling the
        /// <see cref="EnumPreference.ConvertObjectToValueBase(EnumTypeInfo,
        /// object)"/> method on each item of <paramref name="allowedValues"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder WithAllowedValues(
            IEnumerable<object> allowedValues)
        {
            if (allowedValues is null)
            {
                return this;
            }

            _allowedValues = EnumPreference.ProcessAllowedValues(_enumTypeInfo,
                allowedValues, false);

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
        public EnumPreferenceBuilder WithAllowedValues(
            params Enum[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValues((IEnumerable<Enum>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="EnumPreference.AllowedValues"/> with the
        /// provided <paramref name="allowedValues"/> upon
        /// <see cref="Build()"/>, by calling the
        /// <see cref="EnumPreference.ConvertObjectToValueBase(EnumTypeInfo,
        /// object)"/> method on each item of <paramref name="allowedValues"/>.
        /// </summary>
        /// <param name="allowedValues"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder WithAllowedValues(
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
        public EnumPreferenceBuilder WithAllowedValuesAndDoNotSort(
            IEnumerable<Enum> allowedValues)
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
        public EnumPreferenceBuilder WithAllowedValuesAndDoNotSort(
            params Enum[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndDoNotSort(
                (IEnumerable<Enum>)allowedValues);
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
        public EnumPreferenceBuilder WithAllowedValuesAndSort(
            IEnumerable<Enum> allowedValues)
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
        public EnumPreferenceBuilder WithAllowedValuesAndSort(
            params Enum[] allowedValues)
        {
            if (allowedValues is null)
            {
                throw new ArgumentNullException(nameof(allowedValues));
            }

            return WithAllowedValuesAndSort((IEnumerable<Enum>)allowedValues);
        }

        /// <summary>
        /// Will set <see cref="StructPreference{T}.DefaultValue"/> to
        /// <paramref name="defaultValue"/> upon <see cref="Build()"/>.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EnumPreferenceBuilder WithDefaultValue(Enum defaultValue)
        {
            if (!(defaultValue is null))
            {
                EnumHelpers.ThrowIfTypeNotEqual(_enumType, defaultValue,
                    enumParamName: nameof(defaultValue));
            }

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
        public EnumPreferenceBuilder WithDescription(string description)
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
        public EnumPreferenceBuilder WithNoAllowedValues()
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
        public EnumPreferenceBuilder WithValidityProcessor(
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
        public EnumPreferenceBuilder WithValidityProcessor(
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
        public EnumPreferenceBuilder WithValue(Enum value)
        {
            if (!(value is null))
            {
                EnumHelpers.ThrowIfTypeNotEqual(_enumType, value,
                    enumParamName: nameof(value));
            }

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
        public EnumPreferenceBuilder WithValueAndAsDefault(
            Enum valueAndAsDefault)
        {
            if (!(valueAndAsDefault is null))
            {
                EnumHelpers.ThrowIfTypeNotEqual(_enumType, valueAndAsDefault,
                    enumParamName: nameof(valueAndAsDefault));
            }

            return WithValue(valueAndAsDefault)
                .WithDefaultValue(valueAndAsDefault);
        }

        /// <summary>
        /// Builds an <see cref="EnumPreference"/>, using the <see cref="Type"/>
        /// of <paramref name="value"/>, with <paramref name="name"/> (after it
        /// is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>). <paramref name="value"/> must not be
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">The <see cref="EnumPreference.Value"/>. Must not
        /// be <see langword="null"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="value"/> is <see langword="null"/>.</exception>
        public static EnumPreference Build(string name, Enum value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Create(processedName, value.GetType())
                .WithValue(value)
                .Build();
        }

        /// <summary>
        /// Builds a <see cref="EnumPreference"/>, using
        /// <paramref name="enumType"/> as its <see cref="Enum"/>
        /// <see cref="Type"/>, with <paramref name="name"/> (after it is
        /// processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters. Or, <paramref name="enumType"/> is not an
        /// <see cref="Enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="enumType"/> is <see langword="null"/>.</exception>
        public static EnumPreference Build(string name, Type enumType)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            EnumHelpers.ThrowIfNullOrNotEnum(enumType, nameof(enumType));

            return Create(processedName, enumType)
                .Build();
        }

        /// <summary>
        /// Builds an <see cref="EnumPreference"/>, using the <see cref="Type"/>
        /// of <typeparamref name="TEnum"/>, with <paramref name="name"/> (after
        /// it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="name"></param>
        /// <param name="value">The <see cref="EnumPreference.Value"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="value"/> is <see langword="null"/>.</exception>
        public static EnumPreference Build<TEnum>(string name, TEnum value)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return Create(processedName, typeof(TEnum))
                .WithValue(value)
                .Build();
        }

        /// <summary>
        /// Builds an <see cref="EnumPreference"/>, using the <see cref="Type"/>
        /// of <typeparamref name="TEnum"/>, with <paramref name="name"/> (after
        /// it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="name"></param>
        /// <param name="value">The <see cref="EnumPreference.Value"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        public static EnumPreference Build<TEnum>(string name, TEnum? value)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return Create(processedName, typeof(TEnum))
                .WithValue(value)
                .Build();
        }

        /// <summary>
        /// Builds an <see cref="EnumPreference"/>, using the <see cref="Type"/>
        /// of <typeparamref name="TEnum"/>, with <paramref name="name"/> (after
        /// it is processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreference Build<TEnum>(string name)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return Create(processedName, typeof(TEnum))
                .Build();
        }

        /// <summary>
        /// Creates an <see cref="EnumPreferenceBuilder"/>, using
        /// <paramref name="enumTypeInfo"/> as its <see cref="Enum"/>
        /// <see cref="Type"/>, with <paramref name="name"/> (after it is
        /// processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enumTypeInfo">The <see cref="Type"/> of the
        /// <see cref="Enum"/> that is to be stored in this
        /// <see cref="EnumPreference"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreferenceBuilder Create(string name,
            EnumTypeInfo enumTypeInfo)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (enumTypeInfo is null)
            {
                throw new ArgumentNullException(nameof(enumTypeInfo));
            }

            return new EnumPreferenceBuilder(processedName, enumTypeInfo);
        }

        /// <summary>
        /// Creates an <see cref="EnumPreferenceBuilder"/>, using
        /// <paramref name="enumType"/> as its <see cref="Enum"/>
        /// <see cref="Type"/>, with <paramref name="name"/> (after it is
        /// processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enumType">The <see cref="Type"/> of the
        /// <see cref="Enum"/> that is to be stored in this
        /// <see cref="EnumPreference"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreferenceBuilder Create(string name, Type enumType)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var enumTypeInfo = new EnumTypeInfo(enumType);

            return new EnumPreferenceBuilder(processedName, enumTypeInfo);
        }

        /// <summary>
        /// Creates an <see cref="EnumPreferenceBuilder"/>, using
        /// <typeparamref name="TEnum"/> as its <see cref="Enum"/>
        /// <see cref="Type"/>, with <paramref name="name"/> (after it is
        /// processed with
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string,
        /// string)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public static EnumPreferenceBuilder Create<TEnum>(string name)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var enumTypeInfo = new EnumTypeInfo(typeof(TEnum?));

            return new EnumPreferenceBuilder(processedName, enumTypeInfo);
        }
    }
}
