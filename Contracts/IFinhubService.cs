namespace StocksApp.Contracts
{
    public interface IFinhubService
    {
        Task<Dictionary<string,object>> GetFinhubData(string stock);
    }
}
