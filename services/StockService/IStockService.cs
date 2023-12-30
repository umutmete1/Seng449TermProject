namespace TermProject.services.StockService;

public interface IStockService
{
    Task<List<Stock>> GetStocksAsync();
    Task<Stock> GetStockByIdAsync(int id);
    Task<Stock> AddStockAsync(Stock stock);
    Task<Stock> UpdateStockAsync(Stock stock);
    Task<Stock> DeleteStockAsync(int id);

}