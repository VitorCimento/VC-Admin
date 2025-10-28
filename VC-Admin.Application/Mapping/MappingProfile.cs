using AutoMapper;
using VC_Admin.Application.DTO;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterRequest>().ReverseMap();

        // Mapeamento Entity -> DTO, adicionar aqui.
    }
}
