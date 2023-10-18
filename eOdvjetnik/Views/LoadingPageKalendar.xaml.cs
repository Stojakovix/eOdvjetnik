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
        IdiNaKalendar();
    }

    public async void IdiNaKalendar()
    {
       await Shell.Current.GoToAsync("///Kalendar");
    }
    
}