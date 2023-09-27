using eOdvjetnik.ViewModel;
using eOdvjetnik.Model;
using Syncfusion.Maui.DataGrid;
using System.Diagnostics;
using Syncfusion.Maui.ListView;
using eOdvjetnik.Services;

namespace eOdvjetnik.Views;

public partial class Spisi : ContentPage
{
    public SpisiViewModel viewModel = new SpisiViewModel();
    private bool isInitialized;
	public Spisi()
	{
        InitializeComponent();
        this.BindingContext = viewModel;
        isInitialized = false;

	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isInitialized)
        {
            isInitialized = true;
        }
        else
        {

            viewModel.GenerateFiles();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.FileItems.Clear();

    }

    private async void ListViewItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs  e)
    {
        try
        {
            
            //var selectedTariffItem = (TariffItem)e.SelectedItem;
            var selectedFileItem = (FileItem)e.DataItem;
            string itemId = selectedFileItem.Id.ToString();
            //Preferences.Set("listItemId", itemId);
            TrecaSreca.Set("listItemId", itemId);
            //Debug.WriteLine("Item tapped " + itemId);
            Debug.WriteLine(itemId + " u ListViewItemSelectedu u spisima");

            await Navigation.PushAsync(new SpiDok());
            //((ListView)sender).SelectedItem = null;
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }
    //private Task SaveToPreferences(string oznaka, string bodovi, string concatenatedName, string parent_name)
    //{

    //    Preferences.Set("SelectedOznaka", oznaka);
    //    Preferences.Set("SelectedBodovi", bodovi);
    //    Preferences.Set("SelectedConcatenatedName", concatenatedName);
    //    Preferences.Set("SelectedParentName", parent_name);


    //    return Task.CompletedTask;
    //}
}