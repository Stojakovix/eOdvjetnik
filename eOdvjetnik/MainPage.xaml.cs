using SMBLibrary.Client;
using SMBLibrary;
using System.Net;
using Microsoft.Maui.Controls;

namespace eOdvjetnik;

public partial class MainPage : ContentPage
{
    //int count = 0;

    public MainPage()
	{
		InitializeComponent();

        //SMB
        SMB1Client client = new SMB1Client(); // SMB2Client can be used as well
        bool isConnected = client.Connect(IPAddress.Parse("192.168.1.113"), SMBTransportType.DirectTCPTransport);
        if (isConnected)
        {
            NTStatus status = client.Login(String.Empty, "robi", "walter");
            if (status == NTStatus.STATUS_SUCCESS)
            {
                var shares = client.ListShares(out status);
                var shares2 = "SPOJENO";
                CounterBtn.Text = shares2;
                
                client.Logoff();
            }
            client.Disconnect();
        }
        else {
            
            //shares.Text = $"Spojeno {count} ";
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

