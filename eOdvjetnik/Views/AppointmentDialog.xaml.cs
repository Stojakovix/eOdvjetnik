using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using System.Diagnostics;
using eOdvjetnik.Model;
using System.ComponentModel;

namespace eOdvjetnik.Views;

public partial class AppointmentDialog : ContentPage
{

    SchedulerAppointment appointment;
    DateTime selectedDate;
    SfScheduler scheduler;

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
    public AppointmentDialog(SchedulerAppointment appointment, DateTime selectedDate, SfScheduler scheduler)
    {
        InitializeComponent();
        this.appointment = appointment;
        this.selectedDate = selectedDate;
        this.scheduler = scheduler;
        eventNameText.Placeholder = "Unesite naziv...";
        organizerText.Placeholder = "Unesite opis...";

        UpdateEditor();
        saveButton.Clicked += SaveButton_Clicked;
        cancelButton.Clicked += CancelButton_Clicked;
        switchAllDay.Toggled += SwitchAllDay_Toggled;
        DeleteButton.Clicked += DeleteButton_Clicked;

        categoryPicker.SelectedIndexChanged += OnCategoryPickerSelectedIndexChanged;

    }
    private void OnCategoryPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedColorItem = (ColorItem)categoryPicker.SelectedItem;
        eventNameText.BackgroundColor = selectedColorItem.BojaPozadine;
        AppoitmentColor = selectedColorItem.BojaPozadine;
        AppoitmentColorName = selectedColorItem.NazivBoje;
        Debug.WriteLine(AppoitmentColorName);
    }
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            
            if (appointment == null)
            {
                this.Navigation.PopAsync();

            }
            else
            {
                (this.scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Remove(this.appointment);
                //var todoItem = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, DescriptionNotes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
                if(appointment.Id != null)
                {
                    string query = "DELETE FROM events WHERE internal_event_id = " + appointment.Id;
                    Debug.WriteLine("Deleted Appointment " + appointment.Id);
                    externalSQLConnect.sqlQuery(query);
                }
                else
                {
                    Debug.WriteLine("Appointment id was null");
                }
                
                Shell.Current.GoToAsync("//Kalendar");
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
        Shell.Current.GoToAsync("//Kalendar");
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
                    Shell.Current.GoToAsync("///Kalendar");
                }
            }
            else
            {
                AppointmentDetails();
                AddAppointmentToRemoteServer(appointment);
                Navigation.PopAsync();
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
                    string query = $"INSERT INTO events (TimeFrom, TimeTo, EventName, AllDay, DescriptionNotes, internal_event_id, color, hardwareid) VALUES ('{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.Subject}', '{appointment.IsAllDay}', '{appointment.Notes}', '{appointment.Id}', '{AppoitmentColorName}' , '{hardware_id}')";
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query);
                    Debug.WriteLine("Appointment added to remote server.");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer if");
                }
            }
            else
            {
                try
                {
                    AppointmentDetails();
                    var hardware_id = Preferences.Get("key", "default_value");
                    string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', color = '{AppoitmentColorName}', hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
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

        this.Navigation.PopAsync();

    }
    private void UpdateEditor()
    {
        if (appointment != null)
        {
            eventNameText.Text = appointment.Subject.ToString();
            organizerText.Text = appointment.Notes;
            startDate_picker.Date = appointment.StartTime;
            endDate_picker.Date = appointment.EndTime;


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