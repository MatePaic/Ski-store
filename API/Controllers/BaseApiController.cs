using API.Extensions;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")] 
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository,
            ISpecification<T> specification, int pageNumber, int pageSize) where T : BaseEntity
        {
            var productItems = await repository.GetWithSpecificationAsync(specification);
            var count = await repository.CountAsync(specification);
            var paginationMetaData = new PaginationMetadata<T>(count, pageSize, pageNumber);

            Response.AddPaginationHeader(paginationMetaData);

            return Ok(productItems);
        }
    }
}
