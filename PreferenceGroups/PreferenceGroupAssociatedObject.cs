using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace PreferenceGroups
{
    public partial class PreferenceGroup
    {
        /// <summary>
        /// Stores whether the <see cref="PreferenceGroup"/> was instantiated
        /// allowing non-<see cref="Nullable{T}"/> <see langword="struct"/>s.
        /// </summary>
        private readonly bool _allowedNonNullableStructs = false;

        /// <summary>
        /// The backing store for an associated <see langword="object"/> such
        /// that when a value of a <see cref="Preference"/> of the
        /// <see cref="PreferenceGroup"/> that shares
        /// </summary>
        private readonly object _associatedObject = null;

        /// <summary>
        /// The <see cref="Type"/> of <see cref="_associatedObject"/>.
        /// </summary>
        private readonly Type _associatedObjectType = null;

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/>, where the
        /// <see cref="Dictionary{TKey, TValue}.Keys"/> are the
        /// <see cref="Preference.Name"/>s and the
        /// <see cref="Dictionary{TKey, TValue}.Values"/> are the
        /// <see cref="_associatedObject"/> <see langword="property"/> names.
        /// </summary>
        private readonly Dictionary<string, string> _associatedNames = null;

        /// <summary>
        /// Initializes a <see cref="PreferenceGroup"/> from
        /// <paramref name="object"/>, which must be a <see langword="class"/>
        /// and the <see langword="public"/> properties that have an attached
        /// <see cref="PreferenceAttribute"/>. It is optional for
        /// <paramref name="object"/> to have an attached
        /// <see cref="PreferenceGroupAttribute"/>, in which case the
        /// <see cref="PreferenceGroupAttribute.Description"/> will be used to
        /// assign <see cref="Description"/>. The parameter
        /// <paramref name="useValuesAsDefault"/> allows for using the current
        /// values of <paramref name="object"/> as the <c>DefaultValue</c>
        /// for each <see cref="Preference"/>, unless the
        /// <see cref="PreferenceAttribute.DefaultValue"/> is not
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="useValuesAsDefault">If <see langword="true"/>, the
        /// current values of <paramref name="object"/> will be the
        /// <c>DefaultValue</c> of each <see cref="Preference"/>. This only
        /// occurs if the <see cref="PreferenceAttribute.DefaultValue"/> is
        /// <see langword="null"/>.</param>
        /// <param name="allowNonNullableStructs">Whether or not to allow a
        /// property of <paramref name="object"/> that is not a
        /// <see cref="Nullable{T}"/> <see langword="struct"/>, i.e. if
        /// <see langword="false"/> such properties will be ignored.</param>
        /// <exception cref="ArgumentException"><paramref name="object"/> is not
        /// a <see langword="class"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="object"/> is
        /// <see langword="null"/>.</exception>
        private PreferenceGroup(object @object, bool useValuesAsDefault,
            bool allowNonNullableStructs)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            _allowedNonNullableStructs = allowNonNullableStructs;
            _associatedObject = @object;
            _associatedObjectType = _associatedObject.GetType();

            if (!_associatedObjectType.IsClass)
            {
                throw new ArgumentException(paramName: nameof(@object),
                    message: "The type must be a class.");
            }

            Description = null;
            var groupAttribute = _associatedObjectType
                .GetCustomAttribute<PreferenceGroupAttribute>();

            if (!(groupAttribute is null))
            {
                Description = groupAttribute.Description?.Trim();
            }

            _associatedNames = new Dictionary<string, string>();

            foreach (var property in _associatedObjectType.GetProperties())
            {
                var propertyName = Preference.ProcessNameOrThrowIfInvalid(
                    property.Name, nameof(property));
                var preferenceName = propertyName;
                bool? preferenceAllowUndefinedValues = null;
                bool? preferenceSortAllowedValues = null;
                object preferenceDefaultValue = null;
                string preferenceDescription = null;
                object[] preferenceAllowedValues = null;
                Type preferenceValueValidtyProcessorClassType = null;
                var preferenceAttribute = property
                    .GetCustomAttribute<PreferenceAttribute>();

                if (!(preferenceAttribute is null))
                {
                    if (!(preferenceAttribute.Name is null))
                    {
                        preferenceName = Preference.ProcessNameOrThrowIfInvalid(
                            preferenceAttribute.Name,
                            nameof(preferenceAttribute));
                    }

                    preferenceAllowUndefinedValues = preferenceAttribute
                        .AllowUndefinedValues;
                    preferenceSortAllowedValues = preferenceAttribute
                        .SortAllowValues;
                    preferenceValueValidtyProcessorClassType
                        = preferenceAttribute.ValueValidityProcessorClassType;
                    preferenceDefaultValue = preferenceAttribute.DefaultValue;
                    preferenceDescription = preferenceAttribute.Description;
                    preferenceAllowedValues = preferenceAttribute.AllowedValues;
                    _associatedNames[preferenceName] = propertyName;
                }

                var propertyType = property.PropertyType;

                if (EnumTypeInfo.IsEnum(propertyType, out var enumTypeInfo,
                    nameof(propertyType)))
                {
                    ClassValidityProcessor<Enum> valueValidityProcessor = null;

                    try
                    {
                        valueValidityProcessor = (ClassValidityProcessor<Enum>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var valueAsObject = property.GetValue(_associatedObject);
                    var value = enumTypeInfo.ToEnum(valueAsObject,
                        nameof(valueAsObject));
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : enumTypeInfo.ToEnum(preferenceDefaultValue,
                            nameof(preferenceDefaultValue));
                    _dictionary[preferenceName] = EnumPreferenceBuilder
                        .Create(preferenceName, enumTypeInfo)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? EnumPreference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? EnumPreference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(bool?))
                {
                    StructValidityProcessor<bool> valueValidityProcessor = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<bool>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (bool?)property.GetValue(_associatedObject);
                    bool? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (bool?)preferenceDefaultValue;

                    _dictionary[preferenceName] = BooleanPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(bool)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<bool> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<bool>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (bool)property.GetValue(_associatedObject);
#pragma warning disable IDE0075 // Simplify conditional expression
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (bool)preferenceDefaultValue;
#pragma warning restore IDE0075 // Simplify conditional expression

                    _dictionary[preferenceName] = BooleanPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(sbyte?))
                {
                    StructValidityProcessor<sbyte> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<sbyte>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (sbyte?)property.GetValue(_associatedObject);
                    sbyte? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (sbyte?)preferenceDefaultValue;

                    _dictionary[preferenceName] = SBytePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(sbyte)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<sbyte> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<sbyte>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (sbyte)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (sbyte)preferenceDefaultValue;

                    _dictionary[preferenceName] = SBytePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(byte?))
                {
                    StructValidityProcessor<byte> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<byte>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (byte?)property.GetValue(_associatedObject);
                    byte? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (byte?)preferenceDefaultValue;

                    _dictionary[preferenceName] = BytePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(byte)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<byte> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<byte>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (byte)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (byte)preferenceDefaultValue;

                    _dictionary[preferenceName] = BytePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(short?))
                {
                    StructValidityProcessor<short> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<short>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (short?)property.GetValue(_associatedObject);
                    short? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (short?)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int16PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(short)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<short> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<short>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (short)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (short)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int16PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(ushort?))
                {
                    StructValidityProcessor<ushort> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<ushort>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (ushort?)property.GetValue(_associatedObject);
                    ushort? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (ushort?)preferenceDefaultValue;

                    _dictionary[preferenceName] = UInt16PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(ushort)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<ushort> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<ushort>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (ushort)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (ushort)preferenceDefaultValue;

                    _dictionary[preferenceName] = UInt16PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(int?))
                {
                    StructValidityProcessor<int> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<int>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (int?)property.GetValue(_associatedObject);
                    int? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (int?)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int32PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(int)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<int> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<int>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (int)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (int)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int32PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(uint?))
                {
                    StructValidityProcessor<uint> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<uint>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (uint?)property.GetValue(_associatedObject);
                    uint? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (uint?)preferenceDefaultValue;

                    _dictionary[preferenceName] = UInt32PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(uint)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<uint> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<uint>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (uint)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (uint)preferenceDefaultValue;

                    _dictionary[preferenceName] = UInt32PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(long?))
                {
                    StructValidityProcessor<long> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<long>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (long?)property.GetValue(_associatedObject);
                    long? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (long?)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int64PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(long)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<long> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<long>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (long)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (long)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int64PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(ulong?))
                {
                    StructValidityProcessor<ulong> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<ulong>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (ulong?)property.GetValue(_associatedObject);
                    ulong? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (ulong?)preferenceDefaultValue;

                    _dictionary[preferenceName] = UInt64PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(ulong)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<ulong> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<ulong>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (ulong)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (ulong)preferenceDefaultValue;

                    _dictionary[preferenceName] = UInt64PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(float?))
                {
                    StructValidityProcessor<float> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<float>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (float?)property.GetValue(_associatedObject);
                    float? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (float?)preferenceDefaultValue;

                    _dictionary[preferenceName] = SinglePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(float)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<float> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<float>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (float)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (float)preferenceDefaultValue;

                    _dictionary[preferenceName] = SinglePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(double?))
                {
                    StructValidityProcessor<double> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<double>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (double?)property.GetValue(_associatedObject);
                    double? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (double?)preferenceDefaultValue;

                    _dictionary[preferenceName] = DoublePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(double)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<double> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<double>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (double)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (double)preferenceDefaultValue;

                    _dictionary[preferenceName] = DoublePreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(decimal?))
                {
                    StructValidityProcessor<decimal> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<decimal>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (decimal?)property.GetValue(_associatedObject);
                    decimal? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (decimal?)preferenceDefaultValue;

                    _dictionary[preferenceName] = DecimalPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(decimal)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<decimal> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<decimal>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (decimal)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (decimal)preferenceDefaultValue;

                    _dictionary[preferenceName] = DecimalPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(string))
                {
                    ClassValidityProcessor<string> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor
                            = (ClassValidityProcessor<string>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (string)property.GetValue(_associatedObject);
                    string defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (string)preferenceDefaultValue;

                    _dictionary[preferenceName] = StringPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(byte[]))
                {
                    ClassValidityProcessor<byte[]> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor
                            = (ClassValidityProcessor<byte[]>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (byte[])property.GetValue(_associatedObject);
                    byte[] defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (byte[])preferenceDefaultValue;

                    _dictionary[preferenceName] = BytesPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(TimeSpan?))
                {
                    StructValidityProcessor<TimeSpan> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<TimeSpan>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (TimeSpan?)property.GetValue(_associatedObject);
                    TimeSpan? defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (TimeSpan?)preferenceDefaultValue;

                    _dictionary[preferenceName] = TimeSpanPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(TimeSpan)
                    && _allowedNonNullableStructs)
                {
                    StructValidityProcessor<TimeSpan> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor = (StructValidityProcessor<TimeSpan>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (TimeSpan)property.GetValue(_associatedObject);
                    var defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : default)
                        : (TimeSpan)preferenceDefaultValue;

                    _dictionary[preferenceName] = TimeSpanPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
                else if (propertyType == typeof(IPAddress))
                {
                    ClassValidityProcessor<IPAddress> valueValidityProcessor
                        = null;

                    try
                    {
                        valueValidityProcessor
                            = (ClassValidityProcessor<IPAddress>)
                            Activator.CreateInstance(
                                preferenceValueValidtyProcessorClassType);
                    }
                    catch
                    { }

                    var value = (IPAddress)property.GetValue(_associatedObject);
                    IPAddress defaultValue = preferenceDefaultValue is null
                        ? (useValuesAsDefault ? value : null)
                        : (IPAddress)preferenceDefaultValue;

                    _dictionary[preferenceName] = IPAddressPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .SetAllowUndefinedValues(preferenceAllowUndefinedValues
                            ?? Preference.DefaultAllowUndefinedValues)
                        .SetSortAllowedValues(preferenceSortAllowedValues
                            ?? Preference.DefaultSortAllowedValues)
                        .SetValidityProcessor(valueValidityProcessor)
                        .WithDescription(preferenceDescription)
                        .WithAllowedValues(preferenceAllowedValues)
                        .Build();
                }
            }
        }

        /// <summary>
        /// Updates <see langword="this"/> with the values of
        /// <paramref name="object"/>, that is a <see langword="class"/>, from
        /// its <see langword="public"/> properties that have the
        /// <see cref="PreferenceAttribute"/> attached to them.
        /// </summary>
        /// <param name="object"></param>
        /// <exception cref="ArgumentException"><paramref name="object"/> is not
        /// a <see langword="class"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="object"/> is
        /// <see langword="null"/>.</exception>
        public void UpdateValuesFrom(object @object)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            if (!@object.GetType().IsClass)
            {
                throw new ArgumentException(paramName: nameof(@object),
                    message: "The type must be a class.");
            }

            UpdateValuesFrom(new PreferenceGroup(@object, false, true));
        }

        /// <summary>
        /// Updates the values of <see langword="this"/> to
        /// <paramref name="object"/>, that is a <see langword="class"/>, with
        /// its <see langword="public"/> properties that have the
        /// <see cref="PreferenceAttribute"/> attached to them.
        /// </summary>
        /// <param name="object"></param>
        public void UpdateValuesTo(object @object)
        {
            if (@object is null || !@object.GetType().IsClass)
            {
                return;
            }

            UpdateValuesTo(new PreferenceGroup(@object, false,
                _allowedNonNullableStructs));
        }

        /// <summary>
        /// Initializes a <see cref="PreferenceGroup"/> from
        /// <paramref name="object"/>, which must be a <see langword="class"/>
        /// and the <see langword="public"/> properties that have an attached
        /// <see cref="PreferenceAttribute"/>. It is optional for
        /// <paramref name="object"/> to have an attached
        /// <see cref="PreferenceGroupAttribute"/>, in which case the
        /// <see cref="PreferenceGroupAttribute.Description"/> will be used to
        /// assign <see cref="Description"/>. The parameter
        /// <paramref name="useValuesAsDefault"/> allows for using the current
        /// values of <paramref name="object"/> as the <c>DefaultValue</c>
        /// for each <see cref="Preference"/>, unless the
        /// <see cref="PreferenceAttribute.DefaultValue"/> is not
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="useValuesAsDefault">If <see langword="true"/>, the
        /// current values of <paramref name="object"/> will be the
        /// <c>DefaultValue</c> of each <see cref="Preference"/>. This only
        /// occurs if the <see cref="PreferenceAttribute.DefaultValue"/> is
        /// <see langword="null"/>.</param>
        /// <param name="allowNonNullableStructs">Whether or not to allow a
        /// property of <paramref name="object"/> that is not a
        /// <see cref="Nullable{T}"/> <see langword="struct"/>, i.e. if
        /// <see langword="false"/> such properties will be ignored.</param>
        /// <exception cref="ArgumentException"><paramref name="object"/> is not
        /// a <see langword="class"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="object"/> is
        /// <see langword="null"/>.</exception>
        public static PreferenceGroup From(object @object,
            bool useValuesAsDefault = true, bool allowNonNullableStructs = true)
            => new PreferenceGroup(@object, useValuesAsDefault,
                allowNonNullableStructs);
    }
}
