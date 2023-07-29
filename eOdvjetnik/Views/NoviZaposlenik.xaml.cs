using eOdvjetnik.Model;
using eOdvjetnik.ViewModel;
using System.Diagnostics;

namespace eOdvjetnik.Views;

public partial class NoviZaposlenik : ContentPage
{
	public NoviZaposlenik()
	{
		InitializeComponent();
        BindingContext = App.SharedPostavkeViewModel;
    }
}