using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties for common
    /// <see cref="ClassValidityProcessor{T}.Pre"/> and
    /// <see cref="ClassValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="ClassValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class EnumValidityProcessor : ClassValidityProcessor<Enum>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="ClassValidityProcessor{T}.IsValid"/> if it is
        /// defined and not zero.
        /// </summary>
        public static ClassValidityProcessor<Enum> IsDefinedAndNotZero
            => new ClassValidityProcessor<Enum>()
            {
                IsValid = enumValue => new ClassValidityResult<Enum>(
                    EnumHelpers.GetExceptionIfNotDefinedOrIsZero(enumValue,
                        nameof(enumValue)))
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="ClassValidityProcessor{T}.IsValid"/> if it is
        /// defined.
        /// </summary>
        public static ClassValidityProcessor<Enum> IsDefined
            => new ClassValidityProcessor<Enum>()
            {
                IsValid = enumValue => new ClassValidityResult<Enum>(
                    EnumHelpers.GetExceptionIfNotDefined(enumValue,
                        nameof(enumValue)))
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="ClassValidityProcessor{T}.IsValid"/> if it is zero.
        /// </summary>
        public static ClassValidityProcessor<Enum> NotZero
            => new ClassValidityProcessor<Enum>()
            {
                IsValid = enumValue => new ClassValidityResult<Enum>(
                    EnumHelpers.GetExceptionIfZero(enumValue,
                        nameof(enumValue)))
            };
    }
}
