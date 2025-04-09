using System.ComponentModel.DataAnnotations;
namespace HealthManagementSystem.Models
#nullable disable
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public string LicenseNumber { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        //generate a randomize license number for the doctor
        public Doctor()
        {
            LicenseNumber = GenerateLicense();
        }
        private string GenerateLicense()
        {
            Random random = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char letter = letters[random.Next(letters.Length)];
            int numbers = random.Next(100, 1000);
            return $"{letter}{numbers}";
        }

    }
}