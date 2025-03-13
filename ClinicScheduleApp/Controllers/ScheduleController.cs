using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ClinicScheduleApp.DataAccess;
using ClinicSchedule.Entities;
using ClinicScheduleApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClinicScheduleApp.Controllers
{
    public class ScheduleController : Controller
    {
        public ScheduleController(ClinicScheduleDbContext clinicScheduleDbContext)
        {
            _clinicScheduleDbContext = clinicScheduleDbContext;
        }

        [HttpGet("/schedules")]
        public IActionResult GetAllSchedules()
        {
            var schedules = _clinicScheduleDbContext.Schedules
                    .Include(s => s.Clinicians)
                    .Include(s => s.Appointments)
                    .OrderByDescending(s => s.DateCreated)
                    .ToList();

            return View("Items", schedules);
        }

        [HttpGet("/schedules/{id}")]
        public IActionResult GetScheduleById(int id)
        {
            var schedule = _clinicScheduleDbContext.Schedules
                    .Include(s => s.Clinicians)
                    .Include(s => s.Appointments)
                    .Where(s => s.ScheduleId == id)
                    .FirstOrDefault();

            if (schedule == null)
                return NotFound();

            var viewModel = new ScheduleDetailsViewModel
            {
                ActiveSchedule = schedule,
                InPersonAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.InPerson),
                PhoneAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.Phone),
                VideoAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.Video),
                NewClinician = new Clinician { ScheduleId = id },
                NewAppointment = new Appointment { ScheduleId = id }
            };

            return View("Details", viewModel);
        }

        [Authorize] // Ensure only authenticated users can access this action
        [HttpGet("/schedules/add-request")]
        public IActionResult GetAddScheduleRequest()
        {
            return View("AddSchedule", new Schedule());
        }

        [Authorize] // Ensure only authenticated users can access this action
        [HttpPost("/schedules")]
        public IActionResult AddNewSchedule(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _clinicScheduleDbContext.Schedules.Add(schedule);
                _clinicScheduleDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"The schedule \"{schedule.Name}\" was added.";

                return RedirectToAction("GetAllSchedules", "Schedule");
            }
            else
            {
                return View("AddSchedule", schedule);
            }
        }

        [Authorize] // Ensure only authenticated users can access this action
        [HttpGet("/schedules/{id}/edit-request")]
        public IActionResult GetEditRequestById(int id)
        {
            var schedule = _clinicScheduleDbContext.Schedules.Find(id);
            return View("EditSchedule", schedule);
        }

        [Authorize] // Ensure only authenticated users can access this action
        [HttpPost("/schedules/edit-requests")]
        public IActionResult ProcessEditRequest(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _clinicScheduleDbContext.Schedules.Update(schedule);
                _clinicScheduleDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"The schedule \"{schedule.Name}\" was updated.";

                return RedirectToAction("GetAllSchedules", "Schedule");
            }
            else
            {
                return View("EditSchedule", schedule);
            }
        }

        [Authorize(Roles = "Admin")] // Ensure only authenticated users can access this action
        [HttpGet("/schedules/{id}/delete-request")]
        public IActionResult GetDeleteRequestById(int id)
        {
            var schedule = _clinicScheduleDbContext.Schedules.Find(id);
            return View("DeleteConfirmation", schedule);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ProcessDeleteRequestById(int id)
        {
            var schedule = _clinicScheduleDbContext.Schedules.Find(id);

            _clinicScheduleDbContext.Schedules.Remove(schedule);
            _clinicScheduleDbContext.SaveChanges();

            TempData["LastActionMessage"] = $"The schedule \"{schedule.Name}\" was deleted.";

            return RedirectToAction("GetAllSchedules", "Schedule");
        }

        [Authorize]
        [HttpPost("/schedules/{id}/add-appointment")]
        public IActionResult AddNewAppointment(int id, Appointment newAppointment)
        {
            if (ModelState.IsValid)
            {
                newAppointment.ScheduleId = id;
                _clinicScheduleDbContext.Appointments.Add(newAppointment);
                _clinicScheduleDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"Appointment for \"{newAppointment.PatientName}\" was added to the schedule.";

                return RedirectToAction("GetScheduleById", new { id });
            }

            var schedule = _clinicScheduleDbContext.Schedules
                    .Include(s => s.Clinicians)
                    .Include(s => s.Appointments)
                    .FirstOrDefault(s => s.ScheduleId == id);

            var viewModel = new ScheduleDetailsViewModel
            {
                ActiveSchedule = schedule,
                InPersonAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.InPerson),
                PhoneAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.Phone),
                VideoAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.Video),
                NewClinician = new Clinician { ScheduleId = id },
                NewAppointment = newAppointment
            };

            return View("Details", viewModel);
        }

        [Authorize]
        [HttpPost("/schedules/{id}/add-clinician")]
        public IActionResult AddNewClinician(int id, Clinician newClinician)
        {
            if (ModelState.IsValid)
            {
                newClinician.ScheduleId = id;
                _clinicScheduleDbContext.Clinicians.Add(newClinician);
                _clinicScheduleDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"Clinician \"{newClinician.FullName}\" was added to the schedule.";

                return RedirectToAction("GetScheduleById", new { id });
            }

            var schedule = _clinicScheduleDbContext.Schedules
                    .Include(s => s.Clinicians)
                    .Include(s => s.Appointments)
                    .FirstOrDefault(s => s.ScheduleId == id);

            var viewModel = new ScheduleDetailsViewModel
            {
                ActiveSchedule = schedule,
                InPersonAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.InPerson),
                PhoneAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.Phone),
                VideoAppointmentCount = schedule.Appointments.Count(a => a.AppointmentType == AppointmentTypeOptions.Video),
                NewClinician = newClinician,
                NewAppointment = new Appointment { ScheduleId = id }
            };

            return View("Details", viewModel);
        }

        private ClinicScheduleDbContext _clinicScheduleDbContext;
    }

}
