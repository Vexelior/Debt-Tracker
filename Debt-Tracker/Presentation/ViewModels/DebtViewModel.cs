using System.Collections.ObjectModel;
using Core.Interfaces;
using Core.Models;

namespace Presentation.ViewModels
{
    public class DebtViewModel : BaseViewModel
    {
        private readonly IDebtRepository _debtRepository;

        public ObservableCollection<Debt> Debts { get; set; } = new ObservableCollection<Debt>();

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public DebtViewModel() { }

        public DebtViewModel(IDebtRepository debtRepository)
        {
            _debtRepository = debtRepository;
            LoadDebts();
        }

        private async void LoadDebts()
        {
            var debts = await _debtRepository.GetAllAsync();

            var enumerableDebts = debts.ToList();
            if (!enumerableDebts.Any())
            {
                ErrorMessage = "No debts found.";
                return;
            }

            foreach (var debt in enumerableDebts)
            {
                Debts.Add(debt);
            }
        }
    }
}
