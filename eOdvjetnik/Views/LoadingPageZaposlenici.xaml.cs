using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPageZaposlenici : ContentPage
{
    
    public LoadingPageZaposlenici()
	{
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine("LoadingPage:Zaposlenici");
        OpenZaposlenici();
    }

    public async void OpenZaposlenici()
    {
       await Shell.Current.GoToAsync("///Zaposlenici");
    }
    
}