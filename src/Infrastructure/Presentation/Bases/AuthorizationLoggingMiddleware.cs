using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Contracts.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
 

namespace Presentation.Bases
{
    public class AuthorizationLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public AuthorizationLoggingMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var policyMetadata = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (policyMetadata != null && !string.IsNullOrWhiteSpace(policyMetadata.Policy))
            {
                _logger.LogInfo($"Authorization Required: {policyMetadata.Policy} for {context.Request.Path}");
            }

            await _next(context);
        }
    }
}
