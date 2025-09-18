using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties and methods for common
    /// <see cref="StructValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class TimeSpanValidityProcessor : StructValidityProcessor<TimeSpan>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal to zero.
        /// </summary>
        public static TimeSpanValidityProcessor IsGreaterThanOrEqualToZero
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < TimeSpan.Zero)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than zero."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than zero.
        /// </summary>
        public static TimeSpanValidityProcessor IsGreaterThanZero
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= TimeSpan.Zero)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to zero."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to zero.
        /// </summary>
        public static TimeSpanValidityProcessor IsLessThanOrEqualToZero
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > TimeSpan.Zero)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than zero."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than zero.
        /// </summary>
        public static TimeSpanValidityProcessor IsLessThanZero
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= TimeSpan.Zero)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to zero."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to zero.
        /// </summary>
        public static TimeSpanValidityProcessor IsZero
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != TimeSpan.Zero)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to zero."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
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
        public static TimeSpanValidityProcessor IsEqualTo(TimeSpan? other)
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<TimeSpan>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
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
        public static TimeSpanValidityProcessor IsGreaterThan(TimeSpan? other)
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<TimeSpan>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value <= other.Value)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to other."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
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
        public static TimeSpanValidityProcessor IsGreaterThanOrEqualTo(
            TimeSpan? other) => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<TimeSpan>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value < other.Value)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than other."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
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
        public static TimeSpanValidityProcessor IsLessThan(TimeSpan? other)
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<TimeSpan>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value >= other.Value)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
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
        public static TimeSpanValidityProcessor IsLessThanOrEqualTo(
            TimeSpan? other) => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<TimeSpan>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value > other.Value)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than other."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
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
        public static TimeSpanValidityProcessor IsNotEqualTo(TimeSpan? other)
            => new TimeSpanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<TimeSpan>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValidityResult<TimeSpan>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<TimeSpan>.IsValid();
                },
            };
    }
}
