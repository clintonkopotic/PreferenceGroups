using System;
using System.Globalization;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> methods for updating a
    /// <see cref="Preference"/> by deserializing from a <see cref="JValue"/>.
    /// </summary>
    public static class PreferenceJsonDeserializer
    {
        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="bool"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Boolean"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="bool"/> and returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is cast into a
        /// <see cref="Nullable{T}"/> <see cref="int"/> and only returns
        /// <see langword="true"/> if it is not equal to zero and
        /// <see langword="false"/> otherwise.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/> and the result of the
        /// <see cref="Convert.ToBoolean(string, IFormatProvider)"/> method is
        /// returned.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static bool? DeserializeAsBoolean(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Boolean)
            {
                return (bool?)jValue;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                var int32Value = (int?)jValue;

                if (int32Value is null)
                {
                    return null;
                }

                return int32Value.Value != 0;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }

                return Convert.ToBoolean(@string, CultureInfo.InvariantCulture);
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as a "
                + $"{nameof(Boolean)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="byte"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="byte"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToByte(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToByte(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static byte? DeserializeAsByte(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (byte?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToByte(@string, fromBase: 16)
                        : Convert.ToByte(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Byte)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into an
        /// <see cref="Array"/> of <see cref="byte"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Bytes"/>, then it is just cast into an
        /// <see cref="Array"/> of <see cref="byte"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, and the result of calling the
        /// <see cref="Convert.FromBase64String(string)"/> method is
        /// returned.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static byte[] DeserializeAsBytes(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Bytes)
            {
                return (byte[])jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    return Convert.FromBase64String(@string.Trim());
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as a "
                + $"{nameof(Byte)}[].");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="decimal"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Float"/> or
        /// <see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="decimal"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called. Then the resulting <see cref="string"/> is used with the
        /// <see cref="Convert.ToDecimal(string, IFormatProvider)"/> method with
        /// <see cref="CultureInfo.InvariantCulture"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static decimal? DeserializeAsDecimal(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Float
                || jValue.Type == JTokenType.Integer)
            {
                return (decimal?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToDecimal(@string.Trim(),
                        CultureInfo.InvariantCulture);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Decimal)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="double"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Float"/> or
        /// <see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="double"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called. Then the resulting <see cref="string"/> is used with the
        /// <see cref="Convert.ToDouble(string, IFormatProvider)"/> method with
        /// <see cref="CultureInfo.InvariantCulture"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static double? DeserializeAsDouble(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Float
                || jValue.Type == JTokenType.Integer)
            {
                return (double?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim();

                    if ("-∞".Equals(@string))
                    {
                        return double.NegativeInfinity;
                    }
                    else if ("+∞".Equals(@string) || "∞".Equals(@string))
                    {
                        return double.PositiveInfinity;
                    }

                    return Convert.ToDouble(@string,
                        CultureInfo.InvariantCulture);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Double)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="short"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="short"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToInt16(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToInt16(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static short? DeserializeAsInt16(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (short?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToInt16(@string, fromBase: 16)
                        : Convert.ToInt16(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Int16)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="int"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="int"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToInt32(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToInt32(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static int? DeserializeAsInt32(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (int?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToInt32(@string, fromBase: 16)
                        : Convert.ToInt32(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Int32)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="long"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="long"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToInt64(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToInt64(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static long? DeserializeAsInt64(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (long?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToInt64(@string, fromBase: 16)
                        : Convert.ToInt64(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Int64)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into an
        /// <see cref="Array"/> of <see cref="byte"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Bytes"/>, then it is just cast into an
        /// <see cref="Array"/> of <see cref="byte"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, and the result of calling the
        /// <see cref="Convert.FromBase64String(string)"/> method is
        /// returned.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static IPAddress DeserializeAsIPAddress(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Bytes)
            {
                return new IPAddress((byte[])jValue);
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return new IPAddress((long)jValue);
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim();

                    if (@string.StartsWith("0x",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new IPAddress(Convert.ToInt64(@string,
                            fromBase: 16));
                    }

                    return IPAddress.Parse(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as a "
                + $"{nameof(IPAddress)}.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="sbyte"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="sbyte"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToSByte(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToSByte(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static sbyte? DeserializeAsSByte(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (sbyte?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToSByte(@string, fromBase: 16)
                        : Convert.ToSByte(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(SByte)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="float"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Float"/> or
        /// <see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="float"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called. Then the resulting <see cref="string"/> is used with the
        /// <see cref="Convert.ToSingle(string, IFormatProvider)"/> method with
        /// <see cref="CultureInfo.InvariantCulture"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static float? DeserializeAsSingle(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Float
                || jValue.Type == JTokenType.Integer)
            {
                return (float?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim();

                    if ("-∞".Equals(@string))
                    {
                        return float.NegativeInfinity;
                    }
                    else if ("+∞".Equals(@string) || "∞".Equals(@string))
                    {
                        return float.PositiveInfinity;
                    }

                    return Convert.ToSingle(@string,
                        CultureInfo.InvariantCulture);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(Single)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a <see cref="string"/>
        /// if the <see cref="JValue.Type"/> is <see cref="JTokenType.String"/>
        /// by casting it. If the <see cref="JValue.Type"/> of
        /// <paramref name="jValue"/> is <see cref="JTokenType.Null"/>, then
        /// <see langword="null"/> is returned.
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static string DeserializeAsString(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.String)
            {
                return (string)jValue;
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(String)}.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="TimeSpan"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.TimeSpan"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="TimeSpan"/> and returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="long"/> and the result of instantiating a
        /// <see cref="TimeSpan"/>, with the <see cref="TimeSpan(long)"/>
        /// constructor, returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, and the result of calling the
        /// <see cref="TimeSpan.Parse(string)"/> method is returned.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static TimeSpan? DeserializeAsTimeSpan(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.TimeSpan)
            {
                return (TimeSpan?)jValue;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return new TimeSpan((long)jValue);
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    return TimeSpan.Parse(@string.Trim());
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(TimeSpan)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="ushort"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="ushort"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToUInt16(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToUInt16(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static ushort? DeserializeAsUInt16(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (ushort?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToUInt16(@string, fromBase: 16)
                        : Convert.ToUInt16(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(UInt16)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="uint"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="uint"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToUInt32(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToUInt32(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static uint? DeserializeAsUInt32(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (uint?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToUInt32(@string, fromBase: 16)
                        : Convert.ToUInt32(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(UInt32)}?.");
        }

        /// <summary>
        /// Deserializes <paramref name="jValue"/> into a
        /// <see cref="Nullable{T}"/> <see cref="ulong"/>, where if the
        /// <see cref="JValue.Type"/> is:
        /// <list type="bullet">
        /// <item><see cref="JTokenType.Null"/>, then <see langword="null"/> is
        /// returned.</item>
        /// <item><see cref="JTokenType.Integer"/>, then it is just cast into a
        /// <see cref="Nullable{T}"/> <see cref="ulong"/> and returned.</item>
        /// <item><see cref="JTokenType.String"/>, then it is cast into a
        /// <see cref="string"/>, the <see cref="string.Trim()"/> method is
        /// called, and then the <see cref="string.ToUpperInvariant()"/> method.
        /// Then the resulting <see cref="string"/> is checked to see if it
        /// <see cref="string.StartsWith(string)"/> <c>"0X"</c>. If it does,
        /// then the <see cref="Convert.ToUInt64(string, int)"/> method is called
        /// with <c>fromBase</c> set to 16; otherwise the
        /// <see cref="Convert.ToUInt64(string)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="jValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="jValue"/> has an unexpected
        /// <see cref="JValue.Type"/>.</exception>
        public static ulong? DeserializeAsUInt64(JValue jValue)
        {
            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                return null;
            }
            else if (jValue.Type == JTokenType.Integer)
            {
                return (uint?)jValue;
            }
            else if (jValue.Type == JTokenType.String)
            {
                var @string = (string)jValue;

                if (@string is null)
                {
                    return null;
                }
                else
                {
                    @string = @string.Trim().ToUpperInvariant();

                    return @string.StartsWith("0X")
                        ? Convert.ToUInt64(@string, fromBase: 16)
                        : Convert.ToUInt64(@string);
                }
            }

            throw new InvalidOperationException("An unexpected "
                + $"{nameof(JTokenType)} of {jValue.Type} to deserialize as an "
                + $"{nameof(UInt64)}?.");
        }

        /// <summary>
        /// Attempts to upate <paramref name="booleanPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsBoolean(JValue)"/> is called and
        /// its result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="booleanPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="booleanPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(BooleanPreference booleanPreference,
            JValue jValue)
        {
            if (booleanPreference is null)
            {
                throw new ArgumentNullException(nameof(booleanPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                booleanPreference.SetValueToNull();

                return;
            }

            var @bool = DeserializeAsBoolean(jValue);

            if (@bool is null)
            {
                booleanPreference.SetValueToNull();
            }
            else
            {
                booleanPreference.Value = @bool;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="bytePreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsByte(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="bytePreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytePreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(BytePreference bytePreference,
            JValue jValue)
        {
            if (bytePreference is null)
            {
                throw new ArgumentNullException(nameof(bytePreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                bytePreference.SetValueToNull();

                return;
            }

            var @byte = DeserializeAsByte(jValue);

            if (@byte is null)
            {
                bytePreference.SetValueToNull();
            }
            else
            {
                bytePreference.Value = @byte;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="bytesPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsBytes(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="ClassPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="bytesPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytesPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(BytesPreference bytesPreference,
            JValue jValue)
        {
            if (bytesPreference is null)
            {
                throw new ArgumentNullException(nameof(bytesPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                bytesPreference.SetValueToNull();

                return;
            }

            var bytes = DeserializeAsBytes(jValue);

            if (bytes is null)
            {
                bytesPreference.SetValueToNull();
            }
            else
            {
                bytesPreference.Value = bytes;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="decimalPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsDecimal(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="decimalPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decimalPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(DecimalPreference decimalPreference,
            JValue jValue)
        {
            if (decimalPreference is null)
            {
                throw new ArgumentNullException(nameof(decimalPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                decimalPreference.SetValueToNull();

                return;
            }

            var @decimal = DeserializeAsDecimal(jValue);

            if (@decimal is null)
            {
                decimalPreference.SetValueToNull();
            }
            else
            {
                decimalPreference.Value = @decimal;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="doublePreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsDouble(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="doublePreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="doublePreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(DoublePreference doublePreference,
            JValue jValue)
        {
            if (doublePreference is null)
            {
                throw new ArgumentNullException(nameof(doublePreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                doublePreference.SetValueToNull();

                return;
            }

            var @double = DeserializeAsDouble(jValue);

            if (@double is null)
            {
                doublePreference.SetValueToNull();
            }
            else
            {
                doublePreference.Value = @double;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="int16Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsInt16(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="int16Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="int16Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(Int16Preference int16Preference,
            JValue jValue)
        {
            if (int16Preference is null)
            {
                throw new ArgumentNullException(nameof(int16Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                int16Preference.SetValueToNull();

                return;
            }

            var @short = DeserializeAsInt16(jValue);

            if (@short is null)
            {
                int16Preference.SetValueToNull();
            }
            else
            {
                int16Preference.Value = @short;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="int32Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsInt32(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="int32Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="int32Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(Int32Preference int32Preference,
            JValue jValue)
        {
            if (int32Preference is null)
            {
                throw new ArgumentNullException(nameof(int32Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                int32Preference.SetValueToNull();

                return;
            }

            var @int = DeserializeAsInt32(jValue);

            if (@int is null)
            {
                int32Preference.SetValueToNull();
            }
            else
            {
                int32Preference.Value = @int;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="int64Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsInt64(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="int64Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="int64Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(Int64Preference int64Preference,
            JValue jValue)
        {
            if (int64Preference is null)
            {
                throw new ArgumentNullException(nameof(int64Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                int64Preference.SetValueToNull();

                return;
            }

            var @int = DeserializeAsInt64(jValue);

            if (@int is null)
            {
                int64Preference.SetValueToNull();
            }
            else
            {
                int64Preference.Value = @int;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="ipAddressPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsIPAddress(JValue)"/> is called and
        /// its result is used to set
        /// <see cref="ClassPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="ipAddressPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ipAddressPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(IPAddressPreference ipAddressPreference,
            JValue jValue)
        {
            if (ipAddressPreference is null)
            {
                throw new ArgumentNullException(nameof(ipAddressPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                ipAddressPreference.SetValueToNull();

                return;
            }

            var ipAddress = DeserializeAsIPAddress(jValue);

            if (ipAddress is null)
            {
                ipAddressPreference.SetValueToNull();
            }
            else
            {
                ipAddressPreference.Value = ipAddress;
            }
        }

        /// <summary>
        /// Attempts to update <paramref name="preference"/> from
        /// <paramref name="jObject"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jObject"/> is <see langword="null"/> or its
        /// <see cref="JObject.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueFromObject(object)"/> is called with
        /// <see langword="null"/>.</item>
        /// <item>The <see cref="Preference.Name"/> is called with the
        /// <see cref="Preference.ProcessNameOrThrowIfInvalid(string, string)"/>
        /// method, and its result is used to check with the
        /// <see cref="JObject.ContainsKey(string)"/> method if
        /// <paramref name="jObject"/> has a property with that <c>name</c>. If
        /// it does, then that property is cast to a <see cref="JValue"/> and
        /// the <see cref="UpdateFrom(Preference, JValue)"/> method is
        /// called.</item>
        /// </list>
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="jObject"></param>
        /// <returns><see langword="true"/> if <paramref name="preference"/> was
        /// updated from <paramref name="jObject"/>, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static bool UpdateFrom(Preference preference, JObject jObject)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (jObject is null || jObject.Type == JTokenType.Null)
            {
                return false;
            }

            var processedName = Preference.ProcessNameOrThrowIfInvalid(
                preference.Name, nameof(preference));

            if (!jObject.ContainsKey(processedName))
            {
                return false;
            }

            UpdateFrom(preference, (JValue)jObject[processedName]);

            return true;
        }

        /// <summary>
        /// Attempts to update <paramref name="preference"/> from
        /// <paramref name="jToken"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jToken"/> is <see langword="null"/> or its
        /// <see cref="JToken.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>If the <see cref="JToken.Type"/> of <paramref name="jToken"/>
        /// is <see cref="JTokenType.Object"/>, then <paramref name="jToken"/>
        /// is cast to a <see cref="JObject"/> and the
        /// <see cref="UpdateFrom(Preference, JObject)"/> method is
        /// called.</item>
        /// <item>Otherwise, <paramref name="jToken"/> is cast as a
        /// <see cref="JValue"/> and the
        /// <see cref="UpdateFrom(Preference, JValue)"/> method is
        /// called.</item>
        /// </list>
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="jToken"></param>
        /// <returns><see langword="true"/> if <paramref name="preference"/> was
        /// updated from <paramref name="jToken"/>, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        public static bool UpdateFrom(Preference preference, JToken jToken)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (jToken is null || jToken.Type == JTokenType.Null)
            {
                preference.SetValueToNull();

                return true;
            }
            else if (jToken.Type == JTokenType.Object)
            {
                return UpdateFrom(preference, (JObject)jToken);
            }

            // If jToken isn't an object, then assume it's a JValue.
            UpdateFrom(preference, (JValue)jToken);

            return true;
        }

        /// <summary>
        /// Attempts to update <paramref name="preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>If <see cref="Preference.GetValueType()"/> returns
        /// <see cref="Nullable{T}"/> of <see cref="bool"/>, then
        /// <paramref name="preference"/> is cast to
        /// <see cref="BooleanPreference"/>, and the
        /// <see cref="UpdateFrom(BooleanPreference, JValue)"/> is called.</item>
        /// <item>If <see cref="Preference.GetValueType()"/> returns
        /// <see cref="Nullable{T}"/> of <see cref="int"/>, then
        /// <paramref name="preference"/> is cast to
        /// <see cref="Int32Preference"/>, and the
        /// <see cref="UpdateFrom(Int32Preference, JValue)"/> is called.</item>
        /// <item>If <see cref="Preference.GetValueType()"/> returns
        /// <see cref="string"/>, then <paramref name="preference"/> is cast to
        /// <see cref="StringPreference"/>, and the
        /// <see cref="UpdateFrom(StringPreference, JValue)"/> is called.</item>
        /// </list>
        /// </summary>
        /// <param name="preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="preference"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The
        /// <see cref="Preference.GetValueType()"/> returned an unexpected
        /// <see cref="Type"/>.</exception>
        public static void UpdateFrom(Preference preference, JValue jValue)
        {
            if (preference is null)
            {
                throw new ArgumentNullException(nameof(preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                preference.SetValueToNull();

                return;
            }

            if (preference.IsEnum)
            {
                preference.SetValueFromObject(jValue.Value);

                return;
            }

            var valueType = preference.GetValueType();

            if (valueType == typeof(bool?))
            {
                var booleanPreference = (BooleanPreference)preference;
                UpdateFrom(booleanPreference, jValue);
            }
            else if (valueType == typeof(sbyte?))
            {
                var sBytePreference = (SBytePreference)preference;
                UpdateFrom(sBytePreference, jValue);
            }
            else if (valueType == typeof(byte?))
            {
                var bytePreference = (BytePreference)preference;
                UpdateFrom(bytePreference, jValue);
            }
            else if (valueType == typeof(short?))
            {
                var int16Preference = (Int16Preference)preference;
                UpdateFrom(int16Preference, jValue);
            }
            else if (valueType == typeof(ushort?))
            {
                var uInt16Preference = (UInt16Preference)preference;
                UpdateFrom(uInt16Preference, jValue);
            }
            else if (valueType == typeof(int?))
            {
                var int32Preference = (Int32Preference)preference;
                UpdateFrom(int32Preference, jValue);
            }
            else if (valueType == typeof(uint?))
            {
                var uInt32Preference = (UInt32Preference)preference;
                UpdateFrom(uInt32Preference, jValue);
            }
            else if (valueType == typeof(long?))
            {
                var int64Preference = (Int64Preference)preference;
                UpdateFrom(int64Preference, jValue);
            }
            else if (valueType == typeof(ulong?))
            {
                var uInt64Preference = (UInt64Preference)preference;
                UpdateFrom(uInt64Preference, jValue);
            }
            else if (valueType == typeof(float?))
            {
                var singlePreference = (SinglePreference)preference;
                UpdateFrom(singlePreference, jValue);
            }
            else if (valueType == typeof(double?))
            {
                var doublePreference = (DoublePreference)preference;
                UpdateFrom(doublePreference, jValue);
            }
            else if (valueType == typeof(decimal?))
            {
                var decimalPreference = (DecimalPreference)preference;
                UpdateFrom(decimalPreference, jValue);
            }
            else if (valueType == typeof(string))
            {
                var stringPreference = (StringPreference)preference;
                UpdateFrom(stringPreference, jValue);
            }
            else if (valueType == typeof(byte[]))
            {
                var bytesPreference = (BytesPreference)preference;
                UpdateFrom(bytesPreference, jValue);
            }
            else if (valueType == typeof(TimeSpan))
            {
                var timeSpanPreference = (TimeSpanPreference)preference;
                UpdateFrom(timeSpanPreference, jValue);
            }
            else if (valueType == typeof(IPAddress))
            {
                var ipAddressPreference = (IPAddressPreference)preference;
                UpdateFrom(ipAddressPreference, jValue);
            }
            else
            {
                throw new InvalidOperationException("An unexpected value "
                    + $"{nameof(Type)} of \"{valueType.Name}\" to update.");
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="sBytePreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsSByte(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="sBytePreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sBytePreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(SBytePreference sBytePreference,
            JValue jValue)
        {
            if (sBytePreference is null)
            {
                throw new ArgumentNullException(nameof(sBytePreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                sBytePreference.SetValueToNull();

                return;
            }

            var @sbyte = DeserializeAsSByte(jValue);

            if (@sbyte is null)
            {
                sBytePreference.SetValueToNull();
            }
            else
            {
                sBytePreference.Value = @sbyte;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="singlePreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsSingle(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="singlePreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="singlePreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(SinglePreference singlePreference,
            JValue jValue)
        {
            if (singlePreference is null)
            {
                throw new ArgumentNullException(nameof(singlePreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                singlePreference.SetValueToNull();

                return;
            }

            var @float = DeserializeAsSingle(jValue);

            if (@float is null)
            {
                singlePreference.SetValueToNull();
            }
            else
            {
                singlePreference.Value = @float;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="stringPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsString(JValue)"/> is called and
        /// its result is used to set
        /// <see cref="StringPreference.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="stringPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stringPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(StringPreference stringPreference,
            JValue jValue)
        {
            if (stringPreference is null)
            {
                throw new ArgumentNullException(nameof(stringPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                stringPreference.SetValueToNull();

                return;
            }

            var @string = DeserializeAsString(jValue);

            if (@string is null)
            {
                stringPreference.SetValueToNull();
            }
            else
            {
                stringPreference.Value = @string;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="timeSpanPreference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsTimeSpan(JValue)"/> is called and
        /// its result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="timeSpanPreference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="timeSpanPreference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(TimeSpanPreference timeSpanPreference,
            JValue jValue)
        {
            if (timeSpanPreference is null)
            {
                throw new ArgumentNullException(nameof(timeSpanPreference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                timeSpanPreference.SetValueToNull();

                return;
            }

            var timeSpan = DeserializeAsTimeSpan(jValue);

            if (timeSpan is null)
            {
                timeSpanPreference.SetValueToNull();
            }
            else
            {
                timeSpanPreference.Value = timeSpan;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="uInt16Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsUInt16(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="uInt16Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="uInt16Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(UInt16Preference uInt16Preference,
            JValue jValue)
        {
            if (uInt16Preference is null)
            {
                throw new ArgumentNullException(nameof(uInt16Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                uInt16Preference.SetValueToNull();

                return;
            }

            var @ushort = DeserializeAsUInt16(jValue);

            if (@ushort is null)
            {
                uInt16Preference.SetValueToNull();
            }
            else
            {
                uInt16Preference.Value = @ushort;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="uInt32Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsUInt32(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="uInt32Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="uInt32Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(UInt32Preference uInt32Preference,
            JValue jValue)
        {
            if (uInt32Preference is null)
            {
                throw new ArgumentNullException(nameof(uInt32Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                uInt32Preference.SetValueToNull();

                return;
            }

            var @uint = DeserializeAsUInt32(jValue);

            if (@uint is null)
            {
                uInt32Preference.SetValueToNull();
            }
            else
            {
                uInt32Preference.Value = @uint;
            }
        }

        /// <summary>
        /// Attempts to upate <paramref name="uInt64Preference"/> from
        /// <paramref name="jValue"/> by going through the following sequence:
        /// <list type="bullet">
        /// <item>If <paramref name="jValue"/> is <see langword="null"/> or the
        /// <see cref="JValue.Type"/> is <see cref="JTokenType.Null"/>, then
        /// <see cref="Preference.SetValueToNull()"/> is called.</item>
        /// <item>The <see cref="DeserializeAsUInt64(JValue)"/> is called and its
        /// result is used to set
        /// <see cref="StructPreference{T}.Value"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="uInt64Preference"></param>
        /// <param name="jValue"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="uInt64Preference"/> is
        /// <see langword="null"/>.</exception>
        public static void UpdateFrom(UInt64Preference uInt64Preference,
            JValue jValue)
        {
            if (uInt64Preference is null)
            {
                throw new ArgumentNullException(nameof(uInt64Preference));
            }

            if (jValue is null || jValue.Type == JTokenType.Null)
            {
                uInt64Preference.SetValueToNull();

                return;
            }

            var @uint = DeserializeAsUInt64(jValue);

            if (@uint is null)
            {
                uInt64Preference.SetValueToNull();
            }
            else
            {
                uInt64Preference.Value = @uint;
            }
        }
    }
}
