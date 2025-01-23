using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Services{
public interface IPatientService
{
    Task<List<Patient>> GetAllPatientsAsync();
    Task<Patient> GetPatientByIdAsync(int id);
    Task <Patient> CreatePatientAsync(Patient patient);
  
}
}