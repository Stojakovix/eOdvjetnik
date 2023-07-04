namespace eOdvjetnik.Views;
using eOdvjetnik.ViewModel;
using eOdvjetnik.Model;


public partial class NoviSpis : ContentPage
{
	FileItem FileItem;
    
    public NoviSpis()
	{
        InitializeComponent();
        BindingContext = new NoviSpisViewModel();

    }
}