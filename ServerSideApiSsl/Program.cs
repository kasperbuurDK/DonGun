using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using ServerSideApiSsl.Database;
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<FileHub>("/filehub");

            app.Run();
        }
    }
}
