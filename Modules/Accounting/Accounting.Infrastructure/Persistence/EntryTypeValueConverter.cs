using Accounting.Domain.Models.Entities.Entries;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Accounting.Infrastructure.Persistence;

/// <summary>
/// Reads legacy <c>Compined</c> rows from SQL; writes <c>Combined</c> going forward.
/// </summary>
public static class EntryTypeValueConverter
{
    private const string LegacyCombinedName = "Compined";

    public static ValueConverter<EntryType, string> Instance { get; } = new(
        entryType => entryType.ToString(),
        stored => ParseStoredValue(stored));

    private static EntryType ParseStoredValue(string stored)
    {
        if (stored == LegacyCombinedName)
            return EntryType.Combined;

        return Enum.Parse<EntryType>(stored);
    }
}
