using MongoDB.Driver;
using Hospital.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class AppointmentService : IAppointmentService
    {
        
        private readonly IMongoCollection<Appointment> _appointmentCollection;
        private readonly IMongoCollection<Doctor> _doctorCollection;
        private readonly IMongoCollection<Patient> _patientCollection;
        private readonly ICounterService _counterService;

        public AppointmentService(IConfiguration configuration,ICounterService counterService)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _appointmentCollection = database.GetCollection<Appointment>("Appointment");
            _doctorCollection = database.GetCollection<Doctor>("Doctor");
            _patientCollection = database.GetCollection<Patient>("Patient");
            _counterService = counterService;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync(int pageNumber, int pageSize)
        {
            var sortDefinition = Builders<Appointment>.Sort.Ascending(a => a.ADate);
            if (pageNumber == 1 && pageSize < 0)
            {
                return await _appointmentCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
            }
            if (pageNumber <= 0 || pageSize <= 0)
            {
                  return new List<Appointment>();
            }
            return await _appointmentCollection.Find(a => true).Sort(sortDefinition).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _appointmentCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            
            var doctorExists = await _doctorCollection.Find(d => d.Id == appointment.DoctorId).AnyAsync();
            if (!doctorExists)
            {
                throw new Exception($"Doctor with ID {appointment.DoctorId} does not exist.");
            }

            
            var patientExists = await _patientCollection.Find(p => p.Id == appointment.PatientId).AnyAsync();
            if (!patientExists)
            {
                throw new Exception($"Patient with ID {appointment.PatientId} does not exist.");
            }

            if (appointment.ADate < DateTime.UtcNow)
            {
               throw new Exception("Appointment date and time must be in the future.");
            }


            var existingAppointmentsD = await _appointmentCollection.Find(a => 
                a.DoctorId == appointment.DoctorId &&
                a.ADate >= appointment.ADate.AddMinutes(-30) &&
                a.ADate <= appointment.ADate.AddMinutes(30))
                .ToListAsync();

            if (existingAppointmentsD.Any())
            {
                throw new Exception("The appointment must be at least 30 minutes away from the last appointment for the same doctor.");
            }

            var existingAppointmentsP = await _appointmentCollection.Find(a => 
                a.PatientId == appointment.PatientId &&
                a.ADate >= appointment.ADate.AddMinutes(-120) &&
                a.ADate <= appointment.ADate.AddMinutes(120))
                .ToListAsync();

            if (existingAppointmentsP.Any())
            {
                throw new Exception("The appointment must be at least 2 hours away from the last appointment for the same patient.");
            }
            
             appointment.Id = await _counterService.GetNextSequenceAsync("appointmentId");
             await _appointmentCollection.InsertOneAsync(appointment);
             return appointment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _appointmentCollection.DeleteOneAsync(a => a.Id == id);
            return result.DeletedCount > 0;
        }
    }
}