using OrderApi.Messaging.Extensions;
using OrderApi.Persistence.Extensions;
using OrderApi.WebApi.Extensions;
using Scalar.AspNetCore;

namespace OrderApi.WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();

        services.RegisterServices();
        services.AddPersistence();
        services.AddMessaging(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.ApplyMigrations();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                if (env.IsDevelopment())
                {
                    endpoints.MapOpenApi();
                    endpoints.MapScalarApiReference();
                }
            }
        );
    }
}