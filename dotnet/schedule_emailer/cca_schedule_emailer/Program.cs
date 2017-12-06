using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CCA.Schedule.Download;
using log4net;
using log4net.Config;

namespace cca_schedule_emailer
{
    public class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            try
            {
                if (args.Length < 5)
                    throw new Exception("Usage ... smtpuser smtppass to user password cc");

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

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
