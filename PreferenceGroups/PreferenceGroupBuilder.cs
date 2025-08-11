using System;

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
        /// Will add <paramref name="preference"/> when the group is built.
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

            _ = Preference.ProcessNameOrThrowIfInvalid(preference.Name);
            _group.Add(preference);

            return this;
        }

        /// <summary>
        /// Will add the resulting <see cref="Int32Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="Int32PreferenceBuilder"/> build steps.
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
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);
            var builder = Int32PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="Int32Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/>.
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
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return AddInt32(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="Int32Preference"/> with the provided
        /// <paramref name="name"/>.
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
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return AddInt32(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="StringPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="StringPreferenceBuilder"/> build steps.
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
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);
            var builder = StringPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return Add(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="StringPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/>.
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
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return AddString(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="StringPreference"/> with the provided
        /// <paramref name="name"/>.
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
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return AddString(processedName, action: null);
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
        /// associated members of <paramref name="object"/>.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static PreferenceGroup BuildFrom(object @object)
            => new PreferenceGroup(@object);

        /// <summary>
        /// Instantiates a new <see cref="PreferenceGroupBuilder"/>.
        /// </summary>
        /// <returns></returns>
        public static PreferenceGroupBuilder Create()
            => new PreferenceGroupBuilder();
    }
}
