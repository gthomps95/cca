using System;

namespace CCA.TheraNest.API
{
    public class Appointment
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public StaffMember StaffMember { get; set; }

        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public DateTime? GetApptDate()
        {
            DateTime date;
            if (!DateTime.TryParse($"{StartDate} {StartTime}", out date))
                return null;

            
            return date;
        }
    }
}
