using eOdvjetnik.Data;
using eOdvjetnik.Models;
using System.Diagnostics;
using Microsoft.Maui.Devices;

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
	
	async void OnSaveClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(Item.Name))
		{
			await DisplayAlert("Please write name", "please", "ok");
			return;
		}
		await database.SaveItemAsync(Item);
		await Shell.Current.GoToAsync("..");

	}
	async void OnDeleteClicked(object sender, EventArgs e)
	{
		if (Item.ID == 0)
			return;
		await database.DeleteItemAsync(Item);
		await Shell.Current.GoToAsync("..");
	}	
	async void OnCancelClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
	
}

	
