namespace TodoAppLib.DateTimeProvider;


public class DateTimeProvider : IDateTimeProvider
{
    private DateTime? _now;

    public DateTime Now => _now ?? DateTime.Now;
    public DateOnly Date => DateOnly.FromDateTime(Now);
    public TimeOnly Time => TimeOnly.FromDateTime(Now);


    public DateTimeProvider(DateTime? now) => _now = now;
}
