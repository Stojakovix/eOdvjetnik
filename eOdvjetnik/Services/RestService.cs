using eOdvjetnik.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

namespace eOdvjetnik.Services
{
    public class RestService : IRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        IHttpsClientHandlerService _httpsClientHandlerService;

        public List<DevInfoItem> Items { get; set; }

        public RestService(IHttpsClientHandlerService service)
        {
#if DEBUG 
            _httpsClientHandlerService = service;
            HttpMessageHandler handler = _httpsClientHandlerService.GetPlatformMessageHandler();
            if (handler != null)
                _client = new HttpClient(handler);
            else
                _client = new HttpClient();
#else
        _client = new HttpClient();
#endif 
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<List<DevInfoItem>> RefreshDataAsync()
        {
            Items = new List<DevInfoItem>();

            Uri uri = new(string.Format(RestConstants.RestUrl, string.Empty));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<DevInfoItem>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);

            }
            return Items;
        }

        public async Task SaveDevInfoItemAsync (DevInfoItem devInfoItem, bool isNewItem = false)
        {
            Uri uri = new(string.Format(RestConstants.RestUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<DevInfoItem>(devInfoItem, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                    response = await _client.PostAsync(uri, content);
                else
                    response = await _client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"\tdevInfoItem successfully saved.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
        public async Task DeleteDevInfoItemAsync(string id)
        {
            Uri uri = new(string.Format(RestConstants.RestUrl, id));

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"\tDevInfoItem successfully deleted.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}
