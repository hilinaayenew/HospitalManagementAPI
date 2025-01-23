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
    public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            var patient = await _patientService.GetAllPatientsAsync();
            return Ok(patient);
        }

    [HttpGet("{id:int}")]
    
    public ActionResult<Patient> GetPatientByID(int id)
        {
            var patient =  _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Pokemon with ID {id} not found.");
            }
            return Ok(patient);
        }
           

    [HttpPost]
     public async Task<ActionResult<Patient>> CreatePatient([FromBody] Patient newPatient)
        {
            if (newPatient == null)
            {
                return BadRequest("The Pokemon data is empty. Retry!");
            }
            var patient = await _patientService.CreatePatientAsync(newPatient);
            return CreatedAtAction(nameof(GetPatientByID), new { id = patient.Id }, patient);
        }
}
}