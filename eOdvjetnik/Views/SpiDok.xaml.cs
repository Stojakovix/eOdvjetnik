using eOdvjetnik.ViewModel;
using eOdvjetnik.Services;
using eOdvjetnik.Models;


namespace eOdvjetnik.Views;

public partial class SpiDok : ContentPage
{
	public SpiDokViewModel viewModel = new SpiDokViewModel();
	public SpiDok()
	{
		//InitializeComponent();
		//this.BindingContext = viewModel;

	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        InitializeComponent();
        this.BindingContext = viewModel;

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("//Spisi");
    }
}