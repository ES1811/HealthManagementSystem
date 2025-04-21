using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
#nullable disable
namespace HealthManagementSystem.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; } //foreign key
        [Required]
        public int DoctorId { get; set; } //foreign key
        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;
        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; } //navigation property
        [ForeignKey("DoctorId")]
        [JsonIgnore]
        public Doctor Doctor { get; set; } //navigation property
    }
}