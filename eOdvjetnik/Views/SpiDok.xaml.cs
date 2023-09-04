using eOdvjetnik.ViewModel;
using eOdvjetnik.Services;
using eOdvjetnik.Models;


namespace eOdvjetnik.Views;

public partial class SpiDok : ContentPage
{
	public static SpiDokViewModel viewModel;
	public SpiDok()
	{
		InitializeComponent();
		this.BindingContext = viewModel;

	}
}