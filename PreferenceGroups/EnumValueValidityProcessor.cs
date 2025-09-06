using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties for common
    /// <see cref="ClassValueValidityProcessor{T}.Pre"/> and
    /// <see cref="ClassValueValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="ClassValueValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class EnumValueValidityProcessor : ClassValueValidityProcessor<Enum>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// defined and not zero.
        /// </summary>
        public static ClassValueValidityProcessor<Enum> IsDefinedAndNotZero
            => new ClassValueValidityProcessor<Enum>()
            {
                IsValid = enumValue => new ClassValueValidityResult<Enum>(
                    EnumHelpers.GetExceptionIfNotDefinedOrIsZero(enumValue,
                        nameof(enumValue)))
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// defined.
        /// </summary>
        public static ClassValueValidityProcessor<Enum> IsDefined
            => new ClassValueValidityProcessor<Enum>()
            {
                IsValid = enumValue => new ClassValueValidityResult<Enum>(
                    EnumHelpers.GetExceptionIfNotDefined(enumValue,
                        nameof(enumValue)))
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is zero.
        /// </summary>
        public static ClassValueValidityProcessor<Enum> NotZero
            => new ClassValueValidityProcessor<Enum>()
            {
                IsValid = enumValue => new ClassValueValidityResult<Enum>(
                    EnumHelpers.GetExceptionIfZero(enumValue,
                        nameof(enumValue)))
            };
    }
}
