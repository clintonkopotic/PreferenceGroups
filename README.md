# PreferenceGroups

The `PreferenceGroups` package is a library for user preferences, orgainzed by groups of primative types, with metadata like a description of the prefence and utilizes a file store using the [JSON with comments file format](https://jsonc.org/) (the `*.jsonc` file extension). The motivation is to better encapsulate preferences so from the developer implementing them, to the user being presented with them, and to the file store with comments, the context is preserved throughout.

A `Preference` is the the basic unit of the library and it has an associated `Name` and `Value`, along with some additional metadata like what the `DefaultValue` is. A `PreferenceGroup` is a dictionary of `Preference`s where the key is the `Name` of the prefenence. A `PreferenceStore` is a generic dictionary of `PreferenceStoreItem`s, where a `PreferenceStoreItem` can be a `Preference`, `PreferenceGroup`, `PreferenceStore`, an array of `PreferenceGroup`, or an array of `PreferenceStore`. This allows for a `PreferenceGroup` to be used in plugin architechtures, such that a plugin developer could define a group in a structured way and understand the behaivor that group will be implemented to the user. Different groups could be provided for expected plugins to provide.

## Usage

A basic `PreferenceGroup` could be instantiated with:

```csharp
using PreferenceGroups;

PreferenceGroup group = PreferenceGroupBuilder
    .Create()
    .AddInt32(name: "Number", b => b
        .WithDefaultValue(13))
    .AddString(name: "String", b => b
        .WithDescription("A string prefence."))
    .Build();

PreferenceFile file = new(path: "Preferences.jsonc",
    indentChar: ' ', indentDepth: 2);

// Writes group to the file, overwriting it if exists.
file.Write(group);

// Updates group according to the contents of the file.
file.Update(group);
```

The contents of the `Preferences.jsonc` file will be:

```jsonc
{
  // Default value: 13.
  "Number": null,

  // A string prefence.
  "String": null
}
```
