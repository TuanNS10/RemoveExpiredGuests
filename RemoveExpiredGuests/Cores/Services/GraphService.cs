/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: GraphService
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan     Create
/// ====================================================================================================
/// 
using Microsoft.Graph;

using RemoveExpiredGuests.Bases.Definitions;
using RemoveExpiredGuests.Bases.Attributes;
using RemoveExpiredGuests.Bases.Exceptions;
using RemoveExpiredGuests.Bases.Customizes;
using RemoveExpiredGuests.Cores.Providers;

/// <summary>
/// Application services
/// </summary>
namespace RemoveExpiredGuests.Cores.Services
{
    /// <summary>
    /// Application Graph API Service
    /// </summary>

    public class GraphService : GraphServiceClient, IValidation
    {
        private AppSetting AppSetting
        {
            get { return AppSetting.GetInstance(); }
        }

        /// <summary>
        /// Gets the Users request builder
        /// </summary>
        public new UsersRequestBuilder Users { get; private set; }

        /// <summary>
        /// Gets the Guests request builder
        /// </summary>
        public GuestsRequestBuilder Guests { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiEndpoint">The Graph API endpoint with API Version.</param>
        public GraphService(string apiEndpoint) : base(apiEndpoint, new GraphAuthentication())
        {
            var userClient = base.Users.Client;
            var userUrl = base.Users.RequestUrl;

            Users = new UsersRequestBuilder(userUrl, userClient);
            Guests = new GuestsRequestBuilder(userUrl, userClient);
        }

        /// <summary>
        /// Validate the Graph service object
        /// </summary>
        public void Validate()
        {
            var result = string.IsNullOrEmpty(AppSetting.ApiVersion)
                      || AppSetting.ApiVersion.ToLower() != "beta"
                      && AppSetting.ApiVersion.ToLower() != "v1.0";
            if (result)
            {
                throw new KeyValueException(SettingKey.ApiVersion);
            }

            result = AppSetting.LimitDays <= 0;
            if (result)
            {
                throw new KeyValueException(SettingKey.DeleteLimitDay);
            }
        }
    }
}