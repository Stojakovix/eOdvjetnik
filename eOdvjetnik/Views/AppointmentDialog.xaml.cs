using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using eOdvjetnik.Models;


namespace eOdvjetnik.Views;

public partial class AppointmentDialog : ContentPage
{
    SchedulerAppointment appointment;
    DateTime selectedDate;
    SfScheduler scheduler;
    public int MinuteIncrement { get; set; }
    public AppointmentDialog(SchedulerAppointment appointment, DateTime selectedDate, SfScheduler scheduler)
    {
        InitializeComponent();
        this.appointment = appointment;
        this.selectedDate = selectedDate;
        this.scheduler = scheduler;
        cancelButton.BackgroundColor = Color.FromArgb("#E5E5E5");
        saveButton.BackgroundColor = Color.FromArgb("#2196F3");
        eventNameText.Placeholder = "Event name";
        organizerText.Placeholder = "update the notes";

        UpdateEditor();
        saveButton.Clicked += SaveButton_Clicked;
        cancelButton.Clicked += CancelButton_Clicked;
        switchAllDay.Toggled += SwitchAllDay_Toggled;
        DeleteButton.Clicked += DeleteButton_Clicked;

    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        (this.scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Remove(this.appointment);
        var todoItem = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, DescriptionNotes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
        App.Database.DeleteSchedulerAppointmentAsync(todoItem);
        this.Navigation.PopAsync();
    }

    private void SwitchAllDay_Toggled(object sender, ToggledEventArgs e)
    {
        if ((sender as Switch).IsToggled)
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
            (sender as Switch).IsToggled = false;
        }
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.Navigation.PopAsync();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
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
            }
        }
        else
        {
            AppointmentDetails();
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

            if (this.scheduler.AppointmentsSource == null)
            {
                this.scheduler.AppointmentsSource = new ObservableCollection<SchedulerAppointment>();
            }

            appointment.Id = (this.scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Count;

            (this.scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Add(appointment);
        }
        else
        {
            appointment.Subject = this.eventNameText.Text;
            appointment.StartTime = this.startDate_picker.Date.Add(this.startTime_picker.Time);
            appointment.EndTime = this.endDate_picker.Date.Add(this.endTime_picker.Time);
            appointment.IsAllDay = this.switchAllDay.IsToggled;
            appointment.Notes = this.organizerText.Text;
        }

        //// - add or edit the appointment in the database collection
        var todoItem = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, DescriptionNotes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
        App.Database.SaveSchedulerAppointmentAsync(todoItem);

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