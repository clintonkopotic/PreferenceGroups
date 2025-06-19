using System;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains the result from setting a value or the default value of a
    /// <see cref="Preference"/>.
    /// </summary>
    public class SetValueResult
    {
        /// <summary>
        /// When <see langword="true"/>, <see cref="Exception"/> is
        /// <see langword="null"/> and <see cref="StepFailure"/> is
        /// <see cref="SetValueStepFailure.None"/>.
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// The <see cref="System.Exception"/> that was generated, if any.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// At which step was the <see cref="Exception"/> generated, if any.
        /// </summary>
        public SetValueStepFailure StepFailure { get; }

        /// <summary>
        /// Instantiates a <see cref="Succeeded"/> result.
        /// </summary>
        public SetValueResult() : this(null, SetValueStepFailure.None)
        { }

        /// <summary>
        /// Instantiates a result with the provided parameters.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="stepFailure"></param>
        public SetValueResult(Exception exception, SetValueStepFailure stepFailure)
        {
            Succeeded = !(exception is null)
                && stepFailure == SetValueStepFailure.None;
            Exception = exception;
            StepFailure = stepFailure;
        }
    }
}
