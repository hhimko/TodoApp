namespace TodoAppWebsite.Models.ViewModels;


public class ScheduleViewModel
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public IEnumerable<IScheduledItem> Items { get; set; }
    public IEnumerable<TodoItem> TodoItems => Items.Where(x => x is TodoItem).Cast<TodoItem>();
    public IScheduledItem? CurrentyActive { get; set; }


    public ScheduleViewModel(IEnumerable<TodoItem> todoItems, IDateTimeProvider dateTime)
    {
        _dateTimeProvider = dateTime;
        Items = GetScheduleItems(todoItems);
        CurrentyActive = GetCurrentlyActive(Items);
    }

    private IEnumerable<IScheduledItem> GetScheduleItems(IEnumerable<TodoItem> todoItems)
    {
        List<IScheduledItem> scheduleItems = new();

        todoItems = todoItems.Where(todoItem => todoItem.ScheduledTime is not null).ToList();
        if (!todoItems.Any())
            return scheduleItems;

        todoItems.OrderBy(todoItem => todoItem.ScheduledTime!.Start);

        var now = _dateTimeProvider.Time;
        now = new TimeOnly(now.Hour, now.Minute); // (mili)seconds are stripped

        var firstTime = todoItems.ElementAt(0).ScheduledTime!.Start;
        if (firstTime > now)
        {
            var timeRange = new TimeRange(now, firstTime);
            scheduleItems.Add(new FreeTimeInterval(timeRange));
        }


        int i;
        for (i = 0; i < todoItems.Count() - 1; i++)
        {
            TodoItem current = todoItems.ElementAt(i);
            TodoItem next = todoItems.ElementAt(i + 1);
            scheduleItems.Add(current);

            if (next.ScheduledTime!.Start > current.ScheduledTime!.End)
            {
                var timeRange = new TimeRange(current.ScheduledTime!.End, next.ScheduledTime!.Start);
                scheduleItems.Add(new FreeTimeInterval(timeRange));
            }
        }

        scheduleItems.Add(todoItems.ElementAt(i));

        return scheduleItems;
    }

    private IScheduledItem? GetCurrentlyActive(IEnumerable<IScheduledItem> scheduledItems)
    {
        var now = _dateTimeProvider.Time;
        now = new TimeOnly(now.Hour, now.Minute); // (mili)seconds are stripped

        return scheduledItems.FirstOrDefault(item => item.ScheduledTime?.Start <= now && now < item.ScheduledTime?.End);
    }
}
