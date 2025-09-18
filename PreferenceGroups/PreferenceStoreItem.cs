using System;
using System.Collections.Generic;
using System.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// The individual item within a <see cref="PreferenceStore"/>, that is one
    /// of <see cref="PreferenceStoreItemKind"/>.
    /// </summary>
    public class PreferenceStoreItem
    {
        private readonly object item;

        /// <summary>
        /// A description of the <see cref="PreferenceStoreItem"/> that is
        /// intended to be shown to the user and in the file as a comment.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The <see cref="Kind"/> of the <see cref="PreferenceStoreItem"/>.
        /// </summary>
        public PreferenceStoreItemKind Kind { get; }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(IEnumerable<PreferenceGroup> groups)
            : this(null, groups)
        { }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="stores"/>.
        /// </summary>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentNullException"><paramref name="stores"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(IEnumerable<PreferenceStore> stores)
            : this(null, stores)
        { }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(params PreferenceGroup[] groups)
            : this((IEnumerable<PreferenceGroup>)groups)
        { }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="stores"/>.
        /// </summary>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentNullException"><paramref name="stores"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(params PreferenceStore[] stores)
            : this(null, stores)
        { }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="preference"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public PreferenceStoreItem(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }
            
            item = preference;
            Description = preference.Description?.Trim();
            Kind = PreferenceStoreItemKind.Preference;
        }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="group"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            item = group;
            Description = group.Description?.Trim();
            Kind = PreferenceStoreItemKind.PreferenceGroup;
        }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="store"/>.
        /// </summary>
        /// <param name="store"></param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(PreferenceStore store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            item = store;
            Description = store.Description?.Trim();
            Kind = PreferenceStoreItemKind.PreferenceStore;
        }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(string description,
            IEnumerable<PreferenceGroup> groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            item = groups.ToArray();
            Description = description?.Trim();
            Kind = PreferenceStoreItemKind.ArrayOfPreferenceGroups;
        }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="stores"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentNullException"><paramref name="stores"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(string description,
            IEnumerable<PreferenceStore> stores)
        {
            if (stores is null)
            {
                throw new ArgumentNullException(nameof(stores));
            }

            item = stores.ToArray();
            Description = description?.Trim();
            Kind = PreferenceStoreItemKind.ArrayOfPreferenceStores;
        }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(string description,
            params PreferenceGroup[] groups)
            : this(description, (IEnumerable<PreferenceGroup>)groups)
        { }

        /// <summary>
        /// Initializes a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="stores"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="stores"></param>
        /// <exception cref="ArgumentNullException"><paramref name="stores"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceStoreItem(string description,
            params PreferenceStore[] stores)
            : this(description, (IEnumerable<PreferenceStore>)stores)
        { }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceGroups"/>, then
        /// the item is cast and returned as an <see cref="Array"/> of
        /// <see cref="PreferenceGroup"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// an <see cref="Array"/> of <see cref="PreferenceGroup"/>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="Kind"/>
        /// does not equal
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceGroups"/>.
        /// </exception>
        public PreferenceGroup[] GetAsArrayOfPreferenceGroups()
        {
            if (Kind != PreferenceStoreItemKind.ArrayOfPreferenceGroups)
            {
                throw new InvalidOperationException($"{nameof(Kind)} is not "
                    + $"{nameof(PreferenceStoreItemKind)}."
                    + nameof(PreferenceStoreItemKind.ArrayOfPreferenceGroups)
                    + $", rather is "
                    + $"{nameof(PreferenceStoreItemKind)}.{Kind}.");
            }

            return (PreferenceGroup[])item;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceStores"/>, then
        /// the item is cast and returned as an <see cref="Array"/> of
        /// <see cref="PreferenceStore"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// an <see cref="Array"/> of <see cref="PreferenceStore"/>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="Kind"/>
        /// does not equal
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceStores"/>.
        /// </exception>
        public PreferenceStore[] GetAsArrayOfPreferenceStores()
        {
            if (Kind != PreferenceStoreItemKind.ArrayOfPreferenceStores)
            {
                throw new InvalidOperationException($"{nameof(Kind)} is not "
                    + $"{nameof(PreferenceStoreItemKind)}"
                    + $".{nameof(PreferenceStoreItemKind.ArrayOfPreferenceStores)}, "
                    + $"rather is {nameof(PreferenceStoreItemKind)}.{Kind}.");
            }

            return (PreferenceStore[])item;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.Preference"/>, then the item is
        /// cast and returned as a <see cref="Preference"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// a<see cref="Preference"/>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="Kind"/>
        /// does not equal
        /// <see cref="PreferenceStoreItemKind.Preference"/>.</exception>
        public Preference GetAsPreference()
        {
            if (Kind != PreferenceStoreItemKind.Preference)
            {
                throw new InvalidOperationException($"{nameof(Kind)} is not "
                    + $"{nameof(PreferenceStoreItemKind)}"
                    + $".{nameof(PreferenceStoreItemKind.Preference)}, rather "
                    + $"is {nameof(PreferenceStoreItemKind)}.{Kind}.");
            }

            return (Preference)item;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.PreferenceGroup"/>, then the item
        /// is cast and returned as a <see cref="PreferenceGroup"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// a<see cref="PreferenceGroup"/>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="Kind"/>
        /// does not equal
        /// <see cref="PreferenceStoreItemKind.PreferenceGroup"/>.</exception>
        public PreferenceGroup GetAsPreferenceGroup()
        {
            if (Kind != PreferenceStoreItemKind.PreferenceGroup)
            {
                throw new InvalidOperationException($"{nameof(Kind)} is not "
                    + $"{nameof(PreferenceStoreItemKind)}"
                    + $".{nameof(PreferenceStoreItemKind.PreferenceGroup)}, "
                    + $"rather is {nameof(PreferenceStoreItemKind)}.{Kind}.");
            }

            return (PreferenceGroup)item;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.PreferenceStore"/>, then the item
        /// is cast and returned as a <see cref="PreferenceStore"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">The item could not be cast to
        /// a<see cref="PreferenceStore"/>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="Kind"/>
        /// does not equal
        /// <see cref="PreferenceStoreItemKind.PreferenceStore"/>.</exception>
        public PreferenceStore GetAsPreferenceStore()
        {
            if (Kind != PreferenceStoreItemKind.PreferenceStore)
            {
                throw new InvalidOperationException($"{nameof(Kind)} is not "
                    + $"{nameof(PreferenceStoreItemKind)}"
                    + $".{nameof(PreferenceStoreItemKind.PreferenceStore)}, "
                    + $"rather is {nameof(PreferenceStoreItemKind)}.{Kind}.");
            }

            return (PreferenceStore)item;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceGroups"/> and
        /// the item is an <see cref="Array"/> of <see cref="PreferenceGroup"/>,
        /// then <paramref name="groups"/> has it, otherwise it is
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <returns><see langword="true"/> if successful,
        /// <see langword="false"/> otherwise.</returns>
        public bool TryGetAsArrayOfPreferenceGroups(
            out PreferenceGroup[] groups)
        {
            groups = null;

            if (Kind == PreferenceStoreItemKind.ArrayOfPreferenceGroups
                && item is PreferenceGroup[] itemAs)
            {
                groups = itemAs;

                return true;
            }

            return false;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.ArrayOfPreferenceGroups"/> and
        /// the item is an <see cref="Array"/> of <see cref="PreferenceStore"/>,
        /// then <paramref name="stores"/> has it, otherwise it is
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="stores"></param>
        /// <returns><see langword="true"/> if successful,
        /// <see langword="false"/> otherwise.</returns>
        public bool TryGetAsArrayOfPreferenceStores(
            out PreferenceStore[] stores)
        {
            stores = null;

            if (Kind == PreferenceStoreItemKind.ArrayOfPreferenceStores
                && item is PreferenceStore[] itemAs)
            {
                stores = itemAs;

                return true;
            }

            return false;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.Preference"/> and the item is a
        /// <see cref="Preference"/>, then <paramref name="preference"/> has it,
        /// otherwise it is <see langword="null"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns><see langword="true"/> if successful,
        /// <see langword="false"/> otherwise.</returns>
        public bool TryGetAsPreference(out Preference preference)
        {
            preference = null;

            if (Kind == PreferenceStoreItemKind.Preference
                && item is Preference itemAs)
            {
                preference = itemAs;

                return true;
            }

            return false;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.PreferenceGroup"/> and the item
        /// is a <see cref="PreferenceGroup"/>, then <paramref name="group"/>
        /// has it, otherwise it is <see langword="null"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <returns><see langword="true"/> if successful,
        /// <see langword="false"/> otherwise.</returns>
        public bool TryGetAsPreferenceGroup(out PreferenceGroup group)
        {
            group = null;

            if (Kind == PreferenceStoreItemKind.PreferenceGroup
                && item is PreferenceGroup itemAs)
            {
                group = itemAs;

                return true;
            }

            return false;
        }

        /// <summary>
        /// If the <see cref="Kind"/> equals
        /// <see cref="PreferenceStoreItemKind.PreferenceStore"/> and the item
        /// is a <see cref="PreferenceStore"/>, then <paramref name="store"/>
        /// has it, otherwise it is <see langword="null"/>.
        /// </summary>
        /// <param name="store"></param>
        /// <returns><see langword="true"/> if successful,
        /// <see langword="false"/> otherwise.</returns>
        public bool TryGetAsPreferenceStore(out PreferenceStore store)
        {
            store = null;

            if (Kind == PreferenceStoreItemKind.PreferenceStore
                && item is PreferenceStore itemAs)
            {
                store = itemAs;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Ensures that <paramref name="item"/> is not <see langword="null"/>
        /// and its <see cref="Kind"/> does not equal
        /// <see cref="PreferenceStoreItemKind.None"/>. If it does then an
        /// <see cref="ArgumentNullException"/> or an
        /// <see cref="ArgumentException"/> is thrown, respectively.
        /// </summary>
        /// <param name="item">What to ensure is not <see langword="null"/>
        /// and its <see cref="Kind"/> does not equal
        /// <see cref="PreferenceStoreItemKind.None"/>.</param>
        /// <param name="paramName">The name of the <paramref name="item"/>
        /// parameter. If it is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="item"/> will be
        /// used.</param>
        /// <exception cref="ArgumentException">The <see cref="Kind"/> of
        /// <paramref name="item"/> equals
        /// <see cref="PreferenceStoreItemKind.None"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfItemIsNullOrKindIsNone(PreferenceStoreItem item,
            string paramName = null)
        {
            if (item is null)
            {
                throw new ArgumentNullException(paramName ?? nameof(item));
            }

            if (item.Kind == PreferenceStoreItemKind.None)
            {
                throw new ArgumentException(
                    paramName: paramName ?? nameof(item),
                    message: $"The item cannot have {nameof(Kind)} of "
                    + $"{nameof(PreferenceStoreItemKind)}"
                    + $".{nameof(PreferenceStoreItemKind.None)}");
            }

            if (!Enum.IsDefined(typeof(PreferenceStoreItemKind), item.Kind))
            {
                throw new ArgumentException(
                    paramName: paramName ?? nameof(item),
                    message: $"The {paramName ?? nameof(item)} must be a "
                    + $"known {nameof(PreferenceStoreItemKind)}, not "
                    + $"{item.Kind}.");
            }
        }

        /// <summary>
        /// Ensures that <paramref name="itemKind"/> does not equal
        /// <see cref="PreferenceStoreItemKind.None"/>. If it does then an
        /// <see cref="ArgumentException"/> is thrown.
        /// </summary>
        /// <param name="itemKind">What to ensure does not equal
        /// <see cref="PreferenceStoreItemKind.None"/>.</param>
        /// <param name="paramName">The name of the <paramref name="itemKind"/>
        /// parameter. If it is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="itemKind"/> will be
        /// used.</param>
        /// <exception cref="ArgumentException"><paramref name="itemKind"/>
        /// equals <see cref="PreferenceStoreItemKind.None"/>.</exception>
        public static void ThrowIfItemKindIsUnknown(
            PreferenceStoreItemKind itemKind, string paramName = null)
        {
            if (itemKind == PreferenceStoreItemKind.None)
            {
                throw new ArgumentException(
                    paramName: paramName ?? nameof(itemKind),
                    message: $"The {paramName ?? nameof(itemKind)} cannot be "
                    + $"{nameof(PreferenceStoreItemKind)}"
                    + $".{nameof(PreferenceStoreItemKind.None)}");
            }

            if (!Enum.IsDefined(typeof(PreferenceStoreItemKind), itemKind))
            {
                throw new ArgumentException(
                    paramName: paramName ?? nameof(itemKind),
                    message: $"The {paramName ?? nameof(itemKind)} must be a "
                    + $"known {nameof(PreferenceStoreItemKind)}, not "
                    + $"{itemKind}.");
            }
        }

        /// <summary>
        /// Creates a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="preference"/>.
        /// </summary>
        /// <param name="preference"></param>
        public static implicit operator PreferenceStoreItem(
            Preference preference)
            => new PreferenceStoreItem(preference);

        /// <summary>
        /// Creates a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="group"/>.
        /// </summary>
        /// <param name="group"></param>
        public static implicit operator PreferenceStoreItem(
            PreferenceGroup group)
            => new PreferenceStoreItem(group);

        /// <summary>
        /// Creates a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups"></param>
        public static implicit operator PreferenceStoreItem(
            PreferenceGroup[] groups)
            => new PreferenceStoreItem(groups);

        /// <summary>
        /// Creates a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="store"/>.
        /// </summary>
        /// <param name="store"></param>
        public static implicit operator PreferenceStoreItem(
            PreferenceStore store)
            => new PreferenceStoreItem(store);

        /// <summary>
        /// Creates a <see cref="PreferenceStoreItem"/> from
        /// <paramref name="stores"/>.
        /// </summary>
        /// <param name="stores"></param>
        public static implicit operator PreferenceStoreItem(
            PreferenceStore[] stores)
            => new PreferenceStoreItem(stores);
    }
}
