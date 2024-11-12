using Core.Interfaces;
using Core.Models;
using Infrastructure.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;
using Infrastructure;

namespace Presentation.ViewModels
{
    public class EditDebtViewModel : BaseViewModel
    {
        private readonly IDebtRepository _debtRepository;
        private readonly IMongoDbService _mongoDbService;

        private Debt _debt;
        private string _creditor;
        private decimal _amount;

        public Debt Debt
        {
            get => _debt;
            set => SetProperty(ref _debt, value);
        }

        public string Id
        {
            get => _debt.Id.ToString();
            set => _debt.Id = new ObjectId(value);
        }

        public string Creditor
        {
            get => _creditor;
            set => SetProperty(ref _creditor, value);
        }

        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public EditDebtViewModel(Debt debt)
        {
            Debt = debt;
            Creditor = Debt.Creditor ?? string.Empty;
            Amount = Debt.Amount;
            Id = _debt.Id.ToString();

            _mongoDbService = new MongoDbService();
            _debtRepository = new DebtRepository(_mongoDbService);

            LoadDebtAsync(_debt.Id.ToString());
        }

        private async Task LoadDebtAsync(string debtId)
        {
            Debt = await _debtRepository.GetByIdAsync(debtId);
        }

        public async Task UpdateDebtAsync()
        {
            Debt debt = new Debt
            {
                Id = _debt.Id,
                Creditor = _creditor,
                Amount = _amount
            };

            await _debtRepository.UpdateAsync(debt);
            MessagingCenter.Send(this, "DebtModified");
        }

        public async Task DeleteDebtAsync(string id)
        {
            await _debtRepository.DeleteAsync(id);
            MessagingCenter.Send(this, "DebtModified");
        }
    }
}
