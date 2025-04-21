using HealthManagementSystem.DTOs;
using HealthManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthManagementSystem.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public AppointmentController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        //get all the appointments
        [HttpGet]
        public async Task<ActionResult<List<Appointment>>> GetAppointments()
        {
            var apps = await _ctx.Appointments.Include(p => p.Patient).Include(d => d.Doctor).ToListAsync();
            if(apps.Count == 0)
            {
                return NotFound("There are currently no appointments");
            }

            return Ok(apps);
        }
        //find an appointment by Id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Appointment>> FindAppointmentById(int id)
        {
            var apps = await _ctx.Appointments.FindAsync(id);
            if(apps == null)
            {
                return NotFound("No appointments found with that ID");
            }
            return Ok(apps);
        }
        //create an appointment
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentDTO appDTO)
        {          
            //make sure it actually exists or if it has correct date
            if(appDTO == null || appDTO.AppointmentDate <= DateTime.Now)
            {
                return BadRequest("Invalid appointment details.");
            }

            //check if either doctor or patient exist
            var existingDoctor = await _ctx.Doctors.AnyAsync(d => d.Id == appDTO.DoctorId);
            var existingPatient = await _ctx.Patients.AnyAsync(p => p.Id == appDTO.PatientId);

            if(!existingDoctor || !existingPatient)
            {
                return BadRequest("Invalid Doctor or Patient.");
            }

            //creating the appointment
            var newAppointment = new Appointment
            {
                PatientId = appDTO.PatientId,
                DoctorId = appDTO.DoctorId,
                AppointmentDate = appDTO.AppointmentDate
            };

            await _ctx.Appointments.AddAsync(newAppointment);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointments), new { id = newAppointment.Id }, newAppointment);
        }
    }
}