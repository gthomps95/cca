using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace aws_schedule_emailer
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(string input, ILambdaContext context)
        {
            /*
            var smtpuser = args[0];
            var smtppass = args[1];
            var toemail = args[2];
            var username = args[3];
            var password = args[4];
            var cc = string.Join(", ", args.Skip(5));

            Task.Run(async () =>
            {
                var se = new ScheduleExecute();
                await se.ExecuteAsync(smtpuser, smtppass, smtpuser, toemail, cc, username, password);
            }).Wait();
            */
        }
    }
}
