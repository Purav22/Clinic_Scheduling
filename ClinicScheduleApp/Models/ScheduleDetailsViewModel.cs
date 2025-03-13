using ClinicSchedule.Entities;

namespace ClinicScheduleApp.Models
{
    public class ScheduleDetailsViewModel
    {
        public Schedule? ActiveSchedule { get; set; }

        public Clinician NewClinician { get; set; } = new Clinician();

        public Appointment NewAppointment { get; set; } = new Appointment();

        public int InPersonAppointmentCount { get; set; }

        public int PhoneAppointmentCount { get; set; }

        public int VideoAppointmentCount { get; set; }
    }
}
