namespace TodoAppLib.Models;


public record TimeRange
{
    public TimeOnly Start { get; init; }
    public TimeSpan Interval { get; init; }
    public TimeOnly End { get; init; }


    public TimeRange(TimeOnly start, TimeSpan interval)
    {
        Start = start;
        Interval = interval;

        End = TimeOnly.FromTimeSpan(Start - TimeOnly.FromTimeSpan(Interval));
    }

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;

        Interval = end - start;
    }
}
