using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Hospital.Services;

namespace Hospital.Controllers{
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
    
    public ActionResult<Patient> GetPatientByID(int id)
        {
            var patient = _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }
            return Ok(patient);
        }
           

    [HttpPost]
     public async Task<ActionResult<Patient>> CreatePatient([FromBody] Patient newPatient)
        {
            if (newPatient == null)
            {
                return BadRequest("The Patient data is empty. Retry!");
            }
            var patient = await _patientService.CreatePatientAsync(newPatient);
            return CreatedAtAction(nameof(GetPatientByID), new { id = patient.Id }, patient);
        }
    [HttpPut("update medical history/{id}")]
    public async Task<IActionResult> UpdateMedicalHistory(int id,[FromBody] string medicalHistory)
        {
                try{
                   var updated= await _patientService.UpdateMedicalHistoryAsync(id,medicalHistory);
                    return Ok(updated);
                }
                catch(Exception ex){
                     return NotFound(ex.Message);
                }  

        }
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