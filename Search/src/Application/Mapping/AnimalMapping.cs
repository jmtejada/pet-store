using AutoMapper;

namespace PetStore.Search.Application.Mapping
{
    public class AnimalMapping : Profile
    {
        public AnimalMapping()
        {
            CreateMap<Domain.Animal, DTOs.AnimalDto>();
        }
    }
}