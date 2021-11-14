using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using Catalog.API.Repositories;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Catalog.API.Helpers
{
  public static class StartupHelper
  {
    private static MongoDbSettings GetMongoDbSettings(IConfiguration configuration)
    {
      return configuration
        .GetSection(nameof(MongoDbSettings)) // captura a parte do appsettings
        .Get<MongoDbSettings>();  // converte para a classe
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
      // corrigindo a representação dos dados serializados (Guid, DateTimeOffset)
      BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
      BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

      var mongoDbSettings = GetMongoDbSettings(configuration);

      // registrando a conexão do MongoDB
      services.AddSingleton<IMongoClient>(serviceProvider =>
      {
        return new MongoClient(mongoDbSettings.ConnectionString);
      });

      return services;
    }

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
      // adicionando o mongodb
      services.AddMongoDb(configuration);

      // injetando o repo
      services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

      return services;
    }
    public static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {
      var mongoDbSettings = GetMongoDbSettings(configuration);

      services.AddHealthChecks()
        .AddMongoDb(
          mongoDbSettings.ConnectionString,
          name: "mongodb",
          timeout: TimeSpan.FromSeconds(3),     // tempo do timeout
          tags: new[] { "ready" }               // adiciona uma tag
        );
      return services;
    }
    public static IEndpointRouteBuilder AddHealthChecksEndPoints(this IEndpointRouteBuilder endpoints)
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