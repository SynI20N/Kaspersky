using System.Text.Json;
using System.Text.Json.Serialization;

public class ByteJsonConverter : JsonConverter<byte>
{
    public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (byte.TryParse(stringValue, out byte byteValue))
            {
                return byteValue;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetByte();
        }
        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to byte.");
    }

    public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}