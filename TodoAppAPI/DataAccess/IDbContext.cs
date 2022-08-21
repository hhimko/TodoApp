namespace TodoAppAPI.DataAccess;


public interface IDbContext
{
    public TodoItem? GetById(long id);
    public IEnumerable<TodoItem> GetAllByDayNumber(int dayNumber);
}
