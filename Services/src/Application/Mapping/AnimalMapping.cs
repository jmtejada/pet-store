using AutoMapper;

namespace PetStore.Services.Application.Mapping
{
    public class AnimalMapping : Profile
    {
        public AnimalMapping()
        {
            CreateMap<CQRS.Animal.Commands.Create.CreateCommand, Domain.Entities.Animal>();

            CreateMap<Domain.Entities.Animal, DTOs.AnimalDto>()
               .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name!.ToUpper()));

            CreateMap<Domain.Entities.Animal, Events.AnimalCreated>();
            CreateMap<Domain.Entities.Animal, Events.AnimalUpdated>();
        }
    }
}