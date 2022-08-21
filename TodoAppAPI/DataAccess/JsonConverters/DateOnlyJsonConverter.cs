using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoAppAPI.DataAccess.JsonConverters;


public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString()!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly dateOnly, JsonSerializerOptions options)
    {
        writer.WriteStringValue(dateOnly.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}
