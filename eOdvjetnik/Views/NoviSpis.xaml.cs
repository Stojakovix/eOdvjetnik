namespace eOdvjetnik.Views;
using eOdvjetnik.ViewModel;
using eOdvjetnik.Model;


public partial class NoviSpis : ContentPage
{


    private NoviSpisViewModel viewModel;

    public NoviSpis()
    {
        InitializeComponent();
        viewModel = new NoviSpisViewModel();
        BindingContext = viewModel;
    }

    private void OnStatusSelected(object sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            string selectedStatus = picker.SelectedItem as string;
            viewModel.AktivnoPasivno = selectedStatus;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        viewModel = new NoviSpisViewModel();
        BindingContext = viewModel;
    }
}