namespace BettingSystem.Data.Models
{
    public class PagedResult<T> where T : class
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
