using OrderApi.Persistence.Extensions;
using OrderApi.WebApi.Extensions;
using Scalar.AspNetCore;

namespace OrderApi.WebApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();

        services.RegisterServices();
        services.AddPersistence();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

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