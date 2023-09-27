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
   
    public Klijenti()
	{
        InitializeComponent();
        this.BindingContext = App.SharedKlijentiViewModel;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
        
    }

}