/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: AppContext
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using RemoveExpiredGuests.Bases.Definitions;
using RemoveExpiredGuests.Cores.Services;

/// <summary>
/// Application management cores
/// </summary>
namespace RemoveExpiredGuests.Cores
{
    /// <summary>
    /// Application context management
    /// </summary>
    public class AppContext : IValidation
    {
        #region Application context management singleton pattern
        /// <summary>
        /// Initialize application context object
        /// </summary>
        private static readonly System.Lazy<AppContext> appContext
            = new System.Lazy<AppContext>(() => new AppContext());

        /// <summary>
        /// Get the initialized context object
        /// </summary>
        /// <returns>Initialized AppContext object</returns>
        public static AppContext GetInstance()
        {
            return appContext.Value;
        }
        #endregion

        #region Implementation of application context management
        /// <summary>
        /// The application setting
        /// </summary>
        private AppSetting AppSetting
        {
            get
            {
                return AppSetting.GetInstance();
            }
        }

        /// <summary>
        /// Get or set the Graph service (private set)
        /// </summary>
        public GraphService Graph { get; private set; }

        /// <summary>
        /// Get or set the mail service (private set)
        /// </summary>
        public MailService Mail { get; private set; }

        /// <summary>
        /// Get or set the log service (private set)
        /// </summary>
        public LogService Log { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Application context is singleton pattern
        /// So it's constructor is private, so that
        /// DO NOT CHANGE TO PUBLIC CONSTRUCTOR
        /// </remarks>
        private AppContext()
        {
            var apiVersion = "beta";
            if (!string.IsNullOrEmpty(AppSetting.ApiVersion))
            {
                apiVersion = AppSetting.ApiVersion;
            }

            var apiEndpoint = $"https://graph.microsoft.com/{apiVersion}";

            Log   = new LogService();
            Mail  = new MailService();
            Graph = new GraphService(apiEndpoint);
        }

        /// <summary>
        /// Validate application context object
        /// </summary>
        public void Validate()
        {
            AppSetting.Validate();

            Log.Validate();
            Mail.Validate();
            Graph.Validate();
        }

        /// <summary>
        /// Validate object information
        /// </summary>
        /// <returns>True if information is valid, otherwise false</returns>
        public bool IsValid()
        {
            Validate();
            return true;
        }
        #endregion
    }
}