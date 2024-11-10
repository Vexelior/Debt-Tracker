using MongoDB.Driver;

namespace Core.Interfaces
{
    public interface IMongoDbService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
