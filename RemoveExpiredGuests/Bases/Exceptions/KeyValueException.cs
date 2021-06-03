/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: KeyValueException
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using RemoveExpiredGuests.Bases.Attributes;

/// <summary>
/// Define custom error exceptions
/// </summary>
namespace RemoveExpiredGuests.Bases.Exceptions
{
    /// Instant using methods
    using static RemoveExpiredGuests.Bases.Definitions.Constants;

    /// <summary>
    /// Setting value invalid exception
    /// </summary>
    public class KeyValueException : System.Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">The setting key</param>
        public KeyValueException(SettingKey key) : base(Format(KEY_VALUE_ERROR, key.ToString()))
        {
        }
    }
}