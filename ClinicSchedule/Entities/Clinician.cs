using System.ComponentModel.DataAnnotations;

namespace ClinicSchedule.Entities
{
    public class Clinician
    {
        public int ClinicianId { get; set; }

        [Required(ErrorMessage = "Professional registration number is required.")]
        [RegularExpression(@"^\d{2}-\d{4}-[a-zA-Z]{2}$", ErrorMessage = "Professional registration number must be in the format: 12-3456-AB.")]

        public string? ProfessionalRegistrationNumber { get; set; }

        [Required(ErrorMessage = "First name is required.")]

        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]

        public string? LastName { get; set; }

        public string? FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }


        // FK:
        public int? ScheduleId { get; set; }

        // And nav prop:
        public Schedule? Schedule { get; set; }
    }
}
