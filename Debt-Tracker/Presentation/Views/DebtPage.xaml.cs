using Presentation.ViewModels;

namespace Presentation.Views;

public partial class DebtPage : ContentPage
{
    private readonly AddDebtPage _addDebtPage;

    public DebtPage(DebtViewModel debtViewModel, AddDebtPage addDebtPage)
    {
        InitializeComponent();
        BindingContext = debtViewModel;
        _addDebtPage = addDebtPage;
    }

    private async void OnAddDebtButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(_addDebtPage);
    }
}