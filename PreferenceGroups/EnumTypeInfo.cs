using System;
using System.Collections.Generic;

namespace PreferenceGroups
{
    /// <summary>
    /// Provides functionality to work with a <see cref="Type"/> that may be
    /// either an <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
    /// <see cref="Enum"/>. In particular, the <see cref="EnumPreference"/> can
    /// be instantiated from a <see cref="Type"/> that may be
    /// either an <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
    /// <see cref="Enum"/>.
    /// </summary>
    public class EnumTypeInfo : IEquatable<Enum>, IEquatable<EnumTypeInfo>,
        IEquatable<Type>
    {
        /// <summary>
        /// The <see cref="Enum"/> <see cref="Type"/>.
        /// </summary>
        public Type EnumType { get; }

        /// <summary>
        /// Whether or not the <see cref="EnumType"/> has the
        /// <see cref="FlagsAttribute"/> applied to it.
        /// </summary>
        public bool HasFlags { get; }

        /// <summary>
        /// The <see cref="Nullable{T}"/> of <see cref="EnumType"/>.
        /// </summary>
        public Type NullableType { get; }

        /// <summary>
        /// The underlying <see cref="Type"/> of <see cref="EnumType"/>.
        /// </summary>
        public Type UnderlyingType { get; }

        /// <summary>
        /// The zero equivalent value of <see cref="EnumType"/>.
        /// </summary>
        public Enum Zero { get; }

        /// <summary>
        /// Initializes from <paramref name="type"/> which must be either an
        /// <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>.
        /// </summary>
        /// <param name="type"> An <see cref="Enum"/> or a
        /// <see cref="Nullable{T}"/> of an <see cref="Enum"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// parameter. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is used.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public EnumTypeInfo(Type type, string paramName = null)
        {
            paramName = paramName ?? nameof(type);

            if (type is null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (!EnumHelpers.IsEnum(type, out var typeIsNullable,
                out var enumType, paramName))
            {
                throw new ArgumentException(paramName: paramName,
                    message: "Does not represent an Enum or a Nullable of "
                        + "Enum.");
            }

            EnumType = enumType;

            if (typeIsNullable)
            {
                NullableType = type;
            }
            else
            {
                NullableType = typeof(Nullable<>).MakeGenericType(enumType);
            }
            
            HasFlags = EnumHelpers.HasFlags(EnumType, paramName);
            UnderlyingType = Enum.GetUnderlyingType(EnumType);
            Zero = EnumHelpers.GetZero(EnumType, paramName);
        }

        /// <summary>
        /// Determines whether or not if the <see cref="Type"/> of
        /// <paramref name="enum"/> equals to <see cref="EnumType"/>.
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public bool Equals(Enum @enum)
        {
            if (@enum is null)
            {
                return false;
            }

            return EnumType.Equals(@enum.GetType());
        }

        /// <summary>
        /// Determines whether or not if the specificed instance of
        /// <paramref name="other"/> is the same instance as
        /// <see langword="this"/>, or the <see cref="NullableType"/> of
        /// <paramref name="other"/> equals to <see cref="NullableType"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(EnumTypeInfo other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return NullableType.Equals(other.NullableType);
        }

        /// <summary>
        /// Determines whether or not if the specificed instance of
        /// <paramref name="object"/> is the same instance as
        /// <see langword="this"/>, or if <paramref name="object"/> is an
        /// <see cref="Enum"/> then returns the result of
        /// <see cref="Equals(Enum)"/>, or if <paramref name="object"/> is an
        /// <see cref="EnumTypeInfo"/> then returns the result of
        /// <see cref="Equals(EnumTypeInfo)"/>, or if <paramref name="object"/>
        /// is a <see cref="Type"/> then returns the result of
        /// <see cref="Equals(Type)"/>.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public override bool Equals(object @object)
        {
            if (@object is null)
            {
                return false;
            }

            if (ReferenceEquals(this, @object))
            {
                return true;
            }

            if (@object is Enum @enum)
            {
                return Equals(@enum);
            }

            if (@object is EnumTypeInfo enumTypeInfo)
            {
                return Equals(enumTypeInfo);
            }

            if (@object is Type type)
            {
                return Equals(type);
            }

            return base.Equals(@object);
        }

        /// <summary>
        /// Determines whether or not if <paramref name="type"/> is first an
        /// <see cref="Enum"/> or a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>, and if so then whether or not it is equal to
        /// <see langword="this"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Equals(Type type)
        {
            if (type is null)
            {
                return false;
            }

            if (!EnumHelpers.IsEnum(type, out _, out var enumType,
                nameof(type)))
            {
                return false;
            }

            return EnumType.Equals(enumType);
        }

        /// <summary>
        /// Returns a hash code of <see langword="this"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + EnumType.GetHashCode();
                hash = hash * 23 + HasFlags.GetHashCode();
                hash = hash * 23 + NullableType.GetHashCode();
                hash = hash * 23 + Zero.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Returns the <see cref="Array"/> of <see cref="Enum"/> of the values of
        /// constants of <see cref="EnumType"/>.
        /// </summary>
        /// <returns></returns>
        public Enum[] GetValues()
        {
            var values = new List<Enum>();

            foreach (var valueAsObject in Enum.GetValues(EnumType))
            {
                if (valueAsObject is null)
                {
                    continue;
                }

                var value = EnumHelpers.ToEnum(EnumType, valueAsObject,
                    nameof(EnumType), nameof(valueAsObject));
                values.Add(value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns the <see cref="Array"/> of <see cref="Enum"/> of the values
        /// of constants of <see cref="EnumType"/>, except any that are
        /// equal to <see cref="Zero"/>.
        /// </summary>
        /// <returns></returns>
        public Enum[] GetValuesNotZero()
        {
            var values = new List<Enum>();

            foreach (var valueAsObject in Enum.GetValues(EnumType))
            {
                if (valueAsObject is null)
                {
                    continue;
                }

                var value = EnumHelpers.ToEnum(EnumType, valueAsObject,
                    nameof(EnumType), nameof(valueAsObject));

                if (!Zero.Equals(value))
                {
                    values.Add(value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="Enum"/> based on
        /// <see cref="EnumType"/> (see <see
        /// cref="EnumHelpers.ToEnum(Type, object, string, string, bool)"/>).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <param name="ignoreCaseForParse"></param>
        /// <returns></returns>
        public Enum ToEnum(object value, string paramName = null,
            bool ignoreCaseForParse = true)
            => EnumHelpers.ToEnum(EnumType, value, nameof(EnumType),
                paramName ?? nameof(value), ignoreCaseForParse);

        /// <summary>
        /// Returns a <see cref="string"/> representing
        /// <see cref="NullableType"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => NullableType.ToString();

        /// <summary>
        /// Determines whether or not <paramref name="type"/> is an
        /// <see cref="Enum"/> or is a <see cref="Nullable{T}"/> of an
        /// <see cref="Enum"/>. If either is <see langword="true"/>, then
        /// <paramref name="enumTypeInfo"/> will not be <see langword="null"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumTypeInfo">If this method returns
        /// <see langword="true"/>, then this is not
        /// <see langword="null"/>.</param>
        /// <param name="paramName">The name of the <paramref name="type"/>
        /// <see langword="param"/>. If this is <see langword="null"/>, then the
        /// <see langword="nameof"/> <paramref name="type"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is
        /// <see langword="null"/>.</exception>
        public static bool IsEnum(Type type, out EnumTypeInfo enumTypeInfo,
            string paramName = null)
        {
            paramName = paramName ?? nameof(type);

            if (type is null)
            {
                throw new ArgumentNullException(paramName);
            }

            enumTypeInfo = null;

            if (EnumHelpers.IsEnum(type, out _, out _, paramName))
            {
                enumTypeInfo = new EnumTypeInfo(type, paramName);

                return true;
            }

            return false;
        }
    }
}
