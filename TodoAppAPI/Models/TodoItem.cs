using System.Text.Json.Serialization;
using TodoAppAPI.DataAccess.JsonConverters;

namespace TodoAppAPI.Models;


public record TodoItem(
    long Id,
    string Name,
    int DayNumber,
    bool Done = false,
    [property: JsonConverter(typeof(TimeOnlyJsonConverter))] 
    TimeOnly? ScheduledTime = null
)
{
    [JsonIgnore]
    public DateOnly? Date => DateOnly.FromDayNumber(DayNumber);
}
