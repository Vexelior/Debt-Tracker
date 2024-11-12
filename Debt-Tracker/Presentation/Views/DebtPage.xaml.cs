using Core.Interfaces;
using Core.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Presentation.ViewModels;

namespace Presentation.Views;

public partial class DebtPage : ContentPage
{
    private readonly IDebtRepository _debtRepository;
    private readonly IMongoDbService _mongoDbService;

    public DebtPage()
    {
        InitializeComponent();
        _mongoDbService = new MongoDbService();
        _debtRepository = new DebtRepository(_mongoDbService);
        BindingContext = new DebtViewModel(_debtRepository);

        MessagingCenter.Subscribe<AddDebtViewModel>(this, "DebtModified", async (sender) => await RefreshDebts());
        MessagingCenter.Subscribe<EditDebtViewModel>(this, "DebtModified", async (sender) => await RefreshDebts());
    }

    private async Task RefreshDebts()
    {
        var viewModel = BindingContext as DebtViewModel;
        if (viewModel == null)
        {
            return;
        }

        viewModel.LoadDebtsAsync();
    }

    private async void OnAddDebtButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddDebtPage(new AddDebtViewModel(_debtRepository)));
    }

    private async void OnEditDebtButtonClicked(object sender, EventArgs e)
    {

        var button = sender as Button;

        var debt = button?.BindingContext as Debt;
        if (debt == null)
        {
            return;
        }

        await Navigation.PushAsync(new EditDebtPage(debt));
    }
}