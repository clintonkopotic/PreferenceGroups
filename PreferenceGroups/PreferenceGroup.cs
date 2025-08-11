using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace PreferenceGroups
{
    /// <summary>
    /// A group of <see cref="Preference"/>s as a <see cref="ICollection{T}"/>.
    /// </summary>
    public class PreferenceGroup : ICollection<Preference>
    {
        // TODO: Update _associatedObject to handle Preference.Names that do not match the names of the public properties of _associatedObject.

        /// <summary>
        /// The backing store for an associated <see langword="object"/> such
        /// that when a value of a <see cref="Preference"/> of the
        /// <see cref="PreferenceGroup"/> that shares
        /// </summary>
        private readonly object _associatedObject = null;

        /// <summary>
        /// The backing store for the collection, with using
        /// <see cref="Preference.Name"/> as the key in the dictionary.
        /// </summary>
        private readonly Dictionary<string, Preference> _dictionary
            = new Dictionary<string, Preference>();

        /// <summary>
        /// Gets or sets the <see cref="Preference"/> associated with the
        /// specified <paramref name="name"/>. The <see cref="Preference.Name"/>
        /// must equal to <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is empty
        /// or constist only of white-space characters, or
        /// <paramref name="name"/> does not equal to
        /// <see cref="Preference.Name"/> that is being set.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/> or the <see cref="Preference"/> being set is
        /// <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="name"/> was
        /// not found as a <see cref="Preference.Name"/> of any of the group
        /// <see cref="Preference"/>s.</exception>
        public Preference this[string name]
        {
            get
            {
                var processedName = Preference.ProcessNameOrThrowIfInvalid(
                    name);

                return _dictionary[processedName];
            }
            set
            {
                var processedName = ProcessNamesAndThrowIfNotEqual(name, value);

                _dictionary[processedName] = value;

                if (_associatedObject is null)
                {
                    return;
                }

                var objectType = _associatedObject.GetType();
                var objectProperty = objectType.GetProperty(name);

                if (objectProperty is null)
                {
                    return;
                }

                objectProperty.SetValue(_associatedObject,
                    value.GetValueAsObject());
            }
        }

        /// <summary>
        /// The names of the <see cref="Preference"/>s in the group.
        /// </summary>
        public ICollection<string> Names => _dictionary.Keys;

        /// <inheritdoc/>
        public int Count => _dictionary.Count;

        /// <summary>
        /// The description, if any, of the group.
        /// </summary>
        public string Description { get; }

        bool ICollection<Preference>.IsReadOnly
            => ((ICollection<KeyValuePair<string, Preference>>)_dictionary)
                .IsReadOnly;

        internal PreferenceGroup(object @object)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            _associatedObject = @object;
            var objectType = _associatedObject.GetType();

            if (!objectType.IsClass)
            {
                throw new ArgumentException(paramName: nameof(@object),
                    message: "The type must be a class.");
            }

            Description = null;
            var groupAttribute = objectType
                .GetCustomAttribute<PreferenceGroupAttribute>();

            if (!(groupAttribute is null))
            {
                Description = groupAttribute.Description?.Trim();
            }

            foreach (var property in objectType.GetProperties())
            {
                string preferenceName = Preference.ProcessNameOrThrowIfInvalid(
                    property.Name);
                object preferenceDefaultValue = null;
                string preferenceDescription = null;
                var preferenceAttribute = property
                    .GetCustomAttribute<PreferenceAttribute>();

                if (!(preferenceAttribute is null))
                {
                    if (!(preferenceAttribute.Name is null))
                    {
                        preferenceName = Preference.ProcessNameOrThrowIfInvalid(
                            preferenceAttribute.Name);
                    }

                    preferenceDefaultValue = preferenceAttribute.DefaultValue;
                    preferenceDescription = preferenceAttribute.Description;
                }

                var propertyType = property.PropertyType;

                if (propertyType == typeof(int?))
                {
                    var value = (int?)property.GetValue(_associatedObject);
                    int? defaultValue = preferenceDefaultValue is null ? null
                        : (int?)preferenceDefaultValue;

                    _dictionary[preferenceName] = Int32PreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .WithDescription(preferenceDescription)
                        .Build();
                }
                else if (propertyType == typeof(string))
                {
                    var value = (string)property.GetValue(_associatedObject);
                    string defaultValue = preferenceDefaultValue is null ? null
                        : (string)preferenceDefaultValue;

                    _dictionary[preferenceName] = StringPreferenceBuilder
                        .Create(preferenceName)
                        .WithValue(value)
                        .WithDefaultValue(defaultValue)
                        .WithDescription(preferenceDescription)
                        .Build();
                }
            }
        }

        /// <summary>
        /// Instantiates a <see cref="PreferenceGroup"/> with
        /// <paramref name="description"/>, if any.
        /// </summary>
        /// <param name="description"></param>
        public PreferenceGroup(string description = null)
        {
            Description = description?.Trim();
        }

        /// <summary>
        /// Adds <paramref name="preferences"/> to the group.
        /// </summary>
        /// <param name="preferences"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of any of the
        /// <paramref name="preferences"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters or a
        /// <see cref="Preference"/> in the group has the same
        /// <see cref="Preference.Name"/> as one of the
        /// <paramref name="preferences"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preferences"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of any of the
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public void Add(IEnumerable<Preference> preferences)
        {
            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            foreach (var preference in preferences)
            {
                var processedName = Preference.ProcessNameOrThrowIfInvalid(
                    preference.Name);
                _dictionary.Add(processedName, preference);
            }
        }

        /// <summary>
        /// Adds <paramref name="preferences"/> to the group.
        /// </summary>
        /// <param name="preferences"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of any of the
        /// <paramref name="preferences"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters or a
        /// <see cref="Preference"/> in the group has the same
        /// <see cref="Preference.Name"/> as one of the
        /// <paramref name="preferences"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preferences"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of any of the
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public void Add(params Preference[] preferences)
        {
            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            Add((IEnumerable<Preference>)preferences);
        }

        /// <summary>
        /// Adds <paramref name="preference"/> to the group.
        /// </summary>
        /// <param name="preference"></param>
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
        public void Add(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name);
            _dictionary.Add(processedName, preference);
        }

        /// <inheritdoc/>
        public void Clear() => _dictionary.Clear();

        /// <inheritdoc/>
        public bool Contains(Preference preference)
            => !(preference is null)
            && Preference.IsNameValid(preference.Name)
            && _dictionary.ContainsKey(preference.Name)
            && _dictionary.ContainsValue(preference);

        /// <summary>
        /// Determines whether the group contains a <see cref="Preference"/>
        /// with the same <see cref="Preference.Name"/> as
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool ContainsName(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return _dictionary.ContainsKey(processedName);
        }

        /// <inheritdoc/>
        public void CopyTo(Preference[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex),
                    arrayIndex, "Must not be negative.");
            }

            if (arrayIndex + Count >= array.Length)
            {
                throw new ArgumentException(paramName: nameof(array),
                    message: "Not enough space to copy into.");
            }

            foreach (var preference in _dictionary.Values)
            {
                array[arrayIndex] = preference;
                arrayIndex++;
            }
        }

        /// <summary>
        /// Gets the <see cref="Preference"/>'s <c>DefaultValue</c>, as an
        /// <see cref="object"/>, where its <see cref="Preference.Name"/> is
        /// <paramref name="name"/> using the
        /// <see cref="Preference.GetDefaultValueAsObject()"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The <see cref="Preference"/>'s <c>Value</c>, as an
        /// <see cref="object"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="name"/> was
        /// not found as a <see cref="Preference.Name"/> of any of the group
        /// <see cref="Preference"/>s.</exception>
        public object GetDefaultValue(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return this[processedName].GetDefaultValueAsObject();
        }

        /// <summary>
        /// Gets the <see cref="Preference"/>'s <c>DefaultValue</c>, as a
        /// <typeparamref name="T"/>, where its <see cref="Preference.Name"/> is
        /// <paramref name="name"/> using the
        /// <see cref="Preference.GetDefaultValueAs{T}"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The <see cref="Preference"/>'s <c>Value</c>, as an
        /// <see cref="object"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="name"/> was
        /// not found as a <see cref="Preference.Name"/> of any of the group
        /// <see cref="Preference"/>s.</exception>
        public T GetDefaultValueAs<T>(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return this[processedName].GetDefaultValueAs<T>();
        }

        ///<inheritdoc/>
        public IEnumerator<Preference> GetEnumerator()
            => ((IEnumerable<Preference>)_dictionary.Values).GetEnumerator();

        ///<inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_dictionary.Values).GetEnumerator();

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
        /// <exception cref="KeyNotFoundException"><paramref name="name"/> was
        /// not found as a <see cref="Preference.Name"/> of any of the group
        /// <see cref="Preference"/>s.</exception>
        public object GetValue(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return this[processedName].GetValueAsObject();
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
        /// <exception cref="KeyNotFoundException"><paramref name="name"/> was
        /// not found as a <see cref="Preference.Name"/> of any of the group
        /// <see cref="Preference"/>s.</exception>
        public T GetValueAs<T>(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return this[processedName].GetValueAs<T>();
        }

        /// <summary>
        /// Removes the <see cref="Preference"/> in the group where the
        /// <see cref="Preference.Name"/> equals <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see langword="true"/> if the <see cref="Preference"/> is
        /// successfully found and removed; otherwise, <see langword="false"/>,
        /// even if <paramref name="name"/> was not found.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/>.</exception>
        public bool RemoveByName(string name)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            return _dictionary.Remove(processedName);
        }

        /// <inheritdoc/>
        public bool Remove(Preference preference)
        {
            if (preference is null)
            {
                return false;
            }

            var remove = false;

            foreach (var p in _dictionary.Values)
            {
                if (p == preference)
                {
                    remove = true;

                    break;
                }
            }

            if (remove)
            {
                return RemoveByName(preference.Name);
            }

            return false;
        }

        /// <summary>
        /// Sets the <c>Value</c> of the <see cref="Preference"/>, where the
        /// <see cref="Preference.Name"/> matches <paramref name="name"/> with
        /// <paramref name="value"/> by using
        /// <see cref="Preference.SetValueFromObject(object)"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <exception cref="SetValueException">The <c>Value</c> failed to be
        /// set (see <see cref="SetValueException.Result"/> for the reason why
        /// not).</exception>
        public void SetValue(string name, object value)
        {
            string processedName;

            try
            {
                processedName = Preference.ProcessNameOrThrowIfInvalid(name);
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex,
                    SetValueStepFailure.ProcessingName);
            }

            Preference preference;

            try
            {
                preference = this[processedName];
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex,
                    SetValueStepFailure.RetrievingPreference);
            }

            try
            {
                preference.SetValueFromObject(value);
            }
            catch (SetValueException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SetValueException(ex,
                    SetValueStepFailure.SettingValue);
            }
        }

        /// <summary>
        /// Attempts to get the <see cref="Preference"/>'s <c>DefaultValue</c>,
        /// as an <see cref="object"/>, where its <see cref="Preference.Name"/>
        /// is <paramref name="name"/> using the
        /// <see cref="GetDefaultValue(string)"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">The <see cref="Preference"/>'s
        /// <c>DefaultValue</c>, as an <see cref="object"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="defaultValue"/>
        /// is the found <c>DefaultValue</c>; otherwise,
        /// <see langword="false"/>.</returns>
        public bool TryGetDefaultValue(string name, out object defaultValue)
        {
            defaultValue = null;

            try
            {
                defaultValue = GetDefaultValue(name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to get the <see cref="Preference"/>'s <c>DefaultValue</c>,
        /// as a <typeparamref name="T"/>, where its
        /// <see cref="Preference.Name"/> is <paramref name="name"/> using the
        /// <see cref="GetValueAs{T}(string)"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">The <see cref="Preference"/>'s
        /// <c>DefaultValue</c>, as a <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="defaultValue"/>
        /// is the found <c>DefaultValue</c>; otherwise,
        /// <see langword="false"/>.</returns>
        public bool TryGetDefaultValueAs<T>(string name, out T defaultValue)
        {
            defaultValue = default;

            try
            {
                defaultValue = GetDefaultValueAs<T>(name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to get the <see cref="Preference"/>'s <c>Value</c>, as an
        /// <see cref="object"/>, where its <see cref="Preference.Name"/> is
        /// <paramref name="name"/> using the <see cref="GetValue(string)"/>
        /// method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">The <see cref="Preference"/>'s <c>Value</c>, as
        /// an <see cref="object"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is the
        /// found <c>Value</c>; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(string name, out object value)
        {
            value = null;

            try
            {
                value = GetValue(name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to get the <see cref="Preference"/>'s <c>Value</c>, as a
        /// <typeparamref name="T"/>, where its <see cref="Preference.Name"/> is
        /// <paramref name="name"/> using the
        /// <see cref="GetValueAs{T}(string)"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">The <see cref="Preference"/>'s <c>Value</c>, as
        /// a <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is the
        /// found <c>Value</c>; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValueAs<T>(string name, out T value)
        {
            value = default;

            try
            {
                value = GetValueAs<T>(name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to set the <c>Value</c> of the <see cref="Preference"/>,
        /// where the <see cref="Preference.Name"/> matches
        /// <paramref name="name"/> with <paramref name="value"/> by using
        /// <see cref="SetValue(string, object)"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="result">If setting <c>Value</c> was unsuccessful, this
        /// contains why.</param>
        /// <returns><see langword="true"/> if <c>Value</c> was successfully set
        /// with <paramref name="value"/>; otherwise,
        /// <see langword="false"/>.</returns>
        public bool TrySetValue(string name, object value,
            out SetValueResult result)
        {
            result = null;

            try
            {
                SetValue(name, value);

                return true;
            }
            catch (SetValueException ex)
            {
                result = ex.Result;

                return false;
            }
            catch (Exception ex)
            {
                result = new SetValueResult(ex,
                    SetValueStepFailure.SettingValue);

                return false;
            }
        }

        /// <summary>
        /// For each of the <paramref name="preferences"/>, if the
        /// <see cref="Preference.Name"/> of a <see cref="Preference"/> is
        /// already in the group, then the group will be updated with it;
        /// otherwise, the <see cref="Preference"/> will be added to the group.
        /// </summary>
        /// <param name="preferences"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preferences"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public void UpdateOrAdd(IEnumerable<Preference> preferences)
        {
            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            foreach (var preference in preferences)
            {
                if (preference is null)
                {
                    throw new ArgumentNullException(nameof(preference));
                }

                UpdateOrAdd(preference);
            }
        }

        /// <summary>
        /// For each of the <paramref name="preferences"/>, if the
        /// <see cref="Preference.Name"/> of a <see cref="Preference"/> is
        /// already in the group, then the group will be updated with it;
        /// otherwise, the <see cref="Preference"/> will be added to the group.
        /// </summary>
        /// <param name="preferences"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is an empty <see langword="string"/>
        /// or conists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preferences"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of one of the
        /// <paramref name="preferences"/> is
        /// <see langword="null"/>.</exception>
        public void UpdateOrAdd(params Preference[] preferences)
        {
            if (preferences is null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            UpdateOrAdd((IEnumerable<Preference>)preferences);
        }

        /// <summary>
        /// If the <see cref="Preference.Name"/> of
        /// <paramref name="preference"/> is already in the group, then the
        /// group will by updated with <paramref name="preference"/>; otherwise,
        /// <paramref name="preference"/> will be added to the group.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentException">The
        /// <see cref="Preference.Name"/> of <paramref name="preference"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/> or the
        /// <see cref="Preference.Name"/> of <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public void UpdateOrAdd(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name);
            _dictionary[processedName] = preference;
        }

        /// <summary>
        /// Processes both the <paramref name="name"/> and the
        /// <see cref="Preference.Name"/> of <paramref name="preference"/> by
        /// using the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string)"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="preference"></param>
        /// <returns>The processed name.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> does
        /// not equal the <see cref="Preference.Name"/> of
        /// <paramref name="preference"/>, or <paramref name="name"/> or the
        /// <see cref="Preference.Name"/> of <paramref name="name"/> is an
        /// empty <see langword="string"/> or conists only of white-space
        /// characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is
        /// <see langword="null"/> or <paramref name="preference"/> is
        /// <see langword="null"/> or the <see cref="Preference.Name"/> of
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static string ProcessNamesAndThrowIfNotEqual(string name,
            Preference preference)
        {
            var processedName = Preference.ProcessNameOrThrowIfInvalid(name);

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            var processedPreferenceName
                = Preference.ProcessNameOrThrowIfInvalid(preference.Name);

            if (processedName != processedPreferenceName)
            {
                throw new ArgumentException(paramName: nameof(name),
                    message: "Must have the same name as "
                        + $"{nameof(preference)}.");
            }

            return processedPreferenceName;
        }
    }
}
