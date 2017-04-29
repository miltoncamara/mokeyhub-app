using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MonkeyHub
{
	public class MonkeyHubViewModel : BaseViewModel
	{


		private const string BaseUrl = "https://monkey-hub-api.azurewebsites.net/api/";

		public async Task<List<Tag>> GetTagsAsync()
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var response = await httpClient.GetAsync($"{BaseUrl}Tags").ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
				{
					return JsonConvert.DeserializeObject<List<Tag>>(
						await new StreamReader(responseStream)
							.ReadToEndAsync().ConfigureAwait(false));
				}
			}

			return null;
		}

		private string _searchTerm;
		public string SearchTerm
		{
			get
			{
				return this._searchTerm;
			}
			set
			{
				if (SetProperty(ref _searchTerm, value))
					SearchCommand.ChangeCanExecute();
			}
		}

		public ObservableCollection<Tag> Respostas { get; }

		public Command SearchCommand { get; }

		public MonkeyHubViewModel()
		{
			SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);
			Respostas = new ObservableCollection<Tag>();
		}

		async void ExecuteSearchCommand()
		{
			//await Task.Delay(200);
			var resposta = await App.Current.MainPage.DisplayAlert("Teste", "Teste Mensagem", "Ok", "Cancelar");

			if (resposta)
			{
				var items = await GetTagsAsync();
				Respostas.Clear();
				if (items != null)
				{
					foreach (var tag in items)
					{
						Respostas.Add(tag);
					}
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("De Nda", "De mada", "Ok");
			}
		}

		bool CanExecuteSearchCommand()
		{
			return string.IsNullOrWhiteSpace(_searchTerm) == false;
		}
	}
}
