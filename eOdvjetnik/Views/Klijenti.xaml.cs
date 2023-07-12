namespace eOdvjetnik.Views;

using eOdvjetnik.ViewModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using System;
using eOdvjetnik.Model;
using eOdvjetnik.Models;

public partial class Klijenti : ContentPage
{
	public Klijenti()
	{
		InitializeComponent();
        this.BindingContext = new KlijentiViewModel();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
    }
    private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var selectedTariffItem = (ContactItem)e.SelectedItem;

        await SaveToPreferences(selectedTariffItem.Ime, selectedTariffItem.OIB, selectedTariffItem.Adresa);

        ((ListView)sender).SelectedItem = null;
    }


    private Task SaveToPreferences(string Ime, string OIB, string Adresa)
    {

        Preferences.Set("SelectedName", Ime);
        Preferences.Set("SelectedOIB", OIB);
        Preferences.Set("SelectedAddress", Adresa);

        return Task.CompletedTask;
    }
}