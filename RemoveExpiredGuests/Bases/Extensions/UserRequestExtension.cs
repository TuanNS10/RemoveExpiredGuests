/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: UserRequestExtensions
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.Graph;

using RemoveExpiredGuests.Bases.Exceptions;

/// <summary>
/// Define extending methods
/// </summary>
namespace RemoveExpiredGuests.Bases.Extensions
{
    using UsersRequest = GraphServiceUsersCollectionRequest;

    /// <summary>
    /// Extending methods for Microsoft.Graph.GraphServiceUsersCollectionRequest
    /// </summary>
    public static class UserRequestExtension
    {
        /// <summary>
        /// Get all users from Azure Active Directory
        /// -> With additional properties
        /// </summary>
        /// <param name="request">The getting user request</param>
        /// <param name="select">The additional properties</param>
        /// <returns>List of users</returns>
        public static List<User> Fetch(this UsersRequest request, Expression<Func<User, object>> select)
        {
            
        }

        /// <summary>
        /// Get all users from Azure Active Directory
        /// -> Default properties only
        /// </summary>
        /// <param name="request">The getting user request</param>
        /// <returns>List of users</returns>
        public static List<User> Fetch(this UsersRequest request)
        {
           
        }

        /// <summary>
        /// Get an user from Azure Active Directory by User Principal Name
        /// </summary>
        /// <param name="request">The getting user request</param>
        /// <param name="upn">The User Principal Name</param>
        /// <returns>The user if User Principal Name is found, otherwise is null</returns>
        public static User GetByUPN(this UsersRequest request, string upn)
        {
           
        }

        /// <summary>
        /// Get an user who is the owner of notification mail
        /// </summary>
        /// <param name="request">The getting user request</param>
        /// <returns>The user for sending notification result mail</returns>
        public static User GetMailFromUser(this UsersRequest request)
        {
            
        }
    }
}