using TermProject.models;
using TermProject.models.WatchlistModels;

namespace TermProject.services.UserService;

public interface IWatchlistService
{
    Task<List<WatchlistVm>> GetWatchlist(string userId);
    Task<Stock> AddStockToWatchlist(string stockCode, string userId);
    Task<bool> RemoveStockFromWatchlist(string stockCode, string userId);

    Task<bool> IsStockAlreadyAdded(string stockCode, string userId);

}