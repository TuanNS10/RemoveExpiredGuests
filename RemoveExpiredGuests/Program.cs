/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: Program
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using System.IO;
using System.Reflection;

using RemoveExpiredGuests.Bases.Extensions;

/// <summary>
/// Remove expired guest on Azure Active Directory
/// </summary>
namespace RemoveExpiredGuests
{
    /// Instant using methods
    using static Bases.Definitions.Constants;

    /// <summary>
    /// Control application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Start the application
        /// </summary>
        /// <param name="args">Application arguments</param>
        static void Main(string[] args)
        {
            // Move working directory to exe folder
            var exeFile = Assembly.GetExecutingAssembly().Location;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(exeFile));

            // Declare the application context
            Cores.AppContext appContext = null;

            try
            {
                // Initialize the application context and validate it
                appContext = Cores.AppContext.GetInstance();
                if (!appContext.IsValid())
                {
                    return;
                }

                // Get the user who will send the notification mail
                var mailUser = appContext.Graph.Users.Request().GetMailFromUser();

                // Get all guest users
                var guests = appContext.Graph.Guests.Request().Fetch();

                // Validate the expiration condition and try to delete guest
                // if the guest reach the expiration condition
                foreach (var guest in guests)
                {
                    var removeCond = guest.CanRemove();
                    if (removeCond.Valid == true)
                    {
                        var removeResult = guest.Remove();
                        if (removeResult.Valid == true)
                        {
                            appContext.Mail.PushSuccess(guest);

                            var message = Format(GUEST_REMOVED, guest.Mail);
                            appContext.Log.WriteSuccess(message);
                        }
                        else
                        {
                            appContext.Mail.PushError(guest, removeResult.Error);
                            appContext.Log.WriteError(removeResult.Error);
                        }
                    }
                }

                // Send a mail for notifing the processed result
                var sentResult = mailUser.SendMail();
                if (sentResult.Valid == false)
                {
                    appContext.Log.WriteError(sentResult.Error);
                }
            }
            catch (System.Exception error)
            {
                appContext.Log.WriteError(error);
            }
        }
    }
}