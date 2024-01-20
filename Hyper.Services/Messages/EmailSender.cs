using Hyper.Core.Configuration;
using Hyper.Services.Messages;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Hyper.Services.Notifications
{
    public partial class EmailSender : IEmailSender
    {
        private readonly DefaultEmailAccountSettings _defaultEmailAccountSettings;

        public EmailSender(DefaultEmailAccountSettings defaultEmailAccountSettings)
        {
            _defaultEmailAccountSettings = defaultEmailAccountSettings;
        }

        #region Utilities

        /// <summary>
        /// Create an file attachment for the binary data
        /// </summary>
        /// <param name="attachmentFileName">Attachment file name with extensions like (.jpeg, .pdf etc)</param>
        /// <param name="binaryContent">The array of unsigned bytes from which to create the attachment stream.</param>
        /// <param name="cDate">Creation date and time for the specified file or directory</param>
        /// <param name="mDate">Date and time that the specified file or directory was last written to</param>
        /// <param name="rDate">Date and time that the specified file or directory was last access to.</param>
        /// <returns>A leaf-node MIME part that contains an attachment.</returns>
        protected MimePart CreateMimeAttachment(string attachmentFileName, byte[] binaryContent, DateTime cDate, DateTime mDate, DateTime rDate)
        {
            if (!ContentType.TryParse(MimeTypes.GetMimeType(attachmentFileName), out var mimeContentType))
                mimeContentType = new ContentType("application", "octet-stream");

            return new MimePart(mimeContentType)
            {
                FileName = attachmentFileName,
                Content = new MimeContent(new MemoryStream(binaryContent)),
                ContentDisposition = new ContentDisposition
                {
                    CreationDate = cDate,
                    ModificationDate = mDate,
                    ReadDate = rDate
                },
                ContentTransferEncoding = ContentEncoding.Base64
            };
        }

        /// <summary>
        /// Create a new SMTP client for a specific email account
        /// </summary>
        /// <param name="emailAccount">Email account to use. If null, then would be used EmailAccount by default</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the an SMTP client that can be used to send email messages
        /// </returns>
        protected virtual async Task<SmtpClient> BuildSmtpClientAsync(EmailAccount emailAccount = null)
        {
            if (emailAccount is null)
            {
                emailAccount = new EmailAccount();
            }

            var client = new SmtpClient
            {
                ServerCertificateValidationCallback = ValidateServerCertificate
            };

            try
            {
                await client.ConnectAsync(
                    emailAccount.Host,
                    emailAccount.Port,
                    emailAccount.EnableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable);

                if (emailAccount.UseDefaultCredentials)
                {
                    await client.AuthenticateAsync(CredentialCache.DefaultNetworkCredentials);
                }
                else if (!string.IsNullOrWhiteSpace(emailAccount.Username))
                {
                    await client.AuthenticateAsync(new NetworkCredential(emailAccount.Username, emailAccount.Password));
                }

                return client;
            }
            catch (Exception ex)
            {
                client.Dispose();
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validates the remote Secure Sockets Layer (SSL) certificate used for authentication.
        /// </summary>
        /// <param name="sender">An object that contains state information for this validation.</param>
        /// <param name="certificate">The certificate used to authenticate the remote party.</param>
        /// <param name="chain">The chain of certificate authorities associated with the remote certificate.</param>
        /// <param name="sslPolicyErrors">One or more errors associated with the remote certificate.</param>
        /// <returns>A System.Boolean value that determines whether the specified certificate is accepted for authentication</returns>
        protected virtual bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //By default, server certificate verification is disabled.
            return true;
        }

        #endregion

        #region Methods

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
        public virtual async Task SendEmailAsync(string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
            IDictionary<string, byte[]> attachmentFiles = null,
            string replyTo = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            IDictionary<string, string> headers = null)
        {
            var account = new EmailAccount
            {
                DisplayName = _defaultEmailAccountSettings.DisplayName,
                Email = _defaultEmailAccountSettings.Email,
                EnableSsl = _defaultEmailAccountSettings.EnableSsl,
                Host = _defaultEmailAccountSettings.Host,
                Password = _defaultEmailAccountSettings.Password,
                Port = _defaultEmailAccountSettings.Port,
                UseDefaultCredentials = _defaultEmailAccountSettings.UseDefaultCredentials,
                Username = _defaultEmailAccountSettings.Username
            };

            await this.SendEmailAsync(account, subject, body, fromAddress, fromName, toAddress, toName, attachmentFiles, replyTo, replyToName, bcc, cc);
        }

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
        public virtual async Task SendEmailAsync(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
            IDictionary<string, byte[]> attachmentFiles = null,
            string replyTo = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            IDictionary<string, string> headers = null)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));

            if (!string.IsNullOrEmpty(replyTo))
            {
                message.ReplyTo.Add(new MailboxAddress(replyToName, replyTo));
            }

            //BCC
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(new MailboxAddress("", address.Trim()));
                }
            }

            //CC
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.Cc.Add(new MailboxAddress("", address.Trim()));
                }
            }

            //content
            message.Subject = subject;

            //headers
            if (headers != null)
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }

            var multipart = new Multipart("mixed")
            {
                new TextPart(TextFormat.Html) { Text = body }
            };

            foreach (var attachment in attachmentFiles)
            {
                multipart.Add(CreateMimeAttachment(attachment.Key, attachment.Value, DateTime.Now, DateTime.Now, DateTime.Now));
            }

            message.Body = multipart;

            //send email
            using var smtpClient = await this.BuildSmtpClientAsync(emailAccount);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }

        #endregion


    }
}
