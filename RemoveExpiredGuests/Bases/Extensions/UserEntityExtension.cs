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
            try
            {
                var appContext = Cores.AppContext.GetInstance();

                var message = appContext.Mail.GetMessage();
                appContext.Graph.Users[user.Id].SendMail(message).Request().PostAsync().Wait();

                return (true, null);
            }
            catch(Exception error) { return (false, error); }
        }

        /// <summary>
        /// Validate the user was expired or not
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) IsExpired(this User user)
        {
            try
            {
                if(user.SignInActivity != null)
                {
                    var appSetting = Cores.AppSetting.GetInstance();
                    var lastSignedIn = user.SignInActivity.LastSignInDateTime.Value.DateTime;
                    if((DateTime.Now - lastSignedIn.ToLocalTime()).Days > appSetting.LimitDays)
                    {
                        return (true, null);
                    }
                }
                return (false, null);
            }
            catch(Exception error) { return (false, error); }
        }

        /// <summary>
        /// Validate user in group or not
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) IsInGroup(this User user)
        {
            try
            {
                var appContext = Cores.AppContext.GetInstance();
                var groups = appContext.Graph.Users[user.Id].MemberOf.Request().GetAsync().Result.CurrentPage;

                return (groups.Count > 0, null);
            }
            catch(Exception error) { return (false, error); }
        }

        /// <summary>
        /// Validate user can be removed or not
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) CanRemove(this User user)
        {
            var isExpired = user.IsExpired();
            if (!isExpired.Valid && isExpired.Error == null)
            {
                return (isExpired.Valid, isExpired.Error);
            }

            var isInGroup = user.IsInGroup();
            if (!isInGroup.Valid && isInGroup.Error == null)
            {
                return (!isInGroup.Valid, isInGroup.Error);
            }

            return (isExpired.Valid && isExpired.Valid, null);
        }

        /// <summary>
        /// Remove the guest user
        /// </summary>
        /// <param name="user">The user for validating</param>
        /// <returns>An object with validate result, if error occurs attach error to result</returns>
        public static (bool Valid, Exception Error) Remove(this User user)
        {
            var appContext = Cores.AppContext.GetInstance();
            var appSetting = Cores.AppSetting.GetInstance();

            try
            {
                if (appSetting.IsDeleteFlag == true)
                {
                    appContext.Graph.Users[user.Id].Request().DeleteAsync().Wait();
                    return (true, null);
                }
                else
                {
                    return (true, null);
                }
            }
            catch (Exception error) { return (false, error.InnerException); }
        }
    }
}