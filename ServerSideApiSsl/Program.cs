using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using ServerSideApiSsl.Hubs;
using SharedClassLibrary;
using System.Configuration;
using System.Text.Json;

namespace ServerSideApiSsl
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecasetRepository>();
            builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<ICosmosDbService<Character_abstract>>(CosmosDbService<Character_abstract>.InitializeCosmosClientInstance(builder.Configuration.GetSection(CosmosDbOptions.CosmosDb)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<FileHub>("/filehub");
            app.MapControllerRoute ( name: "default", pattern: "{controller=Sheet}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
