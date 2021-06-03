/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: Setting
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using RemoveExpiredGuests.Bases.Extensions;

/// <summary>
/// Define custom attributes
/// </summary>
namespace RemoveExpiredGuests.Bases.Attributes
{
    /// <summary>
    /// Define keys for appSetting section in app.config file
    /// </summary>
    public enum SettingKey
    {
        /// <summary>
        /// Application delete flag key
        /// </summary>
        DeleteFlag,

        /// <summary>
        /// Graph API version key
        /// </summary>
        ApiVersion,

        /// <summary>
        /// Azure application tenant name key
        /// </summary>
        AzureADTenantName,

        /// <summary>
        /// Azure application Id key
        /// </summary>
        AzureADClientID,

        /// <summary>
        /// Azure application secret key
        /// </summary>
        AzureADSecretKey,
        
        /// <summary>
        /// Window event source key
        /// </summary>
        EventSource,

        /// <summary>
        /// Expired limit day key
        /// </summary>
        DeleteLimitDay,

        /// <summary>
        /// Log Path key
        /// </summary>
        LogFilePath,

        /// <summary>
        /// Success mail template file key
        /// </summary>
        InfoMailTemplatePath,

        /// <summary>
        /// Error mail template file key
        /// </summary>
        ErrorMailTemplatePath,

        /// <summary>
        /// Partial Success + Partial Error Mail template file key 
        /// </summary>
        InfoErrorMailTemplatePath,

        /// <summary>
        /// No target mail template file key
        /// </summary>
        NoTargetMailTemplatePath,

        /// <summary>
        /// Success mail title key
        /// </summary>
        InfoMailTitle,

        /// <summary>
        /// Error mail title key
        /// </summary>
        ErrorMailTitle,

        /// <summary>
        /// Partial Success + Partial Error mail title key
        /// </summary>
        InfoErrorMailTitle,

        /// <summary>
        /// No target mail title key
        /// </summary>
        NoTargetMailTitle,

        /// <summary>
        /// From mail address key
        /// </summary>
        FromMailAddress,

        /// <summary>
        /// To mail address key
        /// </summary>
        ToMailAddress,

        /// <summary>
        /// Cc mail address key
        /// </summary>
        CcMailAddress,
    }

    /// <summary>
    /// Setting attribute for mapping the appSetting section with an object
    /// </summary>
    public class SettingMap : System.Attribute
    {
        /// <summary>
        /// Get or set the appSetting key using SettingKey enumeration
        /// </summary>
        public SettingKey Key { get; set; }

        /// <summary>
        /// Get the value in appSetting by key and try to convert it to object data types
        /// </summary>
        /// <param name="type">Data type for converting</param>
        /// <returns>The converted object</returns>
        public object GetValue(System.Type type)
        {
            var appSetting = System.Configuration.ConfigurationManager.AppSettings;
            var settingValueString = appSetting[Key.ToString()];

            try
            {
                if (string.IsNullOrEmpty(settingValueString))
                {
                    return null;
                }
                else
                {
                    if (type == typeof(string))
                    {
                        return settingValueString;
                    }
                    else if (type == typeof(string[]))
                    {
                        return settingValueString.Split(';');
                    }
                    else
                    {
                        return settingValueString.ParseTo(type);
                    }
                }
            }
            catch (System.Exception) { return null; }
        }
    }
}