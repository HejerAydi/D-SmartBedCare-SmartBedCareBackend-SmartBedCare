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

            // LitMedical mappings
            CreateMap<LitMedical, LitMedicalDTO>().ReverseMap();
            CreateMap<CreateLitMedicalDTO, LitMedical>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateAjout, opt => opt.Ignore())
                .ForMember(dest => dest.Disponible, opt => opt.Ignore());
            CreateMap<UpdateLitMedicalDTO, LitMedical>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Location mappings
            CreateMap<Location, LocationDTO>()
                .ForMember(dest => dest.ClientNom, opt => opt.MapFrom(src => src.Client != null ? src.Client.NomComplet : null))
                .ForMember(dest => dest.Lits, opt => opt.MapFrom(src => src.LocationLits))
                .ForMember(dest => dest.Rubriques, opt => opt.MapFrom(src => src.LocationRubriques));
            CreateMap<UpdateLocationDTO, Location>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LocationLit, LocationLitDTO>()
                .ForMember(dest => dest.NumeroSerie, opt => opt.MapFrom(src => src.LitMedical != null ? src.LitMedical.NumeroSerie : null));
            CreateMap<LocationRubrique, LocationRubriqueDTO>()
                .ForMember(dest => dest.NomRubrique, opt => opt.MapFrom(src => src.Rubrique != null ? src.Rubrique.Nom : null));

            // Paiement mappings
            CreateMap<Paiement, PaiementDTO>().ReverseMap();
            CreateMap<CreatePaiementDTO, Paiement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DatePaiement, opt => opt.Ignore());

            // Notification mappings
            CreateMap<Notification, NotificationDTO>().ReverseMap();

            // Historique mappings
            CreateMap<Historique, HistoriqueDTO>()
                .ForMember(dest => dest.UtilisateurNom,
                    opt => opt.MapFrom(src => src.Utilisateur != null ? src.Utilisateur.Nom + " " + src.Utilisateur.Prenom : null));
        }
    }

}
