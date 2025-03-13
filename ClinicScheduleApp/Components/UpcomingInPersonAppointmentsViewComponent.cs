using ClinicSchedule.Entities;
using ClinicScheduleApp.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicScheduleApp.Components
{
    public class UpcomingInPersonAppointmentsViewComponent : ViewComponent
    {
        public UpcomingInPersonAppointmentsViewComponent(ClinicScheduleDbContext clinicScheduleDbContext)
        {
            _clinicScheduleDbContext = clinicScheduleDbContext;
        }

        public IViewComponentResult Invoke(int numberOfAppointmentsToDisplay)
        {

            var appointments = _clinicScheduleDbContext.Appointments
                .Include(a => a.Schedule)
                .Where(a => a.AppointmentDate >= DateTime.Now && a.AppointmentType == AppointmentTypeOptions.InPerson)
                .OrderBy(a => a.AppointmentDate)
                .Take(numberOfAppointmentsToDisplay)
                .ToList();

            AppointmentsViewModel recentAppointmentsViewModel = new AppointmentsViewModel()
            {
                Appointments = appointments,
                NumberOfAppointmentsToDisplay = numberOfAppointmentsToDisplay
            };

            return View(recentAppointmentsViewModel);
        }

        private ClinicScheduleDbContext _clinicScheduleDbContext;
    }
}
