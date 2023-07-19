using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

public class Device : INotifyPropertyChanged
{
    public void ConvertHwidToHwid64()
    {
        hwid64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hwid));
    }
    private int _id;
    public int id
    {
        get { return _id; }
        set
        {
            _id = value;
            OnPropertyChanged(nameof(id));
        }
    }


    private int _company_id;
    public int company_id
    {
        get { return _company_id; }
        set
        {
            _company_id = value;
            OnPropertyChanged(nameof(company_id));
        }
    }

    private string _hwid;
    public string hwid
    {
        get { return _hwid; }
        set
        {
            _hwid = value;
            OnPropertyChanged(nameof(hwid));
        }
    }

    private string _hwid64;
    public string hwid64
    {
        get { return _hwid64; }
        set
        {
            _hwid64 = value;
            OnPropertyChanged(nameof(hwid64));
        }
    }
    private string _licence_active;
    public string licence_active
    {
        get { return _licence_active; }
        set
        {
            _licence_active = value;
            OnPropertyChanged(nameof(licence_active));
        }
    }

    private int _device_type_id;
    public int device_type_id
    {
        get { return _device_type_id; }
        set
        {
            _device_type_id = value;
            OnPropertyChanged(nameof(device_type_id));
        }
    }

    private string _opis;
    public string opis
    {
        get { return _opis; }
        set
        {
            _opis = value;
            OnPropertyChanged(nameof(opis));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class DeviceDataModel
{
    public List<Device> Devices { get; set; }
}