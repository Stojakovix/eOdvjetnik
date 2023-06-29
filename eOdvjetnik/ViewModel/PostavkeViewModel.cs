using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace eOdvjetnik.ViewModel;

public class PostavkeViewModel : INotifyPropertyChanged
{
    public string HWID { get; set; }
    public string HWID64 { get; set; }

    public string LicenceType  { get; set; }
    public string dateTimeString { get; set; }
    public DateTime expiryDate { get; set; }
    public string ExpiryDate { get; set; }
    public string Activation_code { get; set; }


    public PostavkeViewModel()
	{
       HWID = Preferences.Get("key", null);
       LicenceType = Preferences.Get("licence_type", "");
       dateTimeString = Preferences.Get("expire_date", "");
       Activation_code = Preferences.Get("activation_code", "");
       HWID64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(HWID));
       ParseDate();
    }

    public void ParseDate()
    {
        try
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(dateTimeString);
            expiryDate = dateTimeOffset.Date;
            ExpiryDate = expiryDate.ToString("D");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Date parsing error:" + ex.Message);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}