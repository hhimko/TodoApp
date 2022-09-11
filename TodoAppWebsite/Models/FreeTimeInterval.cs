namespace TodoAppWebsite.Models;


public record FreeTimeInterval : IScheduledItem
{
    public TimeRange? ScheduledTime { get; init; }
    public string intervalDescription { get; init; }

    public FreeTimeInterval(TimeRange scheduledTime)
    {
        ScheduledTime = scheduledTime;

        var interval = scheduledTime.Interval;
        string hours = interval.Hours != 0 ? interval.Hours.ToString() + "h" : "";
        string minutes = interval.Minutes != 0 ? interval.Minutes.ToString() + "min" : "";
        intervalDescription = $"{hours} {minutes} free time";
    }
}
