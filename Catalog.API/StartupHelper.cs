using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API
{
  public static class StartupHelper
  {
    public static IEndpointRouteBuilder AddHealthChecks(this IEndpointRouteBuilder endpoints)
    {
      endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
      {
        // verifica os serviços com a tag "ready"
        Predicate = (check) => check.Tags.Contains("ready"),
        // personaliza a resposta do serviço
        ResponseWriter = async (context, report) =>
        {
          var result = JsonSerializer.Serialize(
            new
            {
              status = report.Status.ToString(),
              checks = report.Entries.Select(entry => new
              {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                duration = entry.Value.Duration.ToString()
              })
            }
          );
          context.Response.ContentType = MediaTypeNames.Application.Json;
          await context.Response.WriteAsync(result);
        }
      });
      endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
      {
        // verifica o serviço geral
        Predicate = (_) => false
      });

      return endpoints;
    }
  }
}