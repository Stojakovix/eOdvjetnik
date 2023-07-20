namespace eOdvjetnik.Views;
using eOdvjetnik.ViewModel;

public partial class NoviKlijent : ContentPage
{
	public NoviKlijent()
	{
		InitializeComponent();
		this.BindingContext = new NoviKlijentViewModel();
	}
}