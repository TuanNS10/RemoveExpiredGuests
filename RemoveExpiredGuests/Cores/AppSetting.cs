/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: AppSetting
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
using RemoveExpiredGuests.Bases.Extensions;
using RemoveExpiredGuests.Bases.Definitions;

/// <summary>
/// Application settings cores
/// </summary>
namespace RemoveExpiredGuests.Cores
{
    /// <summary>
    /// Application setting management
    /// </summary>
    public class AppSetting : IValidation
    {
        #region Application setting management singleton pattern
        /// <summary>
        /// Initialize application setting object
        /// </summary>
        private static readonly System.Lazy<AppSetting> appSetting
            = new System.Lazy<AppSetting>(() => new AppSetting());

        /// <summary>
        /// Get the initialized setting object
        /// </summary>
        /// <returns>Initialized AppSetting object</returns>
        public static AppSetting GetInstance()
        {
            return appSetting.Value;
        }
        #endregion

        #region Implementation of application setting management
        #region Properties
        /// <summary>
        /// Get or set the test operation condition flag
        /// </summary>
        [SettingMap(Key = SettingKey.DeleteFlag)]
        public bool IsDeleteFlag { get; set; }

        /// <summary>
        /// Get or set the Azure Tenant name
        /// -> Tenant ID can be used as the tenant name
        /// </summary>
        [SettingMap(Key = SettingKey.AzureADTenantName)]
        public string Tenant { get; set; }

        /// <summary>
        /// Get or set the Azure Active Directory application key
        /// </summary>
        [SettingMap(Key = SettingKey.AzureADClientID)]
        public string ClientId { get; set; }

        /// <summary>
        /// Get or set the Azure Active Directory application secret key
        /// </summary>
        [SettingMap(Key = SettingKey.AzureADSecretKey)]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Get or set the window event log source
        /// </summary>
        [SettingMap(Key = SettingKey.EventSource)]
        public string EventSource { get; set; }

        /// <summary>
        /// Get or set the limited days to remove user
        /// </summary>
        [SettingMap(Key = SettingKey.DeleteLimitDay)]
        public int LimitDays { get; set; }

        /// <summary>
        /// Get or set the error log path
        /// </summary>
        [SettingMap(Key = SettingKey.LogFilePath)]
        public string LogPath { get; set; }

        /// <summary>
        /// Get or set the success mail body template file path
        /// </summary>
        [SettingMap(Key = SettingKey.InfoMailTemplatePath)]
        public string SuccessMailFile { get; set; }

        /// <summary>
        /// Get or set the success mail title
        /// </summary>
        [SettingMap(Key = SettingKey.InfoMailTitle)]
        public string SuccessMailTitle { get; set; }

        /// <summary>
        /// Get or set the error mail title
        /// </summary>
        [SettingMap(Key = SettingKey.ErrorMailTitle)]
        public string ErrorMailTitle { get; set; }

        /// <summary>
        /// Get or set the error mail body template file path
        /// </summary>
        [SettingMap(Key = SettingKey.ErrorMailTemplatePath)]
        public string ErrorMailFile { get; set; }

        /// <summary>
        /// Get or set the partially successful and partially unsuccessful mail body file path
        /// </summary>
        [SettingMap(Key =SettingKey.InfoErrorMailTemplatePath)]
        public string SuccessErrorMailFile { get; set; }

        /// <summary>
        /// Get or set the partially successful and partially unsuccessful mail title
        /// </summary>
        [SettingMap(Key =SettingKey.InfoErrorMailTitle)]
        public string SuccessErrorMailTitle { get; set; }

        /// <summary>
        /// Get or set the no target mail body template file path
        /// </summary>
        [SettingMap(Key = SettingKey.NoTargetMailTemplatePath)]
        public string NoTargetMailFile { get; set; }

        /// <summary>
        /// Get or set the no target mail title
        /// </summary>
        [SettingMap(Key =SettingKey.NoTargetMailTitle)]
        public string NoTargetMailTitle { get; set; }

        /// <summary>
        /// Get or set the Form mail address
        /// </summary>
        [SettingMap(Key = SettingKey.FromMailAddress)]
        public string FromAddresses { get; set; }

        /// <summary>
        /// Get or set the TO mail address list
        /// </summary>
        [SettingMap(Key = SettingKey.ToMailAddress)]
        public string[] ToAddresses { get; set; }

        /// <summary>
        /// Get or set the CC mail address list
        /// </summary>
        [SettingMap(Key = SettingKey.CcMailAddress)]
        public string[] CcAddresses { get; set; }

        /// <summary>
        /// Get or set the Graph API version
        /// </summary>
        [SettingMap(Key = SettingKey.ApiVersion)]
        public string ApiVersion { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        private AppSetting()
        {
            foreach (var prop in GetType().GetProperties())
            {
                foreach (var att in prop.GetCustomAttributes(true))
                {
                    if (att.GetType() == typeof(SettingMap))
                    {
                        var propertyType = prop.PropertyType;
                        var appSetting = att as SettingMap;

                        var settingValue = appSetting.GetValue(propertyType);
                        if (settingValue != null)
                        {
                            prop.SetValue(this, settingValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validate the basic information of application setting
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrEmpty(LogPath))
            {
                LogPath = "Logs";
            }

            if (string.IsNullOrEmpty(EventSource))
            {
                EventSource = "AzureADDelete";
            }
        }
        #endregion
    }
}