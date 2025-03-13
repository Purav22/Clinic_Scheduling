using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ClinicSchedule.Entities;
using ClinicScheduleApp.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClinicScheduleApp.DataAccess
{
    public class ClinicScheduleDbContext : IdentityDbContext<User>
    {
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string password = "Sesame123#";
            string roleName = "Admin";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            // if username doesn't exist, create it and add it to role
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public ClinicScheduleDbContext(DbContextOptions<ClinicScheduleDbContext> options)
            : base(options)
        {
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Clinician> Clinicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map enum values --> strings in DB based on enum name:
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentType)
                .HasConversion<string>()
                .HasMaxLength(64);

            // Seed some Schedules:
            modelBuilder.Entity<Schedule>().HasData(
                new Schedule() { ScheduleId = 1, Name = "Main Street clinic schedule" },
                new Schedule() { ScheduleId = 2, Name = "Surrey street clinic schedule" }
            );

            // Seed some Clinicians:
            modelBuilder.Entity<Clinician>().HasData(
                new Clinician() { ClinicianId = 1, ProfessionalRegistrationNumber = "ADF-173456", FirstName = "Bart", LastName = "Simpson", ScheduleId = 1 },
                new Clinician() { ClinicianId = 2, ProfessionalRegistrationNumber = "DAB-997768", FirstName = "Lisa", LastName = "Simpson", ScheduleId = 1 },
                new Clinician() { ClinicianId = 3, ProfessionalRegistrationNumber = "FGK-874559", FirstName = "Maggie", LastName = "Simpson", ScheduleId = 2 },
                new Clinician() { ClinicianId = 4, ProfessionalRegistrationNumber = "LMP-114092", FirstName = "Marge", LastName = "Simpson", ScheduleId = 2 },
                new Clinician() { ClinicianId = 5, ProfessionalRegistrationNumber = "ZCH-575930", FirstName = "Homer", LastName = "Simpson", ScheduleId = 2 }
            );

            // Seed some Appointments:
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment() { AppointmentId = 1, AppointmentDate = new DateTime(2023, 2, 7), PatientName = "Groucho Marx", ScheduleId = 1 },
                new Appointment() { AppointmentId = 2, AppointmentDate = new DateTime(2023, 2, 9), PatientName = "Harpo Marx", ScheduleId = 1 },
                new Appointment() { AppointmentId = 3, AppointmentDate = new DateTime(2023, 5, 8), PatientName = "Chico Marx", ScheduleId = 2 },
                new Appointment() { AppointmentId = 4, AppointmentDate = new DateTime(2023, 11, 24), PatientName = "Zeppo Marx", ScheduleId = 2 },
                new Appointment() { AppointmentId = 5, AppointmentDate = new DateTime(2023, 12, 29), PatientName = "Gummo Marx", ScheduleId = 2 },
                new Appointment() { AppointmentId = 6, AppointmentDate = new DateTime(2022, 5, 31), PatientName = "John Lennon", ScheduleId = 1, AppointmentType = AppointmentTypeOptions.Video },
                new Appointment() { AppointmentId = 7, AppointmentDate = new DateTime(2022, 7, 17), PatientName = "Paul McCartney", ScheduleId = 1, AppointmentType = AppointmentTypeOptions.Phone },
                new Appointment() { AppointmentId = 8, AppointmentDate = new DateTime(2022, 8, 5), PatientName = "George Harrison", ScheduleId = 2, AppointmentType = AppointmentTypeOptions.Phone },
                new Appointment() { AppointmentId = 9, AppointmentDate = new DateTime(2022, 8, 17), PatientName = "Ringo Starr", ScheduleId = 2, AppointmentType = AppointmentTypeOptions.Video }
            );
        }
    }
}
