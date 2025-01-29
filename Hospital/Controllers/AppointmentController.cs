using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Hospital.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments(int pageNumber, int pageSize)
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync(pageNumber, pageSize);
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment newAppointment)
        {
            
            try
            {
                var appointment = await _appointmentService.CreateAppointmentAsync(newAppointment);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
            }

            catch (MongoWriteException ex)
            {
                
                if (ex.Message.Contains("duplicate key error"))
                {
                    return Conflict($"An appointment with ID {newAppointment.Id} already exists.");
                }
                return StatusCode(500, "An unexpected error occurred."); 
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            var deleted = await _appointmentService.DeleteAsync(id);
            if (deleted)
            {
                return Ok($"Appointment with ID {id} deleted successfully.");
            }
            return NotFound($"Appointment with ID {id} not found.");
        }
    }
}