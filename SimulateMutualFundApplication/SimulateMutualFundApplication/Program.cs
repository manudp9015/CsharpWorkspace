using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MutualFundSimulatorService.Business;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using MutualFundSimulatorService.Interfaces;
using System;
using MutualFundSimulatorService;

namespace MutualFundSimulatorApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services);
            var app = builder.Build();

            // Configure the HTTP pipeline
            app.UseRouting();

            // Add Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mutual Fund Simulator API V1");
                c.RoutePrefix = string.Empty; // Set Swagger UI as the default page (/)
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            // Initialize services
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbPatch = services.GetService<DBPatch>();
                var utility = services.GetService<MutualFundSimulatorUtility>();

                User.CurrentDate = new DateTime(2025, 04, 28);
                dbPatch.CreateTablesForMutualFunds();

                // Run console mode only if --console is explicitly passed
                if (args.Length > 0 && args[0] == "--console")
                {
                    utility.MainMenu();
                    return;
                }
            }

            // Start the Web API
            Console.WriteLine("Starting Web API at http://localhost:5000");
            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Mutual Fund Simulator API",
                    Version = "v1",
                    Description = "API for Mutual Fund Simulator Application"
                });
            });

            services.AddSingleton<User>();
            services.AddSingleton<UserSipInvest>();
            services.AddSingleton<UserLumpsumInvest>();
            services.AddScoped<MutualFundRepository>();
            services.AddScoped<MutualFundBusiness>();
            services.AddScoped<Lumpsum>();
            services.AddScoped<Sip>();
            services.AddScoped<UserLogin>();
            services.AddScoped<IUserLogin, UserLogin>();
            services.AddTransient<DBPatch>();
            services.AddTransient<MutualFundSimulatorUtility>();
        }
    }
}