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
            : base(FormatMessage(result), result.Exception)
        {
            Result = result;
        }

        /// <summary>
        /// Formats the <see cref="Exception.Message"/> from the
        /// <paramref name="result"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string FormatMessage(SetValueResult result)
        {
            string innerMessage = result.Exception?.Message;

            if (string.IsNullOrWhiteSpace(innerMessage))
            {
                innerMessage = string.Empty;
            }
            else
            {
                innerMessage = ": " + innerMessage.Trim();
            }
            
            return $"Failure at step {result.StepFailure}{innerMessage}";
        }
    }
}
