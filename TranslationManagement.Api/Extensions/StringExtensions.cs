using System;

namespace TranslationManagement.Api.Extensions
{
    public static class StringExtensions
    {
        public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue = default(TEnum))
        {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
                return defaultValue;

            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }
    }
}
