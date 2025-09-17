using System;
using System.Collections.Generic;
using System.Net;

namespace PreferenceGroups
{
    /// <summary>
    /// Builds a <see cref="PreferenceGroup"/>.
    /// </summary>
    public class PreferenceGroupBuilder
    {
        private string _description = null;

        private readonly PreferenceGroup _group = new PreferenceGroup();

        private PreferenceGroupBuilder()
        { }

        /// <summary>
        /// Will add <paramref name="preferences"/> to the group.
        /// </summary>
        /// <param name="preferences"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters or a
        /// <see cref="Preference"/> in the group has the same
        /// <see cref="Preference.Name"/> as one of the
        /// <paramref name="preferences"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preferences"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder Add(IEnumerable<Preference> preferences)
        {
            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            foreach (var preference in preferences)
            {
                Add(preference);
            }

            return this;
        }

        /// <summary>
        /// Will add <paramref name="preferences"/> to the group.
        /// </summary>
        /// <param name="preferences"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters or a
        /// <see cref="Preference"/> in the group has the same
        /// <see cref="Preference.Name"/> as one of the
        /// <paramref name="preferences"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preferences"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder Add(params Preference[] preferences)
            => Add((IEnumerable<Preference>)preferences);

        /// <summary>
        /// Will add <paramref name="preference"/> to the group.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of <paramref name="preference"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters or a <see cref="Preference"/> in the group has the same
        /// <see cref="Preference.Name"/> as
        /// <paramref name="preference"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder Add(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            _ = Preference.ProcessNameOrThrowIfInvalid(preference.Name,
                nameof(preference));
            _group.UpdateOrAdd(preference);

