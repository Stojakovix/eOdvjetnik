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


    private  KalendarViewModel _viewModel;
    private bool isInitialized;
    public Kalendar(KalendarViewModel viewModel)
    {
        try
        {
            _viewModel = viewModel;
            this.BindingContext = viewModel;
            InitializeComponent();
            Debug.WriteLine("inicijalizirano");

            
            this.Scheduler.DragDropSettings.TimeIndicatorTextFormat = "HH:mm";
            Scheduler.DaysView.TimeRegions = GetTimeRegion();
            Scheduler.AppointmentDrop += OnSchedulerAppointmentDrop;
            //MySQL Query;
            var hardware_id = TrecaSreca.Get("key");
            isInitialized = false;


        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message + "u kalendar konstruktoru");
        }

    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    if (!isInitialized)
    //    {
    //        isInitialized = true;
    //        Debug.WriteLine("ViewModel initialized");
    //    }

    //    else
    //    {
    //        //isInitialized = true;
    //        if (Scheduler.View is SchedulerView.TimelineMonth || Scheduler.View is SchedulerView.Agenda)
    //        {
    //            _viewModel.GetColors();
    //            _viewModel.AdminLicenceCheck();
    //            Debug.WriteLine("izvršio on appearing adminLicenceCheck");
    //        }
    //        else
    //        {
    //            _viewModel.GetUserEvents();
    //            Debug.WriteLine("Izvršio onAppearing GetUser events");
    //        }

    //    }

    //}

    private void OnSchedulerAppointmentDragStarting(object? sender, AppointmentDragStartingEventArgs e)
    {
        e.Cancel = true; 
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        var appointments = _viewModel.Appointments;
        //var resources = _viewModel.Resources;
        var resources = _viewModel.Resources;
        if(appointments != null)
        {
            //appointments.Clear();
            //resources.Clear();
            Debug.WriteLine("Triggered onDisappearing");
            //resources.Clear();
            if (Scheduler != null)
            {
                Scheduler.AppointmentDragStarting += OnSchedulerAppointmentDragStarting;
            }
            //resources.Clear();
        }


    }

    private void OnSchedulerAppointmentDrop(object? sender, AppointmentDropEventArgs eventArgs)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            var appointment = eventArgs.Appointment;
            
            if (Scheduler.View == SchedulerView.TimelineMonth)
            {
                var droptime = eventArgs.DropTime;
                var timeLength = appointment.EndTime - appointment.StartTime;
                var resourceId = eventArgs.TargetResource.Id;

                if (eventArgs.Appointment != null)
                {
                    appointment.StartTime = eventArgs.DropTime;
                    appointment.EndTime = appointment.StartTime + timeLength;
                    //appointment.ResourceIds.Clear();
                    //appointment.ResourceIds.Add(resourceId);
                    appointment.ResourceIds = new ObservableCollection<object>
                {
                    (object)resourceId // Boxing the integer into an object
                };

                    Debug.WriteLine(appointment.StartTime + " " + appointment.EndTime);
                    Debug.WriteLine(resourceId);

                    string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', resource_id = '{resourceId}' WHERE internal_event_id = " + appointment.Id;
                    Debug.WriteLine(query);
                    externalSQLConnect.sqlQuery(query);

                }
            }

            else
            {
                var SQLUserID = TrecaSreca.Get("UserID");
                var droptime = eventArgs.DropTime;
                var timeLength = appointment.EndTime - appointment.StartTime;
                var resourceId = SQLUserID;

                if (eventArgs.Appointment != null)
                {
                    appointment.StartTime = eventArgs.DropTime;
                    appointment.EndTime = appointment.StartTime + timeLength;
                    //appointment.ResourceIds.Clear();
                    //appointment.ResourceIds.Add(resourceId);
                    appointment.ResourceIds = new ObservableCollection<object>
                    {
                        (object)SQLUserID // Boxing the integer into an object
                    };

                    Debug.WriteLine(appointment.StartTime + " " + appointment.EndTime);
                    Debug.WriteLine(resourceId);

                    string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', resource_id = '{resourceId}' WHERE internal_event_id = " + appointment.Id;
                    Debug.WriteLine(query);
                    externalSQLConnect.sqlQuery(query);

                }


            }
        }


        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message + " u on scheduler appointment dropu");
        }
    }


    private  void OnSchedulerViewChanged(object sender, SchedulerViewChangedEventArgs e)
    {
        try
        {
            Scheduler.ShowBusyIndicator = true;
            // Check if the new view is a TimelineMonth view
            if (Scheduler.View is SchedulerView.TimelineMonth || Scheduler.View is SchedulerView.Agenda)
            {
                //_viewModel.GetColors();
                // _viewModel.AdminLicenceCheck();
                //_viewModel = new KalendarViewModel();
                
                _viewModel.GetAllEvents();
                Debug.WriteLine("izvršen onSchedulerViewChanged u ifu");
            }
            else
            {
                _viewModel.GetUserEvents();
                Debug.WriteLine("izvršen else u OnSchedulerViewChanged");
                // Code to execute when the view changes to something else
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " U OnSchedulerViewChangedu");
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
                var SQLUserID = TrecaSreca.Get("UserID");

                if (e.Resource != null)
                {
                    string resourceId = e.Resource.Id.ToString();
                    
                    Debug.WriteLine(resourceId);
                    
                    if (Scheduler.View == SchedulerView.TimelineMonth)
                    {
                        TrecaSreca.Remove("resourceId");
                        TrecaSreca.Set("resourceId", resourceId);
                        Debug.WriteLine("-----------------------------------------------------Izvršen if " + resourceId);

                    }
                    else
                    {
                        TrecaSreca.Remove("resourceId");
                        if(SQLUserID == null)
                        {
                            SQLUserID = "1";
                            TrecaSreca.Set("resourceId", SQLUserID);
                        } else
                        {
                            TrecaSreca.Set("resourceId", SQLUserID);
                            Debug.WriteLine("-------------------------------------Izvršen else" + SQLUserID);
                        }
                        //TrecaSreca.Set("resourceId", SQLUserID);
                        //Debug.WriteLine("-------------------------------------Izvršen else" + SQLUserID);
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
                        TrecaSreca.Remove("resourceId");
                        TrecaSreca.Set("resourceId", SQLUserID);
                        Navigation.PushAsync(new AppointmentDialog((SchedulerAppointment)e.Appointments[0], (e.Appointments[0] as SchedulerAppointment).StartTime, this.Scheduler));

                        Debug.WriteLine("--------------------------------------------------Appointment pun");
                    }
                    else
                    {
                        TrecaSreca.Remove("resourceId");

                        if (SQLUserID == null)
                        {
                            SQLUserID = "1";
                            TrecaSreca.Set("resourceId", SQLUserID);
                        }
                        else
                        {
                            TrecaSreca.Set("resourceId", SQLUserID);
                            Debug.WriteLine("-------------------------------------Izvršen else" + SQLUserID);
                        }

                        //TrecaSreca.Set("resourceId", SQLUserID);
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