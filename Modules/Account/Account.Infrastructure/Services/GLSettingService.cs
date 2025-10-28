using Account.Application.Repositories;
using Account.Domain.Commands.GLSettings;
using Account.Domain.Models.Entities.GLSettings;

namespace Account.Infrastructure.Services;

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

    public async Task<ApiResponse<GLSetting>> Update(GlSettingUpdateCommand glsetting)
    {
        var dbGLSetting = await _repository.GetGLSetting();
        if (dbGLSetting != null)
        {
            dbGLSetting.IsAllowingDeleteVoucher = glsetting.IsAllowingDeleteVoucher;
            dbGLSetting.IsAllowingEditVoucher = glsetting.IsAllowingEditVoucher;
            dbGLSetting.IsAllowingNegativeBalances = glsetting.IsAllowingNegativeBalances;
            dbGLSetting.DecimalDigitsNumber = glsetting.DecimalDigitsNumber;
            if (glsetting.DepreciationApplication == DepreciationApplication.Monthly)
                dbGLSetting.MonthDays = glsetting.MonthDays;
            dbGLSetting.DepreciationApplication = glsetting.DepreciationApplication;
            dbGLSetting.ModifiedAt = DateTime.Now;

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
