using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Services
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllPatientsAsync(int pageNumber, int pageSize);
        Task<Patient> GetPatientByIdAsync(int id);
        Task <Patient> CreatePatientAsync(Patient patient);
        Task<Patient> UpdateMedicalHistoryAsync(int id, string MedicalHistory);
        Task<bool> DeleteAsync(int id);
    }
    public interface IDoctorService
    {
        Task<List<Doctor>> GetAllDoctorsAsync(int pageNumber, int pageSize);
        Task<Doctor> GetDoctorByIdAsync(int id);
        Task <Doctor> CreateDoctorAsync(Doctor doctor);
        Task<bool> DeleteAsync(int id);
    }


    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAppointmentsAsync(int pageNumber, int pageSize);
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAsync(int id);
    }

    public interface ICounterService
    {
        Task<int> GetNextSequenceAsync(string counterName);
    }
}