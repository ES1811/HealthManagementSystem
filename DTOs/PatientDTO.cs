using System.ComponentModel.DataAnnotations;

#nullable disable
namespace HealthManagementSystem.DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
    }
}