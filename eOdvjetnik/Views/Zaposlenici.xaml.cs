using eOdvjetnik.Model;
using eOdvjetnik.ViewModel;
using System.Diagnostics;

namespace eOdvjetnik.Views;
public partial class Zaposlenici : ContentPage
{
    public Zaposlenici()
    {
        InitializeComponent();
        BindingContext = App.SharedPostavkeViewModel;
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedDevice = (Device)picker.SelectedItem;
        var selectedEmployee = (EmployeeItem)picker.BindingContext;

        selectedEmployee.EmployeeHWID = selectedDevice.hwid;

        string zaposlenik = selectedEmployee.EmployeeName;
        string computer = selectedDevice.hwid;
        Debug.WriteLine("Licenca dodana korisniku: " + zaposlenik + ", hwid: " + computer);
    }



}


