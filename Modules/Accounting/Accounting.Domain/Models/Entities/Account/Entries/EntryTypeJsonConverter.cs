using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Domain.Models.Entities.Account.Entries;

/// <summary>
/// Preserves legacy wire format <c>"Compined"</c> for combined entries (Phase 4.1.4).
/// </summary>
public sealed class EntryTypeJsonConverter : JsonConverter<EntryType>
{
    private const string LegacyCombinedName = "Compined";

    public override EntryType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            LegacyCombinedName or nameof(EntryType.Combined) => EntryType.Combined,
            nameof(EntryType.Payment) => EntryType.Payment,
            nameof(EntryType.Receipt) => EntryType.Receipt,
            nameof(EntryType.Journal) => EntryType.Journal,
            nameof(EntryType.Opening) => EntryType.Opening,
            _ => throw new JsonException($"Unknown EntryType value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, EntryType value, JsonSerializerOptions options)
    {
        var wireName = value == EntryType.Combined ? LegacyCombinedName : value.ToString();
        writer.WriteStringValue(wireName);
    }
}
