using eOdvjetnik.ViewModel;
using eOdvjetnik.Model;
using Syncfusion.Maui.DataGrid;

namespace eOdvjetnik.Views;

public partial class Spisi : ContentPage
{
	public Spisi()
	{
		InitializeComponent();
		this.BindingContext = new SpisiViewModel();

	}
    //void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
    //{
    //    C
    //}
}