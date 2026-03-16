using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TestDTO, Test>().ReverseMap();

            // Utilisateur mappings
            CreateMap<Utilisateur, UtilisateurDTO>().ReverseMap();
            CreateMap<CreateUtilisateurDTO, Utilisateur>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreation, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
            CreateMap<UpdateUtilisateurDTO, Utilisateur>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
