using Abstraction.Constants.ModulePermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Presentation.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Shared
{
    [Route("api/[controller]")]
    [Authorize]
    public class SystemConfigurationController : AppControllerBase
    {
        [HttpGet("GetConfigs")]
        public ActionResult GetSystemConfigrations() =>
         System.IO.File.Exists(Directory.GetCurrentDirectory() + "/systemConfiguration.json")
        ? Content(System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/systemConfiguration.json"), "application/json")
        : NotFound("{\"error\":\"File not found\"}");
    }
}
