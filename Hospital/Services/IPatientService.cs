using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Services{
public interface IPatientService
{
    Task<List<Patient>> GetAllPatientsAsync(int pageNumber, int pageSize);
    Task<Patient> GetPatientByIdAsync(int id);
    Task <Patient> CreatePatientAsync(Patient patient);
    Task<Patient> UpdateMedicalHistoryAsync(int id, string MedicalHistory);
    Task<bool> DeleteAsync(int id);
}
public interface IDoctor{

}
}