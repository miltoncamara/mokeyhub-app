using Xamarin.Forms;

namespace MonkeyHub
{
	public partial class MonkeyHubPage : ContentPage
	{
		public MonkeyHubPage()
		{
			InitializeComponent();
			BindingContext = new MonkeyHubViewModel();
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			Navigation?.PushAsync(new MonkeyHubPage(), true);
		}

		void HandleModal_Clicked(object sender, System.EventArgs e)
		{
			Navigation?.PushModalAsync(new MonkeyHubPage(), true);
		}
	}
}
