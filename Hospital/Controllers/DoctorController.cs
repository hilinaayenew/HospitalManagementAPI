using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Hospital.Services;
using MongoDB.Driver;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetAllDoctors(int pageNumber, int pageSize)
        {
                
                var doctor = await _doctorService.GetAllDoctorsAsync(pageNumber, pageSize);
                
                return Ok(doctor);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Doctor>> GetDoctorByID(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }
            return Ok(doctor);
        }
            

        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] Doctor newDoctor)
        {
            try{
                var doctor = await _doctorService.CreateDoctorAsync(newDoctor);
                return CreatedAtAction(nameof(GetDoctorByID), new { id = doctor.Id }, doctor);
            }
            catch (MongoWriteException ex)
            {
                    
                if (ex.Message.Contains("duplicate key error"))
                {
                    return Conflict($"A Doctor with ID {newDoctor.Id} already exists.");
                }
                    return StatusCode(500, "An unexpected error occurred."); 
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }  

        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            var deleted = await _doctorService.DeleteAsync(id);
            if (deleted)
            {
                return Ok($"doctor with ID {id} deleted successfully.");
            }
            return NotFound($"doctor with ID {id} not found.");
        }    
    }
}