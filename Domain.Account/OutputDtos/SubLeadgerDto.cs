using AAA.ERP.OutputDtos.BaseDtos;
using Shared.BaseEntities;

namespace AAA.ERP.OutputDtos;

public class SubLeadgerDto : BaseTreeSettingDto
{
    public Guid Id { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public string? Notes { get; set; }
    public NodeType? NodeType { get; set; }
    public string? Code { get; set; }  
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}