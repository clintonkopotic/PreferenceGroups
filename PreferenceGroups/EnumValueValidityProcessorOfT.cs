using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties for common
    /// <see cref="StructValueValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValueValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValueValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class EnumValueValidityProcessor<TEnum>
        : StructValueValidityProcessor<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// defined and not zero.
        /// </summary>
        public static EnumValueValidityProcessor<TEnum> IsDefinedAndNotZero
            => new EnumValueValidityProcessor<TEnum>()
            {
                IsValid = enumValue => new StructValueValidityResult<TEnum>(
                    enumValue.GetExceptionIfNotDefinedOrIsZero(
                        nameof(enumValue)))
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// defined.
        /// </summary>
        public static EnumValueValidityProcessor<TEnum> IsDefined
            => new EnumValueValidityProcessor<TEnum>()
            {
                IsValid = enumValue => new StructValueValidityResult<TEnum>(
                    enumValue.GetExceptionIfNotDefined(nameof(enumValue)))
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is zero.
        /// </summary>
        public static EnumValueValidityProcessor<TEnum> NotZero
            => new EnumValueValidityProcessor<TEnum>()
            {
                IsValid = enumValue => new StructValueValidityResult<TEnum>(
                    enumValue.GetExceptionIfZero(nameof(enumValue)))
            };
    }
}
