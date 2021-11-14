using Catalog.API.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Catalog.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Services registration
    public void ConfigureServices(IServiceCollection services)
    {
      // refactor para fazer injeção de dependência centralizada
      services.AddDependencyInjection(Configuration);

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
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
      });

      // adiciona o serviço de Health Checks
      services.AddHealthChecksService(Configuration);
    }

    // Request handling pipeline configuration
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));

        app.UseHttpsRedirection();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();

        endpoints.AddHealthChecksEndPoints();
      });
    }
  }
}
