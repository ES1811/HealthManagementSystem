#nullable disable
namespace HealthManagementSystem.DTOs
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }

    }
}