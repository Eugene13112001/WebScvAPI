using WebScvAPI.Settings;
using WebScvAPI.Containers;
using WebScvAPI.Models;
using Microsoft.Extensions.Options;
using static WebScvAPI.Containers.ICSVServiceData;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<CSVDatabaseSettings>(
        builder.Configuration.GetSection(nameof(CSVDatabaseSettings)));

builder.Services.AddSingleton<ICSVDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<CSVDatabaseSettings>>().Value);
builder.Services.AddSingleton<ICSVServiceData , CSVServiceData>();
builder.Services.AddSingleton<ICSVServiceReader, CSVServiceReader>();
builder.Services.AddSingleton<ICSVServiceWriter, CSVServiceWriter>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Service", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
