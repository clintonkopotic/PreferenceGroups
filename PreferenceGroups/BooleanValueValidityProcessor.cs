using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> methods for common
    /// <see cref="StructValueValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValueValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValueValidityProcessor{T}.IsValid"/> validity checks
    /// for a <see cref="BooleanPreference"/>.
    /// </summary>
    public class BooleanValueValidityProcessor
        : StructValueValidityProcessor<bool>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to <see langword="false"/>.
        /// </summary>
        public static BooleanValueValidityProcessor IsFalse
            => new BooleanValueValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != false)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to false."));
                    }

                    return StructValueValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and not equal to <see langword="false"/>.
        /// </summary>
        public static BooleanValueValidityProcessor IsNotFalse
            => new BooleanValueValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value == false)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is equal to false."));
                    }

                    return StructValueValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and not equal to <see langword="true"/>.
        /// </summary>
        public static BooleanValueValidityProcessor IsNotTrue
            => new BooleanValueValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value == true)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is equal to true."));
                    }

                    return StructValueValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to <see langword="true"/>.
        /// </summary>
        public static BooleanValueValidityProcessor IsTrue
            => new BooleanValueValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != true)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to one."));
                    }

                    return StructValueValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// equal to <paramref name="other"/>, including if both are
        /// <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValueValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static BooleanValueValidityProcessor IsEqualTo(bool? other)
            => new BooleanValueValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValueValidityResult<bool>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValueValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValueValidityProcessor{T}.IsValid"/> if it is
        /// not equal to <paramref name="other"/>, including if both are
        /// <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValueValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static BooleanValueValidityProcessor IsNotEqualTo(bool? other)
            => new BooleanValueValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValueValidityResult<bool>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValueValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValueValidityResult<bool>.IsValid();
                },
            };
    }
}
