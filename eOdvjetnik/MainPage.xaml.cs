namespace eOdvjetnik;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Spojeno {count} ";
		else
			CounterBtn.Text = $"Spojeno {count} ";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

