/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: StringTypeExtensions
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan     Create
/// ====================================================================================================
/// 
using System.ComponentModel;

/// <summary>
/// Define extending methods
/// </summary>
namespace RemoveExpiredGuests.Bases.Extensions
{
    /// <summary>
    /// Extending methods for System.String
    /// </summary>
    public static class StringTypeExtension
    {
        /// <summary>
        /// Parse a string to target type
        /// </summary>
        /// <param name="value">The string value for parsing</param>
        /// <param name="type">The target parsing type</param>
        /// <returns>The parsed object from string</returns>
        public static object ParseTo(this string value, System.Type type)
        {
            var targetType = System.Type.GetType((type as System.Type).FullName);
            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(typeof(string)))
            {
                return converter.ConvertFromInvariantString(value);
            }
            return null;
        }

        /// <summary>
        /// Validate a string is in email format or not
        /// </summary>
        /// <param name="value">The string for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, System.Exception Error) IsMail(this string value)
        {
            try
            {
                new System.Net.Mail.MailAddress(value);
                return (true, null);
            }
            catch (System.Exception error)
            {
                return (false, error);
            }
        }

        /// <summary>
        /// Validate a string array are in email format or not
        /// </summary>
        /// <param name="value">The array of string for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, System.Exception Error) IsMail(this string[] value)
        {
            foreach (var item in value)
            {
                var result = item.IsMail();
                if (!result.Valid)
                {
                    throw result.Error;
                }
            }
            return (true, null);
        }
    }
}