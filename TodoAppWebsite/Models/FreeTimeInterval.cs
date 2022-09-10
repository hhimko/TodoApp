namespace TodoAppWebsite.Models;


public record FreeTimeInterval : IScheduledItem
{
    public TimeOnly? ScheduledTime { get; init; }
    public string intervalDescription { get; init; }

    public FreeTimeInterval(TimeSpan timeInterval, TimeOnly scheduledTime)
    {
        ScheduledTime = scheduledTime;

        string hours = timeInterval.Hours != 0 ? timeInterval.Hours.ToString() + "h" : "";
        string minutes = timeInterval.Minutes != 0 ? timeInterval.Minutes.ToString() + "min" : "";
        intervalDescription = $"{hours} {minutes} free time";
    }
}
