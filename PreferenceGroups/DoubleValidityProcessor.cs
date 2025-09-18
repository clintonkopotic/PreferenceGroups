using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties and methods for common
    /// <see cref="StructValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class DoubleValidityProcessor : StructValidityProcessor<double>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is a finite number. A finite number
        /// is not NaN (see <see cref="double.IsNaN(double)"/>) and is not
        /// infinity (see <see cref="double.IsInfinity(double)"/>).
        /// </summary>
        public static DoubleValidityProcessor IsFinite
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (double.IsNaN(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not a number."));
                    }
                    else if (double.IsNegativeInfinity(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not negative infinity."));
                    }
                    else if (double.IsPositiveInfinity(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not positive infinity."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than one.
        /// </summary>
        public static DoubleValidityProcessor IsGreaterThanOne
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 1d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to one."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal one.
        /// </summary>
        public static DoubleValidityProcessor IsGreaterThanOrEqualToOne
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 1d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than one."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal to zero.
        /// </summary>
        public static DoubleValidityProcessor IsGreaterThanOrEqualToZero
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 0d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than zero."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than zero.
        /// </summary>
        public static DoubleValidityProcessor IsGreaterThanZero
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 0d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to zero."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is negative or positive
        /// infinity (see <see cref="double.IsInfinity(double)"/>).
        /// </summary>
        public static DoubleValidityProcessor IsInfinity
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!double.IsInfinity(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not negative or positve "
                                    + "infinity."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than one.
        /// </summary>
        public static DoubleValidityProcessor IsLessThanOne
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 1d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to one."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to one.
        /// </summary>
        public static DoubleValidityProcessor IsLessThanOrEqualToOne
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 1d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than one."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to zero.
        /// </summary>
        public static DoubleValidityProcessor IsLessThanOrEqualToZero
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 0d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than zero."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than zero.
        /// </summary>
        public static DoubleValidityProcessor IsLessThanZero
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 0d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to zero."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is not a number (see
        /// <see cref="double.IsNaN(double)"/>).
        /// </summary>
        public static DoubleValidityProcessor IsNaN
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!double.IsNaN(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not NaN."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is negative infinity (see
        /// <see cref="double.IsNegativeInfinity(double)"/>).
        /// </summary>
        public static DoubleValidityProcessor IsNegativeInfinity
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!double.IsInfinity(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not negative infinity."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to one.
        /// </summary>
        public static DoubleValidityProcessor IsOne
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 1d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to one."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and is positve infinity (see
        /// <see cref="double.IsPositiveInfinity(double)"/>).
        /// </summary>
        public static DoubleValidityProcessor IsPositiveInfinity
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (!double.IsPositiveInfinity(value.Value))
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not positive infinity."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to zero.
        /// </summary>
        public static DoubleValidityProcessor IsZero
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 0d)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to zero."));
                    }

                    return StructValidityResult<double>.IsValid();
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
        public static DoubleValidityProcessor IsEqualTo(double? other)
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<double>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValidityResult<double>.IsValid();
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
        public static DoubleValidityProcessor IsGreaterThan(double? other)
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<double>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value <= other.Value)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to other."));
                    }

                    return StructValidityResult<double>.IsValid();
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
        public static DoubleValidityProcessor IsGreaterThanOrEqualTo(
            double? other) => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<double>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value < other.Value)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than other."));
                    }

                    return StructValidityResult<double>.IsValid();
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
        public static DoubleValidityProcessor IsLessThan(double? other)
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<double>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value >= other.Value)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<double>.IsValid();
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
        public static DoubleValidityProcessor IsLessThanOrEqualTo(
            double? other) => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<double>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value > other.Value)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than other."));
                    }

                    return StructValidityResult<double>.IsValid();
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
        public static DoubleValidityProcessor IsNotEqualTo(double? other)
            => new DoubleValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<double>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValidityResult<double>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<double>.IsValid();
                },
            };
    }
}
