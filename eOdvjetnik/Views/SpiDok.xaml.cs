using eOdvjetnik.ViewModel;
using eOdvjetnik.Services;
using eOdvjetnik.Models;
using System.Diagnostics;
using System.Security;

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

    private async void OnItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        var selectedFileItem = (SpiDokItem)e.DataItem;
        string fileName = selectedFileItem.Dokument;
        Debug.WriteLine(fileName);
        await viewModel.OpenFile(fileName);
    }


}