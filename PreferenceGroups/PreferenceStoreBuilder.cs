using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Builds a <see cref="PreferenceStore"/>.
    /// </summary>
    public class PreferenceStoreBuilder
    {
        private string _description = null;

        private readonly PreferenceStore _store = new PreferenceStore();

        private PreferenceStoreBuilder()
        { }

        /// <summary>
        /// Will add <paramref name="itemsWithNames"/> to the store.
        /// </summary>
        /// <param name="itemsWithNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">One of the
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemsWithNames"/> is an empty
        /// <see langword="string"/> or conists only of white-space characters,
        /// or one of the <see cref="PreferenceStoreItem.Kind"/> of
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemsWithNames"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>, or an item with the same
        /// name as one of the <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemsWithNames"/> already exists in the
        /// store.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="itemsWithNames"/> is <see langword="null"/> or one
        /// of the <see cref="KeyValuePair{TKey, TValue}.Key"/> or
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemsWithNames"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder Add(IEnumerable<
            KeyValuePair<string, PreferenceStoreItem>> itemsWithNames)
        {
            if (itemsWithNames is null)
            {
                throw new ArgumentNullException(nameof(itemsWithNames));
            }

            foreach (var itemWithName in itemsWithNames)
            {
                Add(itemWithName);
            }

            return this;
        }

        /// <summary>
        /// Will add <paramref name="itemWithName"/> to the store.
        /// </summary>
        /// <param name="itemWithName"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemWithName"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters, or the
        /// <see cref="PreferenceStoreItem.Kind"/> of
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemWithName"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>, or an item with the same
        /// name as <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemWithName"/> already exists in the
        /// store.</exception>
        /// <exception cref="ArgumentNullException">The
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> or
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemWithName"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder Add(
            KeyValuePair<string, PreferenceStoreItem> itemWithName)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                itemWithName.Key, nameof(itemWithName));
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(
                itemWithName.Value, nameof(itemWithName));

            return Add(processedName, itemWithName.Value);
        }

        /// <summary>
        /// Will add <paramref name="itemsWithNames"/> to the store.
        /// </summary>
        /// <param name="itemsWithNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">One of the
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemsWithNames"/> is an empty
        /// <see langword="string"/> or conists only of white-space characters,
        /// or one of the <see cref="PreferenceStoreItem.Kind"/> of
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemsWithNames"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>, or an item with the same
        /// name as one of the <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemsWithNames"/> already exists in the
        /// store.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="itemsWithNames"/> is <see langword="null"/> or one
        /// of the <see cref="KeyValuePair{TKey, TValue}.Key"/> or
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemsWithNames"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder Add(
            params KeyValuePair<string, PreferenceStoreItem>[] itemsWithNames)
        {
            if (itemsWithNames is null)
            {
                throw new ArgumentNullException(nameof(itemsWithNames));
            }

            return Add((IEnumerable<KeyValuePair<string, PreferenceStoreItem>>)
                itemsWithNames);
        }

        /// <summary>
        /// Will add <paramref name="item"/> with <paramref name="name"/> to the
        /// store.
        /// </summary>
        /// <param name="name">The unique name of <paramref name="item"/> to be
        /// identified in the store.</param>
        /// <param name="item">What to store.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or the <see cref="PreferenceStoreItem.Kind"/> of
        /// <paramref name="item"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="item"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder Add(string name, PreferenceStoreItem item)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(item,
                nameof(item));
            _store.UpdateOrAdd(processedName, item);

            return this;
        }

        /// <summary>
        /// Will add <paramref name="groups"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddArrayOfGroups(string name,
            IEnumerable<PreferenceGroup> groups)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            return Add(processedName, new PreferenceStoreItem(groups));
        }

        /// <summary>
        /// Will add <paramref name="groups"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddArrayOfGroups(string name,
            params PreferenceGroup[] groups)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            return AddArrayOfGroups(processedName,
                (IEnumerable<PreferenceGroup>)groups);
        }

        /// <summary>
        /// Will add <paramref name="stores"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddArrayOfStores(string name,
            IEnumerable<PreferenceStore> stores)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            return Add(processedName, new PreferenceStoreItem(stores));
        }

        /// <summary>
        /// Will add <paramref name="stores"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddArrayOfStores(string name,
            params PreferenceStore[] stores)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            return AddArrayOfStores(processedName,
                (IEnumerable<PreferenceStore>)stores);
        }

        /// <summary>
        /// Will add the resulting <see cref="BooleanPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="BooleanPreferenceBuilder"/> build steps to the store.
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
        public PreferenceStoreBuilder AddBoolean(string name,
            Action<BooleanPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = BooleanPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return AddPreference(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="BooleanPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the store.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddBoolean(string name, bool? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddBoolean(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="BooleanPreference"/> with the provided
        /// <paramref name="name"/> to the store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddBoolean(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddBoolean(processedName, action: null);
        }

        /// <summary>
        /// Will add the resulting <see cref="PreferenceGroup"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="PreferenceGroupBuilder"/> build steps to the store.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="PreferenceGroup"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddGroup(string name,
            Action<PreferenceGroupBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = PreferenceGroupBuilder.Create();

            if (!(action is null))
            {
                action(builder);
            }

            return AddGroup(processedName, builder.Build());
        }

        /// <summary>
        /// Will add <paramref name="group"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddGroup(string name,
            PreferenceGroup group)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return Add(processedName, group);
        }

        /// <summary>
        /// Will add a <see cref="PreferenceGroup"/> with
        /// <paramref name="preferences"/> and the
        /// <paramref name="description"/> to the store with the
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="preferences"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddGroup(string name,
            string description, IEnumerable<Preference> preferences)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            return AddGroup(processedName, b => b
                .WithDescription(description)
                .Add(preferences));
        }

        /// <summary>
        /// Will add a <see cref="PreferenceGroup"/> with
        /// <paramref name="preferences"/> and the
        /// <paramref name="description"/> to the store with the
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="preferences"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddGroup(string name,
            string description, params Preference[] preferences)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            return AddGroup(processedName, description,
                (IEnumerable<Preference>)preferences);
        }

        /// <summary>
        /// Will add an empty <see cref="PreferenceGroup"/> to the store with
        /// <paramref name="name"/> and its <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddGroup(string name,
            string description)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddGroup(processedName, PreferenceGroupBuilder.BuildEmpty(
                description));
        }

        /// <summary>
        /// Will add an empty <see cref="PreferenceGroup"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddGroup(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddGroup(processedName, PreferenceGroupBuilder.BuildEmpty());
        }

        /// <summary>
        /// Will add the resulting <see cref="Int32Preference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="Int32PreferenceBuilder"/> build steps to the store.
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
        public PreferenceStoreBuilder AddInt32(string name,
            Action<Int32PreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = Int32PreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return AddPreference(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="Int32Preference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the store.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddInt32(string name, int? value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt32(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="Int32Preference"/> with the provided
        /// <paramref name="name"/> to the store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddInt32(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddInt32(processedName, action: null);
        }

        /// <summary>
        /// Adds <paramref name="preference"/> to the store, using its
        /// <see cref="Preference.Name"/> to the store.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">An item with the same name as
        /// <see cref="Preference.Name"/> of <paramref name="preference"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddPreference(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name, nameof(preference));

            Add(processedName, new PreferenceStoreItem(preference));

            return this;
        }

        /// <summary>
        /// Will add the resulting <see cref="PreferenceGroup"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="PreferenceGroupBuilder"/> build steps to the store.
        /// </summary>
        /// <param name="name">What the name of the
        /// <see cref="PreferenceGroup"/> is to be.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddStore(string name,
            Action<PreferenceStoreBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = Create();

            if (!(action is null))
            {
                action(builder);
            }

            return AddStore(processedName, builder.Build());
        }

        /// <summary>
        /// Will add <paramref name="group"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddStore(string name,
            PreferenceStore group)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return Add(processedName, group);
        }

        /// <summary>
        /// Will add a <see cref="PreferenceGroup"/> with
        /// <paramref name="itemsWithNames"/> and the
        /// <paramref name="description"/> to the store with the
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="itemsWithNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="itemsWithNames"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddStore(string name, string description,
            IEnumerable<KeyValuePair<string, PreferenceStoreItem>>
            itemsWithNames)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (itemsWithNames is null)
            {
                throw new ArgumentNullException(nameof(itemsWithNames));
            }

            return AddStore(processedName, b => b
                .WithDescription(description)
                .Add(itemsWithNames));
        }

        /// <summary>
        /// Will add a <see cref="PreferenceStore"/> with
        /// <paramref name="itemsWithNames"/> and the
        /// <paramref name="description"/> to the store with the
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="itemsWithNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="itemsWithNames"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddStore(string name,
            string description,
            params KeyValuePair<string, PreferenceStoreItem>[] itemsWithNames)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (itemsWithNames is null)
            {
                throw new ArgumentNullException(nameof(itemsWithNames));
            }

            return AddStore(processedName, description,
                (IEnumerable<KeyValuePair<string, PreferenceStoreItem>>)
                itemsWithNames);
        }

        /// <summary>
        /// Will add an empty <see cref="PreferenceStore"/> to the store with
        /// <paramref name="name"/> and its <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddStore(string name, string description)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddStore(processedName, BuildEmpty(description));
        }

        /// <summary>
        /// Will add an empty <see cref="PreferenceStore"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddStore(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddStore(processedName, BuildEmpty());
        }

        /// <summary>
        /// Will add the resulting <see cref="StringPreference"/> from the
        /// provided <paramref name="action"/> of the
        /// <see cref="StringPreferenceBuilder"/> build steps to the store.
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
        public PreferenceStoreBuilder AddString(string name,
            Action<StringPreferenceBuilder> action)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            var builder = StringPreferenceBuilder.Create(processedName);

            if (!(action is null))
            {
                action(builder);
            }

            return AddPreference(builder.Build());
        }

        /// <summary>
        /// Will add a <see cref="StringPreference"/> with the provided
        /// <paramref name="name"/> and <paramref name="value"/> to the store.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddString(string name, string value)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddString(processedName, b => b.WithValue(value));
        }

        /// <summary>
        /// Will add a <see cref="StringPreference"/> with the provided
        /// <paramref name="name"/> to the store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreBuilder AddString(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return AddString(processedName, action: null);
        }

        /// <summary>
        /// Builds and returns the <see cref="PreferenceGroup"/>.
        /// </summary>
        /// <returns></returns>
        public PreferenceStore Build()
            => new PreferenceStore(_description)
            {
                _store
            };

        /// <summary>
        /// Will use <paramref name="description"/> in the built
        /// <see cref="PreferenceStore"/> to describe it to the user and in the
        /// file.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public PreferenceStoreBuilder WithDescription(string description)
        {
            _description = description?.Trim();

            return this;
        }

        /// <summary>
        /// Builds and returns an empty <see cref="PreferenceStore"/> with
        /// <paramref name="description"/>, if any.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static PreferenceStore BuildEmpty(string description = null)
            => Create().WithDescription(description).Build();

        /// <summary>
        /// Instantiates a new <see cref="PreferenceStoreBuilder"/>.
        /// </summary>
        /// <returns></returns>
        public static PreferenceStoreBuilder Create()
            => new PreferenceStoreBuilder();
    }
}
