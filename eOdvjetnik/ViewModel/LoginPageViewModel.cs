


using System.ComponentModel;

namespace eOdvjetnik.ViewModel;

public class LoginPageViewModel : INotifyPropertyChanged
{
    public LoginPageViewModel()
    {





    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
