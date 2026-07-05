using ERP.Infrastructure.DBConfiguration.DbContext;
using Accounting.Domain.Utility;
using ERP.Infrastructure.Utilities;

namespace ERP.Host.Controllers.Account;

/// <summary>Development utilities for database import and export.</summary>
[Route("api/[controller]")]
[ApiController]
public class DbworkController : ControllerBase
{

    private ExportDataToSeed _exportDataToSeed;
    private ImportDataToSeed _importDataToSeed;
    private ApplicationDbContext _context;
    public DbworkController(ExportDataToSeed exportDataToSeed, ApplicationDbContext context, ImportDataToSeed importDataToSeed)
    {
        _exportDataToSeed = exportDataToSeed;
        _context = context;
        _importDataToSeed = importDataToSeed;
    }

    /// <summary>Exports database tables to JSON seed files.</summary>
    [HttpGet("export/{folderName}")]
    public async Task<IActionResult> ExportData(string folderName = "account")
    {
        await _exportDataToSeed.ExportAllTablesToJsonAsync(folderName);
        return Ok("Exported Successfully");
    }
    /// <summary>Imports database seed data from JSON files.</summary>
    [HttpGet("import/{folderName}")]
    public async Task<IActionResult> Import(string folderName = "account")
    {
        await _importDataToSeed.Import(folderName);
        return Ok("Imported Successfully");
    }
}

/// <summary>API for TableData.</summary>
public class TableData
{
    public string TableName { get; set; }
    public List<Dictionary<string, object>> Rows { get; set; }
}
