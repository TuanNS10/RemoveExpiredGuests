/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: Constants
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 

/// <summary>
/// Application definition
/// </summary>
namespace RemoveExpiredGuests.Bases.Definitions
{
    /// <summary>
    /// Define string messages.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Message -> Cannot notification mail owner
        /// </summary>
        public const string USER_FROM_ADDRESS = "Cannot find 「{0}」 address for sending mail.";

        /// <summary>
        /// Message -> Guest has been remove
        /// </summary>
        public const string GUEST_REMOVED = "「{0}」 guest has been removed.";

        /// <summary>
        /// Setting value at key is invalid
        /// </summary>
        public const string KEY_VALUE_ERROR = "Value of 「{0}」 key in app.config is invalid.";

        /// <summary>
        /// Format string value
        /// </summary>
        /// <param name="msg">String format mat</param>
        /// <param name="args">Format arguments</param>
        /// <returns>The formated string</returns>
        public static string Format(string msg, params string[] args)
        {
            return string.Format(msg, args);
        }
    }
}