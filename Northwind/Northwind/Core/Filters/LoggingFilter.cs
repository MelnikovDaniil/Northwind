using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Northwind.Data.Domain.Extensions;
using Serilog;
using System.Linq;

namespace Northwind.Core.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly ILogger _logger;
        private readonly bool _loggingActionParameters;

        public LoggingFilter(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _loggingActionParameters = configuration.GetValue<bool>("LoggingActionParameters");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var parametersString = string.Empty;
            if (_loggingActionParameters)
            {
                if (context.ActionArguments.Any())
                {
                    parametersString = $"with arguments " +
                    $"{context.ActionArguments.ToString("-", ";")}";
                }
                else
                {
                    parametersString = $"without arguments";
                }
            }
            _logger.Information($"Start processing action: {context.ActionDescriptor.DisplayName} {parametersString}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode == 200)
            {
                _logger.Information($"Finish processing action: {context.ActionDescriptor.DisplayName} seccesfuly");
            }
            else
            {
                _logger.Warning($"Finish processing action: {context.ActionDescriptor.DisplayName} with status code " +
                    $"{context.HttpContext.Response.StatusCode}");
            }

        }
    }
}
