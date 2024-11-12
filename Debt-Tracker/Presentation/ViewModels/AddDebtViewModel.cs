using Core.Models;
using Core.Interfaces;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Presentation.Views;

namespace Presentation.ViewModels
{
    public class AddDebtViewModel : BaseViewModel
    {
        public string Creditor { get; set; }
        public decimal Amount { get; set; }

        private readonly IDebtRepository _debtRepository;
        public ObservableCollection<Debt> Debts { get; set; } = new ObservableCollection<Debt>();

        public ICommand AddDebtCommand { get; }

        // Property to bind to the UI for error messages
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(); // Notify the UI about the change
            }
        }

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set
            {
                _showError = value;
                OnPropertyChanged();
            }
        }

        public AddDebtViewModel()
        {
        }

        public AddDebtViewModel(IDebtRepository debtRepository)
        {
            _debtRepository = debtRepository;
            AddDebtCommand = new Command(async () => await AddDebtAsync());
        }

        private async Task AddDebtAsync()
        {
            if (string.IsNullOrEmpty(Creditor) || Amount <= 0)
            {
                ErrorMessage = "Creditor and Amount are required fields, and Amount must be greater than 0.";
                return;
            }

            try
            {
                var debt = new Debt
                {
                    Creditor = Creditor,
                    Amount = Amount
                };

                await _debtRepository.AddAsync(debt);
                ErrorMessage = string.Empty;
                ShowError = false;
                MessagingCenter.Send(this, "DebtModified");

                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
