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

            // Client mappings
            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.PieceJointeBase64,
                    opt => opt.MapFrom(src => src.PieceJointe != null ? Convert.ToBase64String(src.PieceJointe) : null))
                .ReverseMap();
            CreateMap<CreateClientDTO, Client>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreation, opt => opt.Ignore())
                .ForMember(dest => dest.PieceJointe,
                    opt => opt.MapFrom(src => src.PieceJointeBase64 != null ? Convert.FromBase64String(src.PieceJointeBase64) : null));
            CreateMap<UpdateClientDTO, Client>()
                .ForMember(dest => dest.PieceJointe,
                    opt => opt.MapFrom(src => src.PieceJointeBase64 != null ? Convert.FromBase64String(src.PieceJointeBase64) : null))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
