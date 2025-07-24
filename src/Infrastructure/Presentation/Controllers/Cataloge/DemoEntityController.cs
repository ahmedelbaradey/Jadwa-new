using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;
using Domain.Entities.Products;
using Infrastructure.Dto.DemoEntity;
using Abstraction.Contracts.Service.Catalog;
using Microsoft.AspNetCore.Authorization;

namespace Controllers.Cataloge
{
    [Route("api/Catalog/[controller]")]
    [ApiController]
    public class DemoEntityController : BaseController<DemoEntity, DemoEntityDto>
    {
        public DemoEntityController(IDemoEntityService basicService, IAuthorizationService authorizationService) : base(basicService, authorizationService)
        {

        }
    }
}
