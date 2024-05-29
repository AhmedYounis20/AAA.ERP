using AAA.ERP.Utility;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DbworkController : ControllerBase
{

    private ExportDataToSeed _exportDataToSeed;
    public DbworkController(ExportDataToSeed exportDataToSeed)
    {
        _exportDataToSeed = exportDataToSeed;
    }

    [HttpGet("export/{foldername}")]
    public async Task<IActionResult> ExportData(string foldername = "account")
    {
        await _exportDataToSeed.ExportAllTablesToJsonAsync(foldername);
        return Ok();
    }
}

public class TableData
{
    public string TableName { get; set; }
    public List<Dictionary<string, object>> Rows { get; set; }
}