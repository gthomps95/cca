using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCA.TheraNest.API;

namespace CCA.Schedule.Download
{
    public class ScheduleExecute
    {
        public async Task ExecuteAsync(string smtpUser, string smtpPass, string from, string to, string cc, string username, string password)
        {
            var client = await Account.SignIn(username, password);

            var appts = await Appointments.GetAppointmentsAsync(client, DateTime.Now);
            var buffer = FormatAppts(appts);

            await Emailer.EmailScheduleAsync(smtpUser, smtpPass, from, to.Split(';', ' ', ',').Where(s => !string.IsNullOrEmpty(s)).ToArray(), cc.Split(';', ' ', ',').Where(s => !string.IsNullOrEmpty(s)).ToArray(), $"CCA Schedule for {DateTime.Now:d}", buffer);
        }

        private static string FormatAppts(IEnumerable<Appointment> appts)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Date/Time, Title, Staff");

            foreach (var appt in appts.OrderBy(a => a.GetApptDate()).ThenBy(a => a.Title))
            {
                var title = PrepareForCsv(appt.Title);
                var staff = PrepareForCsv(appt.StaffMember.Name);
                var date = PrepareForCsv(appt.GetApptDate()?.ToString("G") ?? "");

                sb.Append($"{date}, {title}, {staff}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string PrepareForCsv(string value)
        {
            return value.Replace(",", " ").Replace("\n", "");
        }
    }
}
