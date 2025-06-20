using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides <see langword="static"/> methods for common
    /// <see cref="ClassValueValidityProcessor{T}.Pre"/> and
    /// <see cref="ClassValueValidityProcessor{T}.Post"/> processing steps, and
    /// <see cref="ClassValueValidityProcessor{T}.IsValid"/> validity checks.
    /// </summary>
    public class StringValueValidityProcessor
        : ClassValueValidityProcessor<string>
    {
        /// <summary>
        /// If the <see cref="string"/> is not <see langword="null"/>, then it
        /// will first be trimmed with the <see cref="string.Trim()"/> method.
        /// </summary>
        public static StringValueValidityProcessor
            EnsureNotNullOrWhiteSpaceAndPostTrim
            => new StringValueValidityProcessor()
            {
                Pre = NoChange,
                IsValid = EnsureNotNullOrWhiteSpace,
                Post = TrimIfNotNull,
            };

        /// <summary>
        /// Will accept all <see cref="string"/>s, and if the
        /// <see cref="string"/> is not <see langword="null"/>, then it will be
        /// trimmed with the <see cref="string.Trim()"/> method.
        /// </summary>
        public static StringValueValidityProcessor PreTrimIfNotNull
            => new StringValueValidityProcessor()
            {
                Pre = TrimIfNotNull,
                IsValid = ForceValidity,
                Post = NoChange,
            };

        /// <summary>
        /// Returns a valid <see cref="ClassValueValidityResult{T}"/> if
        /// <paramref name="valueIn"/> is not <see langword="null"/> or is not
        /// an empty <see cref="string"/>; otherwise an invalid result is
        /// returned with the
        /// <see cref="ClassValueValidityResult{T}.Exception"/> explaining why.
        /// </summary>
        /// <param name="valueIn"></param>
        /// <returns></returns>
        public static ClassValueValidityResult<string> EnsureNotNullOrEmpty(
            string valueIn)
        {
            Exception exception;

            if (valueIn is null)
            {
                exception = new ArgumentException(paramName: nameof(valueIn),
                    message: "Is null.");
            }
            else if (string.IsNullOrEmpty(valueIn))
            {
                exception = new ArgumentException(paramName: nameof(valueIn),
                    message: "Is empty.");
            }
            else
            {
                return ForceValidity(valueIn);
            }

            return ClassValueValidityResult<string>.NotValid(exception);
        }

        /// <summary>
        /// Returns a valid <see cref="ClassValueValidityResult{T}"/> if
        /// <paramref name="valueIn"/> is not <see langword="null"/>, is not an
        /// empty <see cref="string"/>, or is not a <see cref="string"/> that
        /// consists only of white-space characters; otherwise an invalid result
        /// is returned with the
        /// <see cref="ClassValueValidityResult{T}.Exception"/> explaining why.
        /// </summary>
        /// <param name="valueIn"></param>
        /// <returns></returns>
        public static ClassValueValidityResult<string>
            EnsureNotNullOrWhiteSpace(string valueIn)
        {
            Exception exception;

            if (valueIn is null)
            {
                exception = new ArgumentException(paramName: nameof(valueIn),
                    message: "Is null.");
            }
            else if (string.IsNullOrEmpty(valueIn))
            {
                exception = new ArgumentException(paramName: nameof(valueIn),
                    message: "Is empty.");
            }
            else if (string.IsNullOrWhiteSpace(valueIn))
            {
                exception = new ArgumentException(paramName: nameof(valueIn),
                    message: "Consists only of white-space characters.");
            }
            else
            {
                return ForceValidity(valueIn);
            }

            return new ClassValueValidityResult<string>(exception);
        }

        /// <summary>
        /// If the <see cref="string"/> is not <see langword="null"/>, then it
        /// will be trimmed with the <see cref="string.Trim()"/> method.
        /// </summary>
        /// <param name="valueIn">What to <see cref="string.Trim()"/> if it is
        /// not <see langword="null"/>.</param>
        /// <returns></returns>
        public static ClassValueProcessorResult<string> TrimIfNotNull(
            string valueIn)
            => ClassValueProcessorResult<string>.Success(valueIn?.Trim());
    }
}
