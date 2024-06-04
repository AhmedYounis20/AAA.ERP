using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.OutputDtos.BaseDtos;
using AutoMapper;

namespace AAA.ERP.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<BaseEntity, BaseInputModel>().ReverseMap();
        CreateMap<BaseSettingEntity, BaseSettingDto>().ReverseMap();
        CreateMap<BaseSettingEntity, BaseSettingInputModel>().ReverseMap();
    }
}
