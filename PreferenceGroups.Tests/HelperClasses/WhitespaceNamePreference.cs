using System;
using System.Collections.Generic;

namespace PreferenceGroups.Tests.HelperClasses
{
    internal class WhitespaceNamePreference(string name) : Preference(name)
    {
        public override string Name => "\t";

        public override object[]? GetAllowedValuesAsObjects() => null;

        public override string[]? GetAllowedValuesAsStrings(string format,
            IFormatProvider formatProvider)
            => null;

        public override object? GetDefaultValueAsObject()
            => null;

        public override string? GetDefaultValueAsString(string format,
            IFormatProvider formatProvider)
            => null;

        public override object? GetValueAsObject()
            => null;

        public override string? GetValueAsString(string? format,
            IFormatProvider? formatProvider)
            => null;

        public override Type GetValueType()
            => typeof(object);

        public override void SetDefaultValueFromObject(object? defaultValue)
        { }

        public override void SetValueFromObject(object? value)
        { }
    }
}
