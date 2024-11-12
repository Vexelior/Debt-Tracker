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
            LoadDebtsAsync();
        }

        public async void LoadDebtsAsync()
        {
            Debts.Clear();
            var debts = await _debtRepository.GetAllAsync();
            foreach (var debt in debts)
            {
                Debts.Add(debt);
            }

            OnPropertyChanged(nameof(Debts));

            ErrorMessage = Debts.Count == 0 ? "No debts found. Click the '+' button to add a new debt." : string.Empty;
        }
    }
}
