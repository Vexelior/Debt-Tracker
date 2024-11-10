using Core.Interfaces;
using Core.Models;

namespace Application.Services
{
    public class DebtService(IDebtRepository debtRepository) : IDebtService
    {
        public Task AddDebtAsync(Debt debt) => debtRepository.AddAsync(debt);
        public Task<IEnumerable<Debt>> GetAllDebtsAsync() => debtRepository.GetAllAsync();
        public Task<Debt> GetDebtByIdAsync(string id) => debtRepository.GetByIdAsync(id);
        public Task UpdateDebtAsync(Debt debt) => debtRepository.UpdateAsync(debt);
        public Task DeleteDebtAsync(string id) => debtRepository.DeleteAsync(id);
    }
}
