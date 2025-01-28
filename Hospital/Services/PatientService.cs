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


    public async Task<List<Patient>> GetAllPatientsAsync(int pageNumber, int pageSize)
    {
       var sortDefinition = true 
        ? Builders<Patient>.Sort.Ascending(p => p.Name) 
        : Builders<Patient>.Sort.Descending(p => p.Name);
         if (pageNumber <= 0 || pageSize <= 0)
         {
        // Return all patients if pageNumber or pageSize is invalid
        return await _patientCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
        }
        return await _patientCollection.Find(p => true).Sort(sortDefinition).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
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
    public async Task<Patient> UpdateMedicalHistoryAsync(int id, string UpdateMedicalHistory)
        {
            var updated= Builders<Patient>.Update.Set(p=>p.MedicalHistory,UpdateMedicalHistory);
           var result= await _patientCollection.UpdateOneAsync(p => p.Id==id,updated);
            if(result.MatchedCount==0){
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