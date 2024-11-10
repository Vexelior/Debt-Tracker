using Presentation.ViewModels;

namespace Presentation.Views;

public partial class AddDebtPage : ContentPage
{
	public AddDebtPage(AddDebtViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}