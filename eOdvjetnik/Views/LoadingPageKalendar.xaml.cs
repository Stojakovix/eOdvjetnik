using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPageKalendar : ContentPage
{
    
    public LoadingPageKalendar()
	{
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine("LoadingPage:Kalendar");
        OpenKalendar();
    }

    public async void OpenKalendar()
    {
       await Shell.Current.GoToAsync("///Kalendar");
    }
    
}