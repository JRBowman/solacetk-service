using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SolaceTK.Data.Contexts;
using SolaceTK.Data.Services;

var Authority = Environment.GetEnvironmentVariable("SOLTK_AUTHORITY") ?? "https://localhost:5000";
var ExecuteMigration = Environment.GetEnvironmentVariable("SOLTK_EXECMIGRATION") ?? "false";

// ConnectionString Format: "Data Source=127.0.0.1;User ID=sa;Password=aA17gcg8SSt;Initial Catalog=soltk;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
var ConnectionString = Environment.GetEnvironmentVariable("SOLTK_CONNECTIONSTRING") ?? "";

// To enable end-to-end encryption - mount tls.crt and tls.key to /app/certs:
var EnableTls = Environment.GetEnvironmentVariable("SOLTK_ENABLE_TLS") ?? "false";
var ContentDirectory = Environment.GetEnvironmentVariable("SOLTK_CONTENT_DIRECTORY") ?? "Artifacts";

// TODO: Fix with this (OperatingSystem.IsLinux() ? @"/usr/bin/aseprite" : @"C:\Program Files\Aseprite\aseprite");
var AseCli = Environment.GetEnvironmentVariable("SOLTK_ASECLI") ?? @"/usr/bin/aseprite";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CoreContext>(options => options.UseInMemoryDatabase("onbow-core-mem"));

// Add Services:
builder.Services.AddSingleton(new SolTkFileService(ContentDirectory));
builder.Services.AddTransient<ArtifactService>();

builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    //policy.WithExposedHeaders("WWW-Authenticate");
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), ContentDirectory)),
    RequestPath = new PathString("/" + ContentDirectory)
});

app.UseAuthorization();

app.MapControllers();

app.Run();
