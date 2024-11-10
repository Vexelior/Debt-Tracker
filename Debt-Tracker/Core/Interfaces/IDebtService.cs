using Core.Models;

namespace Core.Interfaces
{
    public interface IDebtService
    {
        Task AddDebtAsync(Debt debt);
        Task<IEnumerable<Debt>> GetAllDebtsAsync();
        Task<Debt> GetDebtByIdAsync(string id);
        Task UpdateDebtAsync(Debt debt);
        Task DeleteDebtAsync(string id);
    }
}
