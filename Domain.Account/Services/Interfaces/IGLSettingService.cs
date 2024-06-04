using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.interfaces;

namespace AAA.ERP.Services.Interfaces;

public interface IGLSettingService{

    public Task<ApiResponse> Update(GLSetting glsettting);
    public Task<ApiResponse> Get();
}