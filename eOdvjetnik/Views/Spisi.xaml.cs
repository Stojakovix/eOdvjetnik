using eOdvjetnik.ViewModel;
using Syncfusion.Maui.DataGrid;

namespace eOdvjetnik.Views;

public partial class Spisi : ContentPage
{
	public Spisi()
	{
		InitializeComponent();
		this.BindingContext = new SpisiViewModel();

	}
}