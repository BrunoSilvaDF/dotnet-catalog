using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetCatalog.Repositories;
using DotnetCatalog.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace DotnetCatalog
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Services registration
    //  Dependency injection
    public void ConfigureServices(IServiceCollection services)
    {
      // corrigindo a representação dos dados serializados (Guid, DateTimeOffset)
      BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
      BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

      // registrando a conexão do MongoDB
      services.AddSingleton<IMongoClient>(serviceProvider =>
      {
        var settings = Configuration
          .GetSection(nameof(MongoDbSettings)) // captura a parte do appsettings
          .Get<MongoDbSettings>();  // converte para a classe
        return new MongoClient(settings.ConnectionString);
      });

      // registrando a instância do mongodb
      services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();


      // registering an instance (singleton) of repo
      //  => nesse caso, o ItemsRepo vai receber uma instância do InMemoryItemsRepo
      // services.AddSingleton<IItemsRepository, InMemItemsRepository>();

      services.AddControllers(options =>
      {
        // em tempo de execução, o dotnet remove os sufixos ASYNC dos métodos
        // o ideal é suprimir para termos diferenciação
        options.SuppressAsyncSuffixInActionNames = false;
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotnetCatalog", Version = "v1" });
      });
    }

    // Request handling pipeline configuration
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetCatalog v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
