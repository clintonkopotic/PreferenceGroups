using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> methods for common
    /// <see cref="ClassValidityProcessor{T}.Pre"/> and
    /// <see cref="ClassValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="ClassValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class BytesValidityProcessor : ClassValidityProcessor<byte[]>
    {
        /// <summary>
        /// Removes any elements of the <see cref="Array"/> of
        /// <see cref="byte"/> that equal to zero, if <paramref name="valueIn"/>
        /// is not <see langword="null"/>.
        /// </summary>
        /// <param name="valueIn"></param>
        /// <returns></returns>
        public static ClassValidityProcessorResult<byte[]> RemoveZerosIfNotNull(
            byte[] valueIn)
        {
            if (valueIn is null || valueIn.Length <= 0)
            {
                return ClassValidityProcessorResult<byte[]>.Success(valueIn);
            }

            var result = new List<byte>();

            foreach (var value in valueIn)
            {
                if (value != 0)
                {
                    result.Add(value);
                }
            }

            return ClassValidityProcessorResult<byte[]>.Success(
                valueOut: result.ToArray());
        }

        /// <summary>
        /// Ensures that the two sequences, the value to be checked and
        /// <paramref name="other"/>, are of equal length and their
        /// corresponding elements are equal. If both are
        /// <see langword="null"/>, then they are considered equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static BytesValidityProcessor SequenceEqual(byte[] other)
            => new BytesValidityProcessor()
            {
                IsValid = value =>
                {
                    if (value is null && other is null)
                    {
                        return ClassValidityResult<byte[]>.IsValid();
                    }
                    else if (value is null)
                    {
                        return ClassValidityResult<byte[]>.NotValid(
                            new ArgumentNullException(paramName: nameof(value),
                                message: "Is null."));
                    }
                    else if (other is null)
                    {
                        return ClassValidityResult<byte[]>.NotValid(
                            new ArgumentNullException(paramName: nameof(other),
                                message: "Is null."));
                    }
                    else if (value.Length != other.Length)
                    {
                        return ClassValidityResult<byte[]>.NotValid(
                            new ArgumentException(paramName: nameof(other),
                                message: $"Expecting a {nameof(Array.Length)} "
                                + $"of {value.Length}, but instead have "
                                + $"{other.Length}."));
                    }

                    for (var i = 0; i < value.Length; i++)
                    {
                        if (value[0] != other[i])
                        {
                            return ClassValidityResult<byte[]>.NotValid(
                                new ArgumentException(paramName: nameof(other),
                                    message: $"Expecting a value at index {i} "
                                    + $"of {value[i]}, but instead have "
                                    + $"{other[i]}."));
                        }
                    }

                    return ClassValidityResult<byte[]>.IsValid();
                },
            };
    }
}
