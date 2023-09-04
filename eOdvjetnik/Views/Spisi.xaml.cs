using eOdvjetnik.ViewModel;
using eOdvjetnik.Model;
using Syncfusion.Maui.DataGrid;
using System.Diagnostics;

namespace eOdvjetnik.Views;

public partial class Spisi : ContentPage
{
	public Spisi()
	{
		InitializeComponent();
		this.BindingContext = new SpisiViewModel();

	}

    private async void ListViewItemSelected(object sender, ItemTappedEventArgs e)
    {
        try
        {

            //var selectedTariffItem = (TariffItem)e.SelectedItem;
            var selectedFileItem = (FileItem)e.Item;
            string itemId = selectedFileItem.Id.ToString();
            Preferences.Set("listItemId", itemId);
            Debug.WriteLine(itemId + " u ListViewItemSelectedu u spisima");
            //await Shell.Current.GoToAsync("//SpiDok");
            await Navigation.PushAsync(new SpiDok());
            ((ListView)sender).SelectedItem = null;
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