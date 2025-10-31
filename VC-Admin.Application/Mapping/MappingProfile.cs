using AutoMapper;
using VC_Admin.Application.DTO.Auth;
using VC_Admin.Application.DTO.User;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamentos Entity -> DTO | DTO -> Entity, adicionar aqui.
        AuthMapping();
        UserMapping();
    }

    private void AuthMapping() => CreateMap<User, RegisterRequestDTO>().ReverseMap();

    private void UserMapping()
    {
        // Create
        CreateMap<UserCreateDTO, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLowerInvariant()))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Trim()))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Update
        CreateMap<UserUpdateDTO, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Trim()))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
            {
                if (srcMember == null) return false;
                if (srcMember is string s) return !string.IsNullOrWhiteSpace(s);

                return true;
            }));

        CreateMap<User, UserResponseDTO>();
    }
}