            return this;
        }

        /// <summary>
        /// Will add the resulting <see cref="BooleanPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="BooleanPreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="BooleanPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddBoolean(string name,
            Action<BooleanPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = BooleanPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="BooleanPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddBoolean(string name, bool? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddBoolean(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="BooleanPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddBoolean(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddBoolean(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="BytePreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="BytePreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="BytePreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddByte(string name,
            Action<BytePreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = BytePreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="BytePreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddByte(string name, byte? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddByte(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="BytePreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddByte(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddByte(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="BytesPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="BytesPreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="BytesPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddBytes(string name,
            Action<BytesPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = BytesPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="BytesPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddBytes(string name, byte[] value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddBytes(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="BytesPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddBytes(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddBytes(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="DecimalPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="DecimalPreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="DecimalPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddDecimal(string name,
            Action<DecimalPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = DecimalPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="DecimalPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddDecimal(string name, decimal? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddDecimal(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="DecimalPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddDecimal(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddDecimal(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="DoublePreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="DoublePreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="DoublePreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddDouble(string name,
            Action<DoublePreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = DoublePreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="DoublePreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddDouble(string name, double? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddDouble(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="DoublePreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddDouble(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddDouble(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="EnumPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="EnumPreferenceBuilder{TEnum}"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="EnumPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddEnum<TEnum>(string name,
            Action<EnumPreferenceBuilder<TEnum>> action)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = EnumPreferenceBuilder<TEnum>.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="EnumPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddEnum<TEnum>(string name, TEnum? value)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddEnum<TEnum>(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="EnumPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddEnum<TEnum>(string name)
            where TEnum : struct, Enum
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddEnum<TEnum>(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="Int16Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="Int16PreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="Int16Preference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt16(string name,
            Action<Int16PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = Int16PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="Int16Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt16(string name, short? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt16(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="Int16Preference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt16(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt16(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="Int32Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="Int32PreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="Int32Preference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt32(string name,
            Action<Int32PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = Int32PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="Int32Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt32(string name, int? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt32(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="Int32Preference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt32(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt32(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="Int64Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="Int64PreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="Int64Preference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt64(string name,
            Action<Int64PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = Int64PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="Int64Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt64(string name, long? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt64(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="Int64Preference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddInt64(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt64(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="IPAddressPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="IPAddressPreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="IPAddressPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddIPAddress(string name,
            Action<IPAddressPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = IPAddressPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="IPAddressPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddIPAddress(string name, IPAddress value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddIPAddress(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="IPAddressPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddIPAddress(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddIPAddress(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="SBytePreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="SBytePreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="SBytePreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddSByte(string name,
            Action<SBytePreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = SBytePreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="SBytePreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddSByte(string name, sbyte? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddSByte(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="SBytePreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddSByte(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddSByte(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="SinglePreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="SinglePreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="SinglePreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddSingle(string name,
            Action<SinglePreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = SinglePreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="SinglePreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddSingle(string name, float? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddSingle(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="SinglePreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddSingle(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddSingle(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="StringPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="StringPreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="StringPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddString(string name,
            Action<StringPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = StringPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="StringPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddString(string name, string value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddString(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="StringPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddString(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddString(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="TimeSpanPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="TimeSpanPreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="TimeSpanPreference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddTimeSpan(string name,
            Action<TimeSpanPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = TimeSpanPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="TimeSpanPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddTimeSpan(string name, TimeSpan? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddTimeSpan(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="TimeSpanPreference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddTimeSpan(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddTimeSpan(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="UInt16Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="UInt16PreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="UInt16Preference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt16(string name,
            Action<UInt16PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = UInt16PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="UInt32Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt16(string name, ushort? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddUInt16(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="UInt16Preference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt16(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddUInt16(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="UInt32Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="UInt32PreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="UInt32Preference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt32(string name,
            Action<UInt32PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = UInt32PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="UInt32Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt32(string name, uint? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddUInt32(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="UInt32Preference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt32(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddUInt32(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="UInt64Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="UInt64PreferenceBuilder"/> build steps to the group.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="UInt64Preference"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt64(string name,
            Action<UInt64PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = UInt64PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="UInt64Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt64(string name, ulong? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddUInt64(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="UInt64Preference"/> with the provided
        /// <paramref name="name"/> to the group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceGroupBuilder AddUInt64(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddUInt64(processedName, action: null);
        }

        /// <summary>
        /// Builds and returns the <see cref="PreferenceGroup"/>.
        /// </summary>
        /// <returns></returns>
        public PreferenceGroup Build()
            => new PreferenceGroup(_description)
            {
                _group
            };

        /// <summary>
        /// Will use <paramref name="description"/> in the built
        /// <see cref="PreferenceGroup"/> to describe it to the user and in the
        /// file.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public PreferenceGroupBuilder WithDescription(string description)
        {
            _description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Builds and returns an empty <see cref="PreferenceGroup"/> with
        /// <paramref name="description"/>, if any.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static PreferenceGroup BuildEmpty(string description = null)
            => Create().WithDescription(description).Build();

        /// <summary>
        /// Builds and returns a <see cref="PreferenceGroup"/> by using the
        /// <paramref name="object"/>. The <paramref name="object"/> must be a
        /// <see langword="class"/> and all <see langword="public"/> properties
        /// that have the <see cref="PreferenceAttribute"/> will be used as
        /// members of the group. In addition, any changes to the values of the
        /// resulting <see cref="PreferenceGroup"/> members, will update the
        /// associated members of <paramref name="object"/>. The parameter
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
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="object"/> is not
        /// a <see langword="class"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="object"/> is
        /// <see langword="null"/>.</exception>
        public static PreferenceGroup BuildFrom(object @object,
            bool useValuesAsDefault = true, bool allowNonNullableStructs = true)
            => PreferenceGroup.From(@object, useValuesAsDefault,
                allowNonNullableStructs);

        /// <summary>
        /// Instantiates a new <see cref="PreferenceGroupBuilder"/>.
        /// </summary>
        /// <returns></returns>
        public static PreferenceGroupBuilder Create()
            => new PreferenceGroupBuilder();
    }
}
