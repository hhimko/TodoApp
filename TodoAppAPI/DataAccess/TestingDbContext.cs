namespace TodoAppAPI.DataAccess;


public class TestingDbContext : IDbContext
{
    private List<TodoItem> todoItems = new() { 
        new(0, "Foo", 0),
        new(1, "Bar", 0),
        new(2, "Baz", 0, DateTime.Now)
    };

    public TodoItem? GetById(long id)
    {
        return todoItems.Where(x => x.Id == id).SingleOrDefault();
    }

    public IEnumerable<TodoItem> GetAllByDayNumber(int dayNumber)
    {
        return todoItems.Where(x => x.DayNumber == dayNumber);
    }
}
