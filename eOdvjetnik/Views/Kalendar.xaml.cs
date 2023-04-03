using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Windows.Input;

namespace eOdvjetnik.Views;

public partial class Kalendar : ContentPage
{  
    
    public Kalendar()
    {
        
        InitializeComponent();
        Debug.WriteLine("inicijalizirano");
        _ = new SfScheduler();
    }
    private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
    {
        Navigation.PushAsync(new AppointmentDialog());
    }

}