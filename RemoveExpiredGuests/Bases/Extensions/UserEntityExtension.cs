/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: UserEntityExtensions
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan     Create
/// ====================================================================================================
/// 
using System;

using Microsoft.Graph;

/// <summary>
/// Define extending methods
/// </summary>
namespace RemoveExpiredGuests.Bases.Extensions
{
    /// <summary>
    /// Extending methods for Microsoft.Graph.User
    /// </summary>
    public static class UserEntityExtension
    {
        /// <summary>
        /// Send mail to users with mail information to be managed by MailService
        /// </summary>
        /// <param name="user">The notification mail owner</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) SendMail(this User user)
        {
            
        }

        /// <summary>
        /// Validate the user was expired or not
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) IsExpired(this User user)
        {
          
        }

        /// <summary>
        /// Validate user in group or not
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) IsInGroup(this User user)
        {
            
        }

        /// <summary>
        /// Validate user can be removed or not
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) CanRemove(this User user)
        {
            
        }

        /// <summary>
        /// Remove the guest user
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) Remove(this User user)
        {
            
        }
    }
}