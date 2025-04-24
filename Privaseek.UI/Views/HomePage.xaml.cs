using Privaseek.UI.ViewModels;

namespace Privaseek.UI.Views;

public partial class HomePage : ContentPage
{
	public HomePage(SearchViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}