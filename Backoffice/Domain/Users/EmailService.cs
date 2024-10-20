//using System.Net.Mail;


//public class SimpleAsynchronousExample
//{
//    static bool mailSent = false;

//    public int random(string destination, string body)
//    {
//        MailMessage mailMessage = new MessageMail();

//        mailMessage.From(_icon).To("destinatery");

//        mailMessage.Body("");

//        return 0;
//    }




//    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
//    {
//        // Get the unique identifier for this asynchronous operation.
//        String token = (string)e.UserState;

//        if (e.Cancelled)
//        {
//            Console.WriteLine("[{0}] Send canceled.", token);
//        }
//        if (e.Error != null)
//        {
//            Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
//        }
//        else
//        {
//            Console.WriteLine("Message sent.");
//        }
//        mailSent = true;
//    }

//    public static void Main(string[] args)
//    {
//        // Command-line argument must be the SMTP host.
//        SmtpClient client = new SmtpClient(args[0]);
//        // Specify the email sender.
//        // Create a mailing address that includes a UTF8 character
//        // in the display name.
//        MailAddress from = new MailAddress("jane@contoso.com",
//           "Jane " + (char)0xD8 + " Clayton",
//        System.Text.Encoding.UTF8);
//        // Set destinations for the email message.
//        MailAddress to = new MailAddress("ben@contoso.com");
//        // Specify the message content.
//        MailMessage message = new MailMessage(from, to);
//        message.Body = "This is a test email message sent by an application. ";
//        // Include some non-ASCII characters in body and subject.
//        string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
//        message.Body += Environment.NewLine + someArrows;
//        message.BodyEncoding = System.Text.Encoding.UTF8;
//        message.Subject = "test message 1" + someArrows;
//        message.SubjectEncoding = System.Text.Encoding.UTF8;
//        // Set the method that is called back when the send operation ends.
//        client.SendCompleted += new
//        SendCompletedEventHandler(SendCompletedCallback);
//        // The userState can be any object that allows your callback
//        // method to identify this send operation.
//        // For this example, the userToken is a string constant.
//        string userState = "test message1";
//        client.SendAsync(message, userState);
//        Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
//        string answer = Console.ReadLine();
//        // If the user canceled the send, and mail hasn't been sent yet,
//        // then cancel the pending operation.
//        if (answer.StartsWith("c") && mailSent == false)
//        {
//            client.SendAsyncCancel();
//        }
//        // Clean up.
//        message.Dispose();
//        client.Dispose();
//        Console.WriteLine("Goodbye.");
//    }
//}

using Microsoft.Extensions.Configuration;
using Backoffice.Domain.Shared;
using System;
using System.Net;
using System.Net.Mail;

namespace Backoffice.Domain.Users
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmail(string emailAddress, string message, string subject)
        {
            try
            {
                // SMTP server configuration
                var smtpClient = new SmtpClient(_config["EmailSmtp:SmtpServer"])
                {
                    Port = int.Parse(_config["EmailSmtp:SmtpPort"]),
                    Credentials = new NetworkCredential(_config["EmailSmtp:SmtpEmail"], _config["EmailSmtp:SmtpKey"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSmtp:SmtpEmail"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(emailAddress);
                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw new SmtpException(ex.Message);
                }
            }
            catch (Exception e)
            {
                throw new SmtpException(e.Message);
            }
        }
    }
}
