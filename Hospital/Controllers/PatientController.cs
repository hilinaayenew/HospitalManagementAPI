using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Hospital.Services;
using MongoDB.Driver;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetAllPatients(int pageNumber, int pageSize)
        {
                
            var patient = await _patientService.GetAllPatientsAsync(pageNumber, pageSize);
                
            return Ok(patient);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Patient>> GetPatientByID(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }
            return Ok(patient);
        }
            

        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient([FromBody] Patient newPatient)
        {
            try
            {
                var patient = await _patientService.CreatePatientAsync(newPatient);
                return CreatedAtAction(nameof(GetPatientByID), new { id = patient.Id }, patient);
            }
            catch (MongoWriteException ex)
            {
                        
                if (ex.Message.Contains("duplicate key error"))
                {
                    return Conflict($"A Patient with ID {newPatient.Id} already exists.");
                }
                    return StatusCode(500, "An unexpected error occurred."); 
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }  

        }

        [HttpPut("update medical history/{id}")]
        public async Task<IActionResult> UpdateMedicalHistory(int id,[FromBody] string medicalHistory)
        {
            try
            {
                var updated= await _patientService.UpdateMedicalHistoryAsync(id,medicalHistory);
                return Ok(updated);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }  

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var deleted = await _patientService.DeleteAsync(id);
            if (deleted)
            {
                return Ok($"Patient with ID {id} deleted successfully.");
            }
            return NotFound($"Patient with ID {id} not found.");
        }    
    }
}