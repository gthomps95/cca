using System;
using System.Threading.Tasks;

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
                var smtpPass = Environment.GetEnvironmentVariable("SmtpPass");
                var username = Environment.GetEnvironmentVariable("TheraNestUsername");
                var password = Environment.GetEnvironmentVariable("TheraNestPassword");

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
    }
}
