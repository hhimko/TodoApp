using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoAppAPI.DataAccess.JsonConverters;


public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString()!, "HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly timeOnly, JsonSerializerOptions options)
    {
        writer.WriteStringValue(timeOnly.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
    }
}
