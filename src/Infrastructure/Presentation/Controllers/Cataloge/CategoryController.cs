using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Catalog.Categories.Commands.Add;
using Application.Features.Catalog.Categories.Queries.List;
using Application.Features.Catalog.Categories.Queries.Get;
using Presentation.Bases;

namespace Controllers.Cataloge
{
    [Route("api/Catalog/[controller]")]
    [ApiController]

    public class CategoryController : AppControllerBase
    {
        [HttpGet("List")]
        public async Task<IActionResult> GetCategoriesPaginatedList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetCategroyById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetQuery() { Id = id });
            return NewResult(response);
        }
        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public async Task<IActionResult> AddCateogry([FromBody] AddCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
