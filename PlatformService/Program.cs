using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("---> Using in memory--->Development");
    builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseInMemoryDatabase("InMemoryDatabase"));
 }
else
{
    Console.WriteLine("---> Using Sql Server--->Production");
    builder.Services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn")));
}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.Prepopulation(app, app.Environment.IsProduction());

ConfigurationManager configuration = builder.Configuration;

Console.WriteLine($"CommandService endpoint: {configuration["CommandService"]}");

app.Run();

