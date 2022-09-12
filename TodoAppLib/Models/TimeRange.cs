using System.Text.Json.Serialization;
using TodoAppLib.JsonConverters;

namespace TodoAppLib.Models;


public record TimeRange
{
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly Start { get; init; }

    public TimeSpan Interval { get; init; }
<<<<<<< HEAD

=======
>>>>>>> e0715968c076f88213db28b570fa78dfb9b768df
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly End { get; init; }


    [JsonConstructor]
    public TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;

        Interval = end - start;
    }

    public TimeRange(TimeOnly start, TimeSpan interval)
    {
        Start = start;
        Interval = interval;

        End = TimeOnly.FromTimeSpan(Start - TimeOnly.FromTimeSpan(Interval));
    }
}
