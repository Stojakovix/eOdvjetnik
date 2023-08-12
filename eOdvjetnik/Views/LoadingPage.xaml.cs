using eOdvjetnik.ViewModel;
using Microsoft.Maui.Dispatching;
using System;
using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPage : ContentPage
{
    int Counter { get; set; }
    public LoadingPage()
	{
		InitializeComponent();
        Counter = 0;
        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, e) => RefreshTime();
        timer.Start();
    }

    protected override void OnAppearing()
    {
        Counter = 0;
        base.OnAppearing();
        Shell.Current.GoToAsync("//Kalendar");
        Debug.WriteLine("Ušao u loading");
    }


    void RefreshTime()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Counter++;
            if ( Counter == 1)
            {
                Shell.Current.GoToAsync("//Kalendar");
                Debug.WriteLine("otvorio kalendar");
            }
        }
        );
    }
}