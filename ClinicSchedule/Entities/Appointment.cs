using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSchedule.Entities
{
    public enum AppointmentTypeOptions { InPerson, Phone, Video };

    public class Appointment
    {
        public int AppointmentId { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public string? PatientName { get; set; }

        public AppointmentTypeOptions AppointmentType { get; set; } = AppointmentTypeOptions.InPerson;

        // FK:
        public int? ScheduleId { get; set; }

        // And nav prop:
        public Schedule? Schedule { get; set; }
    }
}
