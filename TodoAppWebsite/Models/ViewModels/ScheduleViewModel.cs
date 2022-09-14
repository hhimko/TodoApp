using TodoAppLib.Models;

namespace TodoAppWebsite.Models.ViewModels;


public class ScheduleViewModel
{
    public IEnumerable<IScheduledItem> Items { get; set; }
    public IScheduledItem? CurrentyActive { get; set; }


    public ScheduleViewModel(IEnumerable<TodoItem> todoItems)
    {
        Items = GetScheduleItems(todoItems);
        CurrentyActive = GetCurrentlyActive(Items);
    }

    private static IEnumerable<IScheduledItem> GetScheduleItems(IEnumerable<TodoItem> todoItems)
    {
        List<IScheduledItem> scheduleItems = new();

        todoItems = todoItems.Where(todoItem => todoItem.ScheduledTime is not null).ToList();
        if (!todoItems.Any())
            return scheduleItems;

        todoItems.OrderBy(todoItem => todoItem.ScheduledTime!.Start);

        var now = TimeOnly.FromDateTime(DateTime.Now);
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

    private static IScheduledItem? GetCurrentlyActive(IEnumerable<IScheduledItem> scheduledItems)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        now = new TimeOnly(now.Hour, now.Minute); // (mili)seconds are stripped

        return scheduledItems.FirstOrDefault(item => item.ScheduledTime?.Start <= now && now < item.ScheduledTime?.End);
    }
}
