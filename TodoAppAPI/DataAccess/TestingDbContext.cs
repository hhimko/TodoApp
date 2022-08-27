namespace TodoAppAPI.DataAccess;


public class TestingDbContext : IDbContext
{
    private List<TodoItem> todoItems = new() { 
        new(0, "Foo", DateTime.Today),
        new(1, "Bar", DateTime.Today),
        new(2, "Baz", DateTime.Today, scheduledTime: new TimeOnly(18, 00))
    };

    public TodoItem? GetById(long id)
    {
        return todoItems.Where(x => x.Id == id).SingleOrDefault();
    }

    public IEnumerable<TodoItem> GetAllByDayNumber(int dayNumber)
    {
        return todoItems.Where(x => x.DayNumber == dayNumber);
    }

    public bool Delete(long id)
    {
        TodoItem? item = GetById(id);

        if (item is null)
            return false;
        
        return todoItems.Remove(item);
    }
}
