using MongoDB.Driver;
using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class CounterService : ICounterService
    {
        private readonly IMongoCollection<Counter> _counterCollection;

        public CounterService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _counterCollection = database.GetCollection<Counter>("Counters");
        }

        public async Task<int> GetNextSequenceAsync(string counterName)
        {
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var counter = await _counterCollection.FindOneAndUpdateAsync(
                c => c.Id == counterName,
                update,
                options
            );

            return counter.SequenceValue;
        }
    }
}