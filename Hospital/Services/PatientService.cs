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
        private readonly ICounterService _counterService;
        public PatientService(IConfiguration configuration,ICounterService counterService)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _patientCollection = database.GetCollection<Patient>("Patient");    
            _counterService = counterService;
        }


        public async Task<List<Patient>> GetAllPatientsAsync(int pageNumber, int pageSize)
        {
        var sortDefinition =  Builders<Patient>.Sort.Ascending(p => p.Name);
          

             
            if (pageNumber == 1 && pageSize < 0)
            {
                 return await _patientCollection.Find( p=> true).Sort(sortDefinition).ToListAsync();
            }
            if (pageNumber <= 0 || pageSize <= 0)
            {
                  return new List<Patient>();
            }
             
            return await _patientCollection.Find(p => true).Sort(sortDefinition).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
        }
        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _patientCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }    

        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            patient.Id = await _counterService.GetNextSequenceAsync("patientId");      
            await _patientCollection.InsertOneAsync(patient);
            return patient;
        }
        public async Task<Patient> UpdateMedicalHistoryAsync(int id, string UpdateMedicalHistory)
        {
             if(UpdateMedicalHistory == ""){
                throw new Exception (" medical history is required");
             }
            var updated= Builders<Patient>.Update.Set(p=>p.MedicalHistory,UpdateMedicalHistory);
            var result= await _patientCollection.UpdateOneAsync(p => p.Id==id,updated);
            if(result.MatchedCount==0)
            {
                throw new Exception("no matching patient found");
            }
           
                return await _patientCollection.Find(p=> p.Id ==id).FirstOrDefaultAsync();
        }
    
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _patientCollection.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }   
    }
}