using System;

namespace PreferenceGroups
{
    /// <summary>
    ///  A three step process for checking if a provided value is valid before
    ///  setting the value. The three steps are:
    ///  <list type="number">
    ///  <item><see cref="Pre"/> – Before checking validity, it may be necessary
    ///  to process the provided value. For example, for a string preference, it
    ///  may be necessary to trim the value to ignore any white-space characters
    ///  at the beginning and end.</item>
    ///  <item><see cref="IsValid"/> – Checks the validity of the value.</item>
    ///  <item><see cref="Post"/> – After checking validity, it may be necessary
    ///  to proces the value further before setting the value. For example, for
    ///  a string preference, it may be necessary to change the case of
    ///  characters to all upper-case to normalize the value.</item>
    ///  </list>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructValidityProcessor<T> where T : struct
    {
        /// <summary>
        /// Is the pre-validity check function step that takes a value of
        /// <typeparamref name="T"/> and returns a
        /// <see cref="StructValidityProcessorResult{T}"/> indicating either the
        /// process <see cref="StructValidityProcessorResult{T}.Succeeded"/> or
        /// <see cref="StructValidityProcessorResult{T}.Failed"/>. Defaults to using
        /// the <see cref="NoChange(T?)"/> <see langword="static"/> method.
        /// </summary>
        public Func<T?, StructValidityProcessorResult<T>> Pre { get; set; }
            = NoChange;

        /// <summary>
        /// The validity check function that takes a value of
        /// <typeparamref name="T"/> to validate and returns a
        /// <see cref="StructValidityResult{T}"/> indicating
        /// either the value is <see cref="StructValidityResult{T}.Valid"/>
        /// or there was an
        /// <see cref="StructValidityResult{T}.Exception"/>. Defaults to
        /// using the <see cref="ForceValidity(T?)"/> <see langword="static"/>
        /// method.
        /// </summary>
        public Func<T?, StructValidityResult<T>> IsValid { get; set; }
            = ForceValidity;

        /// <summary>
        /// Is the post-validity check function step that takes a value of
        /// <typeparamref name="T"/> and returns a
        /// <see cref="StructValidityProcessorResult{T}"/> indicating either the
        /// process <see cref="StructValidityProcessorResult{T}.Succeeded"/> or
        /// <see cref="StructValidityProcessorResult{T}.Failed"/>. Defaults to using
        /// the <see cref="NoChange(T?)"/> <see langword="static"/> method.
        /// </summary>
        public Func<T?, StructValidityProcessorResult<T>> Post { get; set; }
            = NoChange;

        /// <summary>
        /// A helper method intended to be used for either the <see cref="Pre"/>
        /// or <see cref="Post"/> steps by ensuring that
        /// <paramref name="valueIn"/> is not <see langword="null"/>. If
        /// <paramref name="valueIn"/> is <see langword="null"/>, then a
        /// <see cref="StructValidityProcessorResult{T}.Failure(Exception)"/> will
        /// be returned with an <see cref="ArgumentNullException"/>. If
        /// <paramref name="valueIn"/> is not <see langword="null"/>, then the
        /// <see cref="NoChange(T?)"/> method is called.
        /// </summary>
        /// <param name="valueIn">What to ensure is not
        /// <see langword="null"/>.</param>
        /// <returns></returns>
        public static StructValidityProcessorResult<T> EnsureNotNull(T? valueIn)
        {
            if (valueIn is null)
            {
                StructValidityProcessorResult<T>.Failure(new ArgumentNullException(
                    paramName: nameof(valueIn),
                    message: "Cannot be null."));
            }

            return NoChange(valueIn);
        }

        /// <summary>
        /// Will return <see cref="StructValidityResult{T}.IsValid"/>, while
        /// ignoring the <paramref name="_"/> parameter. This is the default for
        /// the <see cref="IsValid"/> step.
        /// </summary>
        /// <param name="_">This parameter is ignored.</param>
        /// <returns></returns>
        public static StructValidityResult<T> ForceValidity(T? _)
            => StructValidityResult<T>.IsValid();


#pragma warning disable CS1580 // Invalid type for parameter in XML comment cref attribute
        /// <summary>
        /// A helper method intended to be used for either the <see cref="Pre"/>
        /// or <see cref="Post"/> steps by calling the
        /// <see cref="StructValidityProcessorResult{T}.Success(T?)"/> method with
        /// no change to <paramref name="valueIn"/>. This is the default function
        /// for both the <see cref="Pre"/> and <see cref="Post"/> steps.
        /// </summary>
        /// <param name="valueIn">What not to change.</param>
        /// <returns></returns>
        public static StructValidityProcessorResult<T> NoChange(T? valueIn)
#pragma warning restore CS1580 // Invalid type for parameter in XML comment cref attribute
            => StructValidityProcessorResult<T>.Success(valueIn);
    }
}
