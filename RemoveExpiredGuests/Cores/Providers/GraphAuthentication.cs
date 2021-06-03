/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: GraphAuthentication
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Microsoft.Graph;
using Microsoft.Identity.Client;

/// <summary>
/// Define the application provider
/// </summary>
namespace RemoveExpiredGuests.Cores.Providers
{
    /// <summary>
    /// Graph authentication provider
    /// -> Attach access token to all request which is sent to graph API
    /// </summary>
    public class GraphAuthentication : DelegateAuthenticationProvider
    {
        /// <summary>
        /// Get or set the Graph API access token string
        /// </summary>
        private string accessToken { get; set; } = string.Empty;

        /// <summary>
        /// Get or set the authentication status flag
        /// </summary>
        private bool IsAuthenticationFail { get; set; } = false;

        /// <summary>
        /// Application setting management
        /// </summary>
        private AppSetting appSetting
        {
            get
            {
                return AppSetting.GetInstance();
            }
        }

        /// <summary>
        /// Application context management
        /// </summary>
        private AppContext appContext
        {
            get
            {
                return AppContext.GetInstance();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GraphAuthentication() : base(null)
        {
            AuthenticateRequestAsyncDelegate = AuthenticateRequest;
        }

        /// <summary>
        /// Authenticate the request which is sent to Graph API
        /// -> Attach access token to request header
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task AuthenticateRequest(HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = GetAccessToken();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Get Graph API access token
        /// </summary>
        /// <returns>Access token string</returns>
        public string GetAccessToken()
        {
            try
            {
                if (IsAuthenticationFail == false)
                {
                    var clientApplication = ConfidentialClientApplicationBuilder
                        .Create(appSetting.ClientId).WithTenantId(appSetting.Tenant)
                        .WithClientSecret(appSetting.ClientSecret).Build();

                    var scopes = new string[] { "https://graph.microsoft.com/.default" };
                    var task = clientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

                    return task.Result.AccessToken;
                }
            }
            catch (System.Exception error)
            {
                IsAuthenticationFail = true;
                appContext.Log.WriteError(error);
            }
            return string.Empty;
        }
    }
}