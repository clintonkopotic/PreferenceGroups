using System;
using System.Collections;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// A generic collection of: <see cref="Preference"/>,
    /// <see cref="PreferenceGroup"/>, <see cref="PreferenceStore"/>, an
    /// <see cref="Array"/> of <see cref="PreferenceGroup"/>, and an
    /// <see cref="Array"/> of <see cref="PreferenceStore"/>, where each
    /// <see cref="PreferenceStoreItem"/> has a unique <c>Name</c>.
    /// </summary>
    public class PreferenceStore : IDictionary<string, PreferenceStoreItem>
    {
        /// <summary>
        /// The backing store for the store, with using a unique <c>Name</c> as
        /// the key in the dictionary and a <see cref="PreferenceStoreItem"/> as
        /// the value.
        /// </summary>
        private readonly Dictionary<string, PreferenceStoreItem> _dictionary
            = new Dictionary<string, PreferenceStoreItem>();

        /// <summary>
        /// Gets or sets the <see cref="PreferenceStoreItem"/> associated with
        /// the specified <paramref name="name"/>. The <paramref name="name"/>
        /// mush be a able to be successfully processed with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method.
        /// </summary>
        /// <param name="name">The unique <c>Name</c> of the
        /// <see cref="PreferenceStoreItem"/>.</param>
        /// <returns>The associated <see cref="PreferenceStoreItem"/> with the
        /// <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters or the <see cref="PreferenceStoreItem.Kind"/> of
        /// <paramref name="value"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public PreferenceStoreItem this[string name]
        {
            get
            {
                var processedName = Preference.ProcessNameOrThrowIfInvalid(
                    name, nameof(name));

                return _dictionary[processedName];
            }
            set
            {
                var processedName = Preference.ProcessNameOrThrowIfInvalid(
                    name, nameof(name));
                PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(value,
                    nameof(value));

                _dictionary[processedName] = value;
            }
        }

        private ICollection<KeyValuePair<string, PreferenceStoreItem>>
            AsICollection
            => _dictionary;

        private IEnumerable<KeyValuePair<string, PreferenceStoreItem>>
            AsIEnumerable
            => _dictionary;

        /// <inheritdoc/>
        public int Count => _dictionary.Count;

        /// <summary>
        /// A description of the <see cref="PreferenceStore"/> that is intended
        /// to be shown to the user and in the file as a comment.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// A collection of the store's items.
        /// </summary>
        public ICollection<PreferenceStoreItem> Items => _dictionary.Values;

        /// <summary>
        /// A collection of the store's names.
        /// </summary>
        public ICollection<string> Names => _dictionary.Keys;

        /// <summary>
        /// Initializes a <see cref="PreferenceStore"/> with a
        /// <paramref name="description"/>, if any.
        /// </summary>
        /// <param name="description"></param>
        public PreferenceStore(string description = null)
        {
            Description = description?.Trim();
        }

        /// <summary>
        /// Adds <paramref name="itemsWithNames"/> to the store.
        /// </summary>
        /// <param name="itemsWithNames"></param>
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
        public void Add(IEnumerable<KeyValuePair<string, PreferenceStoreItem>>
            itemsWithNames)
        {
            if (itemsWithNames is null)
            {
                throw new ArgumentNullException(nameof(itemsWithNames));
            }

            foreach (var itemWithName in itemsWithNames)
            {
                Add(itemWithName);
            }
        }

        /// <summary>
        /// Adds <paramref name="itemWithName"/> to the store.
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
        public void Add(KeyValuePair<string, PreferenceStoreItem> itemWithName)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                itemWithName.Key, nameof(itemWithName));
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(
                itemWithName.Value, nameof(itemWithName));

            Add(processedName, itemWithName.Value);
        }

        /// <summary>
        /// Adds <paramref name="itemsWithNames"/> to the store.
        /// </summary>
        /// <param name="itemsWithNames"></param>
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
        public void Add(params KeyValuePair<string, PreferenceStoreItem>[]
            itemsWithNames)
            => Add((IEnumerable<KeyValuePair<string,PreferenceStoreItem>>)
                itemsWithNames);

        /// <summary>
        /// Adds <paramref name="preference"/> to the store, using its
        /// <see cref="Preference.Name"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentException">An item with the same name as
        /// <see cref="Preference.Name"/> of <paramref name="preference"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public void Add(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name, nameof(preference));

             Add(processedName, new PreferenceStoreItem(preference));
        }

        /// <summary>
        /// Adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void Add(string name, IEnumerable<PreferenceGroup> groups)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            Add(processedName, new PreferenceStoreItem(groups));
        }

        /// <summary>
        /// Adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void Add(string name, IEnumerable<PreferenceStore> stores)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            Add(processedName, new PreferenceStoreItem(stores));
        }

        /// <summary>
        /// Adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void Add(string name, params PreferenceGroup[] groups)
            => Add(name, (IEnumerable<PreferenceGroup>)groups);

        /// <summary>
        /// Adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void Add(string name, params PreferenceStore[] stores)
            => Add(name, (IEnumerable<PreferenceStore>)stores);

        /// <summary>
        /// Adds <paramref name="group"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        public void Add(string name, PreferenceGroup group)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Add(processedName, new PreferenceStoreItem(group));
        }

        /// <summary>
        /// Adds <paramref name="store"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="store"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="store"/> is <see langword="null"/>.</exception>
        public void Add(string name, PreferenceStore store)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            Add(processedName, new PreferenceStoreItem(store));
        }

        /// <summary>
        /// Adds <paramref name="item"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or the <see cref="PreferenceStoreItem.Kind"/> of
        /// <paramref name="item"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>, or an item with the same
        /// name as <paramref name="name"/> already exists in the
        /// store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="item"/> is <see langword="null"/>.</exception>
        public void Add(string name, PreferenceStoreItem item)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(item,
                nameof(item));

            _dictionary.Add(processedName, item);
        }

        /// <summary>
        /// Adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void Add(string name, string description,
            IEnumerable<PreferenceGroup> groups)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            Add(processedName, new PreferenceStoreItem(description, groups));
        }

        /// <summary>
        /// Adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void Add(string name, string description,
            IEnumerable<PreferenceStore> stores)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            Add(processedName, new PreferenceStoreItem(description, stores));
        }

        /// <summary>
        /// Adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void Add(string name, string description,
            params PreferenceGroup[] groups)
            => Add(name, description, (IEnumerable<PreferenceGroup>)groups);

        /// <summary>
        /// Adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or an item with the same name as <paramref name="name"/>
        /// already exists in the store.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void Add(string name, string description,
            params PreferenceStore[] stores)
            => Add(name, description, (IEnumerable<PreferenceStore>)stores);

        /// <summary>
        /// Removes all items from the store.
        /// </summary>
        public void Clear() => _dictionary.Clear();

        /// <summary>
        /// Determines whether the store contains an item with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see langword="true"/> if the store contains an item with
        /// the specified <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool ContainsName(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return _dictionary.ContainsKey(processedName);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, PreferenceStoreItem>>
            GetEnumerator()
            => AsIEnumerable.GetEnumerator();

        /// <summary>
        /// If the store contians the <paramref name="name"/> and the
        /// <see cref="PreferenceStoreItem.Kind"/> of the item equals
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceGroups"/>, then
        /// the item is cast and returned as an <see cref="Array"/> of
        /// <see cref="PreferenceGroup"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// an <see cref="Array"/> of <see cref="PreferenceGroup"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceGroups"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public PreferenceGroup[] GetItemAsArrayOfPreferenceGroups(string name)
            => this[name].GetAsArrayOfPreferenceGroups();

        /// <summary>
        /// If the store contians the <paramref name="name"/> and the
        /// <see cref="PreferenceStoreItem.Kind"/> of the item equals
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceStores"/>, then
        /// the item is cast and returned as an <see cref="Array"/> of
        /// <see cref="PreferenceStore"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// an <see cref="Array"/> of <see cref="PreferenceStore"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceStores"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public PreferenceStore[] GetItemAsArrayOfPreferenceStores(string name)
            => this[name].GetAsArrayOfPreferenceStores();

        /// <summary>
        /// If the store contians the <paramref name="name"/> and the
        /// <see cref="PreferenceStoreItem.Kind"/> of the item equals
        /// <see cref="PreferenceStoreItemKind.Preference"/>, then the item is
        /// cast and returned as a <see cref="Preference"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// a <see cref="Preference"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.Preference"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public Preference GetItemAsPreference(string name)
            => this[name].GetAsPreference();

        /// <summary>
        /// If the store contians the <paramref name="name"/> and the
        /// <see cref="PreferenceStoreItem.Kind"/> of the item equals
        /// <see cref="PreferenceStoreItemKind.PreferenceGroup"/>, then the item
        /// is cast and returned as a <see cref="PreferenceGroup"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// a <see cref="PreferenceGroup"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.PreferenceGroup"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public PreferenceGroup GetItemAsPreferenceGroup(string name)
            => this[name].GetAsPreferenceGroup();

        /// <summary>
        /// If the store contians the <paramref name="name"/> and the
        /// <see cref="PreferenceStoreItem.Kind"/> of the item equals
        /// <see cref="PreferenceStoreItemKind.PreferenceStore"/>, then the item
        /// is cast and returned as a <see cref="PreferenceStore"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// a <see cref="PreferenceStore"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.PreferenceStore"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public PreferenceStore GetItemAsPreferenceStore(string name)
            => this[name].GetAsPreferenceStore();

        /// <summary>
        /// Gets the <see cref="Preference"/>'s <c>Value</c>, as an
        /// <see cref="object"/>, where its <see cref="Preference.Name"/> is
        /// <paramref name="name"/> using the
        /// <see cref="Preference.GetValueAsObject()"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The <see cref="Preference"/>'s <c>Value</c>, as an
        /// <see cref="object"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The store item could not be
        /// cast to a <see cref="Preference"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.Preference"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public object GetValue(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return GetItemAsPreference(processedName).GetValueAsObject();
        }

        /// <summary>
        /// Gets the <see cref="Preference"/>'s <c>Value</c>, as a
        /// <typeparamref name="T"/>, where its <see cref="Preference.Name"/> is
        /// <paramref name="name"/> using the
        /// <see cref="Preference.GetValueAs{T}"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The <see cref="Preference"/>'s <c>Value</c>, as an
        /// <see cref="object"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidCastException">The store item could not be
        /// cast to a <see cref="Preference"/> or its value to
        /// <typeparamref name="T"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="PreferenceStoreItem.Kind"/> of the item does not equal
        /// <see cref="PreferenceStoreItemKind.Preference"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The <paramref name="name"/>
        /// does not exist in the store.</exception>
        public T GetValueAs<T>(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return GetItemAsPreference(processedName).GetValueAs<T>();
        }

        /// <summary>
        /// Removes the item with the specified <paramref name="name"/> from the
        /// store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see langword="true"/> if the item is successfully found
        /// and removed; otherwise, <see langword="false"/>. This method returns
        /// <see langword="false"/> if <paramref name="name"/> is not found in
        /// the store.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool Remove(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return _dictionary.Remove(processedName);
        }

        /// <summary>
        /// Gets the <paramref name="item"/> associated with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns><see langword="true"/> if the store contains an item with
        /// the specified <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool TryGetItem(string name, out PreferenceStoreItem item)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            item = null;

            if (!Preference.IsNameValid(name))
            {
                return false;
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            return _dictionary.TryGetValue(processedName, out item);
        }

        /// <summary>
        /// Gets the <paramref name="groups"/> associated with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <returns><see langword="true"/> if the store contains
        /// <paramref name="groups"/> with the specified
        /// <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool TryGetItemAsArrayOfPreferenceGroups(string name,
            out PreferenceGroup[] groups)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            groups = null;

            if (!TryGetItem(name, out var item) || item is null)
            {
                return false;
            }

            return item.TryGetAsArrayOfPreferenceGroups(out groups);
        }

        /// <summary>
        /// Gets the <paramref name="stores"/> associated with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <returns><see langword="true"/> if the store contains
        /// <paramref name="stores"/> with the specified
        /// <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool TryGetItemAsArrayOfPreferenceStores(string name,
            out PreferenceStore[] stores)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            stores = null;

            if (!TryGetItem(name, out var item) || item is null)
            {
                return false;
            }

            return item.TryGetAsArrayOfPreferenceStores(out stores);
        }

        /// <summary>
        /// Gets the <paramref name="preference"/> associated with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="preference"></param>
        /// <returns><see langword="true"/> if the store contains
        /// <paramref name="preference"/> with the specified
        /// <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool TryGetItemAsPreference(string name,
            out Preference preference)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            preference = null;

            if (!TryGetItem(name, out var item) || item is null)
            {
                return false;
            }

            return item.TryGetAsPreference(out preference);
        }

        /// <summary>
        /// Gets the <paramref name="group"/> associated with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns><see langword="true"/> if the store contains
        /// <paramref name="group"/> with the specified
        /// <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool TryGetItemAsPreferenceGroup(string name,
            out PreferenceGroup group)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            group = null;

            if (!TryGetItem(name, out var item) || item is null)
            {
                return false;
            }

            return item.TryGetAsPreferenceGroup(out group);
        }

        /// <summary>
        /// Gets the <paramref name="store"/> associated with the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="store"></param>
        /// <returns><see langword="true"/> if the store contains
        /// <paramref name="store"/> with the specified
        /// <paramref name="name"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool TryGetItemAsPreferenceStore(string name,
            out PreferenceStore store)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            store = null;

            if (!TryGetItem(name, out var item) || item is null)
            {
                return false;
            }

            return item.TryGetAsPreferenceStore(out store);
        }

        /// <summary>
        /// Updates or adds <paramref name="itemWithName"/> to the store.
        /// </summary>
        /// <param name="itemWithName"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> of
        /// <paramref name="itemWithName"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters, or the
        /// <see cref="PreferenceStoreItem.Kind"/> of
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemWithName"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>.</exception>
        /// <exception cref="ArgumentNullException">The
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> or
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> of
        /// <paramref name="itemWithName"/> is
        /// <see langword="null"/>.</exception>
        public void UpdateOrAdd(KeyValuePair<string, PreferenceStoreItem>
            itemWithName)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                itemWithName.Key, nameof(itemWithName));
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(
                itemWithName.Value, nameof(itemWithName));

            Add(processedName, itemWithName.Value);
        }

        /// <summary>
        /// Updates or adds <paramref name="preference"/> to the store, using
        /// its <see cref="Preference.Name"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name, nameof(preference));

            UpdateOrAdd(processedName, new PreferenceStoreItem(preference));
        }

        /// <summary>
        /// Updates or adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name,
            IEnumerable<PreferenceGroup> groups)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            UpdateOrAdd(processedName, new PreferenceStoreItem(groups));
        }

        /// <summary>
        /// Updates or adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name,
            IEnumerable<PreferenceStore> stores)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            UpdateOrAdd(processedName, new PreferenceStoreItem(stores));
        }

        /// <summary>
        /// Updates or adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, params PreferenceGroup[] groups)
            => UpdateOrAdd(name, (IEnumerable<PreferenceGroup>)groups);

        /// <summary>
        /// Updates or adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, params PreferenceStore[] stores)
            => UpdateOrAdd(name, (IEnumerable<PreferenceStore>)stores);

        /// <summary>
        /// Updates or adds <paramref name="group"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="group"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, PreferenceGroup group)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            UpdateOrAdd(processedName, new PreferenceStoreItem(group));
        }

        /// <summary>
        /// Updates or adds <paramref name="store"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="store"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="store"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, PreferenceStore store)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            UpdateOrAdd(processedName, new PreferenceStoreItem(store));
        }
        
        /// <summary>
        /// Updates or adds <paramref name="item"/> to the store with
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters, or the <see cref="PreferenceStoreItem.Kind"/> of
        /// <paramref name="item"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="item"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, PreferenceStoreItem item)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));
            PreferenceStoreItem.ThrowIfItemIsNullOrKindIsNone(item,
                nameof(item));

            _dictionary[processedName] = item;
        }

        /// <summary>
        /// Updates or adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, string description,
            IEnumerable<PreferenceGroup> groups)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            UpdateOrAdd(processedName, new PreferenceStoreItem(description,
                groups));
        }

        /// <summary>
        /// Updates or adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, string description,
            IEnumerable<PreferenceStore> stores)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name,
                nameof(name));

            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            UpdateOrAdd(processedName, new PreferenceStoreItem(description,
                stores));
        }

        /// <summary>
        /// Updates or adds <paramref name="groups"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="groups"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, string description,
            params PreferenceGroup[] groups)
            => UpdateOrAdd(name, description,
                 (IEnumerable<PreferenceGroup>)groups);

        /// <summary>
        /// Updates or adds <paramref name="stores"/> to the store with
        /// <paramref name="name"/> and the <paramref name="description"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or
        /// <paramref name="stores"/> is <see langword="null"/>.</exception>
        public void UpdateOrAdd(string name, string description,
            params PreferenceStore[] stores)
            => UpdateOrAdd(name, description,
                (IEnumerable<PreferenceStore>)stores);

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<string, PreferenceStoreItem>>.IsReadOnly
            => AsICollection.IsReadOnly;

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<string, PreferenceStoreItem>>.Contains(
            KeyValuePair<string, PreferenceStoreItem> item)
            => AsICollection.Contains(item);

        /// <inheritdoc/>
        void ICollection<KeyValuePair<string, PreferenceStoreItem>>.CopyTo(
            KeyValuePair<string, PreferenceStoreItem>[] array, int arrayIndex)
            => AsICollection.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<string, PreferenceStoreItem>>.Remove(
            KeyValuePair<string, PreferenceStoreItem> item)
            => AsICollection.Remove(item);

        /// <inheritdoc/>
        ICollection<string> IDictionary<string, PreferenceStoreItem>.Keys
            => _dictionary.Keys;

        /// <inheritdoc/>
        ICollection<PreferenceStoreItem>
            IDictionary<string, PreferenceStoreItem>.Values
            => _dictionary.Values;

        /// <inheritdoc/>
        bool IDictionary<string, PreferenceStoreItem>.ContainsKey(string key)
            => _dictionary.ContainsKey(key);

        /// <inheritdoc/>
        bool IDictionary<string, PreferenceStoreItem>.TryGetValue(string key,
            out PreferenceStoreItem value)
            => _dictionary.TryGetValue(key, out value);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_dictionary).GetEnumerator();
    }
}
