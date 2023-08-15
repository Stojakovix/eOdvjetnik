using eOdvjetnik.ViewModel;
using Microsoft.Maui.Dispatching;
using System;
using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPage : ContentPage
{
    
    public LoadingPage()
	{
        InitializeComponent();
    }

    //protected override void OnAppearing()
    //{
    //    try
    //    {
    //        InitializeComponent();
    //        base.OnAppearing();
    //        Debug.WriteLine("Usao u loading");
    //        IdiNaKalendar();
    //    }
    //    catch(Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message);
    //    }
        
    //}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine("Usao u loading");
        IdiNaKalendar();
    }

    public async void IdiNaKalendar()
    {
       await Shell.Current.GoToAsync("///Kalendar");
    }
    
}