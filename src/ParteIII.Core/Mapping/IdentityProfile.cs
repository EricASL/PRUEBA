using AutoMapper;
using ParteIII.Core.Dtos;
using ParteIII.Core.Entities;

namespace ParteIII.Core.Mapping;

public sealed class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<Identity, IdentityDto>();
    }
}