using Privaseek.App.ViewModels;

namespace Privaseek.UI.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}