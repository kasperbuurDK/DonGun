using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using ServerSideApiSsl.Database;
using ServerSideApiSsl.Hubs;
using SharedClassLibrary;
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
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables();
            builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "DevPolicy",
                            policy =>
                            {
                                policy.WithOrigins("https://localhost:7116")
                                        .AllowAnyMethod();
                            });
            });
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ISqlDbService<Player>, SqlDbService<Player>>();
            builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler<Player>>("BasicAuthentication", null);
            builder.Services.AddAuthorization();
            builder.Services.Configure<SqlSettings>(builder.Configuration.GetSection("SqlSettings"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<GameHub>("/gamehub");

            app.Run();
        }
    }
}
