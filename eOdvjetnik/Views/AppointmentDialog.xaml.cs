using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using System.Diagnostics;
using eOdvjetnik.Model;
using CommunityToolkit.Mvvm.Messaging;

namespace eOdvjetnik.Views;

public partial class AppointmentDialog : ContentPage
{

    SchedulerAppointment appointment;
    DateTime selectedDate;
    SfScheduler scheduler;
    public string SQLUserID { get; set; }


    private string _appoitmentColorName;

    public string AppoitmentColorName
    {
        get => _appoitmentColorName;
        set
        {
            if (_appoitmentColorName != value)
            {
                _appoitmentColorName = value;
                OnPropertyChanged(nameof(AppoitmentColorName));
            }
        }
    }
    private Color _appoitmentColor;

    public Color AppoitmentColor
    {
        get => _appoitmentColor;
        set
        {
            if (_appoitmentColor != value)
            {
                _appoitmentColor = value;
                OnPropertyChanged(nameof(AppoitmentColor));
            }
        }
    }
    public string DevicePlatform { get; set; }
    public AppointmentDialog(SchedulerAppointment appointment, DateTime selectedDate, SfScheduler scheduler)
    {

        InitializeComponent();
        this.appointment = appointment;
        this.selectedDate = selectedDate;
        this.scheduler = scheduler;
        eventNameText.Placeholder = "Unesite naziv...";
        organizerText.Placeholder = "Unesite opis...";

        UpdateEditor();
        //saveButton.Clicked += SaveButton_Clicked;
        //cancelButton.Clicked += CancelButton_Clicked;
        switchAllDay.Toggled += SwitchAllDay_Toggled;
        //DeleteButton.Clicked += DeleteButton_Clicked;

        categoryPicker.SelectedIndexChanged += OnCategoryPickerSelectedIndexChanged;
        DevicePlatform = Preferences.Get("vrsta_platforme", "");
        SQLUserID = Preferences.Get("UserID", "");

    }
    private void OnCategoryPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedColorItem = (ColorItem)categoryPicker.SelectedItem;
        eventNameText.BackgroundColor = selectedColorItem.BojaPozadine;
        AppoitmentColor = selectedColorItem.BojaPozadine;
        AppoitmentColorName = selectedColorItem.NazivBoje;
    }


    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            
            if (appointment == null)
            {
                if (DevicePlatform == "MacCatalyst")
                {
                    Shell.Current.GoToAsync("///LoadingPage");

                }
                else
                {
                    Shell.Current.GoToAsync("///Kalendar");

                }

            }
            else
            {
                (this.scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Remove(this.appointment);
                //var todoItem = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, DescriptionNotes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
                if(appointment.Id != null)
                {
                    
                    try
                    {
                        string query = "DELETE FROM events WHERE internal_event_id = " + appointment.Id;
                        Debug.WriteLine("Deleted Appointment " + appointment.Id);
                        externalSQLConnect.sqlQuery(query);
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    if (DevicePlatform == "MacCatalyst")
                    {
                        Shell.Current.GoToAsync("///LoadingPage");

                    }
                    else
                    {
                        Shell.Current.GoToAsync("///Kalendar");

                    }

                }
                else
                {
                    Debug.WriteLine("Appointment id was null");
                }

            }
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }


    private void SwitchAllDay_Toggled(object sender, ToggledEventArgs e)
    {
        if ((sender as Microsoft.Maui.Controls.Switch).IsToggled)
        {
            startTime_picker.Time = new TimeSpan(12, 0, 0);
            startTime_picker.IsEnabled = false;
            endTime_picker.Time = new TimeSpan(12, 0, 0);
            endTime_picker.IsEnabled = false;
        }
        else
        {
            startTime_picker.IsEnabled = true;
            endTime_picker.IsEnabled = true;
            (sender as Microsoft.Maui.Controls.Switch).IsToggled = false;
        }
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        if (DevicePlatform == "MacCatalyst")
        {
            
            Shell.Current.GoToAsync("//LoadingPage");
        }
        else
        {
            Shell.Current.GoToAsync("//Kalendar");
        }
       
       
        Debug.WriteLine("Cancel Clicked");
    }
   

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var endDate = endDate_picker.Date;
            var startDate = startDate_picker.Date;
            var endTime = endTime_picker.Time;
            var startTime = startTime_picker.Time;

            if(AppoitmentColorName == null || AppoitmentColorName == "")
            {
                Application.Current.MainPage.DisplayAlert("", "Potrebno je odabrati kategoriju.", "OK");

            }
            else
            {
                Debug.WriteLine("Ima kategoriju");
            if (eventNameText.Text == null || eventNameText.Text == "")
            {
                Application.Current.MainPage.DisplayAlert("", "Potrebno je unijeti naziv događaja.", "OK");

            }
            else
            {

                if (endDate < startDate)
                {
                    Application.Current.MainPage.DisplayAlert("", "End time should be greater than start time", "OK");
                }
                else if (endDate == startDate)
                {
                    if (endTime < startTime)
                    {
                        Application.Current.MainPage.DisplayAlert("", "End time should be greater than start time", "OK");
                    }
                    else
                    {
                        AppointmentDetails();
                        AddAppointmentToRemoteServer(appointment);

                            if (DevicePlatform == "MacCatalyst")
                            {
                                Shell.Current.GoToAsync("//LoadingPage");
                            }
                            else
                            {
                                Shell.Current.GoToAsync("//Kalendar");
                            }

                          
                        }
                }
                else
                {
                    AppointmentDetails();
                    AddAppointmentToRemoteServer(appointment);
                        if (DevicePlatform == "MacCatalyst")
                        {
                            Shell.Current.GoToAsync("//LoadingPage");
                        }
                        else
                        {
                            Shell.Current.GoToAsync("//Kalendar");
                        }
                    }
            }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in KalendarViewModel SaveButtonClicked");
        }
    }

    private void AddAppointmentToRemoteServer(SchedulerAppointment appointment)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

            string getIDQuery = $"SELECT * FROM events WHERE internal_event_id = {appointment.Id}";
            Debug.WriteLine(getIDQuery);
            Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(getIDQuery);
            
            if (filesData.Length == 0)
            {
                try
                {

                    var hardware_id = Preferences.Get("key", "default_value");
                    string query = $"INSERT INTO events (TimeFrom, TimeTo, EventName, AllDay, DescriptionNotes, internal_event_id, color, user_id, hardwareid) VALUES ('{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.Subject}', '{appointment.IsAllDay}', '{appointment.Notes}', '{appointment.Id}', '{AppoitmentColorName}' , '{SQLUserID}' , '{hardware_id}')";
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query);
                    Debug.WriteLine("Appointment added to remote server.");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer if");
                }
            }
            else if (AppoitmentColorName != null)
            {
                try
                {
                    AppointmentDetails();
                    var hardware_id = Preferences.Get("key", "default_value");
                    string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', color = '{AppoitmentColorName}', user_id = '{SQLUserID}', hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query);
                    Debug.WriteLine("Appointment updated in the server");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer else");
                }
            }
            else
            {
                try
                {
                    AppointmentDetails();
                    var hardware_id = Preferences.Get("key", "default_value");
                    string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', user_id = '{SQLUserID}', hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query);
                    Debug.WriteLine("Appointment updated in the server");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer else");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer");
        }
    }


    private void AppointmentDetails()
    {
        if (appointment == null)
        {
            appointment = new SchedulerAppointment();
            appointment.Subject = this.eventNameText.Text;
            appointment.StartTime = this.startDate_picker.Date.Add(this.startTime_picker.Time);
            appointment.EndTime = this.endDate_picker.Date.Add(this.endTime_picker.Time);
            appointment.IsAllDay = this.switchAllDay.IsToggled;
            appointment.Notes = this.organizerText.Text;
            appointment.Background = AppoitmentColor;

            if (this.scheduler.AppointmentsSource == null)
            {
                this.scheduler.AppointmentsSource = new ObservableCollection<SchedulerAppointment>();
            }


            (this.scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Add(appointment);
        }
        else
        {
            appointment.Subject = this.eventNameText.Text;
            appointment.StartTime = this.startDate_picker.Date.Add(this.startTime_picker.Time);
            appointment.EndTime = this.endDate_picker.Date.Add(this.endTime_picker.Time);
            appointment.IsAllDay = this.switchAllDay.IsToggled;
            appointment.Notes = this.organizerText.Text;
            AppoitmentColorName = appointment.Location;
            appointment.Background = AppoitmentColor;

        }

        //// - add or edit the appointment in the database collection
        var todoItem = new Appointment()
        {
            From = appointment.StartTime,
            To = appointment.EndTime,
            AllDay = appointment.IsAllDay,
            DescriptionNotes = appointment.Notes,
            EventName = appointment.Subject,
            ID = (int)appointment.Id,
            CategoryColor = AppoitmentColorName,

        };



    }
    private void UpdateEditor()
    {
        if (appointment != null)
        {
            eventNameText.Text = appointment.Subject.ToString();
            organizerText.Text = appointment.Notes;
            startDate_picker.Date = appointment.StartTime;
            endDate_picker.Date = appointment.EndTime;

            var appointmentColor = (appointment.Background as SolidColorBrush)?.Color;
            Debug.WriteLine("Tražena boja: " + appointmentColor.ToArgbHex());

            for (int i = 0; i < categoryPicker.Items.Count; i++)
            {
                var colorItem = (ColorItem)categoryPicker.ItemsSource[i];
                if (appointmentColor.ToArgbHex() == colorItem.BojaPozadine.ToArgbHex())
                {
                    categoryPicker.SelectedIndex = i;
                    OnCategoryPickerSelectedIndexChanged(categoryPicker, EventArgs.Empty); 
                    break;
                }
            }


            if (!appointment.IsAllDay)
            {
                startTime_picker.Time = new TimeSpan(appointment.StartTime.Hour, appointment.StartTime.Minute, appointment.StartTime.Second);
                endTime_picker.Time = new TimeSpan(appointment.EndTime.Hour, appointment.EndTime.Minute, appointment.EndTime.Second);
                switchAllDay.IsToggled = false;
            }
            else
            {
                startTime_picker.Time = new TimeSpan(12, 0, 0);
                startTime_picker.IsEnabled = false;
                endTime_picker.Time = new TimeSpan(12, 0, 0);
                endTime_picker.IsEnabled = false;
                switchAllDay.IsToggled = true;
            }

        }
        else
        {
            eventNameText.Text = "";
            organizerText.Text = "";
            switchAllDay.IsToggled = false;
            startDate_picker.Date = selectedDate;
            startTime_picker.Time = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            endDate_picker.Date = selectedDate;
            endTime_picker.Time = new TimeSpan(selectedDate.Hour + 1, selectedDate.Minute, selectedDate.Second);
        }
    }

  
}