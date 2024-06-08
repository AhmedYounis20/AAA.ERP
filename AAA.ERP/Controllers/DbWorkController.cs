using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Utility;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DbworkController : ControllerBase
{

    private ExportDataToSeed _exportDataToSeed;
    private ImportDataToSeed _importDataToSeed;
    private ApplicationDbContext _context;
    public DbworkController(ExportDataToSeed exportDataToSeed,ApplicationDbContext context,ImportDataToSeed importDataToSeed)
    {
        _exportDataToSeed = exportDataToSeed;
        _context = context;
        _importDataToSeed = importDataToSeed;
    }

    [HttpGet("export/{folderName}")]
    public async Task<IActionResult> ExportData(string folderName = "account")
    {
        await _exportDataToSeed.ExportAllTablesToJsonAsync(folderName);
        return Ok("Exported Successfully");
    }
    [HttpGet("import/{folderName}")]
    public async Task<IActionResult> Import(string folderName = "account")
    {
        await _importDataToSeed.Import(folderName);
        return Ok("Imported Successfully");
    }
}

public class TableData
{
    public string TableName { get; set; }
    public List<Dictionary<string, object>> Rows { get; set; }
}