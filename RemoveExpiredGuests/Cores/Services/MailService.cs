/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: MailService
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
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Graph;

using RemoveExpiredGuests.Bases.Definitions;
using RemoveExpiredGuests.Bases.Extensions;

/// <summary>
/// Application services
/// </summary>
namespace RemoveExpiredGuests.Cores.Services
{
    /// <summary>
    /// Mail type
    /// </summary>
    public enum MailType
    {
        /// <summary>
        /// Processed fail mail type
        /// </summary>
        Error,

        /// <summary>
        /// Processed success mail type
        /// </summary>
        Success,
    }

    /// <summary>
    /// Get error message content of mail
    /// </summary>
    public class MailContent
    {
        public User User { get; set; }
        public Exception Error { get; set; }
    }

    /// <summary>
    /// The mail template storing structure
    /// </summary>
    public class MailTemplate
    {
        /// <summary>
        /// Get or set the email subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Get or set the email body
        /// </summary>
        public string Body { get; set; }
    }

    /// <summary>
    /// Application notification mail service
    /// </summary>
    public class MailService : IValidation
    {
        /// <summary>
        /// The error notification mail template
        /// </summary>
        private MailTemplate ErrorTemplate { get; set; }

        /// <summary>
        /// The success notification mail template
        /// </summary>
        private MailTemplate SuccessTemplate { get; set; }

        /// <summary>
        /// The partially successful and partially unsuccessful notification mail template
        /// </summary>
        private MailTemplate SuccessErrorTemplate { get; set; }

        /// <summary>
        /// The no deletion target notification mail template
        /// </summary>
        private MailTemplate NoTargetTetmplate { get; set; }

        /// <summary>
        /// Get List To Recipients
        /// </summary>
        private List<Recipient> ToRecipients { get; set; }

        /// <summary>
        /// Get List CC Recipients
        /// </summary>
        private List<Recipient> CcRecipients { get; set; }

        /// <summary>
        /// Get or set the processed status store
        /// </summary>
        private Dictionary<MailContent, MailType> ProcessedGuests { get; set; }

