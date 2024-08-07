namespace StocksApp.Contracts
{
    /// <summary>
    /// Defines a contract for a service that interacts with the Finhub API to retrieve stock data.
    /// </summary>
    public interface IFinhubService
    {
        /// <summary>
        /// Retrieves stock data from the Finhub API for a given stock symbol.
        /// </summary>
        /// <param name="stock">The stock symbol to retrieve data for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary with the stock data.</returns>
        Task<Dictionary<string, object>> GetFinhubData(string stock);
    }
}
