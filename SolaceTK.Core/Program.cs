using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolaceTK.Core.Contexts;
using System;
using System.Security.Cryptography.X509Certificates;

namespace SolaceTK.Core
{
    public class Program
    {
        public static string Authority = Environment.GetEnvironmentVariable("SOLTK_AUTHORITY") ?? "https://localhost:5000";
        public static string ExecuteMigration = Environment.GetEnvironmentVariable("SOLTK_EXECMIGRATION") ?? "false";

        // ConnectionString Format: "Data Source=127.0.0.1;User ID=sa;Password=aA17gcg8SSt;Initial Catalog=soltk;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string ConnectionString = Environment.GetEnvironmentVariable("SOLTK_CONNECTIONSTRING") ?? "";

        // To enable end-to-end encryption - mount tls.crt and tls.key to /app/certs:
        public static string EnableTls = Environment.GetEnvironmentVariable("SOLTK_ENABLE_TLS") ?? "false";
        public static string ContentDirectory = Environment.GetEnvironmentVariable("SOLTK_CONTENT_DIRECTORY") ?? "Ase";
        
        // TODO: Fix with this (OperatingSystem.IsLinux() ? @"/usr/bin/aseprite" : @"C:\Program Files\Aseprite\aseprite");
        public static string AseCli = Environment.GetEnvironmentVariable("SOLTK_ASECLI") ?? @"/usr/bin/aseprite";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                using (var scope = host.Services.CreateScope())
                {
                    // TODO: this is _not_ good stuff here - need to make this clean and readable for the at runtime DB Migrations for Entity Framework:
                    var behcontext = scope.ServiceProvider.GetRequiredService<BehaviorContext>();
                    Console.WriteLine("BehaviorContext migrating...");
                    if (behcontext.Database.EnsureCreated()) Console.WriteLine("BehaviorContext Database Created...");
                    if (!string.IsNullOrEmpty(ExecuteMigration) && bool.Parse(ExecuteMigration) && !string.IsNullOrEmpty(behcontext.Database.GetPendingMigrations().ToString())) behcontext.Database.Migrate();
                    Console.WriteLine("BehaviorContext migrated.");

                    var contrcontext = scope.ServiceProvider.GetRequiredService<ControllerContext>();
                    Console.WriteLine("ControllerContext migrating...");
                    if (contrcontext.Database.EnsureCreated()) Console.WriteLine("ControllerContext Database Created...");
                    if (!string.IsNullOrEmpty(ExecuteMigration) && bool.Parse(ExecuteMigration) && !string.IsNullOrEmpty(contrcontext.Database.GetPendingMigrations().ToString())) contrcontext.Database.Migrate();
                    Console.WriteLine("ControllerContext migrated.");

                    var corecontext = scope.ServiceProvider.GetRequiredService<CoreContext>();
                    Console.WriteLine("CoreContext migrating...");
                    if (corecontext.Database.EnsureCreated()) Console.WriteLine("CoreContext Database Created...");
                    if (!string.IsNullOrEmpty(ExecuteMigration) && bool.Parse(ExecuteMigration) && !string.IsNullOrEmpty(corecontext.Database.GetPendingMigrations().ToString()))  corecontext.Database.Migrate();
                    Console.WriteLine("CoreContext migrated.");

                    var soundcontext = scope.ServiceProvider.GetRequiredService<SoundContext>();
                    Console.WriteLine("SoundContext migrating...");
                    if (soundcontext.Database.EnsureCreated()) Console.WriteLine("SoundContext Database Created...");
                    if (!string.IsNullOrEmpty(ExecuteMigration) && bool.Parse(ExecuteMigration) && !string.IsNullOrEmpty(soundcontext.Database.GetPendingMigrations().ToString())) soundcontext.Database.Migrate();
                    Console.WriteLine("SoundContext migrated.");

                    var workcontext = scope.ServiceProvider.GetRequiredService<WorkContext>();
                    Console.WriteLine("WorkContext migrating...");
                    if (workcontext.Database.EnsureCreated()) Console.WriteLine("WorkContext Database Created...");
                    if (!string.IsNullOrEmpty(ExecuteMigration) && bool.Parse(ExecuteMigration) && !string.IsNullOrEmpty(workcontext.Database.GetPendingMigrations().ToString())) workcontext.Database.Migrate();
                    Console.WriteLine("WorkContext migrated.");

                    var envcontext = scope.ServiceProvider.GetRequiredService<EnvironmentContext>();
                    Console.WriteLine("EnvironmentContext migrating...");
                    if (envcontext.Database.EnsureCreated()) Console.WriteLine("EnvironmentContext Database Created...");
                    if (!string.IsNullOrEmpty(ExecuteMigration) && bool.Parse(ExecuteMigration) && !string.IsNullOrEmpty(envcontext.Database.GetPendingMigrations().ToString())) envcontext.Database.Migrate();
                    Console.WriteLine("EnvironmentContext migrated.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    if (EnableTls == "true")
                    {
                        webBuilder.ConfigureKestrel(options =>
                        {
                            options.ConfigureHttpsDefaults(def =>
                            {
                                def.ServerCertificate = X509Certificate2.CreateFromPemFile("certs/tls.crt", "certs/tls.key");
                            });
                        });
                    }
                });
    }
}
