using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Collections.ObjectModel;
using eOdvjetnik.Services;
using eOdvjetnik.ViewModel;
using eOdvjetnik.Models;
using System.Diagnostics.CodeAnalysis;
using Syncfusion.Maui.GridCommon.Collections.Generic;


namespace eOdvjetnik.Views;

public partial class Kalendar : ContentPage
{
    private KalendarViewModel _viewModel;
    
    
    private bool isInitialized;
    public Kalendar()
    {
        try
        {
            InitializeComponent();
            Debug.WriteLine("inicijalizirano");
            this.Scheduler.DragDropSettings.TimeIndicatorTextFormat = "HH:mm";
            Scheduler.DaysView.TimeRegions = GetTimeRegion();


            Scheduler.AppointmentDrop += OnSchedulerAppointmentDrop;

            //MySQL Query;
            var hardware_id = Preferences.Get("key", "default_value");
            isInitialized = false;


        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel == null)
        {
            _viewModel = new KalendarViewModel();
            this.BindingContext = _viewModel;
            Debug.WriteLine("ViewModel initialized");
        }

        else if (!isInitialized)
        {
            isInitialized = true;
            _viewModel.AdminLicenceCheck();
            Debug.WriteLine("izvršio on appearing adminLicenceCheck");

        }

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        var appointments = _viewModel.Appointments;
        //var resources = _viewModel.Resources;
        if(appointments != null)
        {
            appointments.Clear();
            //resources.Clear();
        }


    }

    private void OnSchedulerAppointmentDrop(object? sender, AppointmentDropEventArgs eventArgs)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            var appointment = eventArgs.Appointment;

            var droptime = eventArgs.DropTime;
            var timeLength = appointment.EndTime - appointment.StartTime;

            if (eventArgs.Appointment != null)
            {
                appointment.StartTime = eventArgs.DropTime;
                appointment.EndTime = appointment.StartTime + timeLength;

            }

            Debug.WriteLine(appointment.StartTime + " " + appointment.EndTime);

            string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE internal_event_id = " + appointment.Id;
            externalSQLConnect.sqlQuery(query);

        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }


    private void OnSchedulerViewChanged(object sender, SchedulerViewChangedEventArgs e)
    {
        try
        {
            Scheduler.ShowBusyIndicator = true;
            // Check if the new view is a TimelineMonth view
            if (e.NewView is SchedulerView.TimelineMonth)
            {
                _viewModel.AdminLicenceCheck();

            }
            else
            {
                // Code to execute when the view changes to something else
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
    private ObservableCollection<SchedulerTimeRegion> GetTimeRegion()
    {
        var startTime = DateTime.UnixEpoch.AddHours(19);
        var endTime = startTime.AddHours(12);

        var timeRegions = new ObservableCollection<SchedulerTimeRegion>();
        var timeRegion = new SchedulerTimeRegion()

        {

            StartTime = startTime,
            EndTime = endTime,
            //Text = "pauza",
            EnablePointerInteraction = true,
            RecurrenceRule = "FREQ=DAILY;INTERVAL=1",


        };
        timeRegions.Add(timeRegion);

        var wkStart = DateTime.UnixEpoch.AddHours(1);
        var wkEnd = DateTime.UnixEpoch.AddHours(23);
        timeRegion = new SchedulerTimeRegion()
        {
            StartTime = wkStart,
            EndTime = wkEnd,
            //Text = "pauza",
            EnablePointerInteraction = true,
            RecurrenceRule = "FREQ=WEEKLY;BYDAY=SA,SU;INTERVAL=1",
        };
        timeRegions.Add(timeRegion);
        return timeRegions;
    }

    //private void Scheduler_tapped(object sender, SchedulerTappedEventArgs e )
    //{
    //    try
    //    {
    //        if (e.Element == SchedulerElement.SchedulerCell || e.Element == SchedulerElement.Appointment)
    //        {
    //            Debug.WriteLine("------------------" + e.Resource.Id);
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        Debug.WriteLine(ex.Message);
    //    }
    //}
    private void Scheduler_DoubleTapped(object sender, SchedulerDoubleTappedEventArgs e)
    {

        try
        {
            if (e.Element == SchedulerElement.SchedulerCell || e.Element == SchedulerElement.Appointment)
            {
                var SQLUserID = Preferences.Get("UserID", "");

                if (e.Resource != null)
                {
                    string resourceId = e.Resource.Id.ToString();
                    
                    Debug.WriteLine(resourceId);
                    
                    if (Scheduler.View == SchedulerView.TimelineMonth)
                    {
                        Preferences.Remove("resourceId");
                        Preferences.Set("resourceId", resourceId);
                        Debug.WriteLine("-----------------------------------------------------Izvršen if " + resourceId);

                    }
                    else
                    {
                        Preferences.Remove("resourceId");
                        Preferences.Set("resourceId", SQLUserID);
                        Debug.WriteLine("-------------------------------------Izvršen else" + SQLUserID);
                    }

                    Debug.WriteLine(resourceId + "-----------------------------------------------------");
                    if (e.Appointments != null)
                    {
                        Navigation.PushAsync(new AppointmentDialog((SchedulerAppointment)e.Appointments[0], (e.Appointments[0] as SchedulerAppointment).StartTime, this.Scheduler));
                        Debug.WriteLine("izvršen drugi if");
                    }
                    else
                    {
                        Navigation.PushAsync(new AppointmentDialog(null, (DateTime)e.Date, this.Scheduler));
                        Debug.WriteLine("izvršen drugi else ");
                    }
                }
                else
                {

                    //Debug.WriteLine(resourceId + "-----------------------------------------------------");
                    if (e.Appointments != null)
                    {
                        Preferences.Remove("resourceId");
                        Preferences.Set("resourceId", SQLUserID);
                        Navigation.PushAsync(new AppointmentDialog((SchedulerAppointment)e.Appointments[0], (e.Appointments[0] as SchedulerAppointment).StartTime, this.Scheduler));

                        Debug.WriteLine("--------------------------------------------------Appointment pun");
                    }
                    else
                    {
                        Preferences.Remove("resourceId");
                        Preferences.Set("resourceId", SQLUserID);
                        Navigation.PushAsync(new AppointmentDialog(null, (DateTime)e.Date, this.Scheduler));

                        Debug.WriteLine("-------------------------------------------------Appointment prazan");

                    }
                }
            }
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message + "u scheduler double tappedu");
        }
    }

    //pokušaj da se promjeni border prilikom klika na appointment i nestane nakon klika na neki drugi
    //private void scheduler_tapped(object sender, schedulertappedeventargs e)
    //{
    //    try
    //    {
    //        if (e.element == schedulerelement.schedulercell || e.element == schedulerelement.appointment)
    //        {
    //            if (e.appointments != null)
    //            {
    //                scheduler.selectedappointmentbackground = colors.blue;
    //            }
    //            else
    //            {
    //                scheduler.selectedappointmentbackground = colors.transparent;

    //            }
    //        }
    //    }
    //    catch (exception ex)
    //    {

    //        debug.writeline(ex.message);
    //    }
    //}

}