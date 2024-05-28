using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetStore.Search.Application.Common;
using PetStore.Search.Application.DTOs;
using PetStore.Search.Application.Helpers;
using PetStore.Search.Data.Persistence;

namespace PetStore.Search.Application.CQRS.Animal.Queries
{
    public class Search
    {
        #region [ Query ]

        public class Query : IRequest<PagedResult<AnimalDto>>
        {
            public SearchParams? Params { get; set; }
        }

        #endregion [ Query ]

        #region [ Handler ]

        public class Handler(SearchDBContext context, IMapper mapper) : IRequestHandler<Query, PagedResult<AnimalDto>>
        {
            private readonly SearchDBContext context = context;
            private readonly IMapper mapper = mapper;

            public async Task<PagedResult<AnimalDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await context.Animals
                    .AsNoTracking()
                    .OrderBy(request.Params?.OrderBy ?? "Name", request.Params?.OrderDir ?? "ASC")
                    .Where(request.Params?.SearchBy, request.Params?.SearchValue)
                    .ProjectTo<AnimalDto>(mapper.ConfigurationProvider)
                    .GetPagedResultAsync(request?.Params?.Size ?? 2, request?.Params?.Page ?? 1);

                return response;
            }
        }

        #endregion [ Handler ]
    }
}