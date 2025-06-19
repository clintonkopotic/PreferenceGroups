using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains the result from a
    /// <see cref="ClassValueValidityProcessor{T}.Pre"/> or
    /// <see cref="ClassValueValidityProcessor{T}.Post"/> step, with either the
    /// processed value (see <see cref="ValueOut"/>) or an
    /// <see cref="Exception"/> with why the value could not be processed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassValueProcessorResult<T> where T : class
    {
        /// <summary>
        /// If not <see langword="null"/>, contains the generated
        /// <see cref="System.Exception"/> with why the value could not be
        /// processed.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// See <see cref="Exception"/> with why the value could not be
        /// processed.
        /// </summary>
        public bool Failed => !Succeeded;

        /// <summary>
        /// See <see cref="ValueOut"/> with the successfully processed value.
        /// </summary>
        public bool Succeeded => Exception is null;

        /// <summary>
        /// If <see cref="Exception"/> is <see langword="null"/>, then this
        /// contains the processed value.
        /// </summary>
        public T ValueOut { get; }

        /// <summary>
        /// Instantiates with both an <paramref name="exception"/> and a
        /// <paramref name="valueOut"/>, with the convention that
        /// <see cref="Succeeded"/> will only be <see langword="true"/> when
        /// <see cref="Exception"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="exception">If not <see langword="null"/>, contains the
        /// generated <see cref="System.Exception"/> with why the value could
        /// not be processed.</param>
        /// <param name="valueOut">If <see cref="Exception"/> is
        /// <see langword="null"/>, then this contains the processed
        /// value.</param>
        public ClassValueProcessorResult(Exception exception, T valueOut)
        {
            Exception = exception;
            ValueOut = valueOut;
        }

        /// <summary>
        /// Instantiates a <see cref="Failed"/> result with the not
        /// <see langword="null"/> <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">If not <see langword="null"/>, contains the
        /// generated <see cref="System.Exception"/> with why the value could
        /// not be processed.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/>
        /// is <see langword="null"/>.</exception>
        public static ClassValueProcessorResult<T> Failure(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return new ClassValueProcessorResult<T>(exception, null);
        }

        /// <summary>
        /// Instantiates a <see cref="Succeeded"/> result with the
        /// <paramref name="valueOut"/>.
        /// </summary>
        /// <param name="valueOut">Contains the processed value.</param>
        /// <returns></returns>
        public static ClassValueProcessorResult<T> Success(T valueOut)
            => new ClassValueProcessorResult<T>(null, valueOut);
    }
}
