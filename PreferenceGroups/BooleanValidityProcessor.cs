using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> methods for common
    /// <see cref="StructValidityProcessor{T}.Pre"/> and
    /// <see cref="StructValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="StructValidityProcessor{T}.IsValid"/> validity checks
    /// for a <see cref="BooleanPreference"/>.
    /// </summary>
    public class BooleanValidityProcessor : StructValidityProcessor<bool>
    {
        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to <see langword="false"/>.
        /// </summary>
        public static BooleanValidityProcessor IsFalse
            => new BooleanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != false)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to false."));
                    }

                    return StructValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and not equal to <see langword="false"/>.
        /// </summary>
        public static BooleanValidityProcessor IsNotFalse
            => new BooleanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value == false)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is equal to false."));
                    }

                    return StructValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and not equal to <see langword="true"/>.
        /// </summary>
        public static BooleanValidityProcessor IsNotTrue
            => new BooleanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value == true)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is equal to true."));
                    }

                    return StructValidityResult<bool>.IsValid();
                },
            };

        /// <summary>
        /// Ensures that a <c>value</c>
        /// <see cref="StructValidityProcessor{T}.IsValid"/> if it is
        /// not <see langword="null"/> and equal to <see langword="true"/>.
        /// </summary>
        public static BooleanValidityProcessor IsTrue
            => new BooleanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (value.Value != true)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to one."));
                    }

                    return StructValidityResult<bool>.IsValid();
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
        public static BooleanValidityProcessor IsEqualTo(bool? other)
            => new BooleanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<bool>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value != other.Value)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is not equal to other."));
                    }

                    return StructValidityResult<bool>.IsValid();
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
        public static BooleanValidityProcessor IsNotEqualTo(bool? other)
            => new BooleanValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return StructValidityResult<bool>.IsValid();
                    }
                    else if (value is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Value == other.Value)
                    {
                        return StructValidityResult<bool>.NotValid(
                            new ArgumentException(paramName: nameof(value),
                                message: "Is greater than or equal to other."));
                    }

                    return StructValidityResult<bool>.IsValid();
                },
            };
    }
}
