using eOdvjetnik.Data;
using eOdvjetnik.Models;


namespace eOdvjetnik.Views;

[QueryProperty("Item", "Item")]
public partial class DocsItemPage : ContentPage
{
	DocsItem item;
	public DocsItem Item
	{
		get => BindingContext as DocsItem;
		set => BindingContext = value;
	}
	DocsDatabase database;
	public DocsItemPage(DocsDatabase docsDatabase)
	{
		InitializeComponent();
		database = docsDatabase;
	}
	
	
}