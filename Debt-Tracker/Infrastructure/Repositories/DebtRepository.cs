using MongoDB.Driver;
using Core.Models;
using Core.Interfaces;
using MongoDB.Bson;

namespace Infrastructure.Repositories
{
    public class DebtRepository : IDebtRepository
    {
        private readonly IMongoCollection<Debt> _debtCollection;

        public DebtRepository(IMongoDbService mongoDbService)
        {
            _debtCollection = mongoDbService.GetCollection<Debt>("Debts");
        }

        public async Task<IEnumerable<Debt>> GetAllAsync()
        {
            return await _debtCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Debt> GetByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _debtCollection.Find(d => d.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Debt debt)
        {
            await _debtCollection.InsertOneAsync(debt);
        }

        public async Task UpdateAsync(Debt debt)
        {
            await _debtCollection.ReplaceOneAsync(d => d.Id == debt.Id, debt);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _debtCollection.DeleteOneAsync(d => d.Id == objectId);
        }
    }
}
