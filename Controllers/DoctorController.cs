using HealthManagementSystem.DTOs;
using HealthManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HealthManagementSystem.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public DoctorController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        //get all doctors first
        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetDoctors()
        {
            var doctors = await _ctx.Doctors.Include(p => p.Appointments).ToListAsync();
            if (doctors.Count == 0)
            {
                return NotFound("No doctors found");
            }
            return Ok(doctors);
        }
        //get the doctor by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Doctor>> GetDoctorById(int id)
        {
            var doctors = await _ctx.Doctors.FindAsync(id);
            if (doctors == null)
            {
                return NotFound("No doctors found with that ID");
            }
            return Ok(doctors);
        }
        //find a doctor by their License Number
        [HttpGet("{license}")]
        public async Task<ActionResult<Doctor>> GetDoctorByLicense(string license)
        {
            var doctor = await _ctx.Doctors.FirstOrDefaultAsync(l => l.LicenseNumber == license);
            if (doctor == null)
            {
                return NotFound("No doctor found with that License number");
            }
            return Ok(doctor);
        }
        //add a doctor
        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] DoctorDTO doctorDTO)
        {
            var existingDoctor = await _ctx.Doctors.FirstOrDefaultAsync(i => i.LicenseNumber == doctorDTO.LicenseNumber);
            if (existingDoctor != null)
            {
                return BadRequest($"Doctor with license number {doctorDTO.LicenseNumber} already exists");
            }

            //generate the License, see line 139
            string licenseGenerated = GenerateLicense();

            if (string.IsNullOrEmpty(doctorDTO.LicenseNumber))
            {
                doctorDTO.LicenseNumber = GenerateLicense();
            }

            var newDoctor = new Doctor
            {
                Name = doctorDTO.Name,
                Specialization = doctorDTO.Specialization,
                LicenseNumber = licenseGenerated
            };

            await _ctx.Doctors.AddAsync(newDoctor);
            await _ctx.SaveChangesAsync();

            return StatusCode(201, new { Message = "Doctor successfully created" });
        }
        //modify a doctor
        [HttpPut]
        public async Task<ActionResult<Doctor>> UpdateDoctor(int id, [FromBody] DoctorDTO updatedDoctor)
        {
            //add ID mismatch
            if (id != updatedDoctor.Id)
            {
                return BadRequest("ID mismatch, please try again");
            }

            var existingDoctor = await _ctx.Doctors.FirstOrDefaultAsync(i => i.Id == updatedDoctor.Id);
            if (existingDoctor == null)
            {
                return NotFound("No doctor found with that ID");
            }

            existingDoctor.Name = updatedDoctor.Name ?? existingDoctor.Name;
            existingDoctor.Specialization = updatedDoctor.Specialization ?? existingDoctor.Specialization;

            await _ctx.SaveChangesAsync();

            return Ok(new { Message = "Doctor successfully updated", Doctor = existingDoctor });

        }
        //delete a doctor by ID
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Doctor>> DeleteDoctor(int id)
        {
            var existingDoctor = await _ctx.Doctors.FindAsync(id);
            if(existingDoctor == null)
            {
                return NotFound("No doctor found with that ID");
            }

            _ctx.Doctors.Remove(existingDoctor);
            await _ctx.SaveChangesAsync();

            return Ok(new{Message = "Doctor successfully deleted", Doctor = existingDoctor});
        }
        //delete doctor by License number
        [HttpDelete("{license}")]
        public async Task<ActionResult<Doctor>> DeleteDoctorByLicense(string license)
        {
            var existingDoctor = await _ctx.Doctors.FirstOrDefaultAsync(l => l.LicenseNumber == license);
            if(existingDoctor == null)
            {
                return NotFound("No doctor found with that License number");
            }

            _ctx.Doctors.Remove(existingDoctor);
            await _ctx.SaveChangesAsync();

            return Ok(new{Message = "Doctor successfully deleted", Doctor = existingDoctor});
        }

        //additional method to ensure License for the doctor gets generated beforehand
        private static string GenerateLicense()
        {
            Random random = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char letter = letters[random.Next(letters.Length)];
            int numbers = random.Next(100, 1000);
            return $"{letter}{numbers}";
        }
    }
}