using Microsoft.EntityFrameworkCore;
using TermProject.services.StockService;

public class StockService : IStockService{
    private readonly AppDbContext _context;
    public StockService(AppDbContext context)
    {
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
        if (stock == null)
        {
            return null;
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return stock;
    }
}