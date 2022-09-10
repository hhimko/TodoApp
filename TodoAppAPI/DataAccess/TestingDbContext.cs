namespace TodoAppAPI.DataAccess;


public class TestingDbContext : IDbContext
{
    private List<TodoItem> todoItems = new() { 
        new(0, "Foo", DateTime.Today),
        new(1, "Bar", DateTime.Today, done: true, scheduledTime: new TimeOnly(16, 30)),
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

    public TodoItem Insert(TodoItem item)
    {
        long id = todoItems.Max(x => x.Id) + 1 ?? 0;

        TodoItem insert = new(id, item.Description, item.DayNumber, item.Done, item.ScheduledTime);
        todoItems.Add(insert);

        return insert;
    }

    public TodoItem? Update(TodoItem update)
    {
        int idx = todoItems.FindIndex(x => x.Id == update.Id);

        if (idx == -1)
            return null;

        todoItems[idx] = update;
        return update;
    }

    public bool Delete(long id)
    {
        TodoItem? item = GetById(id);

        if (item is null)
            return false;
        
        return todoItems.Remove(item);
    }
}
