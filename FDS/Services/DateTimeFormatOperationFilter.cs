using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace FDS.Services
{
    public class DateTimeFormatOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var parameter in operation.Parameters)
            {
                if (parameter.Schema?.Format == "date-time")
                {
                    parameter.Schema.Example = new OpenApiString(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                }
            }
        }
    }
}
