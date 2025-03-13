using Microsoft.AspNetCore.Mvc;

using ClinicScheduleApp.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ClinicScheduleApp.Components
{
    public class RecentPastAppointments : ViewComponent
    {
        public RecentPastAppointments(ClinicScheduleDbContext clinicScheduleDbContext)
        {
            _clinicScheduleDbContext = clinicScheduleDbContext;
        }

        public IViewComponentResult Invoke(int numberOfAppointmentsToDisplay)
        {
            DateTime now = DateTime.Now;
            var appointments = _clinicScheduleDbContext.Appointments
                    .Include(a => a.Schedule)
                    .Where(a => a.AppointmentDate < now)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

            AppointmentsViewModel recentAppointmentsViewModel = new AppointmentsViewModel() { 
                Appointments = appointments,
                NumberOfAppointmentsToDisplay = numberOfAppointmentsToDisplay
            };

            return View(recentAppointmentsViewModel);
        }

        private ClinicScheduleDbContext _clinicScheduleDbContext;
    }
}
