using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSchedule.Entities
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public string? Name { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;

        // Nav props:
        public ICollection<Clinician>? Clinicians { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
