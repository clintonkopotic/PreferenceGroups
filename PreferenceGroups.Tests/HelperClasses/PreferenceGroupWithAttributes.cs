namespace PreferenceGroups.Tests.HelperClasses;

[PreferenceGroup(Description = GroupDescription)]
internal class PreferenceGroupWithAttributes
{
    public const string GroupDescription = "A group of preferences.";

    public const bool BooleanDefaultValue = false;

    public const int Int32DefaultValue = 4;

    public const string Int32Description = "An integer.";

    public const string StringName = "String123";

    public const string StringDefaultValue = "";

    public const string StringDescription = "A string.";

    public const string EnumName = "MultiDayEnum";

    public const string NullableEnumName = "SingleDayEnum";

    [Preference(Description = Int32Description,
        DefaultValue = Int32DefaultValue)]
    public int? Int32 { get; set; } = null;

    [Preference(StringName, Description = StringDescription,
        DefaultValue = StringDefaultValue)]
    public string? String { get; set; } = null;

    [Preference]
    public bool Boolean { get; set; } = BooleanDefaultValue;

    [Preference(name: EnumName, allowUndefinedValues: true,
        MultiDay.Sunday, MultiDay.Monday, Description = "Testing")]
    public MultiDay MultiDay { get; set; } = MultiDay.None;

    [Preference(name: NullableEnumName)]
    public SingleDay? SingleDay { get; set; } = HelperClasses.SingleDay.None;
}
