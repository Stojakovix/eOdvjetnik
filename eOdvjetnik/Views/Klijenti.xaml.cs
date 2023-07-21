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

        await SaveToPreferences(selectedTariffItem.Ime, selectedTariffItem.OIB, selectedTariffItem.Datum_rodenja, selectedTariffItem.Adresa, selectedTariffItem.Boraviste, selectedTariffItem.Telefon, selectedTariffItem.Fax, selectedTariffItem.Mobitel, selectedTariffItem.Email, selectedTariffItem.Ostalo, selectedTariffItem.Drzava, selectedTariffItem.Pravna);

        ((ListView)sender).SelectedItem = null;
    }


    private Task SaveToPreferences(string Ime, string OIB, DateTime Datum_rodenja, string Adresa, string Boraviste,  string Telefon, string Fax, string Mobitel, string Email, string Ostalo, string Drzava, int Pravna)
    {

        Preferences.Set("SelectedName", Ime);
        Preferences.Set("SelectedOIB", OIB);
        Preferences.Set("SelectedAddress", Adresa);
        Preferences.Set("SelectedRsidence", Boraviste);
        Preferences.Set("SelectedPhone", Telefon);
        Preferences.Set("SelectedFax", Fax);
        Preferences.Set("SelectedMobile", Mobitel);
        Preferences.Set("SelectedEmail", Email);
        Preferences.Set("SelectedOther", Ostalo);
        Preferences.Set("SelectedCountry", Drzava);
        Preferences.Set("SelectedLegalPerson", Pravna);



        return Task.CompletedTask;
    }
}