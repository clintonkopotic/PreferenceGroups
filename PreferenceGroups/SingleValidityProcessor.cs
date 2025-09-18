using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties and methods for common
    /// <see cref="StructValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class SingleValidityProcessor : StructValidityProcessor<float>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is a finite number. A finite number
        /// is not NaN (see <see cref="float.IsNaN(float)"/>) and is not
        /// infinity (see <see cref="float.IsInfinity(float)"/>).
        /// </summary>
        public static SingleValidityProcessor IsFinite
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (float.IsNaN(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not a number."));
                    }
                    else if (float.IsNegativeInfinity(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not negative infinity."));
                    }
                    else if (float.IsPositiveInfinity(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not positive infinity."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than one.
        /// </summary>
        public static SingleValidityProcessor IsGreaterThanOne
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 1f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to one."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal one.
        /// </summary>
        public static SingleValidityProcessor IsGreaterThanOrEqualToOne
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 1f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than one."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal to zero.
        /// </summary>
        public static SingleValidityProcessor IsGreaterThanOrEqualToZero
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 0f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than zero."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than zero.
        /// </summary>
        public static SingleValidityProcessor IsGreaterThanZero
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 0f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to zero."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is negative or positive
        /// infinity (see <see cref="float.IsInfinity(float)"/>).
        /// </summary>
        public static SingleValidityProcessor IsInfinity
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!float.IsInfinity(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not negative or positve "
                                    + "infinity."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than one.
        /// </summary>
        public static SingleValidityProcessor IsLessThanOne
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 1f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to one."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to one.
        /// </summary>
        public static SingleValidityProcessor IsLessThanOrEqualToOne
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 1f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than one."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to zero.
        /// </summary>
        public static SingleValidityProcessor IsLessThanOrEqualToZero
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 0f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than zero."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than zero.
        /// </summary>
        public static SingleValidityProcessor IsLessThanZero
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 0f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to zero."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is not a number (see
        /// <see cref="float.IsNaN(float)"/>).
        /// </summary>
        public static SingleValidityProcessor IsNaN
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!float.IsNaN(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not NaN."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is negative infinity (see
        /// <see cref="float.IsNegativeInfinity(float)"/>).
        /// </summary>
        public static SingleValidityProcessor IsNegativeInfinity
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!float.IsInfinity(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not negative infinity."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to one.
        /// </summary>
        public static SingleValidityProcessor IsOne
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 1f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to one."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is positve infinity (see
        /// <see cref="float.IsPositiveInfinity(float)"/>).
        /// </summary>
        public static SingleValidityProcessor IsPositiveInfinity
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!float.IsPositiveInfinity(value.Value))
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not positive infinity."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to zero.
        /// </summary>
        public static SingleValidityProcessor IsZero
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 0f)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to zero."));
                    }

                    return StructValidityResult<float>.IsValid();
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
        public static SingleValidityProcessor IsEqualTo(float? other)
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<float>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValidityResult<float>.IsValid();
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
        public static SingleValidityProcessor IsGreaterThan(float? other)
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<float>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value <= other.Value)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to other."));
                    }

                    return StructValidityResult<float>.IsValid();
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
        public static SingleValidityProcessor IsGreaterThanOrEqualTo(
            float? other) => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<float>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value < other.Value)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than other."));
                    }

                    return StructValidityResult<float>.IsValid();
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
        public static SingleValidityProcessor IsLessThan(float? other)
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<float>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value >= other.Value)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<float>.IsValid();
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
        public static SingleValidityProcessor IsLessThanOrEqualTo(
            float? other) => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<float>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value > other.Value)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than other."));
                    }

                    return StructValidityResult<float>.IsValid();
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
        public static SingleValidityProcessor IsNotEqualTo(float? other)
            => new SingleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<float>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValidityResult<float>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<float>.IsValid();
                },
            };
    }
}
