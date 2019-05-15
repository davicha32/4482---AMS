using System.Collections.Generic;
using System.Linq;
using AMS.Models;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

/// <summary>
/// This model is used to keep track of an email object.
/// Written by Jack Bradley who utilized the following resources:
/// https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/
/// That guide is helpful in setting up the functionality to send emails.
/// Receiving emails will be another story. 
/// This code isn't really utilized at this point. The next person working on this project could potentially use it to recieve emails however. I just don't think that .netcore has the ability to utilize something like a fetchmail very easily.
/// </summary>

namespace AMS.UnusedModels {
    public class Email : User {

    }

    /// <summary>
    /// This is an email address class. It gets and sets a name plus an address. Pretty self explanatory. 
    /// </summary>
    public class EmailAddress {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    /// <summary>
    /// This is an email message class. It gets and sets a list of ToAddresses and FromAddresses. Easy enough. 
    /// </summary>
    public class EmailMessage {
        public EmailMessage() {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        } 
        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        /// <summary>
        /// Then it has a Subject and Content geter and setter. This code isn't being utilized at the moment, but we decided to migrate towards message rather then content. 
        /// </summary>
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    public interface IEmailConfiguration {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        string PopServer { get; }
        int PopPort { get; }
        string PopUsername { get; }
        string PopPassword { get; }
    }

    public class EmailConfiguration : IEmailConfiguration {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }
    }
    public interface IEmailService {
        void Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }

    public class EmailService : IEmailService {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration) {
            _emailConfiguration = emailConfiguration;
        }


        public void Send(EmailMessage emailMessage) {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html) {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient()) {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10) {
            using (var emailClient = new Pop3Client()) {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++) {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                }

                return emails;
            }
        }
    }
 
}

