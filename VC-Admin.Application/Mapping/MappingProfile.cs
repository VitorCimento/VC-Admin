using AutoMapper;
using VC_Admin.Application.DTO.Auth;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterRequestDTO>().ReverseMap();

        // Mapeamento Entity -> DTO, adicionar aqui.
    }
}
