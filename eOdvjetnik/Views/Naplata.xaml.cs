namespace eOdvjetnik.Views;
using Syncfusion.Maui.Core;
using System;
using Syncfusion.DocIO;
using eOdvjetnik.Services;
using Syncfusion.DocIO.DLS;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using eOdvjetnik.ViewModel;
using Microsoft.Maui.Controls;
using eOdvjetnik.Models;
using System.Collections.ObjectModel;

public partial class Naplata : ContentPage
{
    public ObservableCollection<ReceiptItem> ReceiptItems  = new ObservableCollection<ReceiptItem>();

    

    public string CompanyName { get; set; }
    public string CompanyOIB { get; set; }
    public string CompanyAddress { get; set; }
    public string ClientName { get; set; } = "Klijent nije odabran";
    public string ClientOIB { get; set; } = "OIB";
    public string ClientAddress { get; set; } = "Adresa";

  
    public Naplata()
    {
        InitializeComponent();
        this.BindingContext = App.SharedNaplataViewModel;
        CompanyName = Preferences.Get("naziv_tvrtke", "");
        CompanyOIB = Preferences.Get("OIBTvrtke", "");
        CompanyAddress = Preferences.Get("adresaTvrtke", "");
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.AddDays(7);
        ClientName = Preferences.Get("SelectedName", "");
        ClientOIB = Preferences.Get("SelectedOIB", "");
        ClientAddress = Preferences.Get("SelectedAddress", "");
      
    }

    
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
    }
    public double MinWidth { get; set; }

   //private async void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e) //ovo je za obični ListView, za syncfusion je u NaplataViewModel
   //{
   //    if (e.SelectedItem == null)
   //        return;
   //
   //    var selectedTariffItem = (TariffItem)e.SelectedItem;
   //
   //    await SaveToPreferences(selectedTariffItem.oznaka, selectedTariffItem.bodovi, selectedTariffItem.concatenated_name, selectedTariffItem.parent_name);
   //
   //    ((ListView)sender).SelectedItem = null;
   //}
   //
   //
   //private Task SaveToPreferences(string oznaka, string bodovi, string concatenatedName, string parent_name)
   //{
   //
   //    Preferences.Set("SelectedOznaka", oznaka);
   //    Preferences.Set("SelectedBodovi", bodovi);
   //    Preferences.Set("SelectedConcatenatedName", concatenatedName);
   //    Preferences.Set("SelectedParentName", parent_name);
   //
   //
   //    return Task.CompletedTask;
   //}

}