using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.Interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;

public class GLSettingService : IGLSettingService
{
    IGLSettingRepository _repository;
    public GLSettingService(IGLSettingRepository repository)
    => _repository = repository;

    public async Task<ApiResponse<GLSetting>> Get()
    {
        return new ApiResponse<GLSetting>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = await _repository.GetGLSetting()
        };
    }

    public async Task<ApiResponse<GLSetting>> Update(GLSetting glsetting)
    {
        var dbGLSetting = await _repository.GetGLSetting();
        if (dbGLSetting != null)
        {
            dbGLSetting.IsAllowingDeleteVoucher = glsetting.IsAllowingDeleteVoucher;
            dbGLSetting.IsAllowingEditVoucher = glsetting.IsAllowingEditVoucher;
            dbGLSetting.IsAllowingNegativeBalances = glsetting.IsAllowingNegativeBalances;
            dbGLSetting.DecimalDigitsNumber = glsetting.DecimalDigitsNumber;
            dbGLSetting.MonthDays = glsetting.MonthDays;
            dbGLSetting.DepreciationApplication = glsetting.DepreciationApplication;
            dbGLSetting.ModifiedBy = glsetting.ModifiedBy;

            var updated = await _repository.Update(dbGLSetting);
            return new ApiResponse<GLSetting>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = updated
            };
        }

        return new ApiResponse<GLSetting>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.NotFound,
        };
    }
}