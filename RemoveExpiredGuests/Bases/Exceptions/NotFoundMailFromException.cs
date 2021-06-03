/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: MailFromException
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
/// Define custom error exceptions
/// </summary>
namespace RemoveExpiredGuests.Bases.Exceptions
{
    /// Instant using methods
    using static RemoveExpiredGuests.Bases.Definitions.Constants;

    /// <summary>
    /// Cannot found the mail address for sending mail
    /// </summary>
    public class NotFoundMailFromException : System.Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mail">The email address which is not found</param>
        public NotFoundMailFromException(string mail) : base(Format(USER_FROM_ADDRESS, mail))
        {
        }
    }
}