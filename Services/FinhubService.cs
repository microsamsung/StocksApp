using StocksApp.Contracts;
using System.Text.Json;

namespace StocksApp.Services
{
    /// <summary>
    /// Service class for interacting with the Finhub API.
    /// </summary>
    public class FinhubService : IFinhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinhubService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Factory for creating HTTP clients.</param>
        /// <param name="configuration">Configuration settings.</param>
        public FinhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Retrieves stock data from the Finhub API.
        /// </summary>
        /// <param name="stocks">The stock symbol to retrieve data for.</param>
        /// <returns>A dictionary containing the stock data.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the response from the Finhub server is invalid.</exception>
        public async Task<Dictionary<string, object>> GetFinhubData(string stocks)
        {
            // Create an HttpClient instance using the factory
            using HttpClient httpClient = _httpClientFactory.CreateClient();

            // Build the request message
            var requestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stocks}&token={_configuration["FinhubToken"]}");
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = HttpMethod.Get
            };

            // Send the request and get the response
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            // Ensure the response indicates success
            httpResponseMessage.EnsureSuccessStatusCode();

            // Read the response stream
            await using Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(responseStream);
            string responseText = await reader.ReadToEndAsync();

            // Deserialize the response JSON to a dictionary
            var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseText);
            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from Finhub server.");
            }

            return responseDictionary;
        }
    }
}
