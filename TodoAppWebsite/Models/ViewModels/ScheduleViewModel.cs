namespace TodoAppWebsite.Models.ViewModels;


public class ScheduleViewModel
{
    public IEnumerable<IScheduledItem> Items { get; set; }


    public ScheduleViewModel(IEnumerable<TodoItem> todoItems)
    {
        Items = GetScheduleItems(todoItems);
    }

    private static IEnumerable<IScheduledItem> GetScheduleItems(IEnumerable<TodoItem> todoItems)
    {
        List<IScheduledItem> scheduleItems = new();

        todoItems = todoItems.Where(todoItem => todoItem.ScheduledTime is not null).ToList();
        if (!todoItems.Any())
            return scheduleItems;

        var now = TimeOnly.FromDateTime(DateTime.Now);
        now = new TimeOnly(10, now.Minute); // (mili)seconds are stripped

        var firstTime = todoItems.ElementAt(0).ScheduledTime;
        if (firstTime > now)
        {
            TimeSpan timeLeft = firstTime.Value - now;
            scheduleItems.Add(new FreeTimeInterval(timeLeft, now));
        }

        int i;
        for (i = 0; i < todoItems.Count() - 1; i++)
        {
            TodoItem current = todoItems.ElementAt(i);
            TodoItem next = todoItems.ElementAt(i + 1);
            scheduleItems.Add(current);

            if (next.ScheduledTime > current.ScheduledTime)
            {
                TimeSpan timeLeft = next.ScheduledTime.Value - current.ScheduledTime.Value;
                scheduleItems.Add(new FreeTimeInterval(timeLeft, current.ScheduledTime.Value));
            }
        }

        scheduleItems.Add(todoItems.ElementAt(i));

        return scheduleItems;
    }
}
