using AutoMapper;
using Consumer.Data.Domain;
using Consumer.Events.Animals;

namespace Consumer.Mapping
{
    public class AnimalMapping : Profile
    {
        public AnimalMapping()
        {
            CreateMap<AnimalCreated, Animal>();
        }
    }
}