using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace CCA.Schedule.Download
{
    public class Emailer
    {
        public static async Task EmailScheduleAsync(string smtpUser, string smtpPass, string fromString, string[] toStrings, string[] ccStrings, string subject, string body, string csvFileName, string csvFile)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromString));

            foreach (var to in toStrings)
            {
                message.To.Add(new MailboxAddress(to));
            }

            message.Subject = subject;

            foreach (var cc in ccStrings)
            {
                message.Cc.Add(new MailboxAddress(cc));
            }

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvFile)))
            {
                var attachment = new MimePart("text/csv")
                {
                    ContentObject = new ContentObject(ms),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = csvFileName
                };

                var bodyPart = new TextPart("plain") { Text = body };
                var mp = new Multipart("mixed") {bodyPart, attachment};

                message.Body = mp;

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
}
