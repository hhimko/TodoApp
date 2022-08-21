using System.Text.Json.Serialization;
using TodoAppAPI.DataAccess.JsonConverters;

namespace TodoAppAPI.Models;


public record TodoItem(
    long Id,
    string Name,
    int DayNumber,
    [field: JsonConverter(typeof(TimeOnlyJsonConverter))] 
    DateTime? ScheduledTime = null
)
{
    [JsonIgnore]
    public DateOnly? Date => DateOnly.FromDayNumber(DayNumber);
}
