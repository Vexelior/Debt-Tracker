using MongoDB.Driver;
using Core.Interfaces;

namespace Infrastructure
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService()
        {
            var connectionString = "mongodb+srv://asanderson94:6uf8WUl5v6YpanwH@debt-tracker-cluster.iy5d4.mongodb.net/?retryWrites=true&w=majority&appName=debt-tracker-cluster";

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Debt-Tracker");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
