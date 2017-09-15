// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="ExtensionsForEnum.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    using Cql.Core.Common.Attributes;
    using Cql.Core.Common.Utils;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForEnum.
    /// </summary>
    public static class ExtensionsForEnum
    {
        /// <summary>
        /// Gets the enum value for the specified <paramref name="value" />
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>TEnum.</returns>
        public static TEnum FromDataValue<TEnum>(this string value, TEnum? defaultValue = null)
            where TEnum : struct, IConvertible
        {
            return FromAttrValue<TEnum, DataValueAttribute>(value, defaultValue, attribute => attribute.Value);
        }

        /// <summary>
        /// Gets the enum value for the specified <paramref name="description" />
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="description">The description.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>TEnum.</returns>
        public static TEnum FromDescription<TEnum>(this string description, TEnum? defaultValue = null)
            where TEnum : struct, IConvertible
        {
            return FromAttrValue<TEnum, DescriptionAttribute>(description, defaultValue, attribute => attribute.Description);
        }

        /// <summary>
        /// Gets the enum value for the specified <paramref name="displayText" />
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="displayText">The display text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>TEnum.</returns>
        public static TEnum FromDisplayText<TEnum>(this string displayText, TEnum? defaultValue = null)
            where TEnum : struct, IConvertible
        {
            return FromAttrValue<TEnum, DisplayAttribute>(displayText, defaultValue, attribute => attribute.GetName());
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The data value for the Enum</returns>
        [NotNull]
        public static string GetDataValue(this Enum value)
        {
            var stringValue = Convert.ToString(value);

            var attribute = ReflectionUtils.GetFieldAttribute<DataValueAttribute>(value, stringValue);

            return attribute?.Value ?? stringValue;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The description text for the Enum</returns>
        [NotNull]
        public static string GetDescription(this Enum value)
        {
            var stringValue = Convert.ToString(value);

            var field = value.GetType().GetTypeInfo().GetField(stringValue);

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute == null ? value.GetDisplayText() : attribute.Description;
        }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The display text value for the Enum</returns>
        [NotNull]
        public static string GetDisplayText(this Enum value)
        {
            var stringValue = Convert.ToString(value);

            var field = value.GetType().GetTypeInfo().GetField(stringValue);

            var attribute = field.GetCustomAttribute<DisplayAttribute>();

            return attribute == null ? stringValue : attribute.GetName();
        }

        /// <summary>
        /// Gets the underlying enum value from the specified <paramref name="value" />
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <typeparam name="TAttribute">The type of the t attribute.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="attrValueFactory">The attribute value factory.</param>
        /// <returns>TEnum.</returns>
        /// <exception cref="ArgumentNullException">attrValueFactory</exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static TEnum FromAttrValue<TEnum, TAttribute>([CanBeNull] string value, TEnum? defaultValue, [NotNull] Func<TAttribute, string> attrValueFactory)
            where TEnum : struct where TAttribute : Attribute
        {
            if (attrValueFactory == null)
            {
                throw new ArgumentNullException(nameof(attrValueFactory));
            }

            var type = typeof(TEnum);

            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in typeInfo.GetFields())
            {
                var attribute = field.GetCustomAttribute<TAttribute>();

                if (attribute == null)
                {
                    if (string.Equals(field.Name, value, StringComparison.OrdinalIgnoreCase))
                    {
                        return (TEnum)field.GetValue(null);
                    }
                }
                else
                {
                    if (string.Equals(attrValueFactory(attribute), value, StringComparison.OrdinalIgnoreCase))
                    {
                        return (TEnum)field.GetValue(null);
                    }
                }
            }

            return defaultValue.GetValueOrDefault();
        }
    }
}
