using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Business.ConcreteClass;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Repository.ConcreteClass;
using MutualFundSimulatorService.Repository.Interface;

namespace MutualFundSimulatorApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services);
            var app = builder.Build();

            //builder.Services.AddTransient<DBPatch>(sp =>
            //{
            //    var repository = sp.GetRequiredService<IRepository>();
            //    return new DBPatch(repository);  // Manual construction
            //});

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mutual Fund Simulator API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            using (var scope = app.Services.CreateScope())
            {
                var dbPatch = scope.ServiceProvider.GetService<DBPatch>();
                User.CurrentDate = new DateTime(2025, 08, 07);
                dbPatch.CreateTablesForMutualFunds();
            }

            Console.WriteLine("Starting Web API at http://localhost:5000");
            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Mutual Fund Simulator API",
                    Version = "v1",
                    Description = "API for Mutual Fund Simulator Application"
                });
            });

            services.AddScoped<User>();
            services.AddScoped<UserSipInvest>();
            services.AddScoped<UserLumpsumInvest>();

            // Register repository
            services.AddScoped<IRepository, MutualFundRepository>();

            // Register services
            services.AddScoped<IFundService, FundService>();
            services.AddScoped<ILumpsumService, LumpsumService>();
            services.AddScoped<ISipService, SipService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<INavService, NavService>();
            services.AddScoped<IUserService, UserService>();

            // Register DBPatch
            services.AddTransient<DBPatch>();
            
           


        }
    }
}