namespace eOdvjetnik;
using System.Diagnostics;
using Views;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Popup;
using eOdvjetnik.ViewModel;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;

public partial class AppShell : Shell , INotifyPropertyChanged 
{

    AppShellViewModel ViewModel = new AppShellViewModel();

    private string currentRoute;

    public string CurrentRoute
    {
        get { return currentRoute; }
        set
        {
            if (currentRoute != value)
            {
                currentRoute = value;
                OnPropertyChanged(nameof(CurrentRoute));
            }
        }
    }

    public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(Kalendar), typeof(Kalendar));
		Routing.RegisterRoute(nameof(Dokumenti), typeof(Dokumenti));
		Routing.RegisterRoute(nameof(DocsItemPage), typeof(DocsItemPage));
		Routing.RegisterRoute(nameof(AppointmentDialog), typeof(AppointmentDialog));
		Routing.RegisterRoute(nameof(Klijenti), typeof(Klijenti)); 
		Routing.RegisterRoute(nameof(Naplata), typeof(Naplata));
		Routing.RegisterRoute(nameof(Spisi), typeof(Spisi));
		Routing.RegisterRoute(nameof(NoviSpis), typeof(NoviSpis));
        Routing.RegisterRoute(nameof(Postavke), typeof(Postavke));
        Routing.RegisterRoute(nameof(NoviKlijent), typeof(NoviKlijent));
        Routing.RegisterRoute(nameof(Racun), typeof(Racun));
        Routing.RegisterRoute(nameof(Zaposlenici), typeof(Zaposlenici));
        Routing.RegisterRoute(nameof(UrediKlijenta), typeof(UrediKlijenta));
        Routing.RegisterRoute(nameof(NoviZaposlenik), typeof(NoviZaposlenik));
        Routing.RegisterRoute(nameof(SpiDok), typeof(SpiDok));
        Routing.RegisterRoute(nameof(LoadingPageKalendar), typeof(LoadingPageKalendar));
        Routing.RegisterRoute(nameof(LoadingPageKlijenti), typeof(LoadingPageKlijenti));
        Routing.RegisterRoute(nameof(LoadingPageNaplata), typeof(LoadingPageNaplata));
        Routing.RegisterRoute(nameof(LoadingPageSpisi), typeof(LoadingPageSpisi));

        Routing.RegisterRoute(nameof(AdminKalendar), typeof(AdminKalendar));
        AppTheme currentTheme = AppTheme.Light;

        

        BindingContext = ViewModel;
        

        SfPopup popup = new SfPopup();
		
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);
        if (lastPressedButton != null)
        {
            // Reset the background color of the previous button
            lastPressedButton.BackgroundColor = Color.FromArgb("#faf9fb"); // Set to the initial color
        }

        if (lastPressedButton != null)
        {
            currentRoute = Shell.Current.CurrentItem.CurrentItem.Route;
            Debug.WriteLine("current route is " + currentRoute);
            // Change the background color of the current button
            // Set to the new color Color.FromArgb("#FAFAFA")
            if (currentRoute.EndsWith("Kalendar"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }
            else if (currentRoute.EndsWith("MainPage"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }
            else if (currentRoute.EndsWith("Spisi"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }
            else if (currentRoute.EndsWith("Klijenti"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }
            else if (currentRoute.EndsWith("Naplata"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }
            else if (currentRoute.EndsWith("Dokumenti"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }
            else if (currentRoute.EndsWith("Postavke"))
            {
                lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
            }

        }

    }
    private void OnKorisnickaPodrskaClicked(object sender, EventArgs e)
    {
        ViewModel.SupportVisible = true;
        ViewModel.SupportPopupOpen = true;

    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            LastPressedButton = clickedButton;
            
        }
    }
    private Button lastPressedButton = null;

    // Property to track the last pressed button
    public Button LastPressedButton
    {
        get { return lastPressedButton; }
        set
        {
            if (lastPressedButton != null)
            {
                // Reset the background color of the previous button
                lastPressedButton.BackgroundColor = Color.FromArgb("#faf9fb"); // Set to the initial color
            }

            lastPressedButton = value;

            if (lastPressedButton != null)
            {
                currentRoute = Shell.Current.CurrentItem.CurrentItem.Route;
                Debug.WriteLine("current route is " + currentRoute);
                // Change the background color of the current button
                 // Set to the new color Color.FromArgb("#FAFAFA")
                if (currentRoute.EndsWith("Kalendar"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
                else if (currentRoute.EndsWith("MainPage"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
                else if (currentRoute.EndsWith("Spisi"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
                else if (currentRoute.EndsWith("Klijenti"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
                else if (currentRoute.EndsWith("Naplata"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
                else if (currentRoute.EndsWith("Dokumenti"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
                else if (currentRoute.EndsWith("Postavke"))
                {
                    lastPressedButton.BackgroundColor = Color.FromArgb("#DEE6F2");
                }
   
            }
            

        }

    }
    public event PropertyChangedEventHandler PropertyChanged;

    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
