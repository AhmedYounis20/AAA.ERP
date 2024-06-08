using AAA.ERP.OutputDtos.BaseDtos;
using AutoMapper;
using Domain.Account.InputModels.BaseInputModels;
using Shared.BaseEntities;

namespace Domain.Account.Mappers;

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
