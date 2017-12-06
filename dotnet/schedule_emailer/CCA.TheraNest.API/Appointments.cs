using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CCA.TheraNest.API
{
    public class Appointments
    {
        public static async Task<IEnumerable<Appointment>> GetAppointmentsAsync(HttpClient client, DateTime date)
        {
            var dateStr = date.ToString("MM/dd/yyyy");
            var url = $"appointments?fromDate={dateStr}&toDate={dateStr}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Post appointments was not successful, returned code {response.StatusCode}.");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Appointment>>(result);
        }
    }
}
