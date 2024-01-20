using Hyper.Services.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyper.Services.Notifications
{
    public partial interface IEmailSender
    {

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="fromAddress">From address</param>
        /// <param name="fromName">From display name</param>
        /// <param name="toAddress">To address</param>
        /// <param name="toName">To display name</param>
        /// <param name="attachmentFiles">Attachment files</param>
        /// <param name="replyTo">ReplyTo address</param>
        /// <param name="replyToName">ReplyTo display name</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <param name="headers">Headers</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SendEmailAsync(string subject, string body,
         string fromAddress, string fromName, string toAddress, string toName,
         IDictionary<string, byte[]> attachmentFiles = null,
         string replyTo = null, string replyToName = null,
         IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
         IDictionary<string, string> headers = null);

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="fromAddress">From address</param>
        /// <param name="fromName">From display name</param>
        /// <param name="toAddress">To address</param>
        /// <param name="toName">To display name</param>
        /// <param name="replyTo">ReplyTo address</param>
        /// <param name="replyToName">ReplyTo display name</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <param name="headers">Headers</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SendEmailAsync(EmailAccount emailAccount, string subject, string body,
              string fromAddress, string fromName, string toAddress, string toName,
              IDictionary<string, byte[]> attachmentFiles = null,
              string replyTo = null, string replyToName = null,
              IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
              IDictionary<string, string> headers = null);
    }
}