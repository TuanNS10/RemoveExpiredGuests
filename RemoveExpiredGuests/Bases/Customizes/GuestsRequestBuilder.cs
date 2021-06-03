/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: GuestsRequestBuilder
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2020.12.17      SanhTuan        Create
/// ====================================================================================================
/// 
using Microsoft.Graph;

/// <summary>
/// Define custom type and object
/// </summary>
namespace RemoveExpiredGuests.Bases.Customizes
{
    /// <summary>
    /// Custom user request builder for getting Guests
    /// </summary>
    public class GuestsRequestBuilder : GraphServiceUsersCollectionRequestBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestUrl">The URL for the built request</param>
        /// <param name="client">The Microsoft.Graph.IBaseClient for handling requests</param>
        public GuestsRequestBuilder(string requestUrl, IBaseClient client) : base(requestUrl, client)
        {
        }

        /// <summary>
        /// Builds the request for getting Guests
        /// </summary>
        /// <returns>The request for getting Guests only</returns>
        public new GraphServiceUsersCollectionRequest Request()
        {
            var guestQuery = base.Request().Filter("UserType eq 'Guest'");
            guestQuery = guestQuery.Select(guest => new {
                guest.Id, guest.Mail, guest.DisplayName, guest.SignInActivity
            });

            return (GraphServiceUsersCollectionRequest)guestQuery;
        }
    }
}