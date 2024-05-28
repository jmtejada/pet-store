using Microsoft.AspNetCore.Mvc;
using PetStore.Search.Application.Common;
using PetStore.Search.Application.DTOs;
using PetStore.Search.Application.Helpers;
using Queries = PetStore.Search.Application.CQRS.Animal.Queries;

namespace PetStore.Search.Api.Controllers
{
    [Route("api/search/[controller]")]
    public class AnimalController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<AnimalDto>>> Search([FromQuery] SearchParams searchParams)
        {
            try
            {
                var query = new Queries.Search.Query() { Params = searchParams };
                var result = await Mediator.Send(query);
                if (result.RowsCount > 0)
                    return result;

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}