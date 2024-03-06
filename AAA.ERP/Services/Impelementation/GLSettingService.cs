using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Services.BaseServices.impelemtation;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using AAA.ERP.Responses;
using AAA.ERP.Models.Entities.GLSettings;

namespace AAA.ERP.Services.Impelementation;

public class GLSettingService : IGLSettingService
{
    IGLSettingRepository _repository;
    public GLSettingService(IGLSettingRepository repository)
    => _repository = repository;

    public async Task<ApiResponse> Get()
    {
        return new ApiResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = await _repository.GetGLSetting()
        };
    }

    public async Task<ApiResponse> Update(GLSetting glsetting)
    {
        var dbGLSetting = await _repository.GetGLSetting();
        if (dbGLSetting != null)
        {
            dbGLSetting.IsAllowingDeleteVoucher = glsetting.IsAllowingDeleteVoucher;
            dbGLSetting.IsAllowingEditVoucher = glsetting.IsAllowingEditVoucher;
            dbGLSetting.IsAllowingNegativeBalances = glsetting.IsAllowingNegativeBalances;
            dbGLSetting.Notes = glsetting.Notes;
            dbGLSetting.DecimalDigitsNumber = glsetting.DecimalDigitsNumber;
            dbGLSetting.MonthDays = glsetting.MonthDays;
            dbGLSetting.DepreciationApplication = glsetting.DepreciationApplication;
            dbGLSetting.ModifiedBy = glsetting.ModifiedBy;

            var updated = await _repository.Update(dbGLSetting);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = updated
            };
        }

        return new ApiResponse
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.NotFound,
        };
    }
}