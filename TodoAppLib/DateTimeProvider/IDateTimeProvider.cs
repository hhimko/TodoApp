namespace TodoAppLib.DateTimeProvider;


public interface IDateTimeProvider
{
    public DateTime Now { get; }
    public DateOnly Date { get; }
    public TimeOnly Time { get; }
}
