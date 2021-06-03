/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: MailService
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Graph;

using RemoveExpiredGuests.Bases.Definitions;
using RemoveExpiredGuests.Bases.Extensions;

/// <summary>
/// Application services
/// </summary>
namespace RemoveExpiredGuests.Cores.Services
{
    /// Instant using methods
    using static Bases.Definitions.Constants;

    /// <summary>
    /// Mail type
    /// </summary>
    public enum MailType
    {
        /// <summary>
        /// Processed fail mail type
        /// </summary>
        Error,

        /// <summary>
        /// Processed success mail type
        /// </summary>
        Success,
    }

    /// <summary>
    /// The mail template storing structure
    /// </summary>
    public class MailTemplate
    {
        /// <summary>
        /// Get or set the email subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Get or set the email body
        /// </summary>
        public string Body { get; set; }
    }

    /// <summary>
    /// Application notification mail service
    /// </summary>
    public class MailService : IValidation
    {
        /// <summary>
        /// The error notification mail template
        /// </summary>
        private MailTemplate ErrorTemplate { get; set; }

        /// <summary>
        /// The success notification mail template
        /// </summary>
        private MailTemplate SuccessTemplate { get; set; }

        /// <summary>
        /// The partially successful and partially unsuccessful notification mail template
        /// </summary>
        private MailTemplate SuccessErrorTemplate { get; set; }

        /// <summary>
        /// The no deletion target notification mail template
        /// </summary>
        private MailTemplate NoTargetTetmplate { get; set; }

        /// <summary>
        /// Get List To Recipients
        /// </summary>
        private List<Recipient> ToRecipients { get; set; }
        
        /// <summary>
        /// Get List CC Recipients
        /// </summary>
        private List<Recipient> CcRecipients { get; set; }

        /// <summary>
        /// Get or set the processed status store
        /// </summary>
        private Dictionary<User, MailType> ProcessedGuests { get; set; }

        /// <summary>
        /// Get the application setting
        /// </summary>
        private AppSetting AppSetting
        {
            get
            {
                return AppSetting.GetInstance();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        
    }
}