        /// <summary>
        /// Get the application setting
        /// </summary>
        private AppSetting AppSetting
        {
            get
            {
                return AppSetting.GetInstance();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MailService()
        {
            ProcessedGuests = new Dictionary<MailContent, MailType>();
            ToRecipients = new List<Recipient>();
            CcRecipients = new List<Recipient>();

            ErrorTemplate = new MailTemplate()
            {
                Subject = AppSetting.ErrorMailTitle,
                Body = string.Empty,
            };

            SuccessTemplate = new MailTemplate()
            {
                Subject = AppSetting.SuccessMailTitle,
                Body = string.Empty,
            };

            SuccessErrorTemplate = new MailTemplate()
            {
                Subject = AppSetting.SuccessErrorMailTitle,
                Body = string.Empty,
            };

            NoTargetTetmplate = new MailTemplate()
            {
                Subject = AppSetting.NoTargetMailTitle,
                Body = string.Empty,
            };
        }

        /// <summary>
        /// Validate mail service object
        /// </summary>
        public void Validate()
        {
            SuccessTemplate.Body = ReadMailFile(AppSetting.SuccessMailFile);
            ErrorTemplate.Body = ReadMailFile(AppSetting.ErrorMailFile);
            SuccessErrorTemplate.Body = ReadMailFile(AppSetting.SuccessErrorMailFile);
            NoTargetTetmplate.Body = ReadMailFile(AppSetting.NoTargetMailFile);

            var result = AppSetting.FromAddresses.IsMail();
            if (!result.Valid)
            {
                throw result.Error;
            }

            result = AppSetting.ToAddresses.IsMail();
            if (!result.Valid)
            {
                throw result.Error;
            }

            result = AppSetting.CcAddresses.IsMail();
            if (!result.Valid)
            {
                throw result.Error;
            }

            foreach (var toAddress in AppSetting.ToAddresses)
            {
                ToRecipients.Add(new Recipient()
                {
                    EmailAddress = new EmailAddress()
                    {
                        Address = toAddress
                    }
                });
            }

            foreach (var ccAddress in AppSetting.CcAddresses)
            {
                CcRecipients.Add(new Recipient()
                {
                    EmailAddress = new EmailAddress()
                    {
                        Address = ccAddress
                    }
                });
            }
        }

        /// <summary>
        /// Push the error status to remove user into saving store
        /// </summary>
        /// <param name="user">The processed user(guest)</param>
        public void PushError(User user, Exception error)
        {
            var maillog = new MailContent()
            {
                User = user,
                Error = error
            };
            ProcessedGuests.Add(maillog, MailType.Error);
        }

        /// <summary>
        /// Push the success status to remove user into saving store
        /// </summary>
        /// <param name="user">The processed user(guest)</param>
        public void PushSuccess(User user)
        {
            var maillog = new MailContent()
            {
                User = user,
                Error = null
            };
            ProcessedGuests.Add(maillog, MailType.Success);
        }

        /// <summary>
        /// Get notification mail message
        /// </summary>
        /// <returns>The notification mail message</returns>
        public Message GetMessage()
        {
            var successList = ProcessedGuests.Where(store => store.Value == MailType.Success).ToList();
            var errorList = ProcessedGuests.Where(store => store.Value == MailType.Error).ToList();

            if (successList.Count > 0 && errorList.Count > 0)
            {
                var errorMsg = GetMessageText(errorList);
                var successMsg = GetMessageText(successList);
                var errorDetail = GetErrorText(errorList);

                SuccessErrorTemplate.Body = string.Format(
                    SuccessErrorTemplate.Body, successMsg, errorMsg, errorDetail);
            }

            if (successList.Count > 0 && errorList.Count == 0)
            {
                SuccessTemplate.Body = string.Format(SuccessTemplate.Body, GetMessageText(successList));
            }

            if (successList.Count == 0 && errorList.Count > 0)
            {
                var errorMsg = GetMessageText(errorList);
                var errorDetail = GetErrorText(errorList);

                ErrorTemplate.Body = string.Format(ErrorTemplate.Body, errorMsg, errorDetail);
            }

            if (successList.Count == 0 && errorList.Count == 0)
            {
                NoTargetTetmplate.Body = string.Format(NoTargetTetmplate.Body, GetMessageText(ProcessedGuests));
            }

            return CreateMessage(errorList, successList);
        }

        /// <summary>
        /// Get the template message text with list of error or success
        /// </summary>
        /// <param name="args">The error list or success list</param>
        /// <returns>The error mail list or success mail list</returns>
        private string GetMessageText(IEnumerable<KeyValuePair<MailContent, MailType>> args)
        {
            var errorString = new StringBuilder();
            foreach (var arg in args)
            {
                var mail = arg.Key.User.Mail;
                if (string.IsNullOrEmpty(mail))
                {
                    mail = arg.Key.User.UserPrincipalName;
                }
                errorString.AppendLine(mail);
            }
            return errorString.ToString();
        }

        /// <summary>
        /// Get error message text when delete user failed
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Get error message text when delete user failed</returns>
        private string GetErrorText(IEnumerable<KeyValuePair<MailContent, MailType>> args)
        {
            var errorString = new StringBuilder();
            foreach (var arg in args)
            {
                errorString.AppendLine(arg.Key.Error.Message);
            }
            return errorString.ToString();
        }

        /// <summary>
        /// Read content of mail template file
        /// </summary>
        /// <param name="filePath">The mail content file path</param>
        /// <returns>The mail content string</returns>
        private string ReadMailFile(string filePath)
        {
            return System.IO.File.ReadAllText(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// Create the email notification message
        /// </summary>
        /// <param name="errorList">The processed error list</param>
        /// <param name="successList">The processed success list</param>
        /// <param name="sucerrList">The processed Partial success and partial error list</param>
        /// <returns>The notification message</returns>
        private Message CreateMessage(object errorList, object successList)
        {
            var ErrorList = (IEnumerable<KeyValuePair<MailContent, MailType>>)errorList;
            var SuccessList = (IEnumerable<KeyValuePair<MailContent, MailType>>)successList;

            var subject = string.Empty;
            if (ErrorList.ToList().Count > 0 && SuccessList.ToList().Count > 0)
            {
                subject = SuccessErrorTemplate.Subject;
            }
            else if (ErrorList.ToList().Count > 0 && SuccessList.ToList().Count == 0)
            {
                subject = ErrorTemplate.Subject;
            }
            else if (ErrorList.ToList().Count == 0 && SuccessList.ToList().Count > 0)
            {
                subject = SuccessTemplate.Subject;
            }
            else
            {
                subject = NoTargetTetmplate.Subject;
            }

            var message = new Message()
            {
                ToRecipients = ToRecipients,
                CcRecipients = CcRecipients,
                Subject = subject,
                Body = new ItemBody()
                {
                    ContentType = BodyType.Text
                }
            };
            return CreateBodyMessage(message, errorList, successList);
        }

        /// <summary>
        /// Create the email notification message body
        /// </summary>
        /// <param name="basic">The created message without the body</param>
        /// <param name="errorList">The processed error list</param>
        /// <param name="successList">The processed success list</param>
        /// <returns>The notification message</returns>
        private Message CreateBodyMessage(Message basic, object errorList, object successList)
        {
            var ErrorList = (IEnumerable<KeyValuePair<MailContent, MailType>>)errorList;
            var SuccessList = (IEnumerable<KeyValuePair<MailContent, MailType>>)successList;

            var ErrorBody = ErrorTemplate.Body;
            var SuccessBody = SuccessTemplate.Body;
            var SuccessErrorBody = SuccessErrorTemplate.Body;
            var NoTargetBody = NoTargetTetmplate.Body;

            if (ErrorList.ToList().Count > 0 && SuccessList.ToList().Count > 0)
            {
                basic.Body.Content = $"{ SuccessErrorBody }";
            }
            else if (ErrorList.ToList().Count > 0 && SuccessList.ToList().Count == 0)
            {
                basic.Body.Content = $"{ ErrorBody }";
            }
            else if (ErrorList.ToList().Count == 0 && SuccessList.ToList().Count > 0)
            {
                basic.Body.Content = $"{ SuccessBody }";
            }
            else
            {
                basic.Body.Content = $"{ NoTargetBody }";
            }

            return basic;
        }
    }
}