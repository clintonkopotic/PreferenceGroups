using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties and methods for common
    /// <see cref="StructValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class UInt32ValidityProcessor : StructValidityProcessor<uint>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than one.
        /// </summary>
        public static UInt32ValidityProcessor IsGreaterThanOne
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 1u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to one."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal one.
        /// </summary>
        public static UInt32ValidityProcessor IsGreaterThanOrEqualToOne
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 1u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than one."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal to zero.
        /// </summary>
        public static UInt32ValidityProcessor IsGreaterThanOrEqualToZero
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 0u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than zero."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than zero.
        /// </summary>
        public static UInt32ValidityProcessor IsGreaterThanZero
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 0u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to zero."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than one.
        /// </summary>
        public static UInt32ValidityProcessor IsLessThanOne
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 1u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to one."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to one.
        /// </summary>
        public static UInt32ValidityProcessor IsLessThanOrEqualToOne
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 1u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than one."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to zero.
        /// </summary>
        public static UInt32ValidityProcessor IsLessThanOrEqualToZero
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 0u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than zero."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than zero.
        /// </summary>
        public static UInt32ValidityProcessor IsLessThanZero
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 0u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to zero."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to one.
        /// </summary>
        public static UInt32ValidityProcessor IsOne
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 1u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to one."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to zero.
        /// </summary>
        public static UInt32ValidityProcessor IsZero
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 0u)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to zero."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// equal to <paramref name="other"/>, including if both are
        /// <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static UInt32ValidityProcessor IsEqualTo(uint? other)
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<uint>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// greater than <paramref name="other"/>, including if both are
        /// <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static UInt32ValidityProcessor IsGreaterThan(uint? other)
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<uint>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value <= other.Value)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to other."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// greater than or equal to <paramref name="other"/>, including if both
        /// are <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static UInt32ValidityProcessor IsGreaterThanOrEqualTo(
            uint? other) => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<uint>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value < other.Value)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than other."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// less than <paramref name="other"/>, including if both are
        /// <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static UInt32ValidityProcessor IsLessThan(uint? other)
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<uint>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value >= other.Value)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// less than or equal to <paramref name="other"/>, including if both
        /// are <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static UInt32ValidityProcessor IsLessThanOrEqualTo(
            uint? other) => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<uint>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value > other.Value)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than other."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not equal to <paramref name="other"/>, including if both are
        /// <see langword="null"/>. If either the <c>value</c> or
        /// <paramref name="other"/> is <see langword="null"/> while the other
        /// one is not, then a
        /// <see cref="StructValidityResult{T}.NotValid(Exception)"/> is
        /// called with an <see cref="ArgumentNullException"/> indicating which
        /// parameter is <see langword="null"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static UInt32ValidityProcessor IsNotEqualTo(uint? other)
            => new UInt32ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<uint>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValidityResult<uint>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<uint>.IsValid();
                },
            };
    }
}
