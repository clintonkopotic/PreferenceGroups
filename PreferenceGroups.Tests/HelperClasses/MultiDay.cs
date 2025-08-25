using System;

namespace PreferenceGroups.Tests.HelperClasses
{
    [Flags]
    public enum MultiDay
    {
        None = 0x00,
        Sunday = 0x01,
        Monday = 0x02,
        Tuesday = 0x04,
        Wednesday = 0x08,
        Thursday = 0x10,
        Friday = 0x20,
        Saturday = 0x40,
        Weekend = Sunday | Saturday,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Week = Weekend | Weekdays,
    }
}
