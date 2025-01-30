using MongoDB.Bson;
using MongoDB.Driver;
using Hospital.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IMongoCollection<Doctor> _doctorCollection;
        private readonly ICounterService _counterService;
        public DoctorService(IConfiguration configuration,ICounterService counterService)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _doctorCollection = database.GetCollection<Doctor>("Doctor");
            _counterService = counterService;
        }


        public async Task<List<Doctor>> GetAllDoctorsAsync(int pageNumber, int pageSize)
        {
            var sortDefinition = Builders<Doctor>.Sort.Ascending(p => p.Name);
              if (pageNumber == 1 && pageSize < 0)
            {
                 return await _doctorCollection.Find( p=> true).Sort(sortDefinition).ToListAsync();
            }
             
             if (pageNumber <= 0 || pageSize <= 0)
            {
                  return new List<Doctor>();
            }
            return await _doctorCollection.Find(p => true).Sort(sortDefinition).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
        }
        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await _doctorCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }    

        public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
        {
            doctor.Id = await _counterService.GetNextSequenceAsync("doctorId");     
            await _doctorCollection.InsertOneAsync(doctor);
            return doctor;
        }
        
        
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _doctorCollection.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }   

    }
}