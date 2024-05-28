using Microsoft.AspNetCore.Mvc;
using PetStore.Services.Application.DTOs;
using Commands = PetStore.Services.Application.CQRS.Animal.Commands;

namespace PetStore.Services.Api.Controllers
{
    [Route("api/services/[controller]")]
    public class AnimalController : BaseController
    {
        //[HttpPost]
        //public async Task<AnimalDto> Create([FromBody] Commands.Create.UpdateCommand command) => await Mediator.Send(command);

        [HttpPost]
        public async Task<ActionResult<AnimalDto>> Create([FromBody] Commands.Create.CreateCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return CreatedAtAction(nameof(Create), new { result!.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<AnimalDto> Update([FromRoute] Guid id, [FromBody] Commands.Update.UpdateCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
    }
}