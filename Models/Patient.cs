using System.ComponentModel.DataAnnotations;
#nullable disable
namespace HealthManagementSystem.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Patient Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}