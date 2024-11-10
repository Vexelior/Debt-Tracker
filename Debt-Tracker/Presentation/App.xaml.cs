using Presentation.Views;

namespace Presentation
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App(DebtPage debtPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(debtPage);
        }
    }
}
