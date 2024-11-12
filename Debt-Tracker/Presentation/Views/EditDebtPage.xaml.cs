using Core.Models;
using Presentation.ViewModels;

namespace Presentation.Views;

public partial class EditDebtPage : ContentPage
{
    private readonly Debt _debt;

    private string Creditor
    {
        get => _debt.Creditor;
        set => _debt.Creditor = value;
    }

    private decimal Amount
    {
        get => _debt.Amount;
        set => _debt.Amount = value;
    }

    public EditDebtPage(Debt debt)
    {
        InitializeComponent();
        _debt = debt;
        BindingContext = new EditDebtViewModel(debt);
    }

	private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var viewModel = (EditDebtViewModel)BindingContext;
        await viewModel.UpdateDebtAsync();
        await Navigation.PopAsync();
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var viewModel = (EditDebtViewModel)BindingContext;
        await viewModel.DeleteDebtAsync(_debt.Id.ToString());
        await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}