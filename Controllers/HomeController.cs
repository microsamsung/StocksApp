using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Contracts;
using StocksApp.Model;

namespace StocksApp.Controllers
{
    /// <summary>
    /// The HomeController class handles requests to the home page of the StocksApp.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IFinhubService _finhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="finhubService">Service to interact with Finhub API.</param>
        /// <param name="tradingOptions">Options for trading configuration.</param>
        public HomeController(IFinhubService finhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finhubService = finhubService ?? throw new ArgumentNullException(nameof(finhubService));
            _tradingOptions = tradingOptions ?? throw new ArgumentNullException(nameof(tradingOptions));
        }

        /// <summary>
        /// Handles requests to the root URL and returns the stock data view.
        /// </summary>
        /// <returns>The view displaying stock information.</returns>
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            // Set default stock symbol if not already set in options
            if (string.IsNullOrEmpty(_tradingOptions.Value.DefaultStocksSymbol))
            {
                _tradingOptions.Value.DefaultStocksSymbol = "MSFT";
            }

            // Retrieve stock data from Finhub API
            Dictionary<string, object>? response = await _finhubService.GetFinhubData(_tradingOptions.Value.DefaultStocksSymbol);

            // Map response data to Stock model
            var stock = new Stock
            {
                StockSymbol = _tradingOptions.Value.DefaultStocksSymbol,
                CurrentPrice = Convert.ToDouble(response["c"].ToString()),
                HighestPrice = Convert.ToDouble(response["h"].ToString()),
                LowestPrice = Convert.ToDouble(response["l"].ToString()),
                OpenPrice = Convert.ToDouble(response["o"].ToString())
            };

            // Return the view with the stock data
            return View(stock);
        }
    }
}
