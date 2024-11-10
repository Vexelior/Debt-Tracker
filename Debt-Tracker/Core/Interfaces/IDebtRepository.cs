using Core.Models;

namespace Core.Interfaces
{
    public interface IDebtRepository
    {
        Task AddAsync(Debt debt);
        Task<IEnumerable<Debt>> GetAllAsync();
        Task<Debt> GetByIdAsync(string id);
        Task UpdateAsync(Debt debt);
        Task DeleteAsync(string id);
    }
}
