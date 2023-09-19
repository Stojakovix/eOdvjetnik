namespace eOdvjetnik.Views;

using eOdvjetnik.ViewModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using System;
using eOdvjetnik.Model;
using eOdvjetnik.Models;
using System.Diagnostics;

public partial class Klijenti : ContentPage
{
    public KlijentiViewModel viewModel = new KlijentiViewModel();
    private bool isInitialized;
    public Klijenti()
	{
        InitializeComponent();
        this.BindingContext = viewModel;
        isInitialized = false;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
        if (!isInitialized)
        {
            isInitialized = true;
        }
        else
        {
            viewModel.GenerateFiles();
        }
        Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAA" + isInitialized.ToString());
    }

   


    //private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    //{
    //    if (e.SelectedItem == null)
    //        return;

    //    var selectedTariffItem = (ContactItem)e.SelectedItem;

    //    await SaveToPreferences(selectedTariffItem.Id, selectedTariffItem.Ime, selectedTariffItem.OIB, selectedTariffItem.Datum_rodenja, selectedTariffItem.Adresa, selectedTariffItem.Boraviste, selectedTariffItem.Telefon, selectedTariffItem.Fax, selectedTariffItem.Mobitel, selectedTariffItem.Email, selectedTariffItem.Ostalo, selectedTariffItem.Drzava, selectedTariffItem.Pravna);

    //    ((ListView)sender).SelectedItem = null;
    //}


    //private Task SaveToPreferences(int Id, string Ime, string OIB, string Datum_rodenja, string Adresa, string Boraviste,  string Telefon, string Fax, string Mobitel, string Email, string Ostalo, string Drzava, string Pravna)
    //{
    //    Preferences.Set("SelectedName", Ime);
    //    Preferences.Set("SelectedOIB", OIB);
    //    Preferences.Set("SelectedAddress", Adresa);
    //    Preferences.Set("SelectedRsidence", Boraviste);
    //    Preferences.Set("SelectedPhone", Telefon);
    //    Preferences.Set("SelectedFax", Fax);
    //    Preferences.Set("SelectedMobile", Mobitel);
    //    Preferences.Set("SelectedEmail", Email);
    //    Preferences.Set("SelectedOther", Ostalo);
    //    Preferences.Set("SelectedCountry", Drzava);
    //    Preferences.Set("SelectedLegalPersonString", Pravna);
    //    Preferences.Set("SelectedBrithDateString", Datum_rodenja);

    //    try
    //    {
    //        string IDstring = Id.ToString();
    //        Preferences.Set("SelectedID", IDstring);
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message);
    //    }



    //    return Task.CompletedTask;
    //}
}