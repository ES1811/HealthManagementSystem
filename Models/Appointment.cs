using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        public DateTime AppointmentDate { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } //navigation property
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } //navigation property
    }
}