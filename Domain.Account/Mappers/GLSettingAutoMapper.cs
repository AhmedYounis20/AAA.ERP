using AutoMapper;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.GLSettings;

namespace Domain.Account.Mappers;

public class GLSettingAutoMapper : Profile
{
    public GLSettingAutoMapper()
    {
        CreateMap<GLSetting, GLSettingInputModel>().ReverseMap();
    }
}