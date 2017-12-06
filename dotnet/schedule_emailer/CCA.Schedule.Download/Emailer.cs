using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace CCA.Schedule.Download
{
    public class Emailer
    {
        public static async Task EmailScheduleAsync(string smtpUser, string smtpPass, string fromString, string toString, string[] ccStrings, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromString));
            message.To.Add(new MailboxAddress(toString));
            message.Subject = subject;
            message.Body = new TextPart("plain") {Text = body};

            foreach (var cc in ccStrings)
            {
                message.Cc.Add(new MailboxAddress(cc));
            }

            using (var client = new SmtpClient())
            {
                var creds = new NetworkCredential(smtpUser, smtpPass);

                await client.ConnectAsync("smtp.gmail.com", 587).ConfigureAwait(false);
                await client.AuthenticateAsync(creds);
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
