namespace TermProject.services.UserService;

public interface IUserService
{
    Task<List<Stock>>  GetWatchlist(string userId);
}