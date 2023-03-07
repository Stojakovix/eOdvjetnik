using SMBLibrary.Client;
using SMBLibrary;
using System.Net;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Diagnostics;

namespace eOdvjetnik;

public partial class MainPage : ContentPage
{
    //int count = 0;

    public MainPage()
	{
		InitializeComponent();

        //SMB
        SMB2Client client = new SMB2Client(); // SMB2Client can be used as well
        bool isConnected = client.Connect(System.Net.IPAddress.Parse("192.168.1.211"), SMBTransportType.DirectTCPTransport);
        if (isConnected)
        {
            NTStatus status = client.Login(String.Empty, "user", "walter");
            if (status == NTStatus.STATUS_SUCCESS)
            {
                List <string> shares = client.ListShares(out _);
                System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");
                foreach (var share in shares)
                {
                    System.Diagnostics.Debug.WriteLine(share);
                }
                System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");
                var shares2 = "SPOJENO";
                CounterBtn.Text = shares2;
                
                
            }
            else
            {
                DisplayAlert("Connection", "Connection not established", "try again");
                
            }
            client.Logoff();
            client.Disconnect();
        }
        



        //Kraj SMB
    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        //await Navigation.PushAsync(new kalendar());
        await Shell.Current.GoToAsync("Kalendar");


        








        //if (count == 1)
        //CounterBtn.Text = $"Spojeno {count} ";
        //else
        //CounterBtn.Text = $"Spojeno {count} ";

        //SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

