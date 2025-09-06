using System;
using System.Collections.Generic;
using System.Reflection;

namespace PreferenceGroups
{
    /// <summary>
    /// Contains <see langword="static"/> helper methods for working with
    /// <see cref="Enum"/> types.
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enum"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="enum"/> is not a
        /// defined enumeration constant (see
        /// <see cref="IsDefined(Enum, string, bool)"/>).
        /// </summary>
        /// <param name="enum">What to check if is <see langword="null"/> or
        /// whether or not it is a defined enumeration constant.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefined(Enum @enum,
            string paramName = null, bool allowFlagCombinations = true)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfNotDefined(@enum.GetType(), @enum,
                enumTypeParamName: null,
                valueParamName: paramName,
                allowFlagCombinations: allowFlagCombinations);
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="value"/> are
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined constant of
        /// <paramref name="enumType"/> (see
        /// <see cref="IsDefined(Type, object, string, string, bool)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is a defined constant of
        /// <paramref name="enumType"/>.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefined(Type enumType,
            object value, string enumTypeParamName = null,
            string valueParamName = null, bool allowFlagCombinations = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            var exception = GetExceptionIfNullOrNotEnum(enumType,
                enumTypeParamName);

            if (!(exception is null))
            {
                return exception;
            }

            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                return new ArgumentNullException(valueParamName);
            }

            if (IsNotDefined(enumType, value, enumTypeParamName, valueParamName,
                allowFlagCombinations))
            {
                return new ArgumentException(paramName: valueParamName,
                    message: $"The value {value}, is not a defined enumeration "
                        + $"constant of {enumType}.");
            }

            return null;
        }

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined enumeration constant of
        /// <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefined<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
            => GetExceptionIfNotDefined(typeof(TEnum), value,
                enumTypeParamName: null,
                valueParamName: paramName ?? nameof(value),
                allowFlagCombinations: allowFlagCombinations);

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="value"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="value"/> is not a
        /// defined constant of <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefined<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfNotDefined(typeof(TEnum), value,
                enumTypeParamName: null,
                valueParamName: paramName,
                allowFlagCombinations: allowFlagCombinations);
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enum"/> is <see langword="null"/>, or an
        /// <see cref="ArgumentException"/> if <paramref name="enum"/> is not a
        /// defined enumeration constant (see
        /// <see cref="IsDefined(Enum, string, bool)"/>) or is zero (see
        /// <see cref="IsZero(Enum, string)"/>).
        /// </summary>
        /// <param name="enum">What to check if it is <see langword="null"/> or
        /// if it is not a defined enumeration constant or if it is
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefinedOrIsZero(Enum @enum,
            string paramName = null, bool allowFlagCombinations = true)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfNotDefinedOrIsZero(@enum.GetType(), @enum,
                enumTypeParamName: null,
                valueParamName: paramName,
                allowFlagCombinations: allowFlagCombinations);
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="value"/> are
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined constant of
        /// <paramref name="enumType"/> (see
        /// <see cref="IsDefined(Type, object, string, string, bool)"/>) or is
        /// zero (see <see cref="IsZero(Type, object, string, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is a defined constant of
        /// <paramref name="enumType"/> or if it is zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefinedOrIsZero(Type enumType,
            object value, string enumTypeParamName = null,
            string valueParamName = null, bool allowFlagCombinations = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            valueParamName = valueParamName ?? nameof(value);
            var exception = GetExceptionIfNotDefined(enumType, value,
                enumTypeParamName, valueParamName, allowFlagCombinations);

            if (!(exception is null))
            {
                return exception;
            }

            exception = GetExceptionIfZero(enumType, value, enumTypeParamName,
                valueParamName);

            if (!(exception is null))
            {
                return exception;
            }

            return null;
        }

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined enumeration constant of
        /// <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>) or is zero (see
        /// <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefinedOrIsZero<TEnum>(
            TEnum value, string paramName = null,
            bool allowFlagCombinations = true)
            where TEnum : struct, Enum
            => GetExceptionIfNotDefinedOrIsZero(typeof(TEnum), value,
                enumTypeParamName: null,
                valueParamName: paramName ?? nameof(value),
                allowFlagCombinations: allowFlagCombinations);

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="value"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="value"/> is not a
        /// defined constant of <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>) or is zero (see
        /// <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/> or if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNotDefinedOrIsZero<TEnum>(
            TEnum? value, string paramName = null,
            bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfNotDefinedOrIsZero(value.Value, paramName,
                allowFlagCombinations);
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="type"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is a
        /// <see cref="Nullable{T}"/> but not of an <see cref="Enum"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/>.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is
        /// used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNullOrNotEnum(Type type,
            string paramName = null)
        {
            paramName = paramName ?? nameof(type);

            if (type is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfNullOrNotEnum(type, out _, out _, paramName);
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="type"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is a
        /// <see cref="Nullable{T}"/> but not of an <see cref="Enum"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/>.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="enumTypeInfo">If no <see cref="Exception"/> is returned,
        /// then this contains the <see cref="EnumTypeInfo"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is
        /// used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNullOrNotEnum(Type type,
            out EnumTypeInfo enumTypeInfo, string paramName = null)
        {
            paramName = paramName ?? nameof(type);
            enumTypeInfo = null;

            if (type is null)
            {
                return new ArgumentNullException(paramName);
            }

            if (EnumTypeInfo.IsEnum(type, out enumTypeInfo, paramName))
            {
                return null;
            }

            return new ArgumentException(paramName: paramName,
                message: "Does not represent an Enum or a Nullable of "
                    + $"Enum, it represents: {type}.");
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="type"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is a
        /// <see cref="Nullable{T}"/> but not of an <see cref="Enum"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/>.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="typeIsNullable">Is <see langword="true"/> if
        /// <paramref name="type"/> is a <see cref="Nullable{T}"/> of
        /// <see cref="Enum"/>, otherwise <see langword="false"/>.</param>
        /// <param name="enumType">If this method returns
        /// <see langword="true"/>, then this is a <see cref="Type"/> of
        /// <see cref="Enum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is
        /// used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfNullOrNotEnum(Type type,
            out bool typeIsNullable, out Type enumType, string paramName = null)
        {
            paramName = paramName ?? nameof(type);
            typeIsNullable = false;
            enumType = null;

            if (type is null)
            {
                return new ArgumentNullException(paramName);
            }

            if (IsEnum(type, out typeIsNullable, out enumType, paramName))
            {
                return null;
            }

            return new ArgumentException(paramName: paramName,
                message: "Does not represent an Enum or a Nullable of "
                    + $"Enum, it represents: {type}.");
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enum"/> is <see langword="null"/>, or an
        /// <see cref="ArgumentException"/> if <paramref name="enum"/> is zero
        /// (see <see cref="IsZero(Enum, string)"/>).
        /// </summary>
        /// <param name="enum">What to check if it is <see langword="null"/> or
        /// if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfZero(Enum @enum,
            string paramName = null)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfZero(@enum.GetType(), @enum,
                valueParamName: paramName);
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="value"/> are
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is zero (see
        /// <see cref="IsZero(Type, object, string, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfZero(Type enumType,
            object value, string enumTypeParamName = null,
            string valueParamName = null)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            var exception = GetExceptionIfNullOrNotEnum(enumType,
                enumTypeParamName);

            if (!(exception is null))
            {
                return exception;
            }

            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                return new ArgumentNullException(valueParamName);
            }

            if (IsZero(enumType, value, enumTypeParamName, valueParamName))
            {
                return new ArgumentException(paramName: valueParamName,
                    message: $"The value {value}, is the zero constant of the "
                        + $"enumeration {enumType}.");
            }

            return null;
        }

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is zero (see
        /// <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfZero<TEnum>(TEnum value,
            string paramName = null)
            where TEnum : struct, Enum
            => GetExceptionIfZero(typeof(TEnum), value,
                enumTypeParamName: null,
                valueParamName: paramName ?? nameof(value));

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="value"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="value"/> is zero
        /// (see <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <returns></returns>
        public static Exception GetExceptionIfZero<TEnum>(TEnum? value,
            string paramName = null)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                return new ArgumentNullException(paramName);
            }

            return GetExceptionIfZero(value.Value, paramName);
        }

        /// <summary>
        /// Returns the <see cref="Array"/> of <see cref="Enum"/> of the values of
        /// constants of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// retrieve the <see cref="Array"/> of <see cref="Enum"/> its values of
        /// constants.</param>
        /// <param name="paramName">The name of the <paramref name="enumType"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enumType"/> is
        /// used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// is <see langword="null"/>.</exception>
        public static Enum[] GetValues(Type enumType, string paramName = null)
        {
            paramName = paramName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType, paramName);
            var values = new List<Enum>();

            foreach (var valueAsObject in Enum.GetValues(enumType))
            {
                if (!(valueAsObject is null))
                {
                    var value = ToEnum(enumType, valueAsObject, paramName);
                    values.Add(value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum[] GetValues<TEnum>()
            where TEnum : struct, Enum
        {
            var values = new List<TEnum>();

            foreach (var valueAsObject in Enum.GetValues(typeof(TEnum)))
            {
                values.Add((TEnum)valueAsObject);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <see cref="Nullable{T}"/>
        /// <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum?[] GetValuesAsNullable<TEnum>()
            where TEnum : struct, Enum
        {
            var values = new List<TEnum?>();

            foreach (var valueAsObject in Enum.GetValues(typeof(TEnum)))
            {
                values.Add((TEnum)valueAsObject);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns the <see cref="Array"/> of <see cref="Enum"/> of the values
        /// of constants of <paramref name="enumType"/>, except any that are
        /// equal to zero (see <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// retrieve the <see cref="Array"/> of <see cref="Enum"/> its values of
        /// constants, except any that are equal to zero (see
        /// <see cref="GetZero(Type, string)"/>).</param>
        /// <param name="paramName">The name of the <paramref name="enumType"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enumType"/> is
        /// used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// is <see langword="null"/>.</exception>
        public static Enum[] GetValuesNotZero(Type enumType,
            string paramName = null)
        {
            paramName = paramName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType, paramName);
            var zeroValue = GetZero(enumType, paramName);
            var values = new List<Enum>();

            foreach (var valueAsObject in Enum.GetValues(enumType))
            {
                if (valueAsObject is null)
                {
                    continue;
                }

                var value = ToEnum(enumType, valueAsObject, paramName);

                if (!zeroValue.Equals(value))
                {
                    values.Add(value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method) that are not zero (see the
        /// <see cref="GetZero{TEnum}()"/> method).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum[] GetValuesNotZero<TEnum>()
            where TEnum : struct, Enum
        {
            var zeroValue = GetZero<TEnum>();
            var values = new List<TEnum>();

            foreach (var valueAsObject in Enum.GetValues(typeof(TEnum)))
            {
                var value = (TEnum)valueAsObject;

                if (!zeroValue.Equals(value))
                {
                    values.Add(value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns an <see cref="Array"/> of <typeparamref name="TEnum"/> with
        /// all of its values (see the <see cref="Enum.GetValues(Type)"/>
        /// method) that are not zero (see the
        /// <see cref="GetZero{TEnum}()"/> method).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum?[] GetValuesNotZeroAsNullable<TEnum>()
            where TEnum : struct, Enum
        {
            var zeroValue = GetZero<TEnum>();
            var values = new List<TEnum?>();

            foreach (var valueAsObject in Enum.GetValues(typeof(TEnum)))
            {
                var value = (TEnum)valueAsObject;

                if (!zeroValue.Equals(value))
                {
                    values.Add(value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns the zero enumeration member of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// retreive its zero enumeration member.</param>
        /// <param name="paramName">The name of the <paramref name="enumType"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enumType"/> is
        /// used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// is <see langword="null"/>.</exception>
        public static Enum GetZero(Type enumType, string paramName = null)
        {
            paramName = paramName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType, paramName);
            var underlyingType = enumType.GetEnumUnderlyingType();
            Enum enumZero;

            if (typeof(sbyte) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, (sbyte)0);
            }
            else if (typeof(byte) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, (byte)0);
            }
            else if (typeof(short) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, (short)0);
            }
            else if (typeof(ushort) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, (ushort)0);
            }
            else if (typeof(int) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, 0);
            }
            else if (typeof(uint) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, 0U);
            }
            else if (typeof(long) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, 0L);
            }
            else if (typeof(ulong) == underlyingType)
            {
                enumZero = (Enum)Enum.ToObject(enumType, 0UL);
            }
            else
            {
                throw new ArgumentException(paramName: nameof(paramName),
                    message: $"An unexpected enumeration "
                        + $"{nameof(underlyingType)} of {underlyingType}.");
            }

            return enumZero;
        }

        /// <summary>
        /// Returns the value of <typeparamref name="TEnum"/> that is equivalent
        /// to zero as its <see cref="Enum.GetUnderlyingType(Type)"/>.
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum GetZero<TEnum>() where TEnum : struct, Enum
            => (TEnum)GetZero(typeof(TEnum));

        /// <summary>
        /// Returns the value of <typeparamref name="TEnum"/> that is equivalent
        /// to zero as its <see cref="Enum.GetUnderlyingType(Type)"/>.
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static TEnum? GetZeroAsNullable<TEnum>()
            where TEnum : struct, Enum
            => GetZero<TEnum>();

        /// <summary>
        /// Determines whether or not <paramref name="enumType"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see the
        /// <see cref="MemberInfo.IsDefined(Type, bool)"/> method).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="paramName">The name of the <paramref name="enumType"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enumType"/> is
        /// used.</param>
        /// <returns><see langword="true"/> if <paramref name="enumType"/> has
        /// the <see cref="FlagsAttribute"/> applied to it, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// is <see langword="null"/>.</exception>
        public static bool HasFlags(Type enumType, string paramName = null)
        {
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                paramName ?? nameof(enumType));

            return enumType.IsDefined(
                attributeType: typeof(FlagsAttribute),
                inherit: false);
        }

        /// <summary>
        /// Determines whether or not <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see the
        /// <see cref="MemberInfo.IsDefined(Type, bool)"/> method).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <returns></returns>
        public static bool HasFlags<TEnum>() where TEnum : struct, Enum
            => HasFlags(typeof(TEnum));

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="enum"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <param name="enum">What to check is a defined constant.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsDefined(Enum @enum, string paramName = null,
            bool allowFlagCombinations = true)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return IsDefined(@enum.GetType(), @enum,
                enumTypeParamName: null,
                valueParamName: paramName,
                allowFlagCombinations: allowFlagCombinations);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a defined
        /// constant of <paramref name="enumType"/> (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it (see <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if <paramref name="value"/> is defined.</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is a defined constant of
        /// <paramref name="enumType"/>.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static bool IsDefined(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null,
            bool allowFlagCombinations = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            // If the enumType does not have the FlagsAttribute applied to it or
            // undefined flag combined combinations are not allowed, then just
            // call the default behavior.
            if (!HasFlags(enumType, enumTypeParamName)
                || !allowFlagCombinations)
            {
                return Enum.IsDefined(enumType, value);
            }

            // Flag combinations are allowed and enumType has the FlagsAttribute
            // applied to it, then we need to check if value is defined by any
            // of the combinations of the constant flags.
            var enumValue = ToEnum(enumType, value, valueParamName);
            var enumZero = GetZero(enumType, enumTypeParamName);
            var underlyingType = Enum.GetUnderlyingType(enumType);

            if (typeof(sbyte) == underlyingType)
            {
                var zeroAsSByte = (sbyte)0;
                var constantValuesAsSByte = zeroAsSByte;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsSByte |= Convert.ToSByte(constantValue);
                }

                var enumValueAsSByte = Convert.ToSByte(enumValue);

                return (enumValueAsSByte != zeroAsSByte)
                    && (enumValueAsSByte
                    == (enumValueAsSByte & constantValuesAsSByte));
            }
            else if (typeof(byte) == underlyingType)
            {
                var zeroAsByte = (byte)0;
                var constantValuesAsByte = zeroAsByte;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsByte |= Convert.ToByte(constantValue);
                }

                var enumValueAsByte = Convert.ToByte(enumValue);

                return (enumValueAsByte != zeroAsByte)
                    && (enumValueAsByte
                    == (enumValueAsByte & constantValuesAsByte));
            }
            else if (typeof(short) == underlyingType)
            {
                var zeroAsInt16 = (short)0;
                var constantValuesAsInt16 = zeroAsInt16;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsInt16 |= Convert.ToInt16(constantValue);
                }

                var enumValueAsInt16 = Convert.ToInt16(enumValue);

                return (enumValueAsInt16 != zeroAsInt16)
                    && (enumValueAsInt16
                    == (enumValueAsInt16 & constantValuesAsInt16));
            }
            else if (typeof(ushort) == underlyingType)
            {
                var zeroAsUInt16 = (ushort)0;
                var constantValuesAsUInt16 = zeroAsUInt16;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsUInt16 |= Convert.ToUInt16(constantValue);
                }

                var enumValueAsUInt16 = Convert.ToUInt16(enumValue);

                return (enumValueAsUInt16 != zeroAsUInt16)
                    && (enumValueAsUInt16
                    == (enumValueAsUInt16 & constantValuesAsUInt16));
            }
            else if (typeof(int) == underlyingType)
            {
                var zeroAsInt32 = 0;
                var constantValuesAsInt32 = zeroAsInt32;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsInt32 |= Convert.ToInt32(constantValue);
                }

                var enumValueAsInt32 = Convert.ToInt32(enumValue);

                return (enumValueAsInt32 != zeroAsInt32)
                    && (enumValueAsInt32
                    == (enumValueAsInt32 & constantValuesAsInt32));
            }
            else if (typeof(uint) == underlyingType)
            {
                var zeroAsUInt32 = 0U;
                var constantValuesAsUInt32 = zeroAsUInt32;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsUInt32 |= Convert.ToUInt32(constantValue);
                }

                var enumValueAsUInt32 = Convert.ToUInt32(enumValue);

                return (enumValueAsUInt32 != zeroAsUInt32)
                    && (enumValueAsUInt32
                    == (enumValueAsUInt32 & constantValuesAsUInt32));
            }
            else if (typeof(long) == underlyingType)
            {
                var zeroAsInt64 = 0L;
                var constantValuesAsInt64 = zeroAsInt64;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsInt64 |= Convert.ToInt64(constantValue);
                }

                var enumValueAsInt64 = Convert.ToInt64(enumValue);

                return (enumValueAsInt64 != zeroAsInt64)
                    && (enumValueAsInt64
                    == (enumValueAsInt64 & constantValuesAsInt64));
            }
            else if (typeof(ulong) == underlyingType)
            {
                var zeroAsUInt64 = 0UL;
                var constantValuesAsUInt64 = zeroAsUInt64;

                foreach (var constantValueAsObject in Enum.GetValues(enumType))
                {
                    var constantValue = (Enum)constantValueAsObject;

                    if (constantValue.Equals(enumZero))
                    {
                        if (enumValue.Equals(enumZero))
                        {
                            // The value is zero and zero is defined.
                            return true;
                        }

                        // Skip zero, since no bits are set.
                        continue;
                    }

                    constantValuesAsUInt64 |= Convert.ToUInt64(constantValue);
                }

                var enumValueAsUInt64 = Convert.ToUInt64(enumValue);

                return (enumValueAsUInt64 != zeroAsUInt64)
                    && (enumValueAsUInt64
                    == (enumValueAsUInt64 & constantValuesAsUInt64));
            }

            throw new InvalidOperationException("The following was an "
                + $"unexpected underlying type: {underlyingType}.");
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is a defined constant.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static bool IsDefined<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
            => IsDefined(typeof(TEnum), value,
                enumTypeParamName: null,
                valueParamName: paramName ?? nameof(value),
                allowFlagCombinations: allowFlagCombinations);

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is a defined constant.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsDefined<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return IsDefined(value.Value, paramName, allowFlagCombinations);
        }

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="enum"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself. And, if
        /// <paramref name="enum"/> is defined, then determines whether or not
        /// <paramref name="enum"/> is not equivalent to the zero enumeration
        /// value (see <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enum">What to check is a defined constant and is not
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsDefinedAndNotZero(Enum @enum,
            string paramName = null, bool allowFlagCombinations = true)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return IsDefined(@enum, paramName, allowFlagCombinations)
                && IsNotZero(@enum, paramName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a defined
        /// constant of <paramref name="enumType"/> (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it (see <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when
        /// <paramref name="value"/> is a single defined constant or a
        /// combination of defined constants, even if that combination is not a
        /// specific constant itself. And if <paramref name="value"/> is
        /// defined, then determines whether or not <paramref name="value"/> is
        /// not equivalent to the zero enumeration value of
        /// <paramref name="enumType"/> (see
        /// <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if <paramref name="value"/> is defined and is not
        /// zero.</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is a defined constant of
        /// <paramref name="enumType"/> and is not zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static bool IsDefinedAndNotZero(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null,
            bool allowFlagCombinations = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            return IsDefined(enumType, value, enumTypeParamName, valueParamName,
                   allowFlagCombinations)
                && IsNotZero(enumType, value, enumTypeParamName,
                    valueParamName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself. And if
        /// <paramref name="value"/> is defined, then determines whether or not
        /// <paramref name="value"/> is not equivalent to the zero enumeration
        /// value (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is a defined constant and is not
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static bool IsDefinedAndNotZero<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            return IsDefined(value, paramName, allowFlagCombinations)
                && IsNotZero(value, paramName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself. And if
        /// <paramref name="value"/> is defined, then determines whether or not
        /// <paramref name="value"/> is not equivalent to the zero enumeration
        /// value (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is a defined constant and is not
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsDefinedAndNotZero<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return IsDefined(value.Value, paramName, allowFlagCombinations)
                && IsNotZero(value.Value, paramName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="type"/> is an
        /// <see cref="Enum"/> or is a <see cref="Nullable{T}"/> of
        /// <see cref="Enum"/>. If either is <see langword="true"/>, then
        /// <paramref name="enumType"/> will contain <paramref name="type"/> or
        /// the underlying type (see
        /// <see cref="Nullable.GetUnderlyingType(Type)"/>), respectively.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeIsNullable">Is <see langword="true"/> if
        /// <paramref name="type"/> is a <see cref="Nullable{T}"/> of
        /// <see cref="Enum"/>, otherwise <see langword="false"/>.</param>
        /// <param name="enumType">If this method returns
        /// <see langword="true"/>, then this is a <see cref="Type"/> of
        /// <see cref="Enum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is used.</param>
        /// <returns><see langword="true"/> if <paramref name="type"/> is an
        /// <see cref="Enum"/> or is a <see cref="Nullable{T}"/> of
        /// <see cref="Enum"/>, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsEnum(Type type, out bool typeIsNullable,
            out Type enumType, string paramName = null)
        {
            if (type is null)
            {
                throw new ArgumentNullException(paramName ?? nameof(type));
            }

            typeIsNullable = false;
            enumType = null;

            // Determine if type is Nullable.
            var underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType is null)
            {
                // type is not Nullable.
                if (type.IsEnum)
                {
                    enumType = type;

                    return true;
                }
            }
            // type is Nullable.
            else
            {
                typeIsNullable = true;

                if (underlyingType.IsEnum)
                {
                    enumType = underlyingType;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is not a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="false"/> is retured when <paramref name="enum"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <param name="enum">What to check is not a defined constant.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsNotDefined(Enum @enum, string paramName = null,
            bool allowFlagCombinations = true)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return !IsDefined(@enum, paramName, allowFlagCombinations);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not a defined
        /// constant of <paramref name="enumType"/> (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it (see <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="false"/> is retured when
        /// <paramref name="value"/> is a single defined constant or a
        /// combination of defined constants, even if that combination is not a
        /// specific constant itself.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if <paramref name="value"/> is not defined.</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is not a defined constant of
        /// <paramref name="enumType"/>.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static bool IsNotDefined(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null,
            bool allowFlagCombinations = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            return !IsDefined(enumType, value, enumTypeParamName,
                valueParamName, allowFlagCombinations);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is not a defined constant.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static bool IsNotDefined<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
            => !IsDefined(value, paramName ?? nameof(value),
                allowFlagCombinations);

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="true"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself.
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is not a defined constant.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsNotDefined<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return !IsDefined(value.Value, paramName, allowFlagCombinations);
        }

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is not a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="false"/> is retured when <paramref name="enum"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself. Or, if
        /// <paramref name="enum"/> is defined, then determines whether or not
        /// <paramref name="enum"/> is equivalent to the zero enumeration value
        /// (see <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enum">What to check is not a defined constant or is
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsNotDefinedOrIsZero(Enum @enum,
            string paramName = null, bool allowFlagCombinations = true)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return IsNotDefined(@enum, paramName, allowFlagCombinations)
                || IsZero(@enum, paramName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not a defined
        /// constant of <paramref name="enumType"/> (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it (see <see cref="HasFlags(Type, string)"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="false"/> is retured when
        /// <paramref name="value"/> is a single defined constant or a
        /// combination of defined constants, even if that combination is not a
        /// specific constant itself. Or if <paramref name="value"/> is defined,
        /// then determines whether or not <paramref name="value"/> is equivalent
        /// to the zero enumeration value of <paramref name="enumType"/> (see
        /// <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if <paramref name="value"/> is not defined or is zero.</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is not a defined constant of
        /// <paramref name="enumType"/> or is zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static bool IsNotDefinedOrIsZero(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null,
            bool allowFlagCombinations = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            return IsNotDefined(enumType, value, enumTypeParamName,
                    valueParamName, allowFlagCombinations)
                || IsZero(enumType, value, enumTypeParamName,
                    valueParamName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="false"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself. Or if
        /// <paramref name="value"/> is defined, then determines whether or not
        /// <paramref name="value"/> is equivalent to the zero enumeration value
        /// (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is not a defined constant or is
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        public static bool IsNotDefinedOrIsZero<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
            => IsNotDefined(value, paramName ?? nameof(value),
                    allowFlagCombinations)
                || IsZero(value);

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not a defined
        /// enumeration constant (see
        /// <see cref="Enum.IsDefined(Type, object)"/>), or if the
        /// <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it (see
        /// <see cref="HasFlags{TEnum}()"/>) and
        /// <paramref name="allowFlagCombinations"/> is <see langword="true"/>
        /// then <see langword="false"/> is retured when <paramref name="value"/>
        /// is a single defined constant or a combination of defined constants,
        /// even if that combination is not a specific constant itself. Or if
        /// <paramref name="value"/> is defined, then determines whether or not
        /// <paramref name="value"/> is equivalent to the zero enumeration value
        /// (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is not a defined constant or is
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsNotDefinedOrIsZero<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            paramName = paramName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return IsNotDefined(value.Value, paramName, allowFlagCombinations)
                || IsZero(value.Value);
        }

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is not equivalent
        /// to the zero enumeration value (see
        /// <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enum">What to check is not zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enum"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsNotZero(Enum @enum, string paramName = null)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return !IsZero(@enum, paramName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not equivalent
        /// to the zero enumeration value of <paramref name="enumType"/> (see
        /// <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if <paramref name="value"/> is not equivalent to zero.</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is not zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static bool IsNotZero(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            return !IsZero(enumType, value, enumTypeParamName);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not equivalent
        /// to the zero enumeration value (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is not zero.</param>
        /// <returns></returns>
        public static bool IsNotZero<TEnum>(TEnum value)
            where TEnum : struct, Enum
            => !IsZero(value);

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is not equivalent
        /// to the zero enumeration value (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is not zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsNotZero<TEnum>(TEnum? value,
            string paramName = null)
            where TEnum : struct, Enum
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName ?? nameof(value));
            }

            return !IsZero(value.Value);
        }

        /// <summary>
        /// Determines whether or not <paramref name="enum"/> is equivalent
        /// to the zero enumeration value (see
        /// <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enum">What to check is  zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enum"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsZero(Enum @enum, string paramName = null)
        {
            paramName = paramName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return @enum.Equals(GetZero(@enum.GetType(), paramName));
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is equivalent
        /// to the zero enumeration value of <paramref name="enumType"/> (see
        /// <see cref="GetZero(Type, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if <paramref name="value"/> is equivalent to zero.</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static bool IsZero(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            var enumValue = ToEnum(enumType, value, enumTypeParamName,
                valueParamName);
            var zeroValue = GetZero(enumType, enumTypeParamName);

            return enumValue.Equals(zeroValue);
        }

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is equivalent
        /// to the zero enumeration value (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is zero.</param>
        /// <returns></returns>
        public static bool IsZero<TEnum>(TEnum value)
            where TEnum : struct, Enum
            => value.Equals(GetZero<TEnum>());

        /// <summary>
        /// Determines whether or not <paramref name="value"/> is equivalent
        /// to the zero enumeration value (see <see cref="GetZero{TEnum}()"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsZero<TEnum>(TEnum? value, string paramName = null)
            where TEnum : struct, Enum
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName ?? nameof(value));
            }

            return value.Value.Equals(GetZero<TEnum>());
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enum"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="enum"/> is not a
        /// defined enumeration constant (see
        /// <see cref="IsDefined(Enum, string, bool)"/>).
        /// </summary>
        /// <param name="enum">What to check if is <see langword="null"/> or
        /// whether or not it is a defined enumeration constant.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> is not a
        /// defined enumeration constant.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfNotDefined(Enum @enum,
            string paramName = null, bool allowFlagCombinations = true)
        {
            var exeption = GetExceptionIfNotDefined(@enum,
                paramName ?? nameof(@enum), allowFlagCombinations);

            if (!(exeption is null))
            {
                throw exeption;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="value"/> are
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined constant of
        /// <paramref name="enumType"/> (see
        /// <see cref="IsDefined(Type, object, string, string, bool)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is a defined constant of
        /// <paramref name="enumType"/>.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is
        /// not an <see cref="Enum"/> or <paramref name="value"/> is not a
        /// defined constant of <paramref name="enumType"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static void ThrowIfNotDefined(Type enumType,
            object value, string enumTypeParamName = null,
            string valueParamName = null, bool allowFlagCombinations = true)
        {
            var exception = GetExceptionIfNotDefined(enumType, value,
                enumTypeParamName ?? nameof(enumType),
                valueParamName ?? nameof(value), allowFlagCombinations);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined enumeration constant of
        /// <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not
        /// a defined constant of <typeparamref name="TEnum"/>.</exception>
        public static void ThrowIfNotDefined<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefined(value,
                paramName ?? nameof(value), allowFlagCombinations);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="value"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="value"/> is not a
        /// defined constant of <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not
        /// a defined constant of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfNotDefined<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefined(value,
                paramName ?? nameof(value), allowFlagCombinations);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enum"/> is <see langword="null"/>, or an
        /// <see cref="ArgumentException"/> if <paramref name="enum"/> is not a
        /// defined enumeration constant (see
        /// <see cref="IsDefined(Enum, string, bool)"/>) or is zero (see
        /// <see cref="IsZero(Enum, string)"/>).
        /// </summary>
        /// <param name="enum">What to check if it is <see langword="null"/> or
        /// if it is not a defined enumeration constant or if it is
        /// zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <paramref name="enum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> is not a
        /// defined enumeration constant or is zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfNotDefinedOrIsZero(Enum @enum,
            string paramName = null, bool allowFlagCombinations = true)
        {
            var exeption = GetExceptionIfNotDefinedOrIsZero(@enum,
                paramName ?? nameof(@enum), allowFlagCombinations);

            if (!(exeption is null))
            {
                throw exeption;
            }
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="value"/> are
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined constant of
        /// <paramref name="enumType"/> (see
        /// <see cref="IsDefined(Type, object, string, string, bool)"/>) or is
        /// zero (see <see cref="IsZero(Type, object, string, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is a defined constant of
        /// <paramref name="enumType"/> or if it is zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// <paramref name="enumType"/> has the <see cref="FlagsAttribute"/>
        /// applied to it, then combinations of defined constants are considered
        /// defined, even when a combination is not a defined constant itself.
        /// Otherwise, if <see langword="false"/>, then only
        /// specified constants are considered defined and only the 
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is
        /// not an <see cref="Enum"/>, or <paramref name="value"/> is not a
        /// defined constant of <paramref name="enumType"/> or is
        /// zero.</exception>
        public static void ThrowIfNotDefinedOrIsZero(Type enumType,
            object value, string enumTypeParamName = null,
            string valueParamName = null, bool allowFlagCombinations = true)
        {
            var exception = GetExceptionIfNotDefinedOrIsZero(enumType, value,
                enumTypeParamName ?? nameof(enumType),
                valueParamName ?? nameof(value), allowFlagCombinations);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is not a defined enumeration constant of
        /// <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>) or is zero (see
        /// <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not
        /// a defined enumeration constant of <typeparamref name="TEnum"/> or is
        /// zero.</exception>
        public static void ThrowIfNotDefinedOrIsZero<TEnum>(TEnum value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefinedOrIsZero(value,
                paramName ?? nameof(value), allowFlagCombinations);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> if
        /// <paramref name="value"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="value"/> is not a
        /// defined constant of <typeparamref name="TEnum"/> (see
        /// <see cref="IsDefined{TEnum}(TEnum, string, bool)"/>) or is zero (see
        /// <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is a defined enumeration
        /// constant of <typeparamref name="TEnum"/> or if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <param name="allowFlagCombinations">If <see langword="true"/> and
        /// the <see cref="Type"/> of <typeparamref name="TEnum"/> has the
        /// <see cref="FlagsAttribute"/> applied to it, then combinations of
        /// defined constants are considered defined, even when that combination
        /// is not a defined constant itself. Otherwise, if
        /// <see langword="false"/>, then only specified constants are
        /// considered defined and only the
        /// <see cref="Enum.IsDefined(Type, object)"/> method is used.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfNotDefinedOrIsZero<TEnum>(TEnum? value,
            string paramName = null, bool allowFlagCombinations = true)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfNotDefinedOrIsZero(value,
                paramName ?? nameof(value), allowFlagCombinations);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="enumType"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="paramName">The name of the <paramref name="enumType"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enumType"/> is
        /// used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// is <see langword="null"/>.</exception>
        public static void ThrowIfNullOrNotEnum(Type enumType,
            string paramName = null)
        {
            paramName = paramName ?? nameof(enumType);

            var exception = GetExceptionIfNullOrNotEnum(enumType, paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="type"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is a
        /// <see cref="Nullable{T}"/> but not of an <see cref="Enum"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/>.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="typeIsNullable">Is <see langword="true"/> if
        /// <paramref name="type"/> is a <see cref="Nullable{T}"/> of
        /// <see cref="Enum"/>, otherwise <see langword="false"/>.</param>
        /// <param name="enumType">If this method returns
        /// <see langword="true"/>, then this is a <see cref="Type"/> of
        /// <see cref="Enum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is
        /// used.</param>
        /// <exception cref="ArgumentException"><paramref name="type"/> is not a
        /// <see cref="Nullable{T}"/> of an <see cref="Enum"/> or is
        /// not an <see cref="Enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfNullOrNotEnum(Type type,
            out bool typeIsNullable, out Type enumType, string paramName = null)
        {
            paramName = paramName ?? nameof(type);

            var exception = GetExceptionIfNullOrNotEnum(type,
                out typeIsNullable, out enumType, paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="type"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="type"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/> or not a
        /// <see cref="Nullable{T}"/> of an <see cref="Enum"/>.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> or not a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>.</param>
        /// <param name="enumTypeInfo">If no <see cref="Exception"/> is
        /// returned, then this contains the <see cref="EnumTypeInfo"/>
        /// <see cref="Type"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is
        /// used.</param>
        /// <exception cref="ArgumentException"><paramref name="type"/>
        /// does not represent an <see cref="Enum"/> or a
        /// <see cref="Nullable{T}"/> of an <see cref="Enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>
        /// is <see langword="null"/>.</exception>
        public static void ThrowIfNullOrNotEnum(Type type,
            out EnumTypeInfo enumTypeInfo, string paramName = null)
        {
            paramName = paramName ?? nameof(type);

            var exception = GetExceptionIfNullOrNotEnum(type, out enumTypeInfo,
                paramName);

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumTypeInfo"/> or <paramref name="enum"/> is
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumTypeInfo"/> does not equal the <see cref="Type"/>
        /// of <paramref name="enum"/>.
        /// </summary>
        /// <param name="enumTypeInfo">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is equal to the <see cref="Type"/> of
        /// <paramref name="enum"/>.</param>
        /// <param name="enum">What to check if its <see cref="Type"/> is equal
        /// to <paramref name="enumTypeInfo"/></param>
        /// <param name="enumTypeInfoParamName">The name of the
        /// <paramref name="enumTypeInfo"/> <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumTypeInfo"/> is used.</param>
        /// <param name="enumParamName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumTypeInfo"/>
        /// does not equal the <see cref="Type"/> of
        /// <paramref name="enum"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumTypeInfo"/> or <paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfTypeNotEqual(EnumTypeInfo enumTypeInfo,
            Enum @enum, string enumTypeInfoParamName = null,
            string enumParamName = null)
        {
            enumTypeInfoParamName = enumTypeInfoParamName
                ?? nameof(enumTypeInfo);
            enumParamName = enumParamName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(enumParamName);
            }

            var enumType = @enum.GetType();

            if (!enumTypeInfo.Equals(enumType))
            {
                throw new ArgumentException(paramName: enumParamName,
                    message: $"Expected a {nameof(Type)} of "
                        + $"{enumTypeInfo.EnumType} or "
                        + $"{enumTypeInfo.NullableType}, but have a "
                        + $"{nameof(Type)} of {enumType}.");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="enum"/> is
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> does not equal the <see cref="Type"/> of
        /// <paramref name="enum"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is equal to the <see cref="Type"/> of
        /// <paramref name="enum"/>. Must be an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>).</param>
        /// <param name="enum">What to check if its <see cref="Type"/> is equal
        /// to <paramref name="enumType"/>.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="enumParamName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>, or <paramref name="enumType"/> does not
        /// equal the <see cref="Type"/> of <paramref name="enum"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="enum"/> is <see langword="null"/>.</exception>
        public static void ThrowIfTypeNotEqual(Type enumType, Enum @enum,
            string enumTypeParamName = null, string enumParamName = null)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            enumParamName = enumParamName ?? nameof(@enum);

            if (@enum is null)
            {
                throw new ArgumentNullException(enumParamName);
            }

            ThrowIfTypeNotEqual(enumType, @enum.GetType(), enumTypeParamName,
                enumParamName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="otherEnumType"/> is
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> or <paramref name="otherEnumType"/> is
        /// not an <see cref="Enum"/> (see <see cref="Type.IsEnum"/>), or an
        /// <see cref="ArgumentException"/> if <paramref name="enumType"/> does
        /// not equal <paramref name="otherEnumType"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is equal to <paramref name="otherEnumType"/>. Must be an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="otherEnumType">The <see cref="Enum"/>
        /// <see cref="Type"/> to check if is equal to
        /// <paramref name="enumType"/>. Must be an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>).</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="otherEnumTypeParamName">The name of the
        /// <paramref name="otherEnumType"/> <see langword="param"/>. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="otherEnumType"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> or
        /// <paramref name="otherEnumType"/> returns <see langword="false"/> for
        /// the <see cref="Type.IsEnum"/> <see langword="property"/>, or
        /// <paramref name="enumType"/> does not equal
        /// <paramref name="otherEnumType"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="otherEnumType"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfTypeNotEqual(Type enumType,
            Type otherEnumType, string enumTypeParamName = null,
            string otherEnumTypeParamName = null)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            otherEnumTypeParamName = otherEnumTypeParamName
                ?? nameof(otherEnumType);
            ThrowIfNullOrNotEnum(otherEnumType, out _, out otherEnumType,
                otherEnumTypeParamName);

            if (!enumType.Equals(otherEnumType))
            {
                throw new ArgumentException(paramName: otherEnumTypeParamName,
                    message: $"Expected a {nameof(Type)} of {enumType}, but "
                        + $"have a {nameof(Type)} of {otherEnumType}.");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enum"/> is <see langword="null"/>, or an
        /// <see cref="ArgumentException"/> if <paramref name="enum"/> is zero
        /// (see <see cref="IsZero(Enum, string)"/>).
        /// </summary>
        /// <param name="enum">What to check if it is <see langword="null"/> or
        /// if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="enum"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="enum"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> is
        /// zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfZero(Enum @enum, string paramName = null)
        {
            var exeption = GetExceptionIfZero(@enum,
                paramName ?? nameof(@enum));

            if (!(exeption is null))
            {
                throw exeption;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="enumType"/> or <paramref name="value"/> are
        /// <see langword="null"/>, or an <see cref="ArgumentException"/> if
        /// <paramref name="enumType"/> is not an <see cref="Enum"/> (see
        /// <see cref="Type.IsEnum"/>), or an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is zero (see
        /// <see cref="IsZero(Type, object, string, string)"/>).
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// check if is <see langword="null"/> or whether or not it is an
        /// <see cref="Enum"/> (see <see cref="Type.IsEnum"/>).</param>
        /// <param name="value">The <see cref="Enum"/> value as an
        /// <see cref="object"/> to see if it is zero.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is
        /// not an <see cref="Enum"/> or <paramref name="value"/> is
        /// zero.</exception>
        public static void ThrowIfZero(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null)
        {
            var exception = GetExceptionIfZero(enumType, value,
                enumTypeParamName ?? nameof(enumType),
                valueParamName ?? nameof(value));

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if
        /// <paramref name="value"/> is zero (see
        /// <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// zero.</exception>
        public static void ThrowIfZero<TEnum>(TEnum value,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfZero(value,
                paramName ?? nameof(value));

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if
        /// <paramref name="value"/> is <see langword="null"/> or an
        /// <see cref="ArgumentException"/> if <paramref name="value"/> is zero
        /// (see <see cref="IsZero{TEnum}(TEnum)"/>).
        /// </summary>
        /// <typeparam name="TEnum">A <see langword="struct"/> and an
        /// <see cref="Enum"/>.</typeparam>
        /// <param name="value">What to check if it is zero.</param>
        /// <param name="paramName">The name of the <paramref name="value"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="value"/> is used.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        public static void ThrowIfZero<TEnum>(TEnum? value,
            string paramName = null)
            where TEnum : struct, Enum
        {
            var exception = GetExceptionIfZero(value,
                paramName ?? nameof(value));

            if (!(exception is null))
            {
                throw exception;
            }
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an enumeration
        /// member of <paramref name="enumType"/>. If <paramref name="value"/>
        /// is an <see cref="Enum"/> and its <see cref="Type"/> equals
        /// <paramref name="enumType"/>, then it is returned as an
        /// <see cref="Enum"/>. If <paramref name="value"/>
        /// is an integral, then the <see cref="Enum.ToObject(Type, object)"/>
        /// method is used and then is returned as an <see cref="Enum"/>. If
        /// <paramref name="value"/> is a <see cref="string"/>, then the
        /// <see cref="Enum.Parse(Type, string, bool)"/> method is used and then
        /// is returned as an <see cref="Enum"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to
        /// use to convert <paramref name="value"/> to an enumeration member of
        /// it.</param>
        /// <param name="value">The <see cref="Enum"/> value (with the same
        /// <see cref="Type"/> as <paramref name="enumType"/>), an integer or 
        /// <see cref="string"/> value to convert to an enumeration member as an
        /// <see cref="object"/>.</param>
        /// <param name="enumTypeParamName">The name of the
        /// <paramref name="enumType"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="enumType"/> is used.</param>
        /// <param name="valueParamName">The name of the
        /// <paramref name="value"/> parameter. If this is
        /// <see langword="null"/>, then the <see langword="nameof"/>
        /// <paramref name="value"/> is used.</param>
        /// <param name="ignoreCaseForParse">If <paramref name="value"/> is a
        /// <see cref="string"/>, then this specifies whether the
        /// <see cref="Enum.Parse(Type, string, bool)"/> operation is 
        /// case-insensitive.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="enumType"/>
        /// returns <see langword="false"/> for the <see cref="Type.IsEnum"/>
        /// <see langword="property"/>, or <paramref name="value"/> is an
        /// <see cref="Enum"/> and its <see cref="Type"/> does not equal
        /// <paramref name="enumType"/>, or the <see cref="Type"/> of
        /// <paramref name="value"/> is not an integral or a
        /// <see cref="string"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>
        /// or <paramref name="value"/> is <see langword="null"/>.</exception>
        public static Enum ToEnum(Type enumType, object value,
            string enumTypeParamName = null, string valueParamName = null,
            bool ignoreCaseForParse = true)
        {
            enumTypeParamName = enumTypeParamName ?? nameof(enumType);
            ThrowIfNullOrNotEnum(enumType, out _, out enumType,
                enumTypeParamName);
            valueParamName = valueParamName ?? nameof(value);

            if (value is null)
            {
                throw new ArgumentNullException(valueParamName);
            }

            Enum @enum;

            if (value is Enum enumValue)
            {
                ThrowIfTypeNotEqual(enumType, enumValue, enumTypeParamName,
                    valueParamName);

                @enum = enumValue;
            }
            else if (value is sbyte sByteValue)
            {
                @enum = (Enum)Enum.ToObject(enumType, sByteValue);
            }
            else if (value is byte byteValue)
            {
                @enum = (Enum)Enum.ToObject(enumType, byteValue);
            }
            else if (value is short int16Value)
            {
                @enum = (Enum)Enum.ToObject(enumType, int16Value);
            }
            else if (value is ushort uInt16Value)
            {
                @enum = (Enum)Enum.ToObject(enumType, uInt16Value);
            }
            else if (value is int int32Value)
            {
                @enum = (Enum)Enum.ToObject(enumType, int32Value);
            }
            else if (value is uint uInt32Value)
            {
                @enum = (Enum)Enum.ToObject(enumType, uInt32Value);
            }
            else if (value is long int64Value)
            {
                @enum = (Enum)Enum.ToObject(enumType, int64Value);
            }
            else if (value is ulong uInt64Value)
            {
                @enum = (Enum)Enum.ToObject(enumType, uInt64Value);
            }
            else if (value is string stringValue)
            {
                @enum = (Enum)Enum.Parse(enumType, stringValue,
                    ignoreCaseForParse);
            }
            else
            {
                throw new ArgumentException(paramName: nameof(valueParamName),
                    message: $"An unexpected {nameof(Type)} of "
                        + $"{value.GetType()}.");
            }

            return @enum;
        }
    }
}
