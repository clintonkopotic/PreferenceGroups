using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Facilitates writing to and reading from a JSONC file for a
    /// <see cref="PreferenceGroup"/>.
    /// </summary>
    /// <remarks>See <see href="https://www.jsonc.org">jsonc.org</see> for
    /// further details about a JSONC file format.</remarks>
    public class PreferenceFile
    {
        /// <summary>
        /// The <see cref="System.Text.Encoding"/> of the file.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// The <see cref="JsonLoadSettings"/> for reading the file as a
        /// <see cref="JToken"/> using the
        /// <see cref="JToken.ReadFrom(JsonReader, JsonLoadSettings)"/> method.
        /// </summary>
        public JsonLoadSettings LoadSettings { get; }

        /// <summary>
        /// The file path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The <see cref="string"/> used for each indentation level of
        /// formatting when using the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor in the <c>Write()</c> methods. Defaults to
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/>.
        /// </summary>
        public string TabString { get; }

        /// <summary>
        /// Instantiates with <paramref name="path"/>,
        /// <paramref name="indentChar"/>, and <paramref name="indentDepth"/>.
        /// The following properties will be defaulted to:
        /// <list type="bullet">
        /// <item><see cref="Encoding"/> to
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/>.</item>
        /// <item><see cref="LoadSettings"/> to
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="indentChar">The indent character for
        /// <see cref="TabString"/> that will be repeated
        /// <paramref name="indentDepth"/> number of times.</param>
        /// <param name="indentDepth">The number of times to repeat
        /// <paramref name="indentChar"/> for <see cref="TabString"/>. Must not
        /// be a negative number and if it is zero then
        /// <see cref="string.Empty"/> is used for
        /// <see cref="TabString"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> cannot be negative.</exception>
        public PreferenceFile(string path, char indentChar, int indentDepth)
            : this(path, null, null, indentChar, indentDepth)
        { }

        /// <summary>
        /// Instantiates with <paramref name="path"/>,
        /// <paramref name="encoding"/> (where if it is <see langword="null"/>
        /// then <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used),
        /// <paramref name="loadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used),
        /// <paramref name="indentChar"/>, and <paramref name="indentDepth"/>.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/> of the
        /// file, where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <param name="loadSettings">The <see cref="JsonLoadSettings"/> for
        /// reading the file as a <see cref="JToken"/> using the
        /// <see cref="JToken.ReadFrom(JsonReader, JsonLoadSettings)"/> method,
        /// where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <param name="indentChar">The indent character for
        /// <see cref="TabString"/> that will be repeated
        /// <paramref name="indentDepth"/> number of times.</param>
        /// <param name="indentDepth">The number of times to repeat
        /// <paramref name="indentChar"/> for <see cref="TabString"/>. Must not
        /// be a negative number and if it is zero then
        /// <see cref="string.Empty"/> is used for
        /// <see cref="TabString"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> cannot be negative.</exception>
        public PreferenceFile(string path, Encoding encoding,
            JsonLoadSettings loadSettings, char indentChar, int indentDepth)
            : this(path, encoding, loadSettings,
                  JsoncSerializerHelper.GetTabString(indentChar, indentDepth))
        { }

        /// <summary>
        /// Instantiates with <paramref name="path"/>,
        /// <paramref name="encoding"/> (where if it is <see langword="null"/>
        /// then <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used),
        /// <paramref name="loadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used),
        /// and <paramref name="tabString"/>.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/> of the
        /// file, where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <param name="loadSettings">The <see cref="JsonLoadSettings"/> for
        /// reading the file as a <see cref="JToken"/> using the
        /// <see cref="JToken.ReadFrom(JsonReader, JsonLoadSettings)"/> method,
        /// where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <param name="tabString">The <see cref="string"/> used for each
        /// indentation level of formatting.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> or
        /// <paramref name="tabString"/> is <see langword="null"/>.</exception>
        public PreferenceFile(string path, Encoding encoding,
            JsonLoadSettings loadSettings, string tabString)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(paramName: nameof(path),
                    message: "Is empty or consists only of white-space "
                    + "characters.");
            }

            if (tabString is null)
            {
                throw new ArgumentNullException(nameof(tabString));
            }

            Path = path;
            Encoding = encoding ?? JsoncSerializerHelper.DefaultEncoding;
            LoadSettings = loadSettings
                ?? JsoncSerializerHelper.DefaultLoadSettings;
            TabString = tabString;
        }

        /// <summary>
        /// Instantiates with <paramref name="path"/>,
        /// <paramref name="encoding"/> (where if it is <see langword="null"/>
        /// then <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used),
        /// and <paramref name="loadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used).
        /// The <see cref="TabString"/> property will default to
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/>.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/> of the
        /// file, where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <param name="loadSettings">The <see cref="JsonLoadSettings"/> for
        /// reading the file as a <see cref="JToken"/> using the
        /// <see cref="JToken.ReadFrom(JsonReader, JsonLoadSettings)"/> method,
        /// where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceFile(string path, Encoding encoding,
            JsonLoadSettings loadSettings)
            : this(path, encoding, loadSettings,
                  JsoncSerializerHelper.DefaultTabString)
        { }

        /// <summary>
        /// Instantiates with <paramref name="path"/> and
        /// <paramref name="encoding"/> (where if it is <see langword="null"/>
        /// then <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used).
        /// The following properties will be defaulted to:
        /// <list type="bullet">
        /// <item><see cref="LoadSettings"/> to
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/>.</item>
        /// <item><see cref="TabString"/> to
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/> of the
        /// file, where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceFile(string path, Encoding encoding)
            : this(path, encoding, null)
        { }

        /// <summary>
        /// Instantiates with <paramref name="path"/>
        /// and <paramref name="tabString"/>. The following properties will be
        /// defaulted to:
        /// <list type="bullet">
        /// <item><see cref="Encoding"/> to
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/>.</item>
        /// <item><see cref="LoadSettings"/> to
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="tabString">The <see cref="string"/> used for each
        /// indentation level of formatting.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> or
        /// <paramref name="tabString"/> is <see langword="null"/>.</exception>
        public PreferenceFile(string path, string tabString)
            : this(path, null, null, tabString)
        { }

        /// <summary>
        /// Instantiates with <paramref name="path"/> and the following
        /// properties will be defaulted to:
        /// <list type="bullet">
        /// <item><see cref="Encoding"/> to
        /// <see cref="JsoncSerializerHelper.DefaultEncoding"/>.</item>
        /// <item><see cref="LoadSettings"/> to
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/>.</item>
        /// <item><see cref="TabString"/> to
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty
        /// or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is
        /// <see langword="null"/>.</exception>
        public PreferenceFile(string path) : this(path, null, null)
        { }

        /// <summary>
        /// Reads the contents of the file at <see cref="Path"/> as a
        /// <see cref="JToken"/> by calling the <see cref="ReadAsJToken()"/>
        /// method and then calling
        /// <see cref="JsoncSerializerHelper.ReadAsJArray(JToken)"/>.
        /// </summary>
        /// <returns>A <see cref="JArray"/> upon successful reading of
        /// <see cref="Path"/>, otherwise <see langword="null"/>.</returns>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text.</exception>
        public JArray ReadAsJArray()
            => JsoncSerializerHelper.ReadAsJArray(ReadAsJToken());

        /// <summary>
        /// Reads the contents of the file at <see cref="Path"/> as a
        /// <see cref="JToken"/> by calling the <see cref="ReadAsJToken()"/>
        /// method and then calling
        /// <see cref="JsoncSerializerHelper.ReadAsJObject(JToken)"/>.
        /// </summary>
        /// <returns>A <see cref="JObject"/> upon successful reading of
        /// <see cref="Path"/>, otherwise <see langword="null"/>.</returns>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text.</exception>
        public JObject ReadAsJObject()
            => JsoncSerializerHelper.ReadAsJObject(ReadAsJToken());

        /// <summary>
        /// Reads the contents of the file at <see cref="Path"/>, by creating a
        /// <see cref="FileStream"/> with the
        /// <see cref="FileStream(string, FileMode, FileAccess, FileShare)"/>
        /// constructor and the following parameters:
        /// <list type="bullet">
        /// <item><see cref="Path"/></item>
        /// <item><see cref="FileMode.Open"/></item>
        /// <item><see cref="FileAccess.Read"/></item>
        /// <item><see cref="FileShare.None"/></item>
        /// </list>
        /// Then a <see cref="StreamReader"/> is created with the
        /// <see cref="StreamReader(Stream, Encoding)"/> constructor with the
        /// created <see cref="FileStream"/> and <see cref="Encoding"/>. And
        /// finally the <see cref="JToken"/> is retured with calling the
        /// <see cref="ReadAsJToken(TextReader, JsonLoadSettings)"/> method,
        /// with the created <see cref="StreamReader"/> and
        /// <see cref="LoadSettings"/>.
        /// </summary>
        /// <returns>A <see cref="JToken"/> upon reading of
        /// <see cref="Path"/>.</returns>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text.</exception>
        public JToken ReadAsJToken()
        {
            using (var fileStream = OpenFileForReading(Path))
            {
                using (var streamReader = new StreamReader(fileStream,
                    Encoding))
                {
                    return ReadAsJToken(streamReader, LoadSettings);
                }
            }
        }

        /// <summary>
        /// Reads the contents of the file at <see cref="Path"/> and returns it
        /// as a <see cref="string"/>.
        /// </summary>
        /// <returns></returns>
        public string ReadAsString()
        {
            using (var fileStream = OpenFileForReading(Path))
            {
                using (var streamReader = new StreamReader(fileStream,
                    Encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Reads the contents of the file at <see cref="Path"/> as a
        /// <see cref="JToken"/> by calling the <see cref="ReadAsJToken()"/>
        /// method and then calling
        /// <see cref="JsoncSerializerHelper.ReadAsJValue(JToken)"/>.
        /// </summary>
        /// <returns>A <see cref="JValue"/> upon successful reading of
        /// <see cref="Path"/>, otherwise <see langword="null"/>.</returns>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text.</exception>
        public JValue ReadAsJValue()
            => JsoncSerializerHelper.ReadAsJValue(ReadAsJToken());

        /// <summary>
        /// Reads from the file at <see cref="Path"/> as a <see cref="JToken"/>
        /// and updates <paramref name="preference"/> by calling the
        /// <see cref="PreferenceJsonDeserializer.UpdateFrom(Preference,
        /// JToken)"/> method.
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="writeIfFileNotFound">If <see langword="true"/> and the
        /// file specified by <see cref="Path"/> is not found, then the
        /// <see cref="Write(Preference)"/> method is called and
        /// <see langword="false"/> is returned. Otherwise, if
        /// <see langword="false"/> and the file specified by <see cref="Path"/>
        /// is not found, then the <see cref="FileNotFoundException"/> is
        /// thrown. The default is <see langword="true"/>.</param>
        /// <param name="writeOnReadException">If <see langword="true"/> and a
        /// <see cref="JsonReaderException"/> is thrown when the
        /// <see cref="ReadAsJToken()"/> method is called, then the
        /// <see cref="Write(Preference)"/> method is called and
        /// <see langword="false"/> is returned. Otherwise, if
        /// <see langword="false"/> and there is an error reading the file as
        /// JSON text, then the <see cref="JsonReaderException"/> is
        /// thrown. The default is <see langword="true"/>.</param>
        /// <param name="writeFileIfMissingItem">If <see langword="true"/> and
        /// the <see cref="Preference.Name"/> of <paramref name="preference"/>
        /// is missing from the file, then the file will be overwritten by
        /// calling the <see cref="Write(Preference)"/> method and
        /// <see langword="false"/> is returned. If <see langword="false"/> then
        /// <paramref name="preference"/> then the
        /// <see cref="PreferenceJsonDeserializer.UpdateFrom(Preference,
        /// JToken)"/> method is called.</param>
        /// <param name="writeAfterUpdate">After successfully updating, the file
        /// will be written over to ensure it has all of the correct
        /// comments and formatting.</param>
        /// <returns><see langword="true"/> if <paramref name="preference"/> was
        /// updated successfully from the file at <see cref="Path"/>; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found and
        /// <paramref name="writeIfFileNotFound"/> is
        /// <see langword="false"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text and <paramref name="writeOnReadException"/> is
        /// <see langword="false"/>.</exception>
        public bool Update(Preference preference,
            bool writeIfFileNotFound = true, bool writeOnReadException = true,
            bool writeFileIfMissingItem = true, bool writeAfterUpdate = true)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            try
            {
                var jToken = ReadAsJToken();

                if (writeFileIfMissingItem)
                {
                    if (jToken is null || jToken.Type == JTokenType.Null)
                    {
                        Write(preference);

                        return false;
                    }
                    else if (jToken.Type == JTokenType.Object)
                    {
                        var jObject = (JObject)jToken;

                        if (jObject is null || jObject.Type == JTokenType.Null)
                        {
                            Write(preference);

                            return false;
                        }

                        var processedName = Preference
                            .ProcessNameOrThrowIfInvalid(preference.Name);

                        if (!jObject.ContainsKey(processedName))
                        {
                            Write(preference);

                            return false;
                        }
                    }
                }

                var updated = PreferenceJsonDeserializer.UpdateFrom(preference,
                    jToken);

                if (writeAfterUpdate)
                {
                    Write(preference);
                }

                return updated;
            }
            catch (FileNotFoundException)
            {
                if (writeIfFileNotFound)
                {
                    Write(preference);

                    return false;
                }

                throw;
            }
            catch (JsonReaderException)
            {
                if (writeOnReadException)
                {
                    Write(preference);

                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads from the file at <see cref="Path"/> and updates
        /// <paramref name="group"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="writeIfFileNotFound">If <see langword="true"/> and the
        /// file specified by <see cref="Path"/> is not found, then the
        /// <see cref="Write(PreferenceGroup)"/> method is called and
        /// <see langword="false"/> is returned. Otherwise, if
        /// <see langword="null"/> and the file specified by <see cref="Path"/>
        /// is not found, then the <see cref="FileNotFoundException"/> is
        /// thrown. The default is <see langword="true"/>.</param>
        /// <param name="writeOnReadException">If <see langword="true"/> and a
        /// <see cref="JsonReaderException"/> is thrown when the
        /// <see cref="ReadAsJToken()"/> method is called, then the
        /// <see cref="Write(PreferenceGroup)"/> method is called and
        /// <see langword="null"/> is returned. Otherwise, if
        /// <see langword="false"/> and there is an error reading the file as
        /// JSON text, then the <see cref="JsonReaderException"/> is
        /// thrown. The default is <see langword="true"/>.</param>
        /// <param name="writeFileIfMissingNames">If <see langword="true"/> and
        /// a <see cref="Preference.Name"/> of a <see cref="Preference"/> in
        /// <paramref name="group"/> is missing from the file, then the file
        /// will be overwritten by calling the
        /// <see cref="Write(PreferenceGroup)"/>.</param>
        /// <param name="writeAfterUpdate">After successfully updating, the file
        /// will be written over to ensure it has all of the correct
        /// comments and formatting.</param>
        /// <returns>A <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the <see cref="Preference.Name"/>s that
        /// were updated from the file in <paramref name="group"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found and
        /// <paramref name="writeIfFileNotFound"/> is
        /// <see langword="false"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text.</exception>
        public IReadOnlyCollection<string> Update(PreferenceGroup group,
            bool writeIfFileNotFound = true, bool writeOnReadException = true,
            bool writeFileIfMissingNames = true, bool writeAfterUpdate = true)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            try
            {
                var jObject = ReadAsJObject();
                var namesOfUpdatedPrefences = PreferenceGroupJsonDeserializer
                    .UpdateFrom(group, jObject);

                if (writeFileIfMissingNames)
                {
                    foreach (var preferenceName in group.Names)
                    {
                        if (Preference.IsNameValid(preferenceName)
                            && !jObject.ContainsKey(preferenceName))
                        {
                            Write(group);

                            return namesOfUpdatedPrefences;
                        }
                    }
                }

                if (writeAfterUpdate)
                {
                    Write(group);
                }

                return namesOfUpdatedPrefences;
            }
            catch (FileNotFoundException)
            {
                if (writeIfFileNotFound)
                {
                    Write(group);

                    return null;
                }

                throw;
            }
            catch (JsonReaderException)
            {
                if (writeOnReadException)
                {
                    Write(group);

                    return null;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads from the file at <see cref="Path"/> and updates
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="writeIfFileNotFound">If <see langword="true"/> and the
        /// file specified by <see cref="Path"/> is not found, then the
        /// <see cref="Write(PreferenceGroup[])"/> method is called and
        /// <see langword="false"/> is returned. Otherwise, if
        /// <see langword="null"/> and the file specified by <see cref="Path"/>
        /// is not found, then the <see cref="FileNotFoundException"/> is
        /// thrown. The default is <see langword="true"/>.</param>
        /// <param name="writeOnReadException">If <see langword="true"/> and a
        /// <see cref="JsonReaderException"/> is thrown when the
        /// <see cref="ReadAsJToken()"/> method is called, then the
        /// <see cref="Write(PreferenceGroup[])"/> method is called and
        /// <see langword="null"/> is returned. Otherwise, if
        /// <see langword="false"/> and there is an error reading the file as
        /// JSON text, then the <see cref="JsonReaderException"/> is
        /// thrown. The default is <see langword="true"/>.</param>
        /// <param name="writeFileIfMissingItems"></param>
        /// <param name="writeAfterUpdate">After successfully updating, the file
        /// will be written over to ensure it has all of the correct
        /// comments and formatting.</param>
        /// <returns>A <see cref="IReadOnlyDictionary{TKey, TValue}"/> of
        /// <see cref="int"/> and <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the index of the array and the
        /// <see cref="Preference.Name"/>s that
        /// were updated from the file in <paramref name="groups"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="Path"/> cannot be found and
        /// <paramref name="writeIfFileNotFound"/> is
        /// <see langword="false"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// the file as JSON text.</exception>
        public IReadOnlyDictionary<int, IReadOnlyCollection<string>> Update(
            PreferenceGroup[] groups, bool writeIfFileNotFound = true,
            bool writeOnReadException = true,
            bool writeFileIfMissingItems = true, bool writeAfterUpdate = true)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            try
            {
                var jArray = ReadAsJArray();
                var updates = PreferenceGroupJsonDeserializer.UpdateFrom(groups,
                    jArray);

                if ((writeFileIfMissingItems && groups.Length > jArray.Count)
                    || writeAfterUpdate)
                {
                    Write(groups);
                }

                return updates;
            }
            catch (FileNotFoundException)
            {
                if (writeIfFileNotFound)
                {
                    Write(groups);

                    return null;
                }

                throw;
            }
            catch (JsonReaderException)
            {
                if (writeOnReadException)
                {
                    Write(groups);

                    return null;
                }

                throw;
            }
        }

        /// <summary>
        /// Writes <paramref name="preference"/> to the file at
        /// <see cref="Path"/>. If the file already exists, then it will be
        /// overwritten. It accomplishes this by creating a
        /// <see cref="FileStream"/> with the
        /// <see cref="FileStream(string, FileMode, FileAccess, FileShare)"/>
        /// constructor and the following parameters:
        /// <list type="bullet">
        /// <item><see cref="Path"/></item>
        /// <item><see cref="FileMode.Create"/></item>
        /// <item><see cref="FileAccess.Write"/></item>
        /// <item><see cref="FileShare.None"/></item>
        /// </list>
        /// Then a <see cref="StreamWriter"/> is created with the
        /// <see cref="StreamWriter(Stream, Encoding)"/> constructor with the
        /// created <see cref="FileStream"/> and <see cref="Encoding"/>. Then a
        /// <see cref="JsoncSerializationContext"/> is created with the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor with the created <see cref="StreamWriter"/> and
        /// <see cref="TabString"/>. And finally the
        /// <see cref="JsoncSerializationContext.Serialize(Preference)"/> and
        /// <see cref="JsoncSerializationContext.Flush()"/> methods are called.
        /// </summary>
        /// <param name="preference"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public void Write(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            using (var fileStream = OpenFileForWriting(Path))
            {
                using (var streamWriter = new StreamWriter(fileStream,
                    Encoding))
                {
                    using (var context = new JsoncSerializationContext(
                        streamWriter, TabString))
                    {
                        context.Serialize(preference);
                        context.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Writes <paramref name="group"/> to the file at
        /// <see cref="Path"/>. If the file already exists, then it will be
        /// overwritten. It accomplishes this by creating a
        /// <see cref="FileStream"/> with the
        /// <see cref="FileStream(string, FileMode, FileAccess, FileShare)"/>
        /// constructor and the following parameters:
        /// <list type="bullet">
        /// <item><see cref="Path"/></item>
        /// <item><see cref="FileMode.Create"/></item>
        /// <item><see cref="FileAccess.Write"/></item>
        /// <item><see cref="FileShare.None"/></item>
        /// </list>
        /// Then a <see cref="StreamWriter"/> is created with the
        /// <see cref="StreamWriter(Stream, Encoding)"/> constructor with the
        /// created <see cref="FileStream"/> and <see cref="Encoding"/>. Then a
        /// <see cref="JsoncSerializationContext"/> is created with the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor with the created <see cref="StreamWriter"/> and
        /// <see cref="TabString"/>. And finally the
        /// <see cref="JsoncSerializationContext.Serialize(PreferenceGroup)"/>
        /// and <see cref="JsoncSerializationContext.Flush()"/> methods are
        /// called.
        /// </summary>
        /// <param name="group"></param>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public void Write(PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            using (var fileStream = OpenFileForWriting(Path))
            {
                using (var streamWriter = new StreamWriter(fileStream,
                    Encoding))
                {
                    using (var context = new JsoncSerializationContext(
                        streamWriter, TabString))
                    {
                        context.Serialize(group);
                        context.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Writes <paramref name="groups"/> to the file at
        /// <see cref="Path"/>. If the file already exists, then it will be
        /// overwritten. It accomplishes this by creating a
        /// <see cref="FileStream"/> with the
        /// <see cref="FileStream(string, FileMode, FileAccess, FileShare)"/>
        /// constructor and the following parameters:
        /// <list type="bullet">
        /// <item><see cref="Path"/></item>
        /// <item><see cref="FileMode.Create"/></item>
        /// <item><see cref="FileAccess.Write"/></item>
        /// <item><see cref="FileShare.None"/></item>
        /// </list>
        /// Then a <see cref="StreamWriter"/> is created with the
        /// <see cref="StreamWriter(Stream, Encoding)"/> constructor with the
        /// created <see cref="FileStream"/> and <see cref="Encoding"/>. Then a
        /// <see cref="JsoncSerializationContext"/> is created with the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor with the created <see cref="StreamWriter"/> and
        /// <see cref="TabString"/>. And finally the
        /// <see cref="JsoncSerializationContext.Serialize(PreferenceGroup[])"/>
        /// and <see cref="JsoncSerializationContext.Flush()"/> methods are
        /// called.
        /// </summary>
        /// <param name="groups"></param>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public void Write(PreferenceGroup[] groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            using (var fileStream = OpenFileForWriting(Path))
            {
                using (var streamWriter = new StreamWriter(fileStream,
                    Encoding))
                {
                    using (var context = new JsoncSerializationContext(
                        streamWriter, TabString))
                    {
                        context.Serialize(groups);
                        context.Flush();
                    }
                }
            }
        }

        private FileStream OpenFileForReading(string path)
            => new FileStream(path, FileMode.Open, FileAccess.Read,
                FileShare.None);

        private FileStream OpenFileForWriting(string path)
            => new FileStream(path, FileMode.Create, FileAccess.Write,
                FileShare.None);

        /// <summary>
        /// Reads the contents of <paramref name="textReader"/> as a
        /// <see cref="JToken"/>. It does this by creating a
        /// <see cref="JsonTextReader"/> and using it to create a
        /// <see cref="JToken"/> by calling the
        /// <see cref="JToken.ReadFrom(JsonReader, JsonLoadSettings)"/> method
        /// with the created <see cref="JsonTextReader"/> and
        /// <paramref name="jsonLoadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used).
        /// </summary>
        /// <param name="textReader"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="JToken"/> upon successful reading of
        /// <paramref name="textReader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textReader"/> is <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="textReader"/> as JSON text.</exception>
        public static JToken ReadAsJToken(TextReader textReader,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                return JToken.ReadFrom(jsonTextReader, jsonLoadSettings
                    ?? JsoncSerializerHelper.DefaultLoadSettings);
            }
        }

        /// <summary>
        /// Reads the contents of <paramref name="string"/>, by calling the
        /// <see cref="ReadStringAsJToken(string, JsonLoadSettings)"/> method,
        /// with (where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used).
        /// Then the <see cref="JsoncSerializerHelper.ReadAsJArray(JToken)"/>
        /// method is called.
        /// </summary>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="JArray"/> upon successful reading of
        /// <paramref name="string"/>, otherwise
        /// <see langword="null"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/> is
        /// empty or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="string"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static JArray ReadStringAsJArray(string @string,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            return JsoncSerializerHelper.ReadAsJArray(
                ReadStringAsJToken(@string, jsonLoadSettings));
        }

        /// <summary>
        /// Reads the contents of <paramref name="string"/>, by calling the
        /// <see cref="ReadStringAsJToken(string, JsonLoadSettings)"/> method,
        /// with (where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used).
        /// Then the <see cref="JsoncSerializerHelper.ReadAsJObject(JToken)"/>
        /// method is called.
        /// </summary>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="JObject"/> upon successful reading of
        /// <paramref name="string"/>, otherwise
        /// <see langword="null"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/> is
        /// empty or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="string"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static JObject ReadStringAsJObject(string @string,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            return JsoncSerializerHelper.ReadAsJObject(
                ReadStringAsJToken(@string, jsonLoadSettings));
        }

        /// <summary>
        /// Reads the contents of <paramref name="string"/>, by creating a
        /// <see cref="StringReader"/>. The <see cref="JToken"/> is retured
        /// with calling the
        /// <see cref="ReadAsJToken(TextReader, JsonLoadSettings)"/> method,
        /// with the created <see cref="StringReader"/> and
        /// <paramref name="jsonLoadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used).
        /// </summary>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="JToken"/> upon reading of
        /// <see cref="Path"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/> is
        /// empty or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="string"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static JToken ReadStringAsJToken(string @string,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            using (var stringReader = new StringReader(@string))
            {
                return ReadAsJToken(stringReader, jsonLoadSettings);
            }
        }

        /// <summary>
        /// Reads the contents of <paramref name="string"/>, by calling the
        /// <see cref="ReadStringAsJToken(string, JsonLoadSettings)"/> method,
        /// with (where if it is <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used).
        /// Then the <see cref="JsoncSerializerHelper.ReadAsJObject(JToken)"/>
        /// method is called.
        /// </summary>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="JObject"/> upon successful reading of
        /// <paramref name="string"/>, otherwise
        /// <see langword="null"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/> is
        /// empty or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="string"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static JValue ReadStringAsJValue(string @string,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            return JsoncSerializerHelper.ReadAsJValue(
                ReadStringAsJToken(@string, jsonLoadSettings));
        }

        /// <summary>
        /// Updates <paramref name="preference"/> from <paramref name="string"/>
        /// by first calling the
        /// <see cref="ReadStringAsJToken(string, JsonLoadSettings)"/> method
        /// with it and <paramref name="jsonLoadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used),
        /// and then calling the
        /// <see cref="PreferenceJsonDeserializer.UpdateFrom(Preference,
        /// JToken)"/> method.
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns><see langword="true"/> if <paramref name="preference"/> was
        /// updated from <paramref name="string"/>, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> or  <paramref name="string"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static bool UpdateFromString(Preference preference,
            string @string, JsonLoadSettings jsonLoadSettings = null)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            var jToken = ReadStringAsJToken(@string, jsonLoadSettings);

            return PreferenceJsonDeserializer.UpdateFrom(preference, jToken);
        }

        /// <summary>
        /// Updates <paramref name="group"/> from <paramref name="string"/> by
        /// first calling the
        /// <see cref="ReadStringAsJObject(string, JsonLoadSettings)"/> method
        /// with it and <paramref name="jsonLoadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used),
        /// and then calling the
        /// <see cref="PreferenceGroupJsonDeserializer.UpdateFrom(
        /// PreferenceGroup, JObject)"/> method.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the <see cref="Preference.Name"/>s of
        /// <paramref name="group"/> that were updated from
        /// <paramref name="string"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="string"/> is <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static IReadOnlyCollection<string> UpdateFromString(
            PreferenceGroup group, string @string,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            var jObject = ReadStringAsJObject(@string, jsonLoadSettings);

            return PreferenceGroupJsonDeserializer.UpdateFrom(group, jObject);
        }

        /// <summary>
        /// Updates <paramref name="groups"/> from <paramref name="string"/> by
        /// first calling the
        /// <see cref="ReadStringAsJObject(string, JsonLoadSettings)"/> method
        /// with it and <paramref name="jsonLoadSettings"/> (where if it is
        /// <see langword="null"/> then
        /// <see cref="JsoncSerializerHelper.DefaultLoadSettings"/> is used),
        /// and then calling the
        /// <see cref="PreferenceGroupJsonDeserializer.UpdateFrom(
        /// PreferenceGroup[], JArray)"/> method.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="string"></param>
        /// <param name="jsonLoadSettings"></param>
        /// <returns>A <see cref="IReadOnlyDictionary{TKey, TValue}"/> of
        /// <see cref="int"/> and <see cref="IReadOnlyCollection{T}"/> of
        /// <see cref="string"/> with the index of the array and the
        /// <see cref="Preference.Name"/>s that
        /// were updated from the file in <paramref name="groups"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="string"/>
        /// is empty or constists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="string"/> is <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException">An error occured while reading
        /// <paramref name="string"/> as JSON text.</exception>
        public static IReadOnlyDictionary<int, IReadOnlyCollection<string>>
            UpdateFromString(PreferenceGroup[] groups, string @string,
            JsonLoadSettings jsonLoadSettings = null)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            if (@string is null)
            {
                throw new ArgumentNullException(nameof(@string));
            }

            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Cannot be empty or consist only "
                    + "of white-space characters.", nameof(@string));
            }

            var jArray = ReadStringAsJArray(@string, jsonLoadSettings);

            return PreferenceGroupJsonDeserializer.UpdateFrom(groups, jArray);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="preference"/>
        /// serialized to JSONC by getting the formatting tab string by calling
        /// the <see cref="JsoncSerializerHelper.GetTabString(char, int)"/>
        /// method with <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/>. Then the result of calling the
        /// <see cref="WriteToString(string, Preference)"/> is returned.
        /// </summary>
        /// <param name="indentChar">The indent character for the tab string
        /// that will be repeated <paramref name="indentDepth"/> number of
        /// times.</param>
        /// <param name="indentDepth">The number of times to repeat
        /// <paramref name="indentChar"/> for the tab string. Must not be a
        /// negative number and if it is zero then <see cref="string.Empty"/> is
        /// returned.</param>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public static string WriteToString(char indentChar, int indentDepth,
            Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (indentDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indentDepth),
                    indentDepth, "Cannot be negative.");
            }

            var tabString = JsoncSerializerHelper.GetTabString(indentChar,
                indentDepth);

            return WriteToString(tabString, preference);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="group"/>
        /// serialized to JSONC by getting the formatting tab string by calling
        /// the <see cref="JsoncSerializerHelper.GetTabString(char, int)"/>
        /// method with <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/>. Then the result of calling the
        /// <see cref="WriteToString(string, PreferenceGroup)"/> is returned.
        /// </summary>
        /// <param name="indentChar">The indent character for the tab string
        /// that will be repeated <paramref name="indentDepth"/> number of
        /// times.</param>
        /// <param name="indentDepth">The number of times to repeat
        /// <paramref name="indentChar"/> for the tab string. Must not be a
        /// negative number and if it is zero then <see cref="string.Empty"/> is
        /// returned.</param>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public static string WriteToString(char indentChar, int indentDepth,
            PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (indentDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indentDepth),
                    indentDepth, "Cannot be negative.");
            }

            return WriteToString(
                JsoncSerializerHelper.GetTabString(indentChar, indentDepth),
                group);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="groups"/>
        /// serialized to JSONC by getting the formatting tab string by calling
        /// the <see cref="JsoncSerializerHelper.GetTabString(char, int)"/>
        /// method with <paramref name="indentChar"/> and
        /// <paramref name="indentDepth"/>. Then the result of calling the
        /// <see cref="WriteToString(string, PreferenceGroup[])"/> is returned.
        /// </summary>
        /// <param name="indentChar">The indent character for the tab string
        /// that will be repeated <paramref name="indentDepth"/> number of
        /// times.</param>
        /// <param name="indentDepth">The number of times to repeat
        /// <paramref name="indentChar"/> for the tab string. Must not be a
        /// negative number and if it is zero then <see cref="string.Empty"/> is
        /// returned.</param>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="indentDepth"/> is negative.</exception>
        public static string WriteToString(char indentChar, int indentDepth,
            PreferenceGroup[] groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            if (indentDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indentDepth),
                    indentDepth, "Cannot be negative.");
            }

            return WriteToString(
                JsoncSerializerHelper.GetTabString(indentChar, indentDepth),
                groups);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="preference"/>
        /// serialized to JSONC by creating a <see cref="StringWriter"/>. Then a
        /// <see cref="JsoncSerializationContext"/> is created with the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor with the created <see cref="StringWriter"/> and
        /// <paramref name="tabString"/>. And then the
        /// <see cref="JsoncSerializationContext.Serialize(Preference)"/> and
        /// <see cref="JsoncSerializationContext.Flush()"/> methods are called.
        /// Lastly, the result of <see cref="StringWriter.ToString()"/> with
        /// the created <see cref="StringWriter"/> is returned.
        /// </summary>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="tabString"/>
        /// or <paramref name="preference"/> is
        /// <see langword="null"/>.</exception>
        public static string WriteToString(string tabString,
            Preference preference)
        {
            if (tabString is null)
            {
                throw new ArgumentNullException(nameof(tabString));
            }

            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            using (var stringWriter = new StringWriter())
            {
                using (var context = new JsoncSerializationContext(stringWriter,
                    tabString))
                {
                    context.Serialize(preference);
                    context.Flush();
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="group"/>
        /// serialized to JSONC by creating a <see cref="StringWriter"/>. Then a
        /// <see cref="JsoncSerializationContext"/> is created with the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor with the created <see cref="StringWriter"/> and
        /// <paramref name="tabString"/>. And then the
        /// <see cref="JsoncSerializationContext.Serialize(PreferenceGroup)"/>
        /// and <see cref="JsoncSerializationContext.Flush()"/> methods are
        /// called. Lastly, the result of <see cref="StringWriter.ToString()"/>
        /// with the created <see cref="StringWriter"/> is returned.
        /// </summary>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="tabString"/>
        /// or <paramref name="group"/> is <see langword="null"/>.</exception>
        public static string WriteToString(string tabString,
            PreferenceGroup group)
        {
            if (tabString is null)
            {
                throw new ArgumentNullException(nameof(tabString));
            }

            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            using (var stringWriter = new StringWriter())
            {
                using (var context = new JsoncSerializationContext(stringWriter,
                    tabString))
                {
                    context.Serialize(group);
                    context.Flush();
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="groups"/>
        /// serialized to JSONC by creating a <see cref="StringWriter"/>. Then a
        /// <see cref="JsoncSerializationContext"/> is created with the
        /// <see cref="JsoncSerializationContext(TextWriter, string)"/>
        /// constructor with the created <see cref="StringWriter"/> and
        /// <paramref name="tabString"/>. And then the
        /// <see cref="JsoncSerializationContext.Serialize(PreferenceGroup[])"/>
        /// and <see cref="JsoncSerializationContext.Flush()"/> methods are
        /// called. Lastly, the result of <see cref="StringWriter.ToString()"/>
        /// with the created <see cref="StringWriter"/> is returned.
        /// </summary>
        /// <param name="tabString">Used for each indentation level of
        /// formatting.</param>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="tabString"/>
        /// or <paramref name="groups"/> is <see langword="null"/>.</exception>
        public static string WriteToString(string tabString,
            PreferenceGroup[] groups)
        {
            if (tabString is null)
            {
                throw new ArgumentNullException(nameof(tabString));
            }

            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            using (var stringWriter = new StringWriter())
            {
                using (var context = new JsoncSerializationContext(stringWriter,
                    tabString))
                {
                    context.Serialize(groups);
                    context.Flush();
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="preference"/>
        /// serialized to JSONC by returning the result of calling the
        /// <see cref="WriteToString(string, Preference)"/> with
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/> and
        /// <paramref name="preference"/>.
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static string WriteToString(Preference preference)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            return WriteToString(JsoncSerializerHelper.DefaultTabString,
                preference);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="group"/>
        /// serialized to JSONC by returning the result of calling the
        /// <see cref="WriteToString(string, PreferenceGroup)"/> with
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/> and
        /// <paramref name="group"/>.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="group"/> is
        /// <see langword="null"/>.</exception>
        public static string WriteToString(PreferenceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return WriteToString(JsoncSerializerHelper.DefaultTabString, group);
        }

        /// <summary>
        /// Returns a <see cref="string"/> with <paramref name="groups"/>
        /// serialized to JSONC by returning the result of calling the
        /// <see cref="WriteToString(string, PreferenceGroup)"/> with
        /// <see cref="JsoncSerializerHelper.DefaultTabString"/> and
        /// <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/> is
        /// <see langword="null"/>.</exception>
        public static string WriteToString(PreferenceGroup[] groups)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            return WriteToString(JsoncSerializerHelper.DefaultTabString,
                groups);
        }
    }
}
