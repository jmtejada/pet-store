using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PetStore.Search.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator? mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    }
}