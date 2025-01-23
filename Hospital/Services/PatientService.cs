using MongoDB.Bson;
using MongoDB.Driver;
using Hospital.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Services
{
public class PatientService : IPatientService
{
    private readonly IMongoCollection<Patient> _patientCollection;
        public PatientService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _patientCollection = database.GetCollection<Patient>("Patient");
        }


    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        return await _patientCollection.Find(_ => true).ToListAsync();
    }
    public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _patientCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }    

    public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            await _patientCollection.InsertOneAsync(patient);
            return patient;
        }
    
        

   
}
}