using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TermProject.models;
using TermProject.services.StockService;

public class StockService : IStockService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;


    public StockService(AppDbContext context, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task<List<Stock>> GetStocksAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<Stock> GetStockByIdAsync(int id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock> AddStockAsync(Stock stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock> UpdateStockAsync(Stock stock)
    {
        _context.Entry(stock).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock> DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return null;

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return stock;
    }

    public async Task<(List<Stock> AddedStocks, int NumberOfAddedStocks)> ReadStockAsync()
    {
        var apiUrl = "https://bigpara.hurriyet.com.tr/api/v1/hisse/list";
        var responseMessage = await _httpClient.GetAsync(apiUrl);

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonContent = await responseMessage.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<StockApiResponse>(jsonContent);
            var currentStocks = await GetStocksAsync();
            var updatedStocks = apiResponse.Data;

            var differentStocks = updatedStocks.Except(currentStocks, new StockComparer()).ToList();

            foreach (var stock in differentStocks)
            {
                await AddStockAsync(stock);
            }

            return (differentStocks, differentStocks.Count);
        }

        

        throw new Exception($"API isteği başarısız: {responseMessage.StatusCode}");
    }
    
    public class StockComparer : IEqualityComparer<Stock>
    {
        public bool Equals(Stock x, Stock y)
        {
            return x.Id == y.Id; 
        }

        public int GetHashCode(Stock obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}