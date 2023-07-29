using eOdvjetnik.ViewModel;
using eOdvjetnik.Services;
using eOdvjetnik.Models;


namespace eOdvjetnik.Views;

public partial class SpiDok : ContentPage
{
	public SpiDok()
	{
		InitializeComponent();
		this.BindingContext = new SpiDokViewModel();

	}
}