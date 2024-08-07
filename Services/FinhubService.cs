using StocksApp.Contracts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinhubService : IFinhubService
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinhubService(IHttpClientFactory? httpClientFactory,IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        public  async Task<Dictionary<string, object>> GetFinhubData(string stocks)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stocks}&token={_configuration["FinhubToken"]}"),
                    Method = HttpMethod.Get

                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                  Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

              StreamReader reader = new StreamReader(stream);

            string responseText = reader.ReadToEnd();

                Dictionary<string, object>?responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseText);
                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from finhub server");
                
                return responseDictionary;

            }
        
        }
    }
}
