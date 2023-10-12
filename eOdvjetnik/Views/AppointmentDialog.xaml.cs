using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using System.Diagnostics;
using eOdvjetnik.Model;
using eOdvjetnik.ViewModel;
using Plugin.LocalNotification;
using System.Globalization;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace eOdvjetnik.Views;

public partial class AppointmentDialog : ContentPage, INotifyPropertyChanged
{
    private static KalendarViewModel kalendarViewModel;
    
    SchedulerAppointment appointment;
    DateTime selectedDate;
    SfScheduler scheduler;
    public ObservableCollection<FileItem> FileItems { get; set; }

    private bool isVisible;

    public bool IsVisible
    {
        get { return isVisible; }
        set
        {
            if (isVisible != value)
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
    }

    public string SQLUserID { get; set; }

    public int IDSpisa { get; set; }  

    public int ResourceId { get; set; }

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
    public DateTime MacBookStartDT { get; set; }
    public DateTime MacBookEndDT { get; set; }

    public AppointmentDialog(SchedulerAppointment appointment, DateTime selectedDate, SfScheduler scheduler)
    {
        try
        {

            InitializeComponent();
            FileItems = new ObservableCollection<FileItem>();
            //za MacBook picker:
            SelectStartHour();
            SelectStartMinute();
            SelectEndHour();
            SelectEndMinute();

            this.appointment = appointment;
            this.selectedDate = AdjustSelectedDate(selectedDate);
            
            this.scheduler = scheduler;
            isVisible = true;
            ResourceId = int.Parse(TrecaSreca.Get("resourceId"));
            Debug.WriteLine("Resource id " + ResourceId);
            //Debug.WriteLine("Svi fajlovi " + Files.Count());

            eventNameText.Placeholder = "Unesite naziv...";
            organizerText.Placeholder = "Unesite opis...";

            UpdateEditor();
            //saveButton.Clicked += SaveButton_Clicked;
            //cancelButton.Clicked += CancelButton_Clicked;
            switchAllDay.Toggled += SwitchAllDay_Toggled;
            //DeleteButton.Clicked += DeleteButton_Clicked;

            categoryPicker.SelectedIndexChanged += OnCategoryPickerSelectedIndexChanged;
            DevicePlatform = TrecaSreca.Get("vrsta_platforme");

            SQLUserID = TrecaSreca.Get("UserID");
            Debug.WriteLine("-------------------------------- " + SQLUserID);

            if (appointment != null)
            {
                DohvatiSpis(); 
            }
        }

        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + "U AppointmentDialog konstruktoru");
            Debug.WriteLine("Resource id " + ResourceId);
        }
    }

    public void DohvatiSpis()
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            //var appId = Convert.ToInt32(appointment.Id);
            string getIDQuery = $"SELECT * FROM events WHERE id \"{appointment.Id}\"";
            Debug.WriteLine(getIDQuery);
            Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(getIDQuery);

            if (filesData != null)
            {
                foreach (Dictionary<string, string> filesRow in filesData)
                {
                    int id;

                    int.TryParse(filesRow["Files"], out id);
                    if (id != 0)
                    {
                        IDSpisa = id;
                    }
                }
            }
            WeakReferenceMessenger.Default.Send(new AppointmentSpisId("fileupdated"));
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }

    private DateTime AdjustSelectedDate(DateTime selectedDate)
    {
        // Check if selectedDate is not null
        if (selectedDate != null)
        {
            // Check if the time part is midnight (00:00:00)
            if (selectedDate.TimeOfDay == TimeSpan.Zero)
            {
                // Set the time part to noon (12:00 PM)
                return selectedDate.Date.AddHours(12);
            }
        }

        // If selectedDate is null or not midnight, return it as is
        return selectedDate;
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
                if (appointment.Id != null)
                {

                    try
                    {
                        string query = "DELETE FROM events WHERE internal_event_id = " + appointment.Id;
                        Debug.WriteLine("Deleted Appointment " + appointment.Id);
                        externalSQLConnect.sqlQuery(query);
                    }
                    catch (Exception ex)
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

            //MacBook picker
            startHourPicker.IsEnabled = false;
            startMinutePicker.IsEnabled = false;
            endHourPicker.IsEnabled = false;
            endMinutePicker.IsEnabled = false;

            int defaultIndex1 = startHourPicker.Items.IndexOf("12");
            if (defaultIndex1 >= 0)
            {
                startHourPicker.SelectedIndex = defaultIndex1;
            }
            int defaultIndex2 = endHourPicker.Items.IndexOf("13");
            if (defaultIndex2 >= 0)
            {
                endHourPicker.SelectedIndex = defaultIndex2;
            }
            int defaultIndex3 = startMinutePicker.Items.IndexOf("00");
            if (defaultIndex3 >= 0)
            {
                startMinutePicker.SelectedIndex = defaultIndex3;
            }
            int defaultIndex4 = endMinutePicker.Items.IndexOf("00");
            if (defaultIndex4 >= 0)
            {
                endMinutePicker.SelectedIndex = defaultIndex4;
            }
            //da, sve ovo samo za Mac
        }
        else
        {
            startTime_picker.IsEnabled = true;
            endTime_picker.IsEnabled = true;
            (sender as Microsoft.Maui.Controls.Switch).IsToggled = false;

            //MacBook picker
            startHourPicker.IsEnabled = true;
            startMinutePicker.IsEnabled = true;
            endHourPicker.IsEnabled = true;
            endMinutePicker.IsEnabled = true;
        }
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        if (DevicePlatform == "MacCatalyst")
        {

            Shell.Current.GoToAsync("///LoadingPage");
        }
        else
        {
            Shell.Current.GoToAsync("///Kalendar");
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
            Debug.WriteLine("regular start time: " + startTime + ", end time: " + endTime);
            Debug.WriteLine(DevicePlatform);


            //MacBookPicker:
            if (DevicePlatform == "MacCatalyst") //MacBookPicker učitavanje, ako radi ok, zamijeniti != s == 
            {
                string selectedStartHour = startHourPicker.SelectedItem.ToString();
                string selectedStartMinute = startMinutePicker.SelectedItem.ToString();
                string MacStartTime = $"{selectedStartHour}:{selectedStartMinute}";

                string selectedEndHour = endHourPicker.SelectedItem.ToString();
                string selectedEndMinute = endMinutePicker.SelectedItem.ToString();
                string MacEndTime = $"{selectedEndHour}:{selectedEndMinute}";

                string format = @"hh\:mm"; 

                try
                {
                    startTime = TimeSpan.ParseExact(MacStartTime, format, CultureInfo.InvariantCulture);
                    endTime = TimeSpan.ParseExact(MacEndTime, format, CultureInfo.InvariantCulture);
                    Debug.WriteLine("Macbook start time: " + MacStartTime + ", end time: " + MacEndTime);
                    Debug.WriteLine("Macbook parsed start time: " + startTime + ", end time: " + endTime);
                    MacBookStartDT = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hours, startTime.Minutes, startTime.Seconds) ;
                    MacBookEndDT = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hours, endTime.Minutes, endTime.Seconds);
                    Debug.WriteLine("MacBookStartDateTime: " + MacBookStartDT + " MacBookEndDateTime: " + MacBookEndDT);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " in AppointmentDialog SaveButtonClicked");
                }
            }
            // MacBookPicker sve do ovdje


            if (AppoitmentColorName == null || AppoitmentColorName == "")
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
                        Application.Current.MainPage.DisplayAlert("", "Datum završetka mora biti isti kao datum početka\rili slijediti nakon njega.", "OK");
                    }
                    else if (endDate == startDate)
                    {
                        if (endTime < startTime)
                        {
                            Application.Current.MainPage.DisplayAlert("", "Vrijeme završetka mora biti veće od vremena početka", "OK");
                        }
                        else
                        {
                            AppointmentDetails();
                            AddAppointmentToRemoteServer(appointment);

                            if (DevicePlatform == "MacCatalyst")
                            {
                                Shell.Current.GoToAsync("//LoadingPage");
                                //kalendarViewModel.AdminLicenceCheck();
                            }
                            else
                            {
                                Shell.Current.GoToAsync("//Kalendar");
                                //kalendarViewModel.AdminLicenceCheck();

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
                            //kalendarViewModel.AdminLicenceCheck();

                        }
                        else
                        {
                            Shell.Current.GoToAsync("//Kalendar");
                            //kalendarViewModel.AdminLicenceCheck();

                        }
                    }
                }
            }
            //SaveNotification();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in AppointmentDialog SaveButtonClicked");
        }
    }

    private void AddAppointmentToRemoteServer(SchedulerAppointment appointment)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            //var appId = Convert.ToInt32(appointment.Id);
            string getIDQuery = $"SELECT * FROM events WHERE internal_event_id = \"{appointment.Id}\"";
            Debug.WriteLine(getIDQuery);
            Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(getIDQuery);

            if (filesData.Length == 0)
            {
                try
                {
                    var appString = appointment.Id.ToString();
                    var trimmedString = appString.Replace("-", "");
                    var appointmentId = int.Parse(trimmedString);

                    var hardware_id = TrecaSreca.Get("key");
                    if (DevicePlatform == "MacCatalyst")
                    {
                        string query = $"INSERT INTO events (TimeFrom, TimeTo, EventName, AllDay, DescriptionNotes, Files, internal_event_id, color, user_id, resource_id, hardwareid) VALUES ('{MacBookStartDT.ToString("yyyy-MM-dd HH:mm:ss")}', '{MacBookEndDT.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.Subject}', '{appointment.IsAllDay}', '{appointment.Notes}', '{IDSpisa}', '{appointment.Id}', '{AppoitmentColorName}' , '{SQLUserID}' , '{ResourceId}' , '{hardware_id}')";
                        externalSQLConnect.sqlQuery(query);
                        Debug.WriteLine(query);
                    }
                    else
                    {
                        string query = $"INSERT INTO events (TimeFrom, TimeTo, EventName, AllDay, DescriptionNotes, Files, internal_event_id, color, user_id, resource_id, hardwareid) VALUES ('{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.Subject}', '{appointment.IsAllDay}', '{appointment.Notes}', '{IDSpisa}', '{appointment.Id}', '{AppoitmentColorName}' , '{SQLUserID}' , '{ResourceId}' , '{hardware_id}')";
                        externalSQLConnect.sqlQuery(query);
                        Debug.WriteLine(query);
                    }
                     Debug.WriteLine("Appointment added to remote server. Appointment id = " + appointmentId);
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
                    var hardware_id = TrecaSreca.Get("key");
                    if (DevicePlatform == "MacCatalyst")
                    {
                        string query = $"UPDATE events SET TimeFrom = '{MacBookStartDT.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{MacBookEndDT.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', Files = '{IDSpisa}', color = '{AppoitmentColorName}',  user_id = '{SQLUserID}', resource_id = '{ResourceId}' , hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
                        externalSQLConnect.sqlQuery(query);
                        Debug.WriteLine(query);
                    }
                    else
                    {
                        string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', Files = '{IDSpisa}', color = '{AppoitmentColorName}',  user_id = '{SQLUserID}', resource_id = '{ResourceId}' , hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
                        externalSQLConnect.sqlQuery(query);
                        Debug.WriteLine(query);
                    }
                       
                    Debug.WriteLine("Appointment updated in the server");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer error");
                }
            }
            else
            {

                AppointmentDetails();
                var hardware_id = TrecaSreca.Get("key");
                if (DevicePlatform == "MacCatalyst")
                {
                    string query = $"UPDATE events SET TimeFrom = '{MacBookStartDT.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{MacBookEndDT.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', Files = '{IDSpisa}', user_id = '{ResourceId}', resource_id = '{ResourceId}' , hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query);
                }
                else
                {
                    string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', EventName = '{appointment.Subject}', AllDay = '{appointment.IsAllDay}', DescriptionNotes = '{appointment.Notes}', Files = '{IDSpisa}', user_id = '{ResourceId}', resource_id = '{ResourceId}' , hardwareid = '{hardware_id}' WHERE internal_event_id = " + appointment.Id;
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query);
                }
              Debug.WriteLine("Appointment updated in the server");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in AppointmentDialog AddAppointmentToServer");
        }
    }

    private void SaveNotification()
    {
        try
        {
            var request = new NotificationRequest
            {
                NotificationId = 1,
                Title = "Dodan novi događaj",
                Description = "U kalendaru je dodan novi događaj",
                BadgeNumber = 1,
                CategoryType = NotificationCategoryType.Status,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(10),
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
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
            appointment.ResourceIds = new ObservableCollection<object>
                        {
                (object)ResourceId // Boxing the integer into an object
            };
            if (DevicePlatform == "MacCatalyst")
            {
                string selectedStartHour = startHourPicker.SelectedItem.ToString();
                string selectedStartMinute = startMinutePicker.SelectedItem.ToString();
                string MacStartTime = $"{selectedStartHour}:{selectedStartMinute}";

                string selectedEndHour = endHourPicker.SelectedItem.ToString();
                string selectedEndMinute = endMinutePicker.SelectedItem.ToString();
                string MacEndTime = $"{selectedEndHour}:{selectedEndMinute}";

                string format = @"hh\:mm";

                try
                {
                    var startTime = TimeSpan.ParseExact(MacStartTime, format, CultureInfo.InvariantCulture);
                    var endTime = TimeSpan.ParseExact(MacEndTime, format, CultureInfo.InvariantCulture);

                    appointment.StartTime = this.startDate_picker.Date.Add(startTime);
                    appointment.EndTime = this.endDate_picker.Date.Add(endTime);
                }
                catch (Exception ex) 
                {
                    Debug.WriteLine(ex.Message + " in AppointmentDialog AppointmentDetails - MacBookPicker");
                }

            }

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
            Debug.WriteLine("U SPISU NEŠTO IMA");
            appointment.Background = AppoitmentColor;
            appointment.ResourceIds = new ObservableCollection<object>
            {
                (object)ResourceId // Boxing the integer into an object
            };
            if (DevicePlatform == "MacCatalyst")
            {
                string selectedStartHour = startHourPicker.SelectedItem.ToString();
                string selectedStartMinute = startMinutePicker.SelectedItem.ToString();
                string MacStartTime = $"{selectedStartHour}:{selectedStartMinute}";

                string selectedEndHour = endHourPicker.SelectedItem.ToString();
                string selectedEndMinute = endMinutePicker.SelectedItem.ToString();
                string MacEndTime = $"{selectedEndHour}:{selectedEndMinute}";

                string format = @"hh\:mm";

                try
                {
                    var startTime = TimeSpan.ParseExact(MacStartTime, format, CultureInfo.InvariantCulture);
                    var endTime = TimeSpan.ParseExact(MacEndTime, format, CultureInfo.InvariantCulture);

                    appointment.StartTime = this.startDate_picker.Date.Add(startTime);
                    appointment.EndTime = this.endDate_picker.Date.Add(endTime);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " in AppointmentDialog AppointmentDetails - MacBookPicker");
                }

            }

        }

        //// - add or edit the appointment in the database collection
        //var todoItem = new Appointment()
        //{
        //    From = appointment.StartTime,
        //    To = appointment.EndTime,
        //    AllDay = appointment.IsAllDay,
        //    DescriptionNotes = appointment.Notes,
        //    EventName = appointment.Subject,
        //    ID = (int)appointment.Id,
        //    CategoryColor = AppoitmentColorName,

        //};



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

                if (DevicePlatform == "MacCatalyst")
                {
                    int startHour = appointment.StartTime.Hour;
                    int startMinute = appointment.StartTime.Minute;
                    int endHour = appointment.EndTime.Hour;
                    int endMinute = appointment.EndTime.Minute;

                    startHourPicker.SelectedIndex = startHour;
                    startMinutePicker.SelectedIndex = startMinute / 5;
                    endHourPicker.SelectedIndex = endHour;
                    endMinutePicker.SelectedIndex = endMinute / 5;
                }
            }
            else
            {
                startTime_picker.Time = new TimeSpan(12, 0, 0);
                startTime_picker.IsEnabled = false;
                endTime_picker.Time = new TimeSpan(12, 0, 0);
                endTime_picker.IsEnabled = false;
                switchAllDay.IsToggled = true;

                if (DevicePlatform == "MacCatalyst")
                {
                    startHourPicker.SelectedIndex = startHourPicker.Items.IndexOf("12");
                    startMinutePicker.SelectedIndex = startMinutePicker.Items.IndexOf("00");
                }
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

            

            if (DevicePlatform == "MacCatalyst")
            {
                int macStartHour = selectedDate.Hour;
                int macEndHour = selectedDate.Hour + 1; 
                startHourPicker.SelectedIndex = macStartHour;
                endHourPicker.SelectedIndex = macEndHour;
            }


        }
    }

    private void SelectStartHour()
    {
        for (int i = 0; i <= 23; i++)
        {
            string startHour = i.ToString("00");
            startHourPicker.Items.Add(startHour);
        }

        int defaultIndex = startHourPicker.Items.IndexOf("12");
        if (defaultIndex >= 0)
        {
            startHourPicker.SelectedIndex = defaultIndex;
        }

        startHourPicker.SelectedIndexChanged += (sender, args) =>
        {
            if (startHourPicker.SelectedIndex != -1)
            {
                string selectedStartHour = startHourPicker.SelectedItem.ToString();
            }
        };
    }
    private void SelectStartMinute()
    {
        for (int i = 0; i <= 55; i += 5)
        {
            string startMinute = i.ToString("00");
            startMinutePicker.Items.Add(startMinute);
        }

        int defaultMinuteIndex = startMinutePicker.Items.IndexOf("00");
        if (defaultMinuteIndex >= 0)
        {
            startMinutePicker.SelectedIndex = defaultMinuteIndex;
        }

        startMinutePicker.SelectedIndexChanged += (sender, args) =>
        {
            if (startMinutePicker.SelectedIndex != -1)
            {
                string selectedStartMinute = startMinutePicker.SelectedItem.ToString();
            }
        };
    }
    private void SelectEndHour()
    {
        for (int i = 0; i <= 23; i++)
        {
            string endHour = i.ToString("00");
            endHourPicker.Items.Add(endHour);
        }

        int defaultIndex = endHourPicker.Items.IndexOf("13");
        if (defaultIndex >= 0)
        {
            endHourPicker.SelectedIndex = defaultIndex;
        }

        endHourPicker.SelectedIndexChanged += (sender, args) =>
        {
            if (endHourPicker.SelectedIndex != -1)
            {
                string selectedendHour = endHourPicker.SelectedItem.ToString();
            }
        };
    }
    private void SelectEndMinute()
    {
        for (int i = 0; i <= 55; i += 5)
        {
            string endMinute = i.ToString("00");
            endMinutePicker.Items.Add(endMinute);
        }

        int defaultMinuteIndex = endMinutePicker.Items.IndexOf("00");
        if (defaultMinuteIndex >= 0)
        {
            endMinutePicker.SelectedIndex = defaultMinuteIndex;
        }

        endMinutePicker.SelectedIndexChanged += (sender, args) =>
        {
            if (endMinutePicker.SelectedIndex != -1)
            {
                string selectedEndMinute = endMinutePicker.SelectedItem.ToString();
            }
        };
    }

    private async void ListViewItemDoubleTapped(object sender, Syncfusion.Maui.ListView.ItemDoubleTappedEventArgs e)
    {

        //var selectedTariffItem = (TariffItem)e.SelectedItem;
        var selectedFileItem = (FileItem)e.DataItem;
        int itemId = selectedFileItem.Id;
        IDSpisa = itemId;
        Debug.WriteLine(itemId + " u ListViewItemSelectedu u appointmentu");
        TrecaSreca.Set("IDSpisa", IDSpisa.ToString());
        //Debug.WriteLine("Item tapped " + itemId);
        Debug.WriteLine(itemId + " u ListViewItemSelectedu u spisima");

        await Navigation.PushAsync(new SpiDok());
    }
    private async void ListViewItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            //var selectedTariffItem = (TariffItem)e.SelectedItem;
            var selectedFileItem = (FileItem)e.DataItem;
            int itemId = selectedFileItem.Id;
            IDSpisa = itemId;
            Debug.WriteLine(itemId + " u ListViewItemSelectedu u appointmentu");
            TrecaSreca.Set("IDSpisa", IDSpisa.ToString());

            WeakReferenceMessenger.Default.Send(new AppointmentSpisId("fileupdated"));
            if (FileItems.Count > 1)
            {
                await Application.Current.MainPage.DisplayAlert("", "Spis povezan uz događaj", "OK");
            }



        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }
}