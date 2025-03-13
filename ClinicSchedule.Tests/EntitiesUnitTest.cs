using ClinicSchedule.Entities;

namespace ClinicSchedule.Tests
{
    public class EntitiesUnitTest
    {
        [Fact]
        public void TestNumberOfPastAppointments()
        {
            Schedule sch1 = new Schedule() {
                Name = "Test schedule #1",
                Appointments = new List<Appointment>()
            };

            sch1.Appointments.Add(new Appointment() { AppointmentDate = new DateTime(2020, 8, 5), PatientName = "Homer Simpson" });
            sch1.Appointments.Add(new Appointment() { AppointmentDate = new DateTime(2021, 11, 24), PatientName = "Marge Simpson" });
            sch1.Appointments.Add(new Appointment() { AppointmentDate = new DateTime(2023, 2, 9), PatientName = "Bart Simpson" });
            sch1.Appointments.Add(new Appointment() { AppointmentDate = new DateTime(2023, 5, 8), PatientName = "Lisa Simpson" });
            sch1.Appointments.Add(new Appointment() { AppointmentDate = new DateTime(2023, 5, 31), PatientName = "Maggie Simpson" });

            // Complete this test method by using a LINQ expression to query for past appointments
            // and assert that it is the expected number - then ensure that the test runs and passes.

            var pastAppointmentsCount = sch1.Appointments.Count(a => a.AppointmentDate < DateTime.Now);
            Assert.Equal(4, pastAppointmentsCount);
        }
    }
}