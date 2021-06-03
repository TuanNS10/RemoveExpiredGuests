/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: UsersRequestBuilder
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using Microsoft.Graph;

/// <summary>
/// Define custom type and object
/// </summary>
namespace RemoveExpiredGuests.Bases.Customizes
{
    /// <summary>
    /// Custom user request builder for getting Users
    /// </summary>
    public class UsersRequestBuilder : GraphServiceUsersCollectionRequestBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestUrl">The URL for the built request</param>
        /// <param name="client">The Microsoft.Graph.IBaseClient for handling requests</param>
        public UsersRequestBuilder(string requestUrl, IBaseClient client) : base(requestUrl, client)
        {
        }

        /// <summary>
        /// Builds the request for getting Users
        /// </summary>
        /// <returns>The request for getting User only</returns>
        public virtual new GraphServiceUsersCollectionRequest Request()
        {
            var guestQuery = base.Request();
            return (GraphServiceUsersCollectionRequest)guestQuery;
        }
    }
}