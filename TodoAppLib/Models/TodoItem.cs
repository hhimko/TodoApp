using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoAppLib.JsonConverters;

namespace TodoAppLib.Models;


public record TodoItem
{
    [Key]
    public long? Id { get; init; }

    [Required]
    public string Description { get; init; } = default!;

    public int DayNumber { get; init; }

    public bool Done { get; init; } = false;

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly? ScheduledTime { get; init; } = null;

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date => DateOnly.FromDayNumber(DayNumber);

    [JsonConstructorAttribute]
    public TodoItem() { }

    public TodoItem(long? id, string description, int dayNumber, bool done = false, TimeOnly? scheduledTime = null)
    {
        Id = id;
        Description = description;
        DayNumber = dayNumber;
        Done = done;
        ScheduledTime = scheduledTime;
    }

    public TodoItem(long? id, string description, DateOnly dateOnly, bool done = false, TimeOnly? scheduledTime = null)
        : this(id, description, dateOnly.DayNumber, done, scheduledTime) { }

    public TodoItem(long? id, string description, DateTime dateTime, bool done = false, TimeOnly? scheduledTime = null)
        : this(id, description, DateOnly.FromDateTime(dateTime).DayNumber, done, scheduledTime) { }
}
