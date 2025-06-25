using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreferenceGroups.Tests
{
    internal class EmptyNamePreference(string name) : Preference(name)
    {
        public override string Name => string.Empty;

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
