using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPageSpisi : ContentPage
{
    
    public LoadingPageSpisi()
	{
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine("LoadingPage:Spisi");
        OpenSpisi();
    }

    public async void OpenSpisi()
    {
       await Shell.Current.GoToAsync("///Spisi");
    }
    
}