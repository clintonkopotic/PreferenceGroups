using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// A helper class for working with <see cref="Enum"/> types.
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method).
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum[] GetValues<TEnum>()
            where TEnum : struct, Enum
        {
            var tEnumType = typeof(TEnum);
            var valuesAsObjects = Enum.GetValues(tEnumType);

            if (valuesAsObjects.Length <= 0)
            {
                return new TEnum[] { };
            }

            var values = new List<TEnum>();

            foreach (var valueAsObject in valuesAsObjects)
            {
                var value = (TEnum)valueAsObject;
                values.Add(value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="Nullable{T}"/>
        /// <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method).
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum?[] GetValuesAsNullable<TEnum>()
            where TEnum : struct, Enum
        {
            var tEnumType = typeof(TEnum);
            var valuesAsObjects = Enum.GetValues(tEnumType);

            if (valuesAsObjects.Length <= 0)
            {
                return new TEnum?[] { };
            }

            var values = new List<TEnum?>();

            foreach (var valueAsObject in valuesAsObjects)
            {
                var value = (TEnum)valueAsObject;
                values.Add(value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method) that are not zero (see the
        /// <see cref="GetZero{TEnum}(TEnum)"/> method).
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum[] GetValuesNotZero<TEnum>()
            where TEnum : struct, Enum
        {
            var tEnumType = typeof(TEnum);
            var valuesAsObjects = Enum.GetValues(tEnumType);

            if (valuesAsObjects.Length <= 0)
            {
                return new TEnum[] { };
            }

            var valuesNotZero = new List<TEnum>();

            foreach (var valueAsObject in valuesAsObjects)
            {
                var value = (TEnum)valueAsObject;

                if (value.IsZero())
                {
                    continue;
                }

                valuesNotZero.Add(value);
            }

            return valuesNotZero.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method) that are not zero (see the
        /// <see cref="GetZero{TEnum}(TEnum)"/> method).
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum?[] GetValuesNotZeroAsNullable<TEnum>()
            where TEnum : struct, Enum
        {
            var tEnumType = typeof(TEnum);
            var valuesAsObjects = Enum.GetValues(tEnumType);

            if (valuesAsObjects.Length <= 0)
            {
                return new TEnum?[] { };
            }

            var valuesNotZero = new List<TEnum?>();

            foreach (var valueAsObject in valuesAsObjects)
            {
                var value = (TEnum?)valueAsObject;

                if (value.IsZero())
                {
                    continue;
                }

                valuesNotZero.Add(value);
            }

            return valuesNotZero.ToArray();
        }

        /// <summary>
        /// Returns the value of <typeparamref name="TEnum"/> that is equivalent
        /// to zero as its <see cref="Enum.GetUnderlyingType(Type)"/>.
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static TEnum GetZero<TEnum>(this TEnum @enum)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum).Zero;

        /// <summary>
        /// Returns the value of <typeparamref name="TEnum"/> that is equivalent
        /// to zero as its <see cref="Enum.GetUnderlyingType(Type)"/>.
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static TEnum? GetZeroAsNullable<TEnum>(this TEnum @enum)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum).Zero;

        /// <summary>
        /// Determines whether or not <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> attached to it.
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static bool HasFlags<TEnum>()
            => typeof(TEnum).IsDefined(
                attributeType: typeof(FlagsAttribute),
                inherit: false);

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is a defined
        /// value of <typeparamref name="TEnum"/>, including 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsDefined<TEnum>(this TEnum @enum)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum).IsDefined;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsDefined<TEnum>(this TEnum? @enum)
            where TEnum : struct, Enum
        {
            if (@enum is null)
            {
                return false;
            }

            return new EnumInfo<TEnum>(@enum.Value).IsDefined;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsDefinedAndNotZero<TEnum>(this TEnum? @enum)
            where TEnum : struct, Enum
        {
            if (@enum is null)
            {
                return true;
            }

            return IsDefinedAndNotZero(@enum.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsDefinedAndNotZero<TEnum>(this TEnum @enum)
            where TEnum : struct, Enum
        {
            var enumInfo = new EnumInfo<TEnum>(@enum);

            return enumInfo.IsDefined && !enumInfo.IsZero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsZero<TEnum>(this TEnum @enum)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum).IsZero;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsZero<TEnum>(this TEnum? @enum)
            where TEnum : struct, Enum
        {
            if (@enum is null)
            {
                return false;
            }

            return new EnumInfo<TEnum>(@enum.Value).IsZero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefined<TEnum>(
            this TEnum @enum, string paramName = null)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum, paramName ?? nameof(@enum))
                .GetExceptionIfNotDefined();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefined<TEnum>(
            this TEnum? @enum, string paramName = null)
            where TEnum : struct, Enum
        {
            if (@enum is null)
            {
                return null;
            }

            return new EnumInfo<TEnum>(@enum.Value, paramName ?? nameof(@enum))
                .GetExceptionIfNotDefined();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefinedOrIsZero<TEnum>(
            this TEnum @enum, string paramName = null)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum, paramName ?? nameof(@enum))
                .GetExceptionIfNotDefinedOrIsZero();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefinedOrIsZero<TEnum>(
            this TEnum? @enum, string paramName = null)
            where TEnum : struct, Enum
        {
            if (@enum is null)
            {
                return null;
            }

            return new EnumInfo<TEnum>(@enum.Value, paramName ?? nameof(@enum))
                .GetExceptionIfNotDefinedOrIsZero();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static Exception GetExceptionIfZero<TEnum>(this TEnum @enum,
            string paramName = null)
            where TEnum : struct, Enum
            => new EnumInfo<TEnum>(@enum, paramName).GetExceptionIfZero();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static Exception GetExceptionIfZero<TEnum>(this TEnum? @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            if (@enum is null)
            {
                return null;
            }

            return new EnumInfo<TEnum>(@enum.Value, paramName)
                .GetExceptionIfZero();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        public static void ThrowIfNotDefined<TEnum>(this TEnum @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefined(@enum, paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        public static void ThrowIfNotDefined<TEnum>(this TEnum? @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefined(@enum, paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        public static void ThrowIfNotDefinedOrIsZero<TEnum>(this TEnum @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefinedOrIsZero(@enum,
                paramName ?? nameof(@enum));

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        public static void ThrowIfNotDefinedOrIsZero<TEnum>(this TEnum? @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefinedOrIsZero(@enum,
                paramName ?? nameof(@enum));

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        public static void ThrowIfZero<TEnum>(this TEnum @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfZero(@enum, paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        /// <param name="enum"></param>
        /// <param name="paramName"></param>
        public static void ThrowIfZero<TEnum>(this TEnum? @enum,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfZero(@enum, paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// A helper class for getting additional information for a
        /// <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">An <see cref="Enum"/>.</typeparam>
        class EnumInfo<TEnum> where TEnum : struct, Enum
        {
            public TEnum EnumValue { get; }

            public bool IsDefined
                => IsDefinedCore(EnumValue, UnderlyingType, Zero);

            public bool IsZero => IsZeroCore(EnumValue, Zero);

            public string ParamName { get; }

            public Type UnderlyingType
                => EnumValue.GetType().GetEnumUnderlyingType();

            public TEnum Zero => GetZeroCore(UnderlyingType);

            public EnumInfo(TEnum enumValue, string paramName = null)
            {
                EnumValue = enumValue;
                ParamName = paramName ?? nameof(enumValue);
            }

            public ArgumentException GetExceptionIfNotDefined()
                => GetExceptionIfNotDefinedCore(EnumValue, UnderlyingType, Zero,
                    ParamName);

            public ArgumentException GetExceptionIfNotDefinedOrIsZero()
                => GetExceptionIfNotDefinedOrIsZeroCore(EnumValue, UnderlyingType,
                    Zero, ParamName);

            public ArgumentException GetExceptionIfZero()
                => GetExceptionIfZeroCore(EnumValue, Zero, ParamName);

            private static ArgumentException GetExceptionIfNotDefinedCore(
                TEnum @enum, Type underlyingType, TEnum zeroValue,
                string paramName = null)
            {
                if (!IsDefinedCore(@enum, underlyingType, zeroValue))
                {
                    return new ArgumentException(paramName: paramName,
                        message: "The following value is not defined: "
                            + $"{@enum}.");
                }

                return null;
            }

            private static ArgumentException
                GetExceptionIfNotDefinedOrIsZeroCore(TEnum @enum,
                Type underlyingType, TEnum zeroValue, string paramName = null)
                => GetExceptionIfNotDefinedCore(@enum, underlyingType,
                        zeroValue, paramName)
                    ?? GetExceptionIfZeroCore(@enum, zeroValue, paramName);

            private static ArgumentException GetExceptionIfZeroCore(TEnum @enum,
                TEnum zeroValue, string paramName = null)
            {
                if (IsZeroCore(@enum, zeroValue))
                {
                    return new ArgumentException(
                        paramName: paramName,
                        message: $"The following value is not defined: {@enum}.");
                }

                return null;
            }

            private static TEnum GetZeroCore(Type underlyingType)
            {
                var enumType = typeof(TEnum);
                TEnum zeroValue;

                if (typeof(sbyte) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, (sbyte)0);
                }
                else if (typeof(byte) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, (byte)0);
                }
                else if (typeof(short) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, (short)0);
                }
                else if (typeof(ushort) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, (ushort)0);
                }
                else if (typeof(int) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, 0);
                }
                else if (typeof(uint) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, 0u);
                }
                else if (typeof(long) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, 0L);
                }
                else if (typeof(ulong) == underlyingType)
                {
                    zeroValue = (TEnum)Enum.ToObject(enumType, 0ul);
                }
                else
                {
                    throw new ArgumentException(
                        paramName: nameof(underlyingType),
                        message: $"An unexpected {nameof(Type)}: "
                            + $"{underlyingType}.");
                }

                return zeroValue;
            }

            private static bool IsDefinedCore(TEnum @enum, Type underlyingType,
                TEnum zeroValue)
            {
                var tEmumType = typeof(TEnum);

                if (tEmumType.IsDefined(
                    attributeType: typeof(FlagsAttribute),
                    inherit: false))
                {
                    if (typeof(sbyte) == underlyingType)
                    {
                        var zeroAsSByte = (sbyte)0;
                        var constantValuesAsSByte = zeroAsSByte;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsSByte |= Convert.ToSByte(
                                value: constantValue);
                        }

                        var enumValueAsSByte = Convert.ToSByte(@enum);

                        return (enumValueAsSByte != zeroAsSByte)
                            && (enumValueAsSByte
                            == (enumValueAsSByte & constantValuesAsSByte));
                    }
                    else if (typeof(byte) == underlyingType)
                    {
                        var zeroAsByte = (byte)0;
                        var constantValuesAsByte = zeroAsByte;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsByte |= Convert.ToByte(
                                value: constantValue);
                        }

                        var enumValueAsByte = Convert.ToByte(@enum);

                        return (enumValueAsByte != zeroAsByte)
                            && (enumValueAsByte
                            == (enumValueAsByte & constantValuesAsByte));
                    }
                    else if (typeof(short) == underlyingType)
                    {
                        var zeroAsInt16 = (short)0;
                        var constantValuesAsInt16 = zeroAsInt16;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsInt16 |= Convert.ToInt16(
                                value: constantValue);
                        }

                        var enumValueAsInt16 = Convert.ToInt16(@enum);

                        return (enumValueAsInt16 != zeroAsInt16)
                            && (enumValueAsInt16
                            == (enumValueAsInt16 & constantValuesAsInt16));
                    }
                    else if (typeof(ushort) == underlyingType)
                    {
                        var zeroAsUInt16 = (ushort)0;
                        var constantValuesAsUInt16 = zeroAsUInt16;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsUInt16 |= Convert.ToUInt16(
                                value: constantValue);
                        }

                        var enumValueAsUInt16 = Convert.ToUInt16(@enum);

                        return (enumValueAsUInt16 != zeroAsUInt16)
                            && (enumValueAsUInt16
                            == (enumValueAsUInt16 & constantValuesAsUInt16));
                    }
                    else if (typeof(int) == underlyingType)
                    {
                        var zeroAsInt32 = 0;
                        var constantValuesAsInt32 = zeroAsInt32;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsInt32 |= Convert.ToInt32(
                                value: constantValue);
                        }

                        var enumValueAsInt32 = Convert.ToInt32(@enum);

                        return (enumValueAsInt32 != zeroAsInt32)
                            && (enumValueAsInt32
                            == (enumValueAsInt32 & constantValuesAsInt32));
                    }
                    else if (typeof(uint) == underlyingType)
                    {
                        var zeroAsUInt32 = 0u;
                        var constantValuesAsUInt32 = zeroAsUInt32;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsUInt32 |= Convert.ToUInt32(
                                value: constantValue);
                        }

                        var enumValueAsUInt32 = Convert.ToUInt32(@enum);

                        return (enumValueAsUInt32 != zeroAsUInt32)
                            && (enumValueAsUInt32
                            == (enumValueAsUInt32 & constantValuesAsUInt32));
                    }
                    else if (typeof(long) == underlyingType)
                    {
                        var zeroAsInt64 = 0L;
                        var constantValuesAsInt64 = zeroAsInt64;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsInt64 |= Convert.ToInt64(
                                value: constantValue);
                        }

                        var enumValueAsInt64 = Convert.ToInt64(@enum);

                        return (enumValueAsInt64 != zeroAsInt64)
                            && (enumValueAsInt64
                            == (enumValueAsInt64 & constantValuesAsInt64));
                    }
                    else if (typeof(ulong) == underlyingType)
                    {
                        var zeroAsUInt64 = 0ul;
                        var constantValuesAsUInt64 = zeroAsUInt64;

                        foreach (var constantValueAsObject
                            in Enum.GetValues(tEmumType))
                        {
                            var constantValue = (TEnum)constantValueAsObject;

                            if (IsZeroCore(constantValue, zeroValue))
                            {
                                if (IsZeroCore(@enum, zeroValue))
                                {
                                    // The value is zero and zero is defined.
                                    return true;
                                }

                                // Skip zero, since no bits are set.
                                continue;
                            }

                            constantValuesAsUInt64 |= Convert.ToUInt64(
                                value: constantValue);
                        }

                        var enumValueAsUInt64 = Convert.ToUInt64(@enum);

                        return (enumValueAsUInt64 != zeroAsUInt64)
                            && (enumValueAsUInt64
                            == (enumValueAsUInt64 & constantValuesAsUInt64));
                    }

                    throw new InvalidOperationException("The following was an "
                        + $"unexpected underlying type: {underlyingType}.");
                }

                return Enum.IsDefined(tEmumType, @enum);
            }

            private static bool IsZeroCore(TEnum @enum, TEnum zeroValue)
                => @enum.Equals(zeroValue);
        }
    }
}
