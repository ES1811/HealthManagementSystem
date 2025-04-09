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
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment app)
        {
            var existingDoctor = await _ctx.Doctors.AnyAsync(i => i.Id == app.DoctorId);
            var existingPatient = await _ctx.Patients.AnyAsync(i => i.Id == app.PatientId);

            if(!existingDoctor || !existingPatient)
            {
                return BadRequest("There's no Doctor or Patient");
            }

            _ctx.Appointments.Add(app);
            await _ctx.SaveChangesAsync();

            return StatusCode(201, new{ Message = "Appointment successfully created"});
        }
    }
}