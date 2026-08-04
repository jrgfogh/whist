using Whist.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

ProgramEntryPoint.BuildApplication(args).Run();

namespace Whist.Server
{
    public static class ProgramEntryPoint
    {
        public static WebApplication BuildApplication(string[] args, Action<IWebHostBuilder>? configureWebHost = null, Action<IServiceCollection>? configureServices = null)
        {
            var builder = WebApplication.CreateBuilder(args);

            configureWebHost?.Invoke(builder.WebHost);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IConductorService, GameConductorService>();
            configureServices?.Invoke(builder.Services);

            var app = builder.Build();

            app.UsePathBase("/whist");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            app.MapHub<WhistHub>("/WhistHub");
            app.MapFallbackToFile("index.html");

            return app;
        }
    }
}
