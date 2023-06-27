namespace eOdvjetnik.Views;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Core;
using System;

public partial class Naplata : ContentPage
{
	public Naplata()
	{
        InitializeComponent();


    }

    private void Button1_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = true;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }

    private void Button2_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = true;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button3_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = true;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button4_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = true;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button5_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = true;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;

    }
    private void Button6_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = true;
        Frame7.IsVisible = false;
    }
    private void Button7_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = true;
    }
}