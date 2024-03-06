using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.GLSettings;
using AutoMapper;

namespace AAA.ERP.Mappers;

public class GLSettingAutoMapper : Profile
{
    public GLSettingAutoMapper()
    {
        CreateMap<GLSetting, GLSettingInputModel>().ReverseMap();
    }
}
