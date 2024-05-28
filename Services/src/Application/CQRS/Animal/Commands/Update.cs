using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using PetStore.Services.Application.DTOs;
using PetStore.Services.Application.Exceptions;
using PetStore.Services.Application.Producers;
using PetStore.Services.Domain.Enums;
using PetStore.Services.Events;

namespace PetStore.Services.Application.CQRS.Animal.Commands
{
    public class Update
    {
        #region [ Command ]

        public class UpdateCommand : IRequest<AnimalDto>
        {
            public Guid Id { get; set; }
            public int Age { get; set; }
            public int Weight { get; set; }
            public string? Description { get; set; }
            public Status Status { get; set; }
        }

        #endregion [ Command ]

        #region [ Validations ]

        public class Validator : AbstractValidator<UpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Age)
                   .GreaterThanOrEqualTo(1)
                   .WithMessage("Esta edad no esta permitida")
                   .LessThan(99)
                   .WithMessage("Supera el limite de edad");

                RuleFor(x => x.Status)
                  .IsInEnum()
                  .WithMessage("Valor no permitido")
                  .WithErrorCode("400");
            }
        }

        #endregion [ Validations ]

        #region [ Handler ]

        public class Handler(ServicesDBContext context, IMapper mapper, IAnimalProducer animalProducer, ILogger<Handler> logger) : IRequestHandler<UpdateCommand, AnimalDto>
        {
            #region [ Dependencies ]

            private readonly ServicesDBContext context = context;
            private readonly IMapper mapper = mapper;
            private readonly IAnimalProducer animalProducer = animalProducer;
            private readonly ILogger<Handler> logger = logger;

            #endregion [ Dependencies ]

            public async Task<AnimalDto> Handle(UpdateCommand request, CancellationToken cancellationToken)
            {
                var animal = await context.Animals.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                            ?? throw new NotFoundException($"PET con Id :{request.Id} NO encontrado");

                animal.Age = request.Age;
                animal.Weight = request.Weight;
                animal.Description = request.Description;
                animal.Status = request.Status;

                context.Animals.Update(animal);
                var success = await context.SaveChangesAsync(cancellationToken) > 0;
                if (success)
                {
                    logger.LogWarning($"[INFO] => {animal.Name!.ToUpper()} pet successfully updated");
                    var response = mapper.Map<AnimalDto>(animal);
                    var animalUpdated = mapper.Map<AnimalUpdated>(animal);
                    animalProducer.SendUpdatedAnimal(animalUpdated);
                    logger.LogWarning($"[INFO] => {animal!.Name.ToUpper()} pet successfully sent");
                    return response;
                }

                throw new Exception("Error updating the pet");
            }
        }

        #endregion [ Handler ]
    }
}