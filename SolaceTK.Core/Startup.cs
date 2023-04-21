using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Services;
using System.IO;

namespace SolaceTK.Core
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // If no SQL ConnectionString is provided - Use InMemory Database:
            if (string.IsNullOrEmpty(Program.ConnectionString))
            {
                services.AddDbContext<GlossaryContext>(options => options.UseInMemoryDatabase("onbow-arranger-mem"));
                services.AddDbContext<SoundContext>(options => options.UseInMemoryDatabase("onbow-sounds-mem"));
                services.AddDbContext<ControllerContext>(options => options.UseInMemoryDatabase("onbow-controller-mem"));
                services.AddDbContext<BehaviorContext>(options => options.UseInMemoryDatabase("onbow-behavior-mem"));
                services.AddDbContext<CoreContext>(options => options.UseInMemoryDatabase("onbow-core-mem"));
                services.AddDbContext<WorkContext>(options => options.UseInMemoryDatabase("onbow-work-mem"));
                services.AddDbContext<EnvironmentContext>(options => options.UseInMemoryDatabase("onbow-environment-mem"));
            }
            else
            {
                services.AddDbContext<GlossaryContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-gloss;"));
                services.AddDbContext<SoundContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-sound;"));
                services.AddDbContext<ControllerContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-controller;"));
                services.AddDbContext<BehaviorContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-behavior;"));
                services.AddDbContext<CoreContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-core;"));
                services.AddDbContext<WorkContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-work;"));
                services.AddDbContext<EnvironmentContext>(options => options.UseSqlServer(Program.ConnectionString + "Initial Catalog=sol-environment;"));
            }


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SolaceTK.Core", Version = "v1", Description = "Version 1 is pre-microservices, this Core API is a placeholder until v2 comes around next." });
            });

            services.AddCors();
            services.AddDistributedMemoryCache();

            services.AddSingleton(new AsepriteService(Program.AseCli, Program.ContentDirectory));


            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Program.Authority;
                    options.TokenValidationParameters.ValidateAudience = false;
                    //options.TokenValidationParameters.IssuerSigningKey = new SecurityKey();
                    options.RequireHttpsMetadata = false;
                    //options.TokenValidationParameters.RequireSignedTokens = false;
                    //options.TokenValidationParameters.ValidateIssuer = false;
                    //options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolaceTK.Core v1"));
            }

            //app.UseHttpsRedirection();

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                //policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Ase")),
                RequestPath = new PathString("/Ase")
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
