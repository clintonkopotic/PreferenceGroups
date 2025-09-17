using System;
using System.Net;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> methods for common
    /// <see cref="ClassValidityProcessor{T}.Pre"/> and
    /// <see cref="ClassValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="ClassValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class IPAddressValidityProcessor : ClassValidityProcessor<IPAddress>
    {
        /// <summary>
        /// Ensures that the two sequences, the value to be checked and
        /// <paramref name="other"/>, are of equal length and their
        /// corresponding elements are equal. If both are
        /// <see langword="null"/>, then they are considered equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IPAddressValidityProcessor Equal(IPAddress other)
            => new IPAddressValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return ClassValidityResult<IPAddress>.IsValid();
                    }
                    else if (value is null)
                    {
                        return ClassValidityResult<IPAddress>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return ClassValidityResult<IPAddress>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Equals(other))
                    {
                        return ClassValidityResult<IPAddress>.NotValid(
                            new ArgumentException(paramName: nameof(other),
                                message: $"Expecting a {nameof(Array.Length)} "
                                + $"of {value}, but instead have "
                                + $"{other}."));
                    }

                    return ClassValidityResult<IPAddress>.IsValid();
                },
            };
    }
}
