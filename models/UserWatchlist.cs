namespace TermProject.models;

public class UserWatchlist
{
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }

    public int StockId { get; set; }
    public Stock Stock { get; set; }
}