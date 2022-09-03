using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoAppLib.JsonConverters;

namespace TodoAppLib.Models;


public record TodoItem
{
    [Key]
    [Required]
    public long? Id { get; init; }

    [Required]
    public string Name { get; init; }

    [JsonIgnore]
    public int DayNumber { get; init; }

    public bool Done { get; init; } = false;

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly? ScheduledTime { get; init; } = null;

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date => DateOnly.FromDayNumber(DayNumber);


    [JsonConstructorAttribute]
    public TodoItem(long? id, string name, int dayNumber, bool done = false, TimeOnly? scheduledTime = null)
    {
        Id = id;
        Name = name;
        DayNumber = dayNumber;
        Done = done;
        ScheduledTime = scheduledTime;
    }

    public TodoItem(long? id, string name, DateOnly dateOnly, bool done = false, TimeOnly? scheduledTime = null)
        : this(id, name, dateOnly.DayNumber, done, scheduledTime) { }

    public TodoItem(long? id, string name, DateTime dateTime, bool done = false, TimeOnly? scheduledTime = null)
        : this(id, name, DateOnly.FromDateTime(dateTime).DayNumber, done, scheduledTime) { }
}
