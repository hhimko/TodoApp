namespace TodoAppLib.Models;


public interface IScheduledItem
{
    TimeRange? ScheduledTime { get; }
}
