namespace TodoAppLib.Models;


public interface IScheduledItem
{
    TimeOnly? ScheduledTime { get; }
}
