using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains the result from the
    /// <see cref="ClassValueValidityProcessor{T}.IsValid"/> step, with either
    /// <see cref="Valid"/> set to <see langword="true"/> or a not
    /// <see langword="null"/> <see cref="Exception"/> with why the value is not
    /// valid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassValueValidityResult<T> where T : class
    {
        /// <summary>
        /// If <see cref="Valid"/> is <see langword="false"/>, this is the
        /// generated <see cref="System.Exception"/> of why the value is not
        /// valid.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// If <see langword="true"/>, the value is valid, otherwise see
        /// <see cref="Exception"/> for why not.
        /// </summary>
        public bool Valid => Exception is null;

        /// <summary>
        /// Intantiates a <see cref="Valid"/> result.
        /// </summary>
        public ClassValueValidityResult() : this(null)
        { }

        /// <summary>
        /// Intantiates a result with <paramref name="exception"/>, where a
        /// <see langword="null"/> indicates <see cref="Valid"/> is
        /// <see langword="true"/>. On the other hand, a
        /// <paramref name="exception"/> that is not
        /// <see langword="null"/> indicates that <see cref="Valid"/> is
        /// <see langword="false"/> and this is why.
        /// </summary>
        /// <param name="exception"></param>
        public ClassValueValidityResult(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Intantiates a <see cref="Valid"/> result.
        /// </summary>
        public static ClassValueValidityResult<T> IsValid()
            => new ClassValueValidityResult<T>();

        /// <summary>
        /// Intantiates a not <see cref="Valid"/> result with a not
        /// <see langword="null"/> <paramref name="exception"/> explaining why.
        /// </summary>
        /// <param name="exception">Why the value is not valid.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> is <see langword="null"/>.</exception>
        public static ClassValueValidityResult<T> NotValid(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return new ClassValueValidityResult<T>(exception);
        }
    }
}
