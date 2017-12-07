using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Amazon.Lambda.Core;
using CCA.Schedule.Download;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace aws_schedule_emailer
{
    public class Function
    {
        
        public async Task ExecuteAsync()
        {
            try
            {
                var smtpUser = Environment.GetEnvironmentVariable("SmtpUser");
                var username = Environment.GetEnvironmentVariable("TheraNestUsername");

                var smtpPass = await DecodeEnvVarAsync("SmtpPass");
                var password = await DecodeEnvVarAsync("TheraNestPassword");

                var toEmail = Environment.GetEnvironmentVariable("ToEmail");
                var ccEmail = Environment.GetEnvironmentVariable("CcEmail");

                Log($"SmtpUser: {smtpUser}");
                Log($"TheraNest User: {username}");
                Log($"To Email: {toEmail}");
                Log($"Cc Email: {ccEmail}");

                var se = new ScheduleExecute();
                await se.ExecuteAsync(smtpUser, smtpPass, smtpUser, toEmail, ccEmail, username, password);
            }
            catch (Exception ex)
            {
                Log($"An error occurred {ex.Message}: {ex.StackTrace}");
                throw;
            }
        }

        private void Log(string message)
        {
            LambdaLogger.Log($"{message}\n");
        }

        private static async Task<string> DecodeEnvVarAsync(string envVarName)
        {
            // retrieve env var text
            var encryptedBase64Text = Environment.GetEnvironmentVariable(envVarName);
            // convert base64-encoded text to bytes
            var encryptedBytes = Convert.FromBase64String(encryptedBase64Text);
            // construct client
            using (var client = new AmazonKeyManagementServiceClient())
            {
                // construct request
                var decryptRequest = new DecryptRequest
                {
                    CiphertextBlob = new MemoryStream(encryptedBytes),
                };
                // call KMS to decrypt data
                var response = await client.DecryptAsync(decryptRequest);
                using (var plaintextStream = response.Plaintext)
                {
                    // get decrypted bytes
                    var plaintextBytes = plaintextStream.ToArray();
                    // convert decrypted bytes to ASCII text
                    var plaintext = Encoding.UTF8.GetString(plaintextBytes);
                    return plaintext;
                }
            }
        }

    }
}
