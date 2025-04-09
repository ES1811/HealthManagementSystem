using HealthManagementSystem.DTOs;
using HealthManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HealthManagementSystem.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public PatientController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        //get all patients first
        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            var patients = await _ctx.Patients.Include(a => a.Appointments).ToListAsync();
            if (patients.Count == 0)
            {
                return NotFound("No patients found");
            }
            return Ok(patients);
        }
        //API constraints must be added to remove the conflicts between routing
        //get a single patient by id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _ctx.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound("No patient found with that ID");
            }
            return Ok(patient);
        }
        //get a single patient by email
        [HttpGet("{email}")]
        public async Task<ActionResult<Patient>> GetPatientByEmail(string email)
        {
            var patient = await _ctx.Patients.FirstOrDefaultAsync(e => e.Email == email);
            if (patient == null)
            {
                return NotFound("No patient found with that email");
            }
            return Ok(patient);
        }
        //add a patient
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient([FromBody] PatientDTO patientDTO)
        {
            var existingPatient = await _ctx.Patients.FirstOrDefaultAsync(e => e.Email == patientDTO.Email);
            if (existingPatient != null)
            {
                return BadRequest("Email is already in use");
            }

            var newPatient = new Patient
            {
                Name = patientDTO.Name,
                Email = patientDTO.Email,
                DateOfBirth = patientDTO.DateOfBirth
            };

            await _ctx.Patients.AddAsync(newPatient);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { Message = "Patient successfully added" });
        }
        //modify a patient by id
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Patient>> UpdatePatient(int id, [FromBody] PatientDTO updatedPatient)
        {
            if (id != updatedPatient.Id)
            {
                return BadRequest("ID mismatch, please try again");
            }
            var existingPatient = await _ctx.Patients.FirstOrDefaultAsync(i => i.Id == updatedPatient.Id);
            if (existingPatient == null)
            {
                return NotFound("Patient not found");
            }
            existingPatient.Name = updatedPatient.Name ?? existingPatient.Name;
            existingPatient.Email = updatedPatient.Email ?? existingPatient.Email;

            await _ctx.SaveChangesAsync();

            return Ok(new { Message = "Patient successfully update", Patient = existingPatient });
        }
        //delete a patient by id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Patient>> DeletePatient(int id)
        {
            var existingPatient = await _ctx.Patients.FindAsync(id);
            if (existingPatient == null)
            {
                return NotFound("No Patient found with that ID");
            }

            _ctx.Patients.Remove(existingPatient);
            await _ctx.SaveChangesAsync();

            return Ok(new { Message = "Patient successfully deleted.", Patient = existingPatient });
        }
        //delete a patient by email
        [HttpDelete("{email}")]
        public async Task<ActionResult<Patient>> DeletePatientByEmail(string email)
        {
            var existingPatient = await _ctx.Patients.FirstOrDefaultAsync(e => e.Email == email);
            if (existingPatient == null)
            {
                return NotFound("No patient found with that email");
            }
            _ctx.Patients.Remove(existingPatient);
            await _ctx.SaveChangesAsync();

            return Ok(new { Message = "Patient successfully deleted.", Patient = existingPatient });
        }
    }
}