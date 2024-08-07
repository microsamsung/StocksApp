using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Contracts;
using StocksApp.Model;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinhubService _services;
        private readonly IOptions<TradingOptions> _options;
        public HomeController(IFinhubService myService, IOptions<TradingOptions> options)
        {
            _services = myService;
            _options = options;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_options.Value.DefaultStocksSymbol == null)
            {
                _options.Value.DefaultStocksSymbol = "MSFT";
            }
            
            Dictionary<string,object>? response =  await _services.GetFinhubData(_options.Value.DefaultStocksSymbol);

            Stock stock = new Stock()
            {
                StockSymbol = _options.Value.DefaultStocksSymbol,
                CurrentPrice = Convert.ToDouble(response["c"].ToString()),
                HighestPrice = Convert.ToDouble(response["h"].ToString()),
                LowestPrice = Convert.ToDouble(response["l"].ToString()),
                OpenPrice = Convert.ToDouble(response["o"].ToString())
            };

            return View(stock);
        }
    }
}
