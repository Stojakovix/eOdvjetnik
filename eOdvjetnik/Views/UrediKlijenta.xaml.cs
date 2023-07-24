namespace eOdvjetnik.Views;
using eOdvjetnik.ViewModel;

public partial class UrediKlijenta : ContentPage
{
	public UrediKlijenta()
	{
		InitializeComponent();
		this.BindingContext = new NoviKlijentViewModel();
	}
}