namespace QuickCare.Areas.Receptionist.Models
{
    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
}

namespace QuickCare.Areas.Receptionist.Controllers.ViewModals
{
    public class PatientViewModal
    {
        public string? Search { get; set; }
        public PatientVM Patient { get; set; } = new PatientVM();
        public List<PatientVM> Patients { get; set; } = new List<PatientVM>();
        public int PageNumber { get; set; }
    }

    public class PatientVM
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? BloodGroupId { get; set; }
        public string? BloodGroup { get; set; }
        public string? Allergies { get; set; }
        public string? MedicalHistory { get; set; }
        public bool IsActive { get; set; } = true;
        public int OrganizationId { get; set; }
    }
}