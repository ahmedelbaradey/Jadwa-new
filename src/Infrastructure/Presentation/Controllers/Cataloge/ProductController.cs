using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Catalog.Products.Commands.Edit;
using Application.Features.Catalog.Products.Commands.Add;
using Application.Features.Catalog.Products.Queries.List;
using Application.Features.Catalog.Categories.Queries.Get;
using Application.Features.Catalog.Products.Commands.Delete;
using Presentation.Bases;
using Abstraction.Constants.ModulePermissions;


namespace Controllers.Cataloge
{
    [Route("api/Catalog/[controller]")]
    [ApiController]
    public class ProductController : AppControllerBase
    {
        [Authorize(Policy = ProductPermission.List)]
        [HttpGet("List")]
        public async Task<IActionResult> GetProductsPaginatedList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = ProductPermission.View)]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {

            var response = await Mediator.Send(new GetQuery() { Id = id });
            return NewResult(response);
        }

        [Authorize(Policy = ProductPermission.Create)]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = ProductPermission.Edit)]
        [HttpPut]
        public async Task<IActionResult> EditProdcuct([FromBody] EditProductCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = ProductPermission.Delete)]
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteProductCommand() { Id = id });
            return NewResult(response);
        }



    }
}
