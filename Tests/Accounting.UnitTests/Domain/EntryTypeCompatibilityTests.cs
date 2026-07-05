using System.Text.Json;
using Accounting.Domain.Models.Entities.Entries;
using FluentAssertions;

namespace Accounting.UnitTests.Domain;

public class EntryTypeCompatibilityTests
{
    [Fact]
    public void Json_converter_writes_legacy_Compined_wire_name()
    {
        var json = JsonSerializer.Serialize(EntryType.Combined, new JsonSerializerOptions
        {
            Converters = { new EntryTypeJsonConverter() }
        });

        json.Should().Be("\"Compined\"");
    }

    [Fact]
    public void Json_converter_reads_legacy_Compined_wire_name()
    {
        var value = JsonSerializer.Deserialize<EntryType>("\"Compined\"", new JsonSerializerOptions
        {
            Converters = { new EntryTypeJsonConverter() }
        });

        value.Should().Be(EntryType.Combined);
    }
}
