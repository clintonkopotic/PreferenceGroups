using System;
using System.Collections.Generic;
using System.Text;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> properties and methods for common
    /// <see cref="StructValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class Int64ValidityProcessor : StructValidityProcessor<long>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than one.
        /// </summary>
        public static Int64ValidityProcessor IsGreaterThanOne
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 1L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to one."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal one.
        /// </summary>
        public static Int64ValidityProcessor IsGreaterThanOrEqualToOne
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 1L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than one."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than or equal to zero.
        /// </summary>
        public static Int64ValidityProcessor IsGreaterThanOrEqualToZero
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value < 0L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than zero."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and greater than zero.
        /// </summary>
        public static Int64ValidityProcessor IsGreaterThanZero
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value <= 0L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to zero."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than one.
        /// </summary>
        public static Int64ValidityProcessor IsLessThanOne
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 1L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to one."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to one.
        /// </summary>
        public static Int64ValidityProcessor IsLessThanOrEqualToOne
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 1L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than one."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than or equal to zero.
        /// </summary>
        public static Int64ValidityProcessor IsLessThanOrEqualToZero
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value > 0L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than zero."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and less than zero.
        /// </summary>
        public static Int64ValidityProcessor IsLessThanZero
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value >= 0L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to zero."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to one.
        /// </summary>
        public static Int64ValidityProcessor IsOne
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 1L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to one."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to zero.
        /// </summary>
        public static Int64ValidityProcessor IsZero
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != 0L)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to zero."));
                    }

                    return StructValidityResult<long>.IsValid();
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
        public static Int64ValidityProcessor IsEqualTo(long? other)
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<long>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValidityResult<long>.IsValid();
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
        public static Int64ValidityProcessor IsGreaterThan(long? other)
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<long>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value <= other.Value)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than or equal to other."));
                    }

                    return StructValidityResult<long>.IsValid();
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
        public static Int64ValidityProcessor IsGreaterThanOrEqualTo(
            long? other) => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<long>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value < other.Value)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is less than other."));
                    }

                    return StructValidityResult<long>.IsValid();
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
        public static Int64ValidityProcessor IsLessThan(long? other)
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<long>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value >= other.Value)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<long>.IsValid();
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
        public static Int64ValidityProcessor IsLessThanOrEqualTo(
            long? other) => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<long>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value > other.Value)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than other."));
                    }

                    return StructValidityResult<long>.IsValid();
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
        public static Int64ValidityProcessor IsNotEqualTo(long? other)
            => new Int64ValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<long>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValidityResult<long>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<long>.IsValid();
                },
            };
    }
}
