using System;

namespace PreferenceGroups
{
    /// <summary>
    /// An <see cref="Exception"/> for when setting a value or default value of
    /// a <see cref="Preference"/> fails.
    /// </summary>
    public class SetValueException : Exception
    {
        /// <summary>
        /// The failed result of setting a value.
        /// </summary>
        public SetValueResult Result { get; }

        /// <summary>
        /// A wrapper constructor that instantiates a
        /// <see cref="SetValueResult"/> with the parameters and then
        /// instantiates the <see cref="SetValueException"/>.
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="stepFailure"></param>
        public SetValueException(Exception innerException,
            SetValueStepFailure stepFailure)
        : this(new SetValueResult(innerException, stepFailure))
        { }

        /// <summary>
        /// Instantiates the exception from the <paramref name="result"/>.
        /// </summary>
        /// <param name="result"></param>
        public SetValueException(SetValueResult result)
            : base(result.Exception?.Message, result.Exception)
        {
            Result = result;
        }
    }
}